using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Agile.Diagnostics.Logging;
using Agile.Framework;
using Agile.Framework.Server;
using Agile.Shared;
using Agile.Shared.IoC;
using Nancy;
using Nancy.ModelBinding;
using Nancy.TinyIoc;

namespace Agile.Web.Rest
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>need to return Nancy HttpStatusCodes for testing, end result from 
    /// client/browser perspective is the same.</remarks>
    public abstract class NancyModuleBase
        <TR, TB>
        : NancyModule
        where TR : class, IRepository<TB>
        where TB : IBaseBiz
    {
        /// <summary>
        /// ctor
        /// </summary>
        public NancyModuleBase(string rootPath) : base(rootPath)
        {
        }  

        protected TR Repository
        {
            get { return Container.Resolve<TR>(); }
        }

        /// <summary>
        /// just the generated routes by default.
        /// Override this in a partial class if more routes are needed.
        /// </summary>
        protected virtual void DefineRoutes()
        {
        }

        protected dynamic GetById(dynamic parameters)
        {
            Logger.Debug("GetById");
            long id = NancyHelper.GetLongId(parameters);
            if (id < 0)
            {
                Logger.Warning("GetById but id is: {0}", id);
                return HttpStatusCode.BadRequest;
            }
            var client = Request.Headers["client"].FirstOrDefault();
            Logger.Debug("{0}", client ?? "No Client");

            try
            {
                Logger.Testing("id:{0}", id);
                var loaded = Repository.Load(id, GetDeepLoaders());

                return (loaded != null)
                    ? Response.AsJson(loaded).WithStatusCode(HttpStatusCode.OK)
                    : ErrorResponse(string.Format("Couldn't Find {0}", id), HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetById");
                // for now do return the ex details, later make this configurable (possible security risk)
                return ErrorResponse(ex);
            }
        }

        protected dynamic GetAll(dynamic parameters)
        {
            Logger.Debug("GetAll");

            var client = Request.Headers["client"].FirstOrDefault();
            Logger.Debug("{0}", client ?? "No Client");

            try
            {
                var loaded = Repository.GetAll(new List<DeepLoader>()); // don't use deeploaders

                return (loaded != null)
                    ? Response.AsJson(loaded).WithStatusCode(HttpStatusCode.OK)
                    : HttpStatusCode.NotFound;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetAll");
                // for now do return the ex details, later make this configurable (possible security risk)
                return ErrorResponse(ex);
            }
        }

        protected IList<DeepLoader> GetDeepLoaders()
        {
            if (Request.Body.Length == 0)
                return null;

            try
            {
                var loaders = this.Bind<List<DeepLoader>>();
                Logger.Debug("{0} deeploaders", loaders.Count);
                return loaders;
            }
            catch (Exception ex) // don't throw. Maybe should return BadRequest TODO
            {
                Logger.Warning(ex.Message, "GetDeepLoaders");
                return null;
            }
        }


        /// <summary>
        /// Post using the 'standard' Repo for the module
        /// </summary>
        protected Response PostRoute(Func<TB
            , PrePostSaveResult> preSave = null
            , Func<TB, bool, PrePostSaveResult> postSave = null) 
        {
            return PostRoute(Repository, preSave, postSave);
        }

        /// <summary>
        /// Post using an explicit Repo for the module
        /// </summary>
        protected Response PostRoute<T>(IRepository<T> repo
            , Func<T, PrePostSaveResult> preSave = null
            , Func<T, bool, PrePostSaveResult> postSave = null) where T : IBaseBiz
        {
            Logger.Debug("POST: bind");

            try
            {
                var instance = this.Bind<T>();
                // below is pointless because an ex is thrown...
//                if (instance == null)
//                    return HttpStatusCode.BadRequest;

                var wasNew = ! repo.Exists(instance);
                instance.IsNew = wasNew; // don't care what was sent, set it from a call to the db

                Logger.Info("POST: {0}", instance);

                if (preSave != null)
                {
                    var result = preSave(instance);
                    if (result != null)
                    {
                        if(result.Messages.Count > 0)
                            Logger.Warning(result.Messages.AsSingleMultilineString());
                        if (result.StatusCode != HttpStatusCode.OK) // make the statusCode whatever was determined in the PreSave
                            return ErrorResponse(result.Messages.AsSingleMultilineString(), result.StatusCode);
                    }
                }

                // make sure it is valid before trying to save it
                if (! instance.IsValid())
                    return ErrorResponse(instance.GetValidationMessages(), HttpStatusCode.BadRequest);

                // SAVE
                Logger.Debug("POST: save {0}", instance);
                var saved = repo.Save(instance);
                
                if (saved == null) // this should only ever happen in unit tests
                    return ErrorResponse("saved instance was null");

                // If all ok return Created and a location header pointing at the new resource
                if (postSave != null)
                {
                    var result = postSave(saved, wasNew);
                    if (result != null)
                    {
                        if (result.Messages.Count > 0)
                            Logger.Warning(result.Messages.AsSingleMultilineString());
                        if (result.StatusCode != HttpStatusCode.OK) // make the statusCode whatever was determined in the PreSave
                            return ErrorResponse(result.Messages.AsSingleMultilineString(), result.StatusCode);
                    }
                }
                return PostResponse(saved, wasNew);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "POST");

                // Update failed to update any records (probably not a matching Id)
                if (ex.Message.Contains("zero rows affected"))
                    return ErrorResponse(ex, HttpStatusCode.NotFound);

                if (ex.Message.Contains("Error converting value"))
                    return ErrorResponse(ex, HttpStatusCode.BadRequest);

                // for now do return the ex details, later make this configurable (possible security risk)
                return ErrorResponse(ex);
            }
            finally
            {
                // Dispose?? not needed right now, look at this when redo db libraries
            }
        }


        /// <summary>
        /// Create the Response for a successful save.
        /// </summary>
        protected virtual Response PostResponse<T>(T saved, bool wasNew)
             where T : IBaseBiz
        {
            return Response.AsJson(saved)
                .WithStatusCode(wasNew ? HttpStatusCode.Created : HttpStatusCode.OK)
                .WithHeader("Location", string.Format("{0}/{1}"
                    , Context.Request.Url.ToString().RemoveTrailingCharacterIfExists("/")
                    , saved.GetId()));
        }

        protected Response ErrorResponse(Exception ex, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            var fullMessage = GetAllMessages(ex);
            return ErrorResponse(fullMessage, statusCode);
        }

        private string GetAllMessages(Exception exception)
        {
#if ! DEBUG
            return string.Empty;
#endif
            return exception.Message;
        }

        protected Response ErrorResponse(string message = "", HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            Logger.Warning(message);
            return Response.AsText(message).WithStatusCode(statusCode);
        }
    }

    public static class StringExtensions
    {
        public static string RemoveTrailingCharacterIfExists(this string value, string trailer)
        {
            if (string.IsNullOrEmpty(trailer))
                return value;
            return value.EndsWith(trailer) ? value.Remove(value.Length - 1 - trailer.Length, trailer.Length)
                : value;
        }

        public static string AsSingleMultilineString(this IList<string> messages)
        {
            var builder = new StringBuilder();
            messages.ForEach(m => builder.AppendLine(m));
            return builder.ToString();
        }
    }

    public class PrePostSaveResult
    {
        /// <summary>
        /// ctor
        /// </summary>
        public PrePostSaveResult(HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            Messages = new List<string>();
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; set; }
        public IList<string> Messages { get; set; }
    }
}
