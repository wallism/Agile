using System.IO;

namespace Agile.Mobile
{
    public interface ILocalStorageHelper
    {
        /// <summary>
        /// Implementation should ensure that all folders that the app needs 
        /// are created, e.g. Pics and Snds. 
        /// Should be called at startup, probably from the bootstrapper.
        /// </summary>
        void InitializeFolders();

        /// <summary>
        ///  Opens a stream to get the file contents.
        /// Don't forget to Dispose!!
        /// Do check if exists first. Can return null
        /// </summary>
        Stream OpenStream(string fileName);

        /// <summary>
        /// Save a jpeg image to local storage.
        /// </summary>
        /// <returns>true if save was successful</returns>
        bool SaveJpeg(Stream stream, string fileName);

        /// <summary>
        /// Delete the file from the apps LocalStorage
        /// </summary>
        /// <param name="fileName">Just the file name, don't pass in the full path</param>
        void Delete(string fileName);

        /// <summary>
        /// Returns true if the image exists
        /// </summary>
        bool Exists(string fileName);
        


        /// <summary>
        /// Gets the platform specific 'app' folder.
        /// This is NOT LocalStorage, putting here until a better location presents itself
        /// </summary>
        string AppFolder { get; }
    }
}