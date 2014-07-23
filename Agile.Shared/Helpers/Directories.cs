using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Agile.Shared;

namespace Agile.Common
{
    /// <summary>
    /// Summary description for Directories.
    /// </summary>
    internal class Directories
    {
        /// <summary>
        /// Constructor
        /// </summary>
        private Directories()
        {
        }

        /// <summary>
        /// Deletes the directory, even if it contains read only items.
        /// Will still fail to delete the directory if there is a file in use.
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="observer"></param>
        /// <remarks>Should return an action result but can't do that until move AgileActionResult down into Agile.Common</remarks>
        public static bool DeleteDirectory(DirectoryInfo directory, IObserver observer)
        {
            try
            {
                //Use a command line here because the normal delete fails on readonly files.
                var deleteProcess = new Process();
                deleteProcess.StartInfo.CreateNoWindow = true;
                deleteProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                deleteProcess.StartInfo.FileName = "cmd.exe";
                deleteProcess.StartInfo.Arguments = string.Format(@" /C ATTRIB -R ""{0}"" /S /D ", directory.FullName);
                deleteProcess.Start();

                Thread.Sleep(50);
                directory.Delete(true);
                directory.Refresh();
                if (directory.Exists)
                    observer.Notify(
                        string.Format(
                            "FAILED to delete directory: {0}. (tip: Probably being used by another process.)",
                            directory.FullName));
                else
                    observer.Notify(string.Format("Successfully Deleted: {0}", directory.FullName));
            }
            catch (Exception ex)
            {
                string message = string.Format("EXCEPTION occurred deleting: {0}. MESSAGE: {1}"
                                               , directory.FullName, ex.Message);
                observer.Notify(message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Opens explorer in a new window and new process.
        /// </summary>
        /// <param name="directory"></param>
        public static void DisplayInExplorer(DirectoryInfo directory)
        {
            try
            {
                //Use a command line here because the normal delete fails on readonly files.
                var deleteProcess = new Process();
                deleteProcess.StartInfo.CreateNoWindow = true;
                deleteProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                deleteProcess.StartInfo.FileName = "cmd.exe";
                deleteProcess.StartInfo.Arguments = string.Format(@"explorer {0}", directory.FullName);
                deleteProcess.Start();
            }
            catch (Exception ex)
            {
                string message =
                    string.Format("EXCEPTION occurred trying to open explorer for directory: {0}. [error:{1}]"
                                  , directory.FullName, ex.Message);
                Debug.WriteLine(message);
            }
        }

//		/// <summary>
//		/// Deletes the directory, even if it contains read only items.
//		/// Will still fail to delete the directory if there is a file in use.
//		/// </summary>
//		/// <param name="directory"></param>
//		/// <param name="observer"></param>
//		public static void DeleteDirectory(DirectoryInfo directory, IObserver observer)
//		{
//			try
//			{
//				//Use a command line here because the normal delete fails on readonly files.
//				Process deleteProcess = new Process();
//				deleteProcess.StartInfo.CreateNoWindow = true;
//				deleteProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
//				deleteProcess.StartInfo.FileName = "cmd.exe";
//				deleteProcess.StartInfo.Arguments = string.Format(@" /C rmdir /S /Q ""{0}""", directory.FullName);
//				deleteProcess.Start();
//				Thread.Sleep(1000);
//				directory.Refresh();
//				if (directory.Exists)
//					observer.Notify(string.Format("FAILED to delete directory: {0}. (tip: Probably being used by another process.)", directory.FullName));
//				else
//					observer.Notify(string.Format("Successfully Deleted: {0}", directory.FullName));
//			}
//			catch(Exception ex)
//			{
//				observer.Notify(string.Format("EXCEPTION occurred deleting: {0}. MESSAGE: {1}"
//					, directory.FullName, ex.Message));
//			} 
//		}
    }
}