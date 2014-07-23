using System;
using System.Configuration;
using System.IO;
using Agile.Common.Environments;

namespace Agile.Environments
{
    /// <summary>
    /// Summary description for DevelopmentEnvironment.
    /// </summary>
    public class DevelopmentEnvironment : AgileEnvironment
    {
        private static DirectoryInfo _rootDevDirectory;


        /// <summary>
        /// Gets the Name of the enviroment, e.g. Development or UAT
        /// </summary>
        public override string Name
        {
            get { return "Development"; }
        }

        /// <summary>
        /// Gets the root directory of all development folders
        /// </summary>
        public static DirectoryInfo RootDevelopmentDirectory
        {
            get
            {
                if (_rootDevDirectory == null)
                    LoadRootDevelopmentDirectory();
                return _rootDevDirectory;
            }
            set { _rootDevDirectory = value; }
        }


        /// <summary>
        /// Set the base development directory from the config file.
        /// </summary>
        private static void LoadRootDevelopmentDirectory()
        {
            string rootDevelopmentPath = ConfigurationManager.AppSettings["DevelopmentDirectory"];
            if (Directory.Exists(rootDevelopmentPath))
                RootDevelopmentDirectory = new DirectoryInfo(rootDevelopmentPath);
            else
            {
                if (!TrySettingDefaultDevelopmentDirectory(@"C:\Dev", @"C:\Source", @"C:\Source\Dev", @"C:\Projects"))
                    throw new ApplicationException(@"Config file does not contain a valid 'DevelopmentDirectory' app setting. 
Default directories, 'C:\Dev', 'C:\Source', 'C:\Source\Dev' and 'C:\Projects' also do not exist.");
            }
        }
        /// <summary>
        /// Sets one of the default dev directories if they exist.
        /// Returns false if none of the given directories (as strings) exist.
        /// </summary>
        /// <param name="directories"></param>
        /// <returns></returns>
        private static bool TrySettingDefaultDevelopmentDirectory(params string[] directories)
        {
            foreach (string directory in directories)
            {
                if (Directory.Exists(directory))
                {
                    RootDevelopmentDirectory = new DirectoryInfo(directory);
                    return true;
                }
            }
            return false;
        }
    }
}