using System;

namespace Agile.Mobile.Helpers
{
    /// <summary>
    /// Has some shared xPlatform code
    /// </summary>
    public abstract class LocalStorageHelperBase
    {
        public const string ImageDirName = "Pics";
        public const string AudioDirName = "Snds";

        public const string MPEG4Extension = ".m4a";

        /// <summary>
        /// Very important to use this method in all implementations method of ILocalStorageHelper
        /// to ensure that the correct path is used.
        /// The file extension is important in determining which folder the file is in.
        /// If there is no match for the ext then the file just goes in the root Local Storage folder for the app.
        /// </summary>
        public string GetFileNameWithDirectory(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName is required. [GetFileNameWithDirectory]");

            fileName = fileName.ToLower();
            var dir = string.Empty;

            // images
            if (fileName.EndsWith(".jpg")|| fileName.EndsWith(".jpeg"))
                dir = ImageDirName;
                // audio
            else if (fileName.EndsWith(".3gpp") || fileName.EndsWith(MPEG4Extension))
                dir = AudioDirName;
            
            var fullName = string.Format("{0}{1}{2}"
                , dir
                , string.IsNullOrEmpty(dir) || fileName.StartsWith("\\") ? "" : "\\" // if there is a dir then we need to have a backslash
                , fileName);
            return fullName;
        }
    }
}
