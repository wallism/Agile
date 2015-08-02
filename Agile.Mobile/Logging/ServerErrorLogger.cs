using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Diagnostics.Logging;
using Agile.Mobile.Web;
using Agile.Mobile.Web.Logging;
using Agile.Shared;
using Agile.Shared.IoC;

namespace Agile.Mobile.Logging
{
    public class ServerErrorLogger : ILogger
    {
        private ISystemHttpService service;

        private ISystemHttpService Service {
            get { return service ?? (service = Container.Resolve<ISystemHttpService>());}
        }

        public void Write(string message, LogLevel level, LogCategory category, Type exType = null)
        {
            // for our first releases we want Warning as well
            if (level != LogLevel.Error 
                && level != LogLevel.Fatal 
                && level != LogLevel.Warning) // don't ever include Info, at least not without making sure all ex catches for web service calls log at a lower level!
                return; // only log major problems to the server

            try  // make absolutely certain no exs happen from logging exs
            {
                var log = new ErrorLog
                {
                    ErrorType = exType == null ? null : exType.Name
                    , Message = message.Substring(0, Math.Min(message.Length, 4096))
                    , PersonId = AppDetail.Instance.UserId
                    , Username = AppDetail.Instance.Username
                    , AppVersion = AppDetail.Instance.AppVersion
                    , OS = AppDetail.Instance.OS
                    , OSVersion = AppDetail.Instance.OSVersion
                    , Device = AppDetail.Instance.Device.Name
                    , Country = AppDetail.Instance.Country
                    , Language = AppDetail.Instance.Language
                    , CreatedUtc = AgileDateTime.UtcNow
                };
                // may throw an ex here if the error logging happens in Bootstrapper Startup 
                // because services do not get registered in the container until the end of bootstrapping
                Service.PostError(log);
            }
            catch (Exception ex)
            {
                // just catch and do nothing!
                Logger.Debug("ERROR [ServerErrorLogger] {0}", ex.Message);
            }

        }

    }
}
