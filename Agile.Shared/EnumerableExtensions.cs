using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agile.Shared
{
    /// <summary>
    /// taken from Microsoft.Practices.ObjectBuilder2
    /// </summary>
    public static class EnumerableExtensions
    {
        public static void ForEach<TItem>(this IEnumerable<TItem> sequence, Action<TItem> action)
        {
            foreach (TItem item in sequence)
            {
                action(item);
            }
        }

    }
}
