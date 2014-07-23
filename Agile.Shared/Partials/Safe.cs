using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agile.Shared
{
    /// <summary>
    /// Safe conversions.
    /// </summary>
    /// <remarks>for full framework</remarks>
    public static partial class Safe
    {

        /// <summary>
        /// Returns the given value as an integer if it is 'safe'.
        /// i.e. If the value is null or non-numeric this method will return 0.
        /// </summary>
        public static T? NullableDB<T>(object value) where T : struct
        {
            if (value == null || value is DBNull)
                return new T?();

            return (T)value;
        }
    }
}
