using System.Text;
using Agile.Shared;

namespace Agile.Common
{
    /// <summary>
    ///     Support functions for strings
    /// </summary>
    public static class Strings
    {
        /// <summary>
        ///     Removes the first instance of the given string
        /// </summary>
        /// <param name="stringToRemove">string containing comma separated columns.</param>
        /// <param name="removeFrom">remove the first instance of the string from this string</param>
        public static string RemoveFirstInstanceOf(string stringToRemove, string removeFrom)
        {
            ArgumentValidation.CheckForNullReference(removeFrom, "removeFrom");

            if (string.IsNullOrEmpty(stringToRemove))
                return removeFrom; // nothing to remove

            int indexOfFirstInstance = removeFrom.IndexOf(stringToRemove);
            if (indexOfFirstInstance < 0)
                return removeFrom;
            return removeFrom.Remove(indexOfFirstInstance, stringToRemove.Length);
        }


        /// <summary>
        ///     Splits the given string by new line, then places the first characters of every
        ///     line in front of each split and returns the result.
        ///     NOTE: also starts with a new line
        /// </summary>
        /// <param name="stringToSplit">the single or multi line string that is to be split</param>
        /// <param name="firstCharactersOfEveryLine">characters that will appear as the first characters of every line.</param>
        /// <remarks>particularly useful for generating comments.</remarks>
        public static string SplitMultiLineString(string stringToSplit, string firstCharactersOfEveryLine)
        {
            var newline = "\n";
            var splitString = stringToSplit.Split(newline.ToCharArray());
            var splitStringToReturn = new StringBuilder();

            foreach (string splitComment in splitString)
            {
                splitStringToReturn.Append(string.Format(@"
{0}{1}", firstCharactersOfEveryLine, splitComment.Trim()));
            }
            return splitStringToReturn.ToString();
        }
    }
}