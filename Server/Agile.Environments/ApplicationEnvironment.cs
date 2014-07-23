using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using Agile.Common;
using Agile.Common.Environments;
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
        /// <summary>
        /// Static so it doesn't keep getting re-initialised
        /// </summary>
        private static AgileEnvironment agileEnvironment;

        /// <summary>
        /// Returns true if the agile environment has been initialized
        /// </summary>
        public static bool IsEnvironmentInitialized
        {
            get { return agileEnvironment != null; }
        }

        private static string showEnvironmentWarning;       
        
        /// <summary>
        /// Returns true if the current environment is the test environment 
        /// (for testing dbupdate)
        /// </summary>
        public static bool IsTestEnvironment
        {
            get
            {
                if (agileEnvironment == null)
                    return false;
                return agileEnvironment is DbUpdateTestingEnvironment;
            }
        }

        /// <summary>
        /// Returns true if the current environment is Training
        /// </summary>
        public static bool IsTraining
        {
            get { return agileEnvironment is TrainingEnvironment; }
        }

        /// <summary>
        /// Returns true if the current environment is HotFix
        /// </summary>
        public static bool IsHotFix
        {
            get { return agileEnvironment is HotFixEnvironment; }
        }

        /// <summary>
        /// Returns true if the current environment is UAT
        /// </summary>
        public static bool IsUAT
        {
            get { return agileEnvironment is UatEnvironment; }
        }

        /// <summary>
        /// Returns true if the current environment is UAT
        /// </summary>
        public static bool IsHotFixUAT
        {
            get { return agileEnvironment is HotFixUatEnvironment; }
        }

        /// <summary>
        /// Returns true if the current environment is Production
        /// </summary>
        public static bool IsProduction
        {
            get { return agileEnvironment is ProductionEnvironment; }
        }

        /// <summary>
        /// Returns true if the current environment is Staging
        /// </summary>
        public static bool IsStaging
        {
            get { return agileEnvironment is StagingEnvironment; }
        }

        /// <summary>
        /// Returns true if the current environment is Development
        /// </summary>
        public static bool IsDevelopment
        {
            get { return agileEnvironment is DevelopmentEnvironment; }
        }

        /// <summary>
        /// Get the environment that is to be updated.
        /// </summary>
        public static AgileEnvironment Environment
        {
            get
            {
                if (agileEnvironment == null)
                {
                    TrySetFromConfig();
                    if (!IsEnvironmentInitialized)
                        throw new Exception("need to set 'AgileEnvironment' in .config.");
                }
                return agileEnvironment;
            }
            set { agileEnvironment = value; }
        }

        /// <summary>
        /// Returns true if an environment warning should be displayed.
        /// </summary>
        /// <remarks>If it is the production system and we are in the production environment 
        /// we do NOT want to display a warning. However if for some reason we run a local instance
        /// but connect to production we want the warning to appear.</remarks>
        public static bool ShowEnvironmentWarning
        {
            get
            {
                if (showEnvironmentWarning == null)
                    showEnvironmentWarning = ConfigurationManager.AppSettings.Get("ShowEnvironmentWarning");
                return showEnvironmentWarning.ToLower() == "true";
            }
        }

        /// <summary>
        /// Manually set the environment
        /// </summary>
        public static void ManuallySetEnvironment(AgileEnvironment environmentOverride)
        {
            agileEnvironment = environmentOverride;
        }

        /// <summary>
        /// Try to set the environment from the config file.
        /// </summary>
        private static void TrySetFromConfig()
        {
            try
            {
                var environment = ConfigurationManager.AppSettings["AgileEnvironment"];

                if (!string.IsNullOrEmpty(environment))
                {
                    agileEnvironment = AgileEnvironment.Build(environment);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                // do not rethrow the error, dont want ex to be bubbled up if all we are doing is implicitly trying to set the value from config.
                // its the callers responsibility to check if the environment has been correctly set, if it cares.
            }
        }
        
        /// <summary>
        /// Clears the cache of the current environment setting.
        /// </summary>
        public static void ClearCache()
        {
            agileEnvironment = null;
        }
        
    }
}