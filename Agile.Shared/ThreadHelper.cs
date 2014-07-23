using System.Threading;
using Agile.Diagnostics.Logging;

namespace Agile.Shared
{
    /// <summary>
    /// Helper methods for Threading
    /// (set to zero will leave existing setting, unless max is less than min)
    /// </summary>
    public static class ThreadHelper
    {
        public static void SetThreadPoolMin(int min, int max)
        {
            int workerThreads = 0;
            int completionPortThreads = 0;
            int minWorker, minIOC;

            // Get the current settings.
            ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);
            ThreadPool.GetMinThreads(out minWorker, out minIOC);

            Logger.Info("MaxThreads:- {0}:{1}", workerThreads, completionPortThreads);
            Logger.Info("MinThreads:- {0}:{1}", minWorker, minIOC);

            if (max == 0)
            {
                // is the current setting less than the min
                if (min >= workerThreads)
                    max = min; // make min and max the same
            }

            if (max != 0)
            {
                var result = ThreadPool.SetMaxThreads(max, max);
                Logger.Info("SetMaxThreads({0}) result={1}", max, result);
            }

            if (min != 0 && (min > minWorker)) // also only INCREASE the min, don't reduce 
            {
                var result = ThreadPool.SetMinThreads(min, min);
                Logger.Info("SetMinThreads({0}) result={1}", min, result);
            }

            // log the changes so can see it worked
            ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);
            ThreadPool.GetMinThreads(out minWorker, out minIOC);

            Logger.Info("MaxThreads:- {0}:{1}", workerThreads, completionPortThreads);
            Logger.Info("MinThreads:- {0}:{1}", minWorker, minIOC);
        }
    }
}
