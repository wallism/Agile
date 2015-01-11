using System.Text.RegularExpressions;

namespace Agile.Shared
{
    public static class RegexPatterns
    {
        /// <summary>
        /// Pattern for matching a string that contains only numeric characters
        /// </summary>
        public static readonly Regex AllNumbers = new Regex(@"^\d+$"); 

        /// <summary>
        /// Characters that are valid for a username
        /// </summary>
        public static Regex ValidUserNameRegex = new Regex("[^A-Za-z0-9._-]");

        /// <summary>
        /// Characters that are valid for a Tag
        /// </summary>
        public static Regex ValidTagRegex = new Regex("[^A-Za-z0-9._]");

        /// <summary>
        /// AlphaNumeric Regex
        /// </summary>
        public static Regex AlphaNumericRegex = new Regex("[A-Za-z0-9]");
        /// <summary>
        /// XmlEscape Characters
        /// </summary>
        public static Regex XmlEscapeCharacters = new Regex("\"'<>&");

    }
}