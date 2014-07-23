using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Agile.Diagnostics.Logging;

namespace Agile.Shared
{
    /// <summary>
    /// Common functions
    /// </summary>
    public static class Common
    {
        /// <summary>
        /// Checks that the given object.ToString contains only numeric characters.
        /// </summary>
        public static bool AreAllCharactersNumeric(object check)
        {
            if (check == null)
                return false;
            return RegexPatterns.AllNumbers.IsMatch(check.ToString());
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
            if (string.IsNullOrEmpty(number))
                return number;

            if (number.Contains("."))
                return number.RemoveFirstInstanceOf(".");
            return number;
        }
    }
}
