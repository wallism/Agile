using System;
using System.Diagnostics;
using System.IO;
using Agile.Common.Environments;

namespace Agile.Environments
{
    /// <summary>
    /// Summary description for AgileEnvironment.
    /// </summary>
    public abstract class AgileEnvironment
    {
        /// <summary>
        /// Suffix for the name of the backup directory.
        /// </summary>
        public const string BACKUPDIRECTORYSUFFIX = "PreviousVersion";

        private static AgileEnvironment build;
        private static AgileEnvironment dbUpdateTest;
        private static AgileEnvironment development;
        private static AgileEnvironment hotFix;
        private static AgileEnvironment hotFixUat;
        private static AgileEnvironment production;
        private static AgileEnvironment staging;
        private static AgileEnvironment training;
        private static AgileEnvironment uat;
        private DirectoryInfo backupWebDirectory;
        private DirectoryInfo releaseToWebDirectory;

        /// <summary>
        /// Gets the name of the name of the env
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// build environment
        /// </summary>
        public static AgileEnvironment BuildMachine
        {
            get
            {
                if (build == null)
                    build = new BuildEnvironment();
                return build;
            }
        }

        /// <summary>
        /// staging environment
        /// </summary>
        public static AgileEnvironment Staging
        {
            get
            {
                if (staging == null)
                    staging = new StagingEnvironment();
                return staging;
            }
        }

        /// <summary>
        /// production environment
        /// </summary>
        public static AgileEnvironment Production
        {
            get
            {
                if (production == null)
                    production = new ProductionEnvironment();
                return production;
            }
        }

        /// <summary>
        /// uat environment
        /// </summary>
        public static AgileEnvironment Uat
        {
            get
            {
                if (uat == null)
                    uat = new UatEnvironment();
                return uat;
            }
        }

        /// <summary>
        /// hotFixUat environment
        /// </summary>
        public static AgileEnvironment HotFixUat
        {
            get
            {
                if (hotFixUat == null)
                    hotFixUat = new HotFixUatEnvironment();
                return hotFixUat;
            }
        }

        /// <summary>
        /// development environment
        /// </summary>
        public static AgileEnvironment Development
        {
            get
            {
                if (development == null)
                    development = new DevelopmentEnvironment();
                return development;
            }
        }

        /// <summary>
        /// hotFix environment
        /// </summary>
        public static AgileEnvironment HotFix
        {
            get
            {
                if (hotFix == null)
                    hotFix = new HotFixEnvironment();
                return hotFix;
            }
        }

        /// <summary>
        /// training environment
        /// </summary>
        public static AgileEnvironment Training
        {
            get
            {
                if (training == null)
                    training = new TrainingEnvironment();
                return training;
            }
        }

        /// <summary>
        /// dbUpdateTest environment
        /// </summary>
        public static AgileEnvironment DbUpdateTest
        {
            get
            {
                if (dbUpdateTest == null)
                    dbUpdateTest = new DbUpdateTestingEnvironment();
                return dbUpdateTest;
            }
        }



        /// <summary>
        /// Gets the environment as a string, e.g. Returns "UAT" for UAT
        /// </summary>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Gets an agile environment by name, e.g. Development or UAT.
        /// Implemented using the Factory Pattern.
        /// </summary>
        /// <param name="environmentName">Name of the environment</param>
        public static AgileEnvironment Build(string environmentName)
        {
            if (string.IsNullOrEmpty(environmentName))
                throw new ArgumentException("Environment name is required");

            Debug.WriteLine("Setting Agile Environment to: " + environmentName);
            if (environmentName.Equals("Build", StringComparison.InvariantCultureIgnoreCase))
                return BuildMachine;

            if (environmentName.Equals("UAT", StringComparison.InvariantCultureIgnoreCase))
                return Uat;

            if (environmentName.Equals("hotFix", StringComparison.InvariantCultureIgnoreCase))
                return HotFix;

            if (environmentName.Equals("hotFixUat", StringComparison.InvariantCultureIgnoreCase))
                return HotFixUat;

            if (environmentName.Equals("prod", StringComparison.InvariantCultureIgnoreCase)
                || environmentName.Equals("production", StringComparison.InvariantCultureIgnoreCase))
                return Production;

            if (environmentName.Equals("staging", StringComparison.InvariantCultureIgnoreCase))
                return Staging;

            if (environmentName.Equals("training", StringComparison.InvariantCultureIgnoreCase))
                return Training;

            if (environmentName.Equals("Dev", StringComparison.InvariantCultureIgnoreCase)
                || environmentName.Equals("Development", StringComparison.InvariantCultureIgnoreCase))
                return Development;

            if (environmentName.Equals("DbUpdate", StringComparison.InvariantCultureIgnoreCase))
                return DbUpdateTest;


            throw new InvalidEnvironmentException("Invalid Environment: " + environmentName);
        }

        /// <summary>
        /// Clears the cache so everything gets re-initialised.
        /// </summary>
        public void ClearCache()
        {
            releaseToWebDirectory = null;
            backupWebDirectory = null;
        }


        /// <summary>
        /// Gets all available environments in an object array.
        /// </summary>
        /// <remarks>useful for the AddRange method of Items on combo boxes etc.</remarks>
        /// <returns></returns>
        public static object[] GetAllForDropDown()
        {
            return new object[]
                       {
                           "Commented out"
//                           BuildEnvironment.Name
//                           , DevelopmentEnvironment.Name
//                           , HotFixEnvironment.Name
//                           , HotFixUatEnvironment.Name
//                           , ProductionEnvironment.Name
//                           , StagingEnvironment.Name
//                           , TrainingEnvironment.Name
//                           , UatEnvironment.Name
                       };
        }
    }
}