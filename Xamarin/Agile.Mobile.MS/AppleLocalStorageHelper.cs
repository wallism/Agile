using System;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;
using Agile.Mobile.Helpers;
using Agile.Diagnostics.Logging;

namespace Agile.Mobile.MS
{
    /// <summary>
    /// Helper methods for file interactions
    /// </summary>
    /// <remarks>DUPLICATED in Android, Apples and Win platform specific projects.
    /// So change one, do a cut and paste (where possible...80%) to the others.
    /// Should compile the same, it just needs to be in a Xamarin level (because that's where DirectoryInfo etc lives)
    /// </remarks>
    public class AppleLocalStorageHelper : LocalStorageHelperBase, ILocalStorageHelper
    {
        public void InitializeFolders()
        {
            var storage = ApplicationData.Current.LocalFolder;
            
                try
                {
                    CreateDirectory(storage, ImageDirName);
                    CreateDirectory(storage, AudioDirName);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "InitializeFolders");
                }
            
        }

        private void CreateDirectory(StorageFolder storage, string name)
        {
//            if (storage.DirectoryExists(name)) 
//                return;
//            Logger.Info("Creating IsolatedStorage Dir: {0}", name);
//            storage.CreateDirectory(name);
        }

        public Stream OpenStream(string fileName)
        {
            throw new NotImplementedException();
//            var fullName = GetFileNameWithDirectory(fileName);
//            var folder = ApplicationData.Current.LocalFolder;
//                try
//                {
//                    if (!folder.FileExists(fullName))
//                        return null;
//                    Logger.Debug("Opening Stream:{0}", fullName);
//                    return folder.OpenFile(fullName, FileMode.Open);
//                }
//                catch (Exception ex)
//                {
//                    Logger.Error(ex, "OpenStream");
//                    return null;
//                }

        }

        public bool SaveJpeg(Stream stream, string fileName)
        {
            throw new NotImplementedException();
        }

        public void Delete(string fileName)
        {
//            var fullName = GetFileNameWithDirectory(fileName);
//            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
//            {
//                if (!storage.FileExists(fullName))
//                    return;
//                try
//                {
//                    Logger.Debug("Deleting {0}", fullName);
//                    storage.DeleteFile(fullName);
//                }
//                catch (Exception ex)
//                {
//                    Logger.Error(ex, "DeleteImageFromLocalStorageForApp");
//                }
//            }
        }

        public bool Exists(string fileName)
        {
            throw new NotImplementedException();
//            var fullName = GetFileNameWithDirectory(fileName);
//            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
//            {
//                return storage.FileExists(fullName);
//            }
        }

        private string appFolder;
        /// <summary>
        /// Gets the platform specific 'app' folder.
        /// This is NOT LocalStorage, putting here until a better location presents itself
        /// </summary>
        public string AppFolder
        {
            get
            {
                throw new NotImplementedException();
//                return appFolder ?? (appFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal));
            }
        }

    }
}
