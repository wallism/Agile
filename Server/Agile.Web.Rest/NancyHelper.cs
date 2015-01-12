using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Agile.Diagnostics.Logging;
using Agile.Shared;

namespace Agile.Web.Rest
{
    public class X : IFormatProvider
    {
        public object GetFormat(Type formatType)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Helper methods useful across different services
    /// </summary>
    public static class NancyHelper
    {
        public static DateTimeOffset? GetDateTimeOffsetDate(dynamic parameters)
        {
            try
            {
                string date = parameters.Date;
                if (string.IsNullOrEmpty(date))
                {
                    // only debug because this potentially gets called all the time without a date param
                    Logger.Debug("Cannot convert Date Parameter - no value");
                    return null;
                }
                // this is a hack but don't want figure out why the + is getting dropped off...
                // so add it if it is missing and there is a space
                var lastIndexOfSpace = date.LastIndexOf(" ", StringComparison.InvariantCultureIgnoreCase);
                if (!date.Contains("+") && lastIndexOfSpace > -1)
                {
                    date = date.Remove(lastIndexOfSpace, 1);
                    date = date.Insert(lastIndexOfSpace, "+");
                }

                return Safe.NullableDateTimeOffset(date);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Converting to DateTimeOffset");
                return null;
            }
        }

        public static long GetLongId(dynamic parameters)
        {
            try
            {
                long id = parameters.Id;
                return id;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return -1;
            }
        }
        public static string GetPinId(dynamic parameters)
        {
            try
            {
                string pin = parameters.Pin;
                return pin;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public static string GetStringValue(dynamic parameters)
        {
            try
            {
                string value = parameters.Value;
                return value;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return "";
            }
        }

        public static string GetStringKey(dynamic parameters)
        {
            try
            {
                string value = parameters.Key;
                return value;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return "";
            }
        }

        public static string GetStringCode(dynamic parameters)
        {
            try
            {
                string value = parameters.Code;
                return value;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return "";
            }
        }

        private static Type type;

        /// <summary>
        /// Register a bootstrapper so can use it to get the version from the running assembly
        /// </summary>
        public static void RegisterTypeToGetVersionFrom(Type bootstrapper)
        {
            type = bootstrapper;
        }

        public static string GetVersion()
        {
            try
            {
                Assembly assembly = null;
                if (type != null)
                    assembly = Assembly.GetAssembly(type);
                
                if (assembly == null)
                    assembly = Assembly.GetExecutingAssembly();

                return (assembly == null)
                    ? "noVersion"
                    : assembly.GetName().Version.ToString();                
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetVersion");
                return String.Format("GetVersion: [{0}]", ex.Message);
            }
        }

        public static string GetInfo()
        {
            return Info;
        }

        /// <summary>
        /// use this if debugging something...but remove any code that sets it.
        /// Should always be empty in PROD!
        /// </summary>
        public static string Info { get; set; }
    }
}
