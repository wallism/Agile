using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Diagnostics.Logging;
using Agile.Mobile.Web.Logging;

namespace Agile.Mobile.Web
{
    public interface ISystemHttpService : IHttpServiceBase<ErrorLog>
    {
        void PostError(ErrorLog log);
        Task<string> GetVersion();
    }

    /// <summary>
    /// Hits the default nancy module in the server.
    /// Used for things like error logging, getting the server version etc.
    /// </summary>
    public class SystemHttpService : HttpServiceBase<ErrorLog>, ISystemHttpService
    {
        /// <summary>
        /// ctor
        /// </summary>
        public SystemHttpService(string serviceName)
            : base(serviceName)
        {            
        }

        // Main module does not change the path base, so no override for GeUrlBase...

        /// <summary>
        /// Post the error, nothing done with the return value
        /// todo: change server and client to be fire and forget
        /// </summary>
        public void PostError(ErrorLog log)
        {
            PostAsync(log, "/logerror");
        }

        public async Task<string> GetVersion()
        {
            var result = await GET<string>(string.Format("{0}/version", GetUrlBase()));
            return result.WasSuccessful ? result.Value : "Error";
        }
    }
}
