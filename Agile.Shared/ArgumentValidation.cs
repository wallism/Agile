using System;

namespace Agile.Shared
{
    /// <summary>
    /// Implemented methods that disappeared from enterprise lib in V2.0
    /// </summary>
    public sealed class ArgumentValidation
    {
        /// <summary>
        /// Checks the variable is not null, throws an exception if it is
        /// </summary>
        /// <param name="variable">the object</param>
        /// <param name="variableName">name of the variable</param>
        public static void CheckForNullReference(object variable, string variableName)
        {
            if (variable == null)
                throw new ArgumentNullException(string.Format("{0} is required but was null", variableName));
        }

        /// <summary>
        /// Checks the variable is not empty, throws an exception if it is
        /// </summary>
        /// <param name="variable">the object</param>
        /// <param name="variableName">name of the variable</param>
        public static void CheckForEmptyString(string variable, string variableName)
        {
            CheckForNullReference(variable, variableName);
            CheckForNullReference(variableName, "variableName");

            if (variable.Length == 0)
            {
                throw new ArgumentException(string.Format("{0} is required but was empty", variableName));
            }
        }
    }
}