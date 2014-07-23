using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Diagnostics.Logging;
using Agile.Mobile.Web;
using Agile.Mobile.Web.Logging;
using Agile.Shared;

namespace Agile.Mobile.Logging
{
    public class ServerErrorLogger : ILogger
    {
        private IMainHttpService service;

        private IMainHttpService Service {
            get { return service ?? (service = new MainHttpService());}
        }

        public void Write(string message, LogLevel level, LogCategory category, Type exType = null)
        {
            if (level != LogLevel.Error)
                return; // only log errors like this


            try  // make absolutely certain no exs happen from logging exs
            {
                var log = new ErrorLog
                {
                    ErrorType = exType == null ? null : exType.Name
                    , Message = message
                    , PersonId = AppDetail.Instance.UserId
                    , Username = AppDetail.Instance.Username
                    , AppVersion = AppDetail.Instance.AppVersion
                    , OS = AppDetail.Instance.OS
                    , OSVersion = AppDetail.Instance.OSVersion
                    , Device = AppDetail.Instance.Device
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
                Logger.Info(ex.Message);
            }

        }

    }
}
