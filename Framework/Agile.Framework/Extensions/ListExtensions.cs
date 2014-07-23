using System;
using System.Collections.Generic;

namespace Agile.Framework
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Don't put into the 'logical' namespace - .Silverlight.Extensions -
    /// By leaving it in Agile.Framework we don't need to add any Silverlight only using 
    /// statements to our Biz objects.</remarks>
    public static class ListExtensions
    {
        /// <summary>
        /// Returns a matching item from the list based on the given Predicate.
        /// </summary>
        /// <remarks>Silverlight generic lists don't have a Find implementation.
        /// This gives us that 'feature' in our Silverlight apps.</remarks>
        public static T Find<T>(this List<T> items, Predicate<T> find) 
            where T : class
        {
            foreach (T item in items)
            {
                if(find(item))
                    return item;
            }
            return null;
        }

    }
}
