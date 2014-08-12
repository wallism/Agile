using System;
using System.IO;
using Agile.Diagnostics.Logging;

namespace Agile.Mobile.Tests
{
    /// <summary>
    /// Helper methods for file interactions
    /// </summary>
    /// <remarks>link to this Android file from iOS and WP projects.
    /// Should compile the same, it just needs to be in a Xamarin level (because that's where DirectoryInfo etc lives)
    /// project but don't want to re-implement for every platform</remarks>
    public class LocalStorageHelperWindows : ILocalStorageHelper
    {
        /// <summary>
        /// ctor
        /// </summary>
        public LocalStorageHelperWindows()
        {
            InitializeFolders();
        }


        public void InitializeFolders()
        {
            AppFolder = Environment.CurrentDirectory;
        }
        public bool SaveJpeg(Stream stream, string fileName)
        {
            // again, don't really need for unit testing (not atm anyway)
            throw new NotImplementedException();
        }

        /// <summary>
        ///  Opens a stream to get the file contents.
        /// Don't forget to Dispose!!
        /// And do check if exists first, can return null
        /// </summary>
        /// <param name="fileName">name of the file, including path beyond the BasePath. Do NOT include \ prefix </param>
        public Stream OpenStream(string fileName)
        {
            if (! Exists(fileName))
                return null;

            var path = string.Format("{0}\\{1}", AppFolder, fileName);
            
            try
            {
                Logger.Debug("Opening Stream:{0}", fileName);
                return File.Open(path, FileMode.Open);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "OpenStream");
                return null;
            }
            
        }


        /// <summary>
        /// Delete the file from the apps LocalStorage
        /// </summary>
        /// <param name="fileName">name of the file, including path beyond the BasePath. Do NOT include \ prefix </param>
        public void Delete(string fileName)
        {
            if (! Exists(fileName)) 
                return;

            var path = string.Format("{0}\\{1}", AppFolder, fileName);
            try
            {
                Logger.Debug("Deleting {0}", fileName);
                File.Delete(path);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "DeleteImage");
            }
            
        }

        /// <summary>
        /// Returns true if the file exists
        /// </summary>
        /// <param name="fileName">name of the file, including path beyond the BasePath. Do NOT including \ prefix </param>
        public bool Exists(string fileName)
        {
            var path = string.Format("{0}\\{1}", AppFolder, fileName);
            var result = File.Exists(path);
            if(!result)
                Logger.Info("Does not exist [{0}]", path);
            return result;
        }

        public string AppFolder { get; private set; }
    }
}