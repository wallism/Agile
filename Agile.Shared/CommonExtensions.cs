using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Agile.Shared
{

    /// <summary>
    ///  Common Extensions. Should only contain extension that are commonly
    ///  used across projects on many layers.
    /// </summary>
    public static class CommonExtensions
    {

        /// <summary>
        /// Returns the given string with 'Pascal casing'
        /// </summary>
        /// <example>'someString' returns 'SomeString'.
        /// NOTE: at the moment 'somestring' should return 'SomeString' but it currently will only return 
        /// 'Somestring' (this should change in the future).</example>
        /// <param name="text">the string to be converted to pascal casing.</param>
        /// <returns></returns>
        public static string ToPascalCase(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return String.Empty;
            string firstCharacter = text.Substring(0, 1);
            var builder = new StringBuilder(text);
            return builder.Replace(firstCharacter, firstCharacter.ToUpper(), 0, 1).ToString();
        }

        /// <summary>
        /// Converts the string to camel casing. ie. changes MyVariableName to myVariableName
        /// </summary>
        /// <param name="text">string to convert to camel case.</param>
        /// <remarks>ToPascalCase is implemented in Agile.Shared</remarks>
        public static string ToCamelCase(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return String.Empty;
            var firstCharacter = text.Substring(0, 1);
            var builder = new StringBuilder(text);
            return builder.Replace(firstCharacter, firstCharacter.ToLower(), 0, 1).ToString();
        }

        /// <summary>
        /// Removes any spaces from the given string (concatenates each word in the string)
        /// </summary>
        public static string RemoveSpacesFromString(this string removeFrom)
        {
            return Regex.Replace(removeFrom, @"[\s]", "");
        }

        /// <summary>
        /// Removes any unsafe characters from the given string, i.e. anything that is not a letter or a number!
        /// Note: Underscores "_" are NOT removed, spaces " " ARE removed.
        /// </summary>
        public static string RemoveNonAlphanumericCharacters(this string removeFrom)
        {
            return Regex.Replace(removeFrom, @"\W*", "");
        }

        /// <summary>
        /// Removes the first instance of the given string
        /// </summary>
        /// <param name="stringToRemove">string containing comma separated columns.</param>
        /// <param name="removeFrom">remove the first instance of the string from this string</param>
        /// <returns></returns>
        public static string RemoveFirstInstanceOf(this string removeFrom, string stringToRemove)
        {
            if (string.IsNullOrEmpty(stringToRemove))
                return removeFrom;

            var indexOfFirstInstance = removeFrom.IndexOf(stringToRemove);
            if (indexOfFirstInstance < 0)
                return removeFrom;
            return removeFrom.Remove(indexOfFirstInstance, stringToRemove.Length);
        }

        /// <summary>
        /// Returns whether the string is decimal or not
        /// Does not accept commas in the string
        /// </summary>
        /// <param name="check">Any string</param>
        /// <returns>The truth of the matter!</returns>
        public static bool IsValidNumber(this string check)
        {
            return !string.IsNullOrEmpty(check) 
                && Regex.Match(check, @"^-?\d*(\.\d+)?$").Success;
        }


        /// <summary>
        /// Gets the string that is between the start and finish strings.
        /// NOTE: Always works from the FIRST OCCURENCE of start and finish
        /// </summary>
        /// <param name="searchIn">Search in this string</param>
        /// <param name="start">Starting from the end of this string</param>
        /// <param name="finish">Get the contents up the start of this string</param>
        /// <remarks>Does not return any of the start and finish strings</remarks>
        public static string GetStringBetween(this string searchIn, string start, string finish)
        {
            if (!searchIn.Contains(start))
                return string.Empty;

            int indexOfStartString = searchIn.IndexOf(start);
            int startPosition = indexOfStartString + start.Length;
            // make sure - only search AFTER the index of the startPosition
            int endPosition = searchIn.Substring(startPosition).IndexOf(finish);
            // need the index of the end position relative to the entire original string
            endPosition += startPosition;

            int lengthOfStringToReturn = endPosition - startPosition;

            if ((startPosition < 0) || (endPosition < 0))
                return string.Empty;

            return searchIn.Substring(startPosition
                                      , lengthOfStringToReturn);
        }


        /// <summary>
        /// Removes all values from all fields that match the given field name.
        /// </summary>
        /// <param name="fieldName">field to remove all values from.</param>
        /// <param name="xml">xml that will have the value(s) removed.</param>
        public static string RemoveValuesFromXml(this string xml, string fieldName)
        {
            return xml.RemoveValuesFromXml(fieldName, string.Empty);
        }

        /// <summary>
        /// Removes all values from all fields that match the given field name.
        /// </summary>
        /// <param name="fieldName">field to remove all values from.</param>
        /// <param name="xml">xml that will have the value(s) removed.</param>
        /// <param name="namespc">namespace that forms part of the field name</param>
        public static string RemoveValuesFromXml(this string xml, string fieldName, XNamespace namespc)
        {
            if (string.IsNullOrEmpty(fieldName) || string.IsNullOrEmpty(xml)) return string.Empty;

            var doc = XDocument.Parse(xml);
            if (namespc == null)
                namespc = string.Empty;

            IEnumerable<XElement> passwords = doc.Descendants(namespc + fieldName);

            var removeThese = new List<XElement>();
            foreach (XElement element in passwords)
                removeThese.Add(element);

            foreach (XElement element in removeThese)
                element.Value = "--STRING REMOVED--";

            return doc.ToString();

        }

    }
}
