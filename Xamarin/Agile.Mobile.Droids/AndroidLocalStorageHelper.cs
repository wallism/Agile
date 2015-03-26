using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using Agile.Diagnostics.Logging;
using Agile.Mobile;
using Agile.Mobile.Helpers;
using Android.Graphics;


namespace Acoustie.Helpers
{

    /// <summary>
    /// Helper methods for file interactions
    /// </summary>
    /// <remarks>DUPLICATED in Android, Apples and Win platform specific projects.
    /// So change one, do a cut and paste (where possible...80%) to the others.
    /// Should compile the same, it just needs to be in a Xamarin level (because that's where DirectoryInfo etc lives)
    /// </remarks>
    public class AndroidLocalStorageHelper : LocalStorageHelperBase, ILocalStorageHelper
    {
        /// <summary>
        /// ctor
        /// </summary>
        public AndroidLocalStorageHelper()
        {
            InitializeFolders();
        }


        public void InitializeFolders()
        {
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
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
        }

        private void CreateDirectory(IsolatedStorageFile storage, string name)
        {
            if (storage.DirectoryExists(name)) 
                return;
            Logger.Info("Creating IsolatedStorage Dir: {0}", name);
            storage.CreateDirectory(name);
        }

        /// <summary>
        ///  Opens a stream to get the file contents.
        /// Don't forget to Dispose!!
        /// And do check if exists first, can return null
        /// </summary>
        public Stream OpenStream(string fileName)
        {
            var fullName = GetFileNameWithDirectory(fileName);
            
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                try
                {
                    if(! storage.FileExists(fullName))
                        return null;
                    Logger.Debug("Opening Stream:{0}", fullName);
                    return storage.OpenFile(fullName, FileMode.Open);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "OpenStream");
                    return null;
                }
            }
        }


        public bool SaveJpeg(Stream stream, string fileName)
        {
            var fullName = GetFileNameWithDirectory(fileName);
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                // delete it if it already exists.
                Delete(fullName);
                try
                {
                    using (var bitmap = BitmapFactory.DecodeStream(stream))
                    {
                        Logger.Info("SaveImage: {0}", fullName);
                        using (var fos = storage.CreateFile(fullName))
                        {
                            Logger.Debug("PIC-Compress (which saves): {0}", fos.Name);
                            var result = bitmap.Compress(Bitmap.CompressFormat.Jpeg, 90, fos);
                            Logger.Debug("PIC-Compress:{0}", result);
                            return result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "SaveImageToLocalStorageForApp [{0}]", fullName);
                    return false;
                }
            }
        }

        /// <summary>
        /// Delete the file from the apps LocalStorage
        /// </summary>
        /// <param name="fileName">Just the file name, don't pass in the full path</param>
        public void Delete(string fileName)
        {
            var fullName = GetFileNameWithDirectory(fileName);
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!storage.FileExists(fullName)) 
                    return;
                try
                {
                    Logger.Debug("Deleting {0}", fullName);
                    storage.DeleteFile(fullName);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "DeleteImageFromLocalStorageForApp");
                }
            }
        }


        /// <summary>
        /// Returns true if the image exists
        /// </summary>
        public bool Exists(string fileName)
        {
            var fullName = GetFileNameWithDirectory(fileName);
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                return storage.FileExists(fullName);
            }
        }

        private string appFolder;
        /// <summary>
        /// Gets the platform specific 'app' folder.
        /// This is NOT LocalStorage, putting here until a better location presents itself
        /// </summary>
        public string AppFolder {
            get
            {
                return appFolder ?? (appFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal));
            }
        }
    }
}