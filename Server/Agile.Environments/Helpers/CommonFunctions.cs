using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Agile.Common
{
    /// <summary>
    /// A collection of frequently used functions.
    /// All functions are static.
    /// </summary>
    public class CommonFunctions
    {
        #region Delegates

        /// <summary>
        /// Delegate for simple functions that take no parameters
        /// and have no return value.
        /// </summary>
        public delegate void SimpleFunction();

        /// <summary>
        /// Delegate for a method that takes a string parameter and returns a string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public delegate string StringDelegate(string value);

        #endregion

        private static readonly Regex _alphaNumericPattern = new Regex("[^a-zA-Z0-9]");
        private static readonly Regex _alphaNumericPatternAllowSpaces = new Regex("[^a-zA-Z0-9 ]");
        private static readonly Regex _alphaPattern = new Regex(@"[^a-zA-Z]");
        private static readonly Regex _alphaPatternAllowSpaces = new Regex(@"[^a-zA-Z ]");

        private static readonly Regex _numericPattern = new Regex(@"^\d+$");

        /// <summary>
        /// Check that the give string contains all numeric characters.
        /// </summary>
        /// <param name="check">check this string for all numeric characers.</param>
        /// <returns>True if the string contains ALL numeric characters.</returns>
        /// <remarks>Negative numbers will also return true.</remarks>
        public static bool AreAllCharactersNumeric(string check)
        {
            ArgumentValidation.CheckForNullReference(check, "check");
            return _numericPattern.IsMatch(check);
        }

        /// <summary>
        /// Check that the give string contains all numeric characters.
        /// </summary>
        /// <param name="check">check this object and see if it is a number.</param>
        /// <returns>True if the string contains ALL numeric characters.</returns>
        /// <remarks>Negative numbers will also return true.
        /// [Converts to a string and then calls the other override.]</remarks>
        public static bool AreAllCharactersNumeric(object check)
        {
            return AreAllCharactersNumeric(check.ToString());
        }

        /// <summary>
        /// Returns true if the given object is numeric (or its ToString mehtod is numeric).
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        public static bool IsNumeric(object check)
        {
            string asString = RemoveNegativeSign(check.ToString());
            asString = RemoveDecimalPoint(asString);
            return AreAllCharactersNumeric(asString);
        }

        /// <summary>
        /// Removes the negative sign from a number if it has one, otherwise just returns the number as is.
        /// ie. Changes -932 to 932
        /// </summary>
        /// <param name="number">number to remove the negative sign from.</param>
        /// <returns></returns>
        private static string RemoveNegativeSign(string number)
        {
            ArgumentValidation.CheckForNullReference(number, "number");

            if (number.StartsWith("-"))
                return number.Remove(0, 1);
            return number;
        }

        /// <summary>
        /// Removes the decimal point from a number if it has one, otherwise just returns the number as is.
        /// ie. Changes 5.5 to 55
        /// </summary>
        /// <param name="number">number to remove the decimal point from.</param>
        /// <returns></returns>
        private static string RemoveDecimalPoint(string number)
        {
            ArgumentValidation.CheckForNullReference(number, "number");

            if (Strings.Contains(number, "."))
                return Strings.RemoveFirstInstanceOf(".", number);
            return number;
        }

        /// <summary>
        /// Checks if the given string contains all Alpha characters.
        /// i.e. A-Z
        /// </summary>
        /// <param name="check">Check this string for all alpha characters.</param>
        /// <returns>true if all characters are alpha characters.</returns>
        public static bool IsAlpha(string check)
        {
            return IsAlpha(check, false);
        }

        /// <summary>
        /// Checks if the given string contains all Alpha characters.
        /// i.e. A-Z
        /// </summary>
        /// <param name="check">Check this string for all alpha characters.</param>
        /// <param name="allowSpaces">set to true if spaces are also allowed in the the string</param>
        /// <returns>true if all characters are alpha characters.</returns>
        public static bool IsAlpha(string check, bool allowSpaces)
        {
            ArgumentValidation.CheckForNullReference(check, "check");
            if (allowSpaces)
                return !_alphaPatternAllowSpaces.IsMatch(check);
            return !_alphaPattern.IsMatch(check);
        }

        /// <summary>
        /// Checks if the given string containss all Alpha Numeric characters.
        /// Warning: Negative numbers will return false (ie "-" is NOT alpha or numeric)
        /// </summary>
        /// <param name="check">Check this string for all alpha numeric characters.</param>
        /// <returns>true if all characters are alpha numeric.</returns>
        /// <remarks>Numeric is not camel cased because of fx cop rule 'CompoundWordsShouldBeCasedCorrectly'</remarks>
        public static bool IsAlphanumeric(string check)
        {
            return IsAlphanumeric(check, false);
        }

        /// <summary>
        /// Checks if the given string containss all Alpha Numeric characters.
        /// Warning: Negative numbers will return false (ie "-" is NOT alpha or numeric)
        /// </summary>
        /// <param name="check">Check this string for all alpha numeric characters.</param>
        /// <param name="allowSpaces">set to true if spaces are also allowed in the the string</param>
        /// <returns>true if all characters are alpha numeric.</returns>
        /// <remarks>Numeric is not camel cased because of fx cop rule 'CompoundWordsShouldBeCasedCorrectly'</remarks>
        public static bool IsAlphanumeric(string check, bool allowSpaces)
        {
            ArgumentValidation.CheckForNullReference(check, "check");
            if (allowSpaces)
                return !_alphaNumericPatternAllowSpaces.IsMatch(check);
            return !_alphaNumericPattern.IsMatch(check);
        }

        /// <summary>
        /// Check if theValue string is a currency
        /// </summary>
        /// <param name="theValue">The string contains currency value</param>
        /// <returns></returns>
        public static bool IsCurrency(string theValue)
        {
            ArgumentValidation.CheckForNullReference(theValue, "theValue");
            double result = 0;
            return Double.TryParse(theValue,
                                   NumberStyles.Currency,
                                   null, out result);
        }

        /// <summary>
        /// Gets a string representation of a currency value - done to
        /// ensure consistency across all apps
        /// </summary>
        /// <param name="value">the value</param>
        /// <returns>the value as a string</returns>
        public static string FormatCurrency(double value)
        {
            return FormatCurrency((decimal) value);
        }


        /// <summary>
        /// Gets a string representation of a currency value - done to
        /// ensure consistency across all apps
        /// </summary>
        /// <param name="value">the value</param>
        /// <returns>the value as a string</returns>
        /// <remarks>If the value is over $10000, and does not include
        /// a cents portion, then the resulting string does not 
        /// include cents. i.e: 50000.00 will return $50000 not $50000.00</remarks>
        public static string FormatCurrency(decimal value)
        {
            if (Math.Abs(value) < 10000)
                return value.ToString("$#0.00", CultureInfo.CurrentCulture);
            else if (Math.Floor((double) value) != (double) value)
                return value.ToString("$#0.00", CultureInfo.CurrentCulture);
            else
                return value.ToString("$#0", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Gets a string representation of a currency value
        /// including the cents portion.
        /// </summary>
        /// <param name="value">the value</param>
        /// <returns>the value as a string</returns>
        public static string FormatLongCurrency(decimal value)
        {
            return value.ToString("c", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// get the leftmost n characters of a string, or less if the string
        /// is less
        /// </summary>
        /// <param name="value">the full string</param>
        /// <param name="length">the number of characters we want</param>
        /// <returns>the leftmost n characters </returns>
        public static string Left(string value, int length)
        {
            ArgumentValidation.CheckForNullReference(value, "value");
            if (value.Length < length)
                return value;
            else
                return value.Substring(0, length);
        }

        /// <summary>
        /// gets the rightmost n chracters of a string
        /// </summary>
        /// <param name="value">the full string</param>
        /// <param name="length">the number of characters we want</param>
        /// <returns>the rightmost n characters</returns>
        public static string Right(string value, int length)
        {
            ArgumentValidation.CheckForNullReference(value, "value");
            if (value.Length <= length)
                return value;
            else
                return value.Substring(value.Length - length, length);
        }

        /// <summary>
        /// This trims a value to the bounds given
        /// </summary>
        /// <param name="value">the raw value</param>
        /// <param name="minimum">the minimum allowed value</param>
        /// <param name="maximum">the maximum allowed value</param>
        /// <returns>the value, trimmed to the min, max</returns>
        public static int Bound(int value, int minimum, int maximum)
        {
            return (value < minimum) ? minimum :
                (value > maximum) ? maximum :
                    value;
        }

        /// <summary>
        /// Returns the given value as an integer if it is 'safe'.
        /// i.e. If the value is null or non-numeric this method will return 0.
        /// </summary>
        public static T? SafeNullable<T>(object value) where T : struct
        {
            if (value == null || value is DBNull)
                return new T?();
            return (T) value;
        }

        /// <summary>
        /// Returns the given value as an integer if it is 'safe'.
        /// i.e. If the value is null or non-numeric this method will return 0.
        /// </summary>
        /// <param name="value">The value to return if it is a safe int.</param>
        /// <returns>0 if the value is not safe, otherwise the value as an int.</returns>
        public static int SafeInt(object value)
        {
            return SafeInt(value, 0);
        }

        /// <summary>
        /// Returns the given value as an integer if it is 'safe'.
        /// i.e. If the value is null or non-numeric this method will 
        /// return the given default value.
        /// </summary>
        /// <param name="value">The value to return if it is a safe int.</param>
        /// <param name="defaultValue">Default value to use if the value is not 'safe'</param>
        /// <returns>The default value if the 'value' is not safe, otherwise the 'value' as an int.</returns>
        public static int SafeInt(object value, int defaultValue)
        {
            if (value == null)
                return defaultValue;
            if (!AreAllCharactersNumeric(value.ToString().Trim()))
                return defaultValue;

            return int.Parse(value.ToString());
        }

        /// <summary>
        /// Returns the given value as an short if it is 'safe'.
        /// i.e. If the value is null or non-numeric this method will return 0.
        /// </summary>
        /// <param name="value">The value to return if it is a safe short.</param>
        /// <returns>0 if the value is not safe, otherwise the value as an short.</returns>
        public static short SafeShort(object value)
        {
            return SafeShort(value, 0);
        }

        /// <summary>
        /// Returns the given value as an short if it is 'safe'.
        /// i.e. If the value is null or non-numeric this method will 
        /// return the given default value.
        /// </summary>
        /// <param name="value">The value to return if it is a safe short.</param>
        /// <param name="defaultValue">Default value to use if the value is not 'safe'</param>
        /// <returns>The default value if the 'value' is not safe, otherwise the 'value' as an short.</returns>
        public static short SafeShort(object value, short defaultValue)
        {
            if (value == null)
                return defaultValue;
            if (!IsNumeric(value.ToString().Trim()))
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
        public static long SafeLong(object value)
        {
            return SafeLong(value, 0);
        }

        /// <summary>
        /// Returns the given value as an long if it is 'safe'.
        /// i.e. If the value is null or non-numeric this method will 
        /// return the given default value.
        /// </summary>
        /// <param name="value">The value to return if it is a safe long.</param>
        /// <param name="defaultValue">Default value to use if the value is not 'safe'</param>
        /// <returns>The default value if the 'value' is not safe, otherwise the 'value' as an long.</returns>
        public static long SafeLong(object value, long defaultValue)
        {
            if (value == null)
                return defaultValue;
            if (!AreAllCharactersNumeric(value.ToString().Trim()))
                return defaultValue;

            return long.Parse(value.ToString());
        }

        /// <summary>
        /// Returns the given value as a double if it is 'safe'.
        /// i.e. If the value is null or non-numeric this method will return 0.
        /// </summary>
        /// <param name="value">The value to return if it is a safe double.</param>
        /// <returns>0 if the value is not safe, otherwise the value as a double.</returns>
        public static double SafeDouble(object value)
        {
            return SafeDouble(value, 0);
        }

        /// <summary>
        /// Returns the given value as a double if it is 'safe'.
        /// i.e. If the value is null or non-numeric this method will 
        /// return the given default value.
        /// </summary>
        /// <param name="value">The value to return if it is a safe double.</param>
        /// <param name="defaultValue">Default value to use if the value is not 'safe'</param>
        /// <returns>The default value if the 'value' is not safe, otherwise the 'value' as a double.</returns>
        public static double SafeDouble(object value, double defaultValue)
        {
            if (value == null)
                return defaultValue;
            if (!IsNumeric(value.ToString().Trim()))
                return defaultValue;

            return double.Parse(value.ToString());
        }

        /// <summary>
        /// Returns the given value as a float if it is 'safe'.
        /// i.e. If the value is null or non-numeric this method will return 0.
        /// </summary>
        /// <param name="value">The value to return if it is a safe float.</param>
        /// <returns>0 if the value is not safe, otherwise the value as a float.</returns>
        public static float SafeFloat(object value)
        {
            return SafeFloat(value, 0);
        }

        /// <summary>
        /// Returns the given value as a float if it is 'safe'.
        /// i.e. If the value is null or non-numeric this method will 
        /// return the given default value.
        /// </summary>
        /// <param name="value">The value to return if it is a safe float.</param>
        /// <param name="defaultValue">Default value to use if the value is not 'safe'</param>
        /// <returns>The default value if the 'value' is not safe, otherwise the 'value' as a float.</returns>
        public static float SafeFloat(object value, float defaultValue)
        {
            if (value == null)
                return defaultValue;
            if (!IsNumeric(value.ToString().Trim()))
                return defaultValue;

            return float.Parse(value.ToString());
        }

        /// <summary>
        /// Returns the given value as a decimal if it is 'safe'.
        /// i.e. If the value is null or non-numeric this method will return 0.
        /// </summary>
        /// <param name="value">The value to return if it is a safe decimal.</param>
        /// <returns>0 if the value is not safe, otherwise the value as a decimal.</returns>
        public static decimal SafeDecimal(object value)
        {
            return SafeDecimal(value, 0);
        }

        /// <summary>
        /// Returns the given value as a decimal if it is 'safe'.
        /// i.e. If the value is null or non-numeric this method will 
        /// return the given default value.
        /// </summary>
        /// <param name="value">The value to return if it is a safe decimal.</param>
        /// <param name="defaultValue">Default value to use if the value is not 'safe'</param>
        /// <returns>The default value if the 'value' is not safe, otherwise the 'value' as a decimal.</returns>
        public static decimal SafeDecimal(object value, decimal defaultValue)
        {
            if (value == null)
                return defaultValue;
            if (!IsNumeric(value.ToString().Trim()))
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
        public static string SafeString(object value)
        {
            if (value == null)
                return string.Empty;
            if (value is string)
                return (string) value;

            return value.ToString();
        }

        /// <summary>
        /// If value is a valid guid it gets returned as a guid,
        /// OTHERWISE it returns Guid.Empty
        /// </summary>
        public static Guid SafeGuid(object value)
        {
            try
            {
                // 32 contiguous digits is the minimum (my understanding of it...)
                if (value == null || value.ToString().Length < 32)
                    return Guid.Empty;

                return new Guid(SafeString(value));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return Guid.Empty;
            }
        }
    }
}