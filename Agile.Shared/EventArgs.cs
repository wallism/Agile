using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agile.Shared
{
    public class EventArgs<T> : EventArgs
    {
        private T data;
        /// <summary>
        /// ctor
        /// </summary>
        public EventArgs(T data)
        {
            this.data = data;
        }
        /// <summary>
        /// Returns the data
        /// </summary>
        public T Data
        {
            get { return data; }
        }
    } 
}
