using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using Agile.Common;
using Agile.Diagnostics.Logging;
using Agile.Shared;

namespace Agile.Environments
{
    /// <summary>
    /// Singleton class used to get details about which environment we are in
    /// and therefore which database to connect to.
    /// E.g. Development, UAT, Build etc.
    /// </summary>
    public static class ApplicationEnvironment
    {
        private static string name = string.Empty;

        /// <summary>
        /// What is the name of the environment, dev, prod, uat etc.
        /// We use conventions from this make decisions about the environment
        /// </summary>
        public static string Name
        {
            get {
                if (string.IsNullOrEmpty(name))
                {
                    // important to note here that the EnvVar TRUMPS any config setting.
                    // If we did it the other way round then we would need to comment out the config setting when developing and remember to uncomment when publishing
                    name = TryGetFromBuildEnvironmentVariable();
                    if (string.IsNullOrEmpty(name))
                        name = TryGetFromConfig();
                }
                return name; 
            }
            set { name = value; }
        }

        private static string TryGetFromBuildEnvironmentVariable()
        {
            try
            {
                return Environment.GetEnvironmentVariable("BuildEnvironment");
            }
            catch (Exception ex)
            {
                Logger.Warning(ex.Message);
                // do not rethrow the error, dont want ex to be bubbled up if all we are doing is implicitly trying to set the value from config.
                // its the callers responsibility to check if the environment has been correctly set, if it cares.
            }
            return string.Empty;
        }


        /// <summary>
        /// Returns true if the current environment is Production
        /// </summary>
        public static bool IsProduction
        {
            get { return Name.Equals("prod", StringComparison.InvariantCultureIgnoreCase)
                || Name.Equals("production", StringComparison.InvariantCultureIgnoreCase); }
        }


        /// <summary>
        /// Try to set the environment from the config file.
        /// </summary>
        private static string TryGetFromConfig()
        {
            try
            {
                return ConfigurationManager.AppSettings["BuildEnvironment"];
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                // do not rethrow the error, dont want ex to be bubbled up if all we are doing is implicitly trying to set the value from config.
                // its the callers responsibility to check if the environment has been correctly set, if it cares.
            }
            return string.Empty;
        }
        
    }
}