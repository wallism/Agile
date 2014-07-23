using System;
using Agile.Diagnostics.Logging;

namespace Agile.Mobile
{
    public static class LogHelper
    {
        public static void LogStartup()
        {
            if(AppDetail.Instance == null)
                throw new Exception("Need to create and set AppDetail!");

            Logger.Info("Started: {0} [{1}]"
                , AppDetail.Instance.Name
                , AppDetail.Instance.AppVersion);
        }
    }
}
