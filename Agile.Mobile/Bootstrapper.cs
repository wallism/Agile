using System;
using System.Collections.Generic;
using Agile.Diagnostics.Loggers;
using Agile.Diagnostics.Logging;
using Agile.Mobile.Logging;
using Agile.Mobile.Web;
using Agile.Shared.IoC;

namespace Agile.Mobile
{
    /// <summary>
    /// Common service registration for all apps. Implement app specifc as well if needed
    /// </summary>
    /// <remarks>made these non static and abstract so
    /// can put on abstract methods that must be implemented 
    /// platform specific.</remarks>
    public abstract class Bootstrapper
    {
        /// <summary>
        /// Call Startup early in the app startup process
        /// </summary>
        public virtual void Startup()
        {
            Logger.Info("Agile.Bootstrapper");
            HttpHelper.RegisterHubSubscriptions();

            RegisterFileHelper();
        }

        protected void AddServerErrorLogger()
        {
            var serverErrorLogger = new ServerErrorLogger();
            Logger.AddLogger(serverErrorLogger);
        }

        protected abstract void RegisterFileHelper();
    }
}
