using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Agile.Diagnostics.Logging;
using Agile.Framework;
using Agile.Framework.Web;
using Agile.Mobile.DAL;
using Agile.Mobile.Environments;
using Agile.Shared.IoC;
using Agile.Shared.PubSub;
using AutoMapper;
using Newtonsoft.Json;

namespace Agile.Mobile.Web
{
    public abstract class HttpServiceBase<T> : IHttpServiceBase<T>
        where T : class
    {
        /// <summary>
        /// ctor
        /// </summary>
        protected HttpServiceBase(string serviceName)
        {
            ServiceName = serviceName;

            Logger.Debug("[{0}] HttpServiceBase: ctor *** {1} Environment *** "
                , typeof(T).Name
                , Environment.Name);
        }

        private string ServiceName { get; set; }


        private INetworkConnectionManager connectionManager;

        private INetworkConnectionManager ConnectionManager
        {
            get
            {
                return connectionManager ?? (connectionManager = Container.Resolve<INetworkConnectionManager>());
            }
        }

        private IMobileEnvironment environment;

        private IMobileEnvironment Environment
        {
            get { return environment ?? (environment =  Container.Resolve<IMobileEnvironment>()); }
        }

        private EndpointDetail endpoint;
        private EndpointDetail Endpoint
        {
            get 
            {
                if (endpoint == null)
                {
                    endpoint = Environment.Services.FirstOrDefault(s => s.Name.Equals(ServiceName, StringComparison.CurrentCultureIgnoreCase));
                    if (endpoint == null)
                    {
                        const string message = "Failed to find endpoint for REST call, probably a problem with environment configuration";
                        Logger.Warning(message);
                        throw new Exception(message);
                    }
                }
                return endpoint; 
            }
        }

        
        /// <summary>
        /// Gets the web host address (essentially the 'base' address for calls)
        /// </summary>
        /// <remarks>each inheritor would override and suffix with the required route</remarks>
        protected virtual string GetUrlBase()
        {
            return Endpoint.BaseUrl;
        }


        protected virtual void AddRequestHeaders(WebRequest request)
        {            
            // Don't add Authorization headers here, callers need to specifiy 'standard' headers to add (especially Authorization)
            request.Headers["client"] = HttpHelper.Platform;
        }

        public Task<ServiceCallResult<T>> GetAsync(long id)
        {
            return GET<T>(string.Format("/{0}", id));
        }

        public Task<ServiceCallResult<T>> GetAsync(long id, IList<DeepLoader> loaders)
        {
            return GET<T>(string.Format("/Load/{0}", id), loaders);
        }

        protected async Task<ServiceCallResult<TP>> GET<TP>(string url, IList<DeepLoader> loaders = null)
        {
            Logger.Debug("GetAsync:{0}", url);
            var request = WebRequest.Create(GetUrlBase() + url);
            if (loaders != null)
            {
                Logger.Debug("add deepLoaders");
                // must be sent as POST because cannot add body to GET request
                request.Method = HttpHelper.POST;
                request.ContentType = ContentTypes.JSON;
                await AddBodyToRequest(HttpHelper.SerializeToJsonThenByteArray(loaders), request);
            }

            var result = await MakeServerRequest<TP>(request);
            return result;
        }

        protected static async Task AddBodyToRequest(byte[] data, WebRequest request)
        {
            Logger.Debug("AddBodyToRequest: {0}", request.ContentType);
            var uri = request.RequestUri.AbsoluteUri;
            try
            {
                using (Stream stream = await Task<Stream>.Factory.FromAsync(request.BeginGetRequestStream, request.EndGetRequestStream, null))
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                Logger.Info(string.Format("[{0}] {1}. AddBodyToRequest [{2}]"
                    , ex.GetType().Name, ex.Message, uri));
            }
        }

        /// <summary>
        /// Make a server request, GET or POST or whatever.
        /// This method adds all other required bits for all calls, e.g. Headers and authorization stuff
        /// </summary>
        /// <typeparam name="TR">Result type</typeparam>
        /// <param name="request"></param>
        protected async Task<ServiceCallResult<TR>> MakeServerRequest<TR>(WebRequest request)
        {
            if (!ConnectionManager.CanSend)
                return new ServiceCallResult<TR>(new Exception("MakeServerRequest: No Connection"));

            Logger.Debug("{0}: {1}", request.Method, request.RequestUri);

            AddRequestHeaders(request);
            
            try
            {
                using (var response = await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null))
                {
                    return new ServiceCallResult<TR>(response);
                }
            }
            catch (Exception ex)
            {
                // do NOT do Logger.Error or warning
                Logger.Info("[MSR] {0}", ex.Message);
//                Hub.Publish(HubEvents.ServiceCallException, ex);
                return new ServiceCallResult<TR>(ex);
            }
        }

        /// <summary>
        /// Quick light weight check if a resource exists.
        /// Just gets the header response if it exists, ie. does not download the resource at all.
        /// This method doesn't throw an ex but if it does handle a thrown ex when a 404 occurs.
        /// </summary>
        public async Task<bool?> CheckResourceExists(string uri)
        {
            if (!ConnectionManager.CanSend)
                return null;

            // create the request with the full uri
            var request = WebRequest.CreateHttp(uri);
            request.Method = HttpHelper.HEAD;

            try
            {
                using (var webResponse = await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null))
                {
                    var response = webResponse as HttpWebResponse;
                    return response != null && response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch (WebException ex)
            {
                var response = ex.Response as HttpWebResponse; // using hasn't done its thing yet so ok to access
                if (response != null)
                {
                    Logger.Debug("Status:{0}", response.StatusCode);
                    if (response.StatusCode == HttpStatusCode.NotFound)
                        return false;
                }
                return null;
            }
            catch (Exception ex)
            {
                // do NOT do Logger.Error or warning 
                Logger.Info("[{0}]{1}", ex.GetType().Name, ex.Message);
                return null;
            }
        }


        /// <summary>
        /// Post the given object (type TR) but map it into a new instance of type TP type first 
        /// (TP should be a DTO, but doesn't have to be)
        /// </summary>
        /// <typeparam name="TP">post type (usually a dto, method will convert to this type and send the conveted object)</typeparam>
        /// <typeparam name="TR">response type (usually a biz object, the response will be deserialized into this type)</typeparam>
        /// <param name="instance"></param>
        /// <returns>The new version of the object returned by the server</returns>
        public async Task<ServiceCallResult<TR>> PostDtoAsync<TR, TP>(TR instance, string url = "") 
            where TR : class
            where TP : class
        {
            var dto = Mapper.DynamicMap<TR, TP>(instance);
            return await Post<TP, TR>(url, dto);
        }

        /// <summary>
        /// Post the given object (use only if you are sure the object doesn't have any dependency issues)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="url"></param>
        /// <returns>The new version of the object returned by the server</returns>
        public Task<ServiceCallResult<T>> PostAsync(T instance, string url = "")
        {
            return Post<T, T>(url, instance);
        }

        /// <summary>
        /// Post the given object (use only if you are sure the object doesn't have any dependency issues)
        /// </summary>
        /// <typeparam name="TP">type to post</typeparam>
        /// <typeparam name="TR">type of result (i.e. deserialize into this)</typeparam>
        /// <param name="instance"></param>
        /// <param name="url"></param>
        /// <returns>The new version of the object returned by the server</returns>
        public Task<ServiceCallResult<TR>> PostAsync<TP, TR>(TP instance, string url = "")
            where TP : class // TP should be a Dto (doesn't have to be but it is recommended)
        {
            return Post<TP, TR>(url, instance);
        }


        /// <summary>
        /// Post an item from the SendQueue
        /// </summary>
        /// <typeparam name="TR">return type the returned data will be serialized into</typeparam>
        /// <param name="queueRecord">make sure the Data component is a dto (if it needs to be)</param>
        /// <returns></returns>
        public async Task<ServiceCallResult<TR>> PostFromSendQueueAsync<TR>(SendQueue queueRecord)
            where TR : class
        {
            return await Post<TR>(queueRecord.Data, queueRecord.Method, queueRecord.ContentType, queueRecord.Url);
        }

        /// <summary>
        /// Post the given object (TP and TR can be the same type, just make sure they map)
        /// </summary>
        /// <typeparam name="TP">type to post</typeparam>
        /// <typeparam name="TR">type of result (i.e. deserialize into this)</typeparam>
        /// <param name="url">additional path info beyond the baseUrl</param>
        /// <param name="instance"></param>
        /// <returns>The new version of the object returned by the server</returns>
        protected async Task<ServiceCallResult<TR>> Post<TP, TR>(string url, TP instance) 
            where TP : class // TP should be a Dto (doesn't have to be but it is recommended)
        {
            byte[] data = HttpHelper.SerializeToJsonThenByteArray(instance);
            return await Post<TR>(data, HttpHelper.POST, ContentTypes.JSON, url);
        }

        protected async Task<ServiceCallResult<TR>> Post<TR>(
            byte[] data
            , string method
            , string contentType
            , string url = "") // post usually doesn't have any extra url info
        {
            if (!ConnectionManager.CanSend)
                return new ServiceCallResult<TR>(new Exception("MakeServerRequest: No Connection"));

            var request = WebRequest.CreateHttp(GetUrlBase() + url);
            request.Method = method;
            request.ContentType = contentType;

            await AddBodyToRequest(data, request);

            var result = await MakeServerRequest<TR>(request);
            return result;
        }

    }
}