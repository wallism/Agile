using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Agile.Framework.Clients.Silverlight
{
    /// <summary>
    /// Generic 'completed event args' for async operations
    /// </summary>
    [DebuggerStepThrough]
    public class AsyncOperationCompletedEventArgs<T> : AsyncCompletedEventArgs
    {
        private readonly object[] results;

        public AsyncOperationCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        public T Result
        {
            get
            {
                RaiseExceptionIfNecessary();
                return ((T)(results[0]));
            }
        }
    }

}
