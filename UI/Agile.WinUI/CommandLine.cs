using System.Diagnostics;
using System.IO;
using System.Threading;
using Agile.Diagnostics.Logging;

namespace Agile.WinUI
{
    /// <summary>
    /// Description of CommandLine...
    /// </summary>
    public static class CommandLine
    {
        /// <summary>
        /// Get and set if the process is still working
        /// </summary>
        public static bool isStillProcessing{get;set;}
        /// <summary>
        /// Returns true if an internal error has occurred.
        /// </summary>
        public static bool internalErrorOccured { get; set; }

        /// <summary>
        /// Run a batchfile in an external process
        /// </summary>
        /// <param name="batchProgramFullPath">full path of the batch program</param>
        /// <param name="timeoutSeconds">timeout after n seconds</param>
        /// <param name="outputDataHandler">handler for the dataoutput stream</param>
        public static bool RunExternalProcessAndWaitToComplete(string batchProgramFullPath, int timeoutSeconds
            , DataReceivedEventHandler outputDataHandler)
        {
            // just let it fail if not a valid dir...for now at least
            FileInfo defaultWorkingDirectory = new FileInfo(batchProgramFullPath);
            return RunExternalProcessAndWaitToComplete(batchProgramFullPath, timeoutSeconds, outputDataHandler, defaultWorkingDirectory.DirectoryName);
        }

        /// <summary>
        /// Run a batchfile in an external process 
        /// NOTE: currently only ONE AT A TIME processing!
        /// </summary>
        /// <param name="runThis">path to the batch file to run or just anything that will run on a single command line.</param>
        /// <param name="timeoutSeconds">timeout after n seconds</param>
        /// <param name="outputDataHandler">handler for the dataoutput stream</param>
        /// <param name="workingDirectory">External process will be set with this as the working directory</param>
        public static bool RunExternalProcessAndWaitToComplete(string runThis, int timeoutSeconds
            , DataReceivedEventHandler outputDataHandler, string workingDirectory)
        {
            Logger.Info("Running [{0}] in working directory [{1}]"
                , runThis, workingDirectory);
            
            isStillProcessing = true;
            internalErrorOccured = false;

            Process process = GetNewHiddenProccess(workingDirectory);

            process.Start();
            process.OutputDataReceived += outputDataHandler;
            process.BeginOutputReadLine();

            StreamWriter writer = process.StandardInput;
            //            writer.WriteLine(string.Format("cd {0}", batchFilePath));
            writer.WriteLine(runThis);

            // Could do with some AsyncEnumerator action here, but this chunk with do for now...
            int attempts = 0;
            while (isStillProcessing) // one minute should do
            {
                if (attempts < timeoutSeconds)
                {
                    attempts++;
                    Thread.Sleep(1000);
                }
                else
                {
                    // the process has failed (well it's taken longer than expected anyway)
                    // so report error and exit
                    writer.WriteLine("TIMEOUT!! trying to process [{0}]. Process took over {1} seconds."
                        , runThis, timeoutSeconds);
                    process.Close();
                    return false;
                }
            }
            process.Close();
            if (internalErrorOccured)
                return false;
            return true;
        }


        /// <summary>
        /// Returns a new Process, setup to be hidden
        /// with all Redirections set to true. 
        /// NOT Started and events NOT hooked up!
        /// </summary>
        private static Process GetNewHiddenProccess(string workingDirectory)
        {
            var process = new Process();
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.FileName = "cmd.exe";

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.WorkingDirectory = workingDirectory;
            return process;
        }

    }
}
