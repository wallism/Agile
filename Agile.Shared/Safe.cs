using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Agile.Diagnostics.Logging;

namespace Agile.Shared
{
    public static partial class Safe
    {
        public static DateTimeOffset DateTimeOffset(object value, DateTimeOffset defaultValue)
        {
            if (value == null)
                return defaultValue;
            try
            {
                if (value is string)
                {
                    return System.DateTimeOffset.Parse(value.ToString());
                }
                if (value is DateTime)
                    return new DateTimeOffset((DateTime) value);
                Logger.Warning("Don't know how to convert Type:{0} to a DateTimeOffset", value.GetType().Name);
                return defaultValue;

            }
            catch (Exception ex)
            {
                Logger.Info("couldn't convert to a DateTimeOffset: {0} [{1}]", value, ex.Message);
                return defaultValue;
            }
        }

        public static DateTimeOffset? NullableDateTimeOffset(object value, DateTimeOffset? defaultValue = null)
        {
            if (value == null)
                return defaultValue;
            try
            {
                if (value is string)
                {
                    return System.DateTimeOffset.Parse(value.ToString());
                }
                if (value is DateTime)
                    return new DateTimeOffset((DateTime)value);
                Logger.Warning("Don't know how to convert Type:{0} to a DateTimeOffset", value.GetType().Name);
                return defaultValue;

            }
            catch (Exception ex)
            {
                Logger.Info("couldn't convert to a nullable DateTimeOffset: {0} [{1}]", value, ex.Message);
                return defaultValue;
            }
        }

        /// <summary>
        /// Returns the given object as an integer if it can be parsed as an int.
        /// i.e. If the value is null or non-numeric this method will return 0.
        /// </summary>
        public static int Int(object value)
        {
            if (value == null)
                return 0;

            try
            {
                return Convert.ToInt32(value);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Safely converts the given value to a byte, returning the default value if it cannot convert to byte.
        /// </summary>
        public static byte Byte(object value, byte defaultValue = 0)
        {
            if (value == null)
                return defaultValue;
            try
            {
                return Convert.ToByte(value);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return defaultValue;
            }
        }

        /// <summary>
        /// Returns the given value as T if it is 'safe'.
        /// i.e. If the value is null or not castable to T
        /// this method will return null.
        /// </summary>
        public static T? Nullable<T>(object value) where T : struct
        {
            if (value == null)
                return null;

            try
            {
                if (value.GetType().Name == "DBNull")
                    return null;
                return (T)value;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the given value as an integer if it is 'safe'.
        /// i.e. If the value is null or non-numeric this method will 
        /// return the given default value.
        /// </summary>
        /// <param name="value">The value to return if it is a safe int.</param>
        /// <param name="defaultValue">Default value to use if the value is not 'safe'</param>
        /// <returns>The default value if the 'value' is not safe, otherwise the 'value' as an int.</returns>
        public static int Int(object value, int defaultValue)
        {
            if (value == null)
                return defaultValue;
            if (!Common.AreAllCharactersNumeric(value.ToString().Trim()))
                return defaultValue;

            return int.Parse(value.ToString());
        }

        /// <summary>
        /// Returns the given value as an short if it is 'safe'.
        /// i.e. If the value is null or non-numeric this method will return 0.
        /// </summary>
        /// <param name="value">The value to return if it is a safe short.</param>
        /// <returns>0 if the value is not safe, otherwise the value as an short.</returns>
        public static short Short(object value)
        {
            return Short(value, 0);
        }

        /// <summary>
        /// Returns the given value as an short if it is 'safe'.
        /// i.e. If the value is null or non-numeric this method will 
        /// return the given default value.
        /// </summary>
        /// <param name="value">The value to return if it is a safe short.</param>
        /// <param name="defaultValue">Default value to use if the value is not 'safe'</param>
        /// <returns>The default value if the 'value' is not safe, otherwise the 'value' as an short.</returns>
        public static short Short(object value, short defaultValue)
        {
            if (value == null)
                return defaultValue;
            if (!Common.IsNumeric(value.ToString().Trim()))
                return defaultValue;

            int number = int.Parse(value.ToString());
            if ((number < -32768) || (number > 32768))
                return defaultValue;

            return short.Parse(value.ToString());
        }

        /// <summary>
        /// Returns the given value as an long if it is 'safe'.
        /// i.e. If the value is null or non-numeric this method will return 0.
        /// </summary>
        /// <param name="value">The value to return if it is a safe long.</param>
        /// <returns>0 if the value is not safe, otherwise the value as an long.</returns>
        public static long Long(object value)
        {
            return Long(value, 0);
        }

        /// <summary>
        /// Returns the given value as an long if it is 'safe'.
        /// i.e. If the value is null or non-numeric this method will 
        /// return the given default value.
        /// </summary>
        /// <param name="value">The value to return if it is a safe long.</param>
        /// <param name="defaultValue">Default value to use if the value is not 'safe'</param>
        /// <returns>The default value if the 'value' is not safe, otherwise the 'value' as an long.</returns>
        public static long Long(object value, long defaultValue)
        {
            if (value == null)
                return defaultValue;
            if (!Common.AreAllCharactersNumeric(value.ToString().Trim()))
                return defaultValue;

            return long.Parse(value.ToString());
        }

        /// <summary>
        /// Returns the given value as a double if it is 'safe'.
        /// i.e. If the value is null or non-numeric this method will return 0.
        /// </summary>
        /// <param name="value">The value to return if it is a safe double.</param>
        /// <returns>0 if the value is not safe, otherwise the value as a double.</returns>
        public static double Double(object value)
        {
            return Double(value, 0);
        }

        /// <summary>
        /// Returns the given value as a double if it is 'safe'.
        /// i.e. If the value is null or non-numeric this method will 
        /// return the given default value.
        /// </summary>
        /// <param name="value">The value to return if it is a safe double.</param>
        /// <param name="defaultValue">Default value to use if the value is not 'safe'</param>
        /// <returns>The default value if the 'value' is not safe, otherwise the 'value' as a double.</returns>
        public static double Double(object value, double defaultValue)
        {
            if (value == null)
                return defaultValue;
            if (!Common.IsNumeric(value.ToString().Trim()))
                return defaultValue;

            return double.Parse(value.ToString());
        }

        /// <summary>
        /// Returns the given value as a float if it is 'safe'.
        /// i.e. If the value is null or non-numeric this method will return 0.
        /// </summary>
        /// <param name="value">The value to return if it is a safe float.</param>
        /// <returns>0 if the value is not safe, otherwise the value as a float.</returns>
        public static float Float(object value)
        {
            return Float(value, 0);
        }

        /// <summary>
        /// Returns the given value as a float if it is 'safe'.
        /// i.e. If the value is null or non-numeric this method will 
        /// return the given default value.
        /// </summary>
        /// <param name="value">The value to return if it is a safe float.</param>
        /// <param name="defaultValue">Default value to use if the value is not 'safe'</param>
        /// <returns>The default value if the 'value' is not safe, otherwise the 'value' as a float.</returns>
        public static float Float(object value, float defaultValue)
        {
            if (value == null)
                return defaultValue;
            if (!Common.IsNumeric(value.ToString().Trim()))
                return defaultValue;

            return float.Parse(value.ToString());
        }

        /// <summary>
        /// Returns the given value as a decimal if it is 'safe'.
        /// i.e. If the value is null or non-numeric this method will return 0.
        /// </summary>
        /// <param name="value">The value to return if it is a safe decimal.</param>
        /// <returns>0 if the value is not safe, otherwise the value as a decimal.</returns>
        public static decimal Decimal(object value)
        {
            return Decimal(value, 0);
        }

        /// <summary>
        /// Returns the given value as a decimal if it is 'safe'.
        /// i.e. If the value is null or non-numeric this method will 
        /// return the given default value.
        /// </summary>
        /// <param name="value">The value to return if it is a safe decimal.</param>
        /// <param name="defaultValue">Default value to use if the value is not 'safe'</param>
        /// <returns>The default value if the 'value' is not safe, otherwise the 'value' as a decimal.</returns>
        public static decimal Decimal(object value, decimal defaultValue)
        {
            if (value == null)
                return defaultValue;
            if (!Common.IsNumeric(value.ToString().Trim()))
                return defaultValue;

            return decimal.Parse(value.ToString());
        }

        /// <summary>
        /// If the value is null this method will return an empty string
        /// OR if it 'is a string' then it returns the value
        /// OTHERWISE it returns the objects .ToString method.
        /// </summary>
        /// <param name="value">The value to return if it is a safe string.</param>
        /// <returns>string.Empty if the value is not safe, otherwise the value as an string.</returns>
        public static string String(object value)
        {
            if (value == null)
                return string.Empty;
            if (value is string)
                return (string)value;

            return value.ToString();
        }

        public static bool Bool(object value, bool defaultValue = false)
        {
            if (value == null)
                return defaultValue;
            
            bool parsed;
            var result = bool.TryParse(value.ToString(), out parsed);
            return result ? parsed : defaultValue;
        }

        /// <summary>
        /// If value is a valid guid it gets returned as a guid,
        /// OTHERWISE it returns Guid.Empty
        /// </summary>
        public static Guid Guid(object value)
        {
            try
            {
                // 32 contiguous digits is the minimum (my understanding of it...)
                if (value == null || value.ToString().Length < 32)
                    return System.Guid.Empty;

                return new Guid(String(value));
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return System.Guid.Empty;
            }
        }
    }
}
