using System;
using System.Runtime.Serialization;

namespace Agile.Framework
{
    /// <summary>
    /// Used to indicate if the Save operation on a BizObject 
    /// was successful.
    /// </summary>
    /// <remarks>rudimentary for now but easily extended later.</remarks>
    public class SaveResult
    {
        /// <summary>
        /// True if the save was successful
        /// </summary>
        public bool Failed { get; set; }
        /// <summary>
        /// If the save was not successful, there should be an error message.
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// Save was successful
        /// </summary>
        public static SaveResult Success()
        {
            return new SaveResult { Failed = false };
        }


        // actually below methods should only be available server side. Need to split out...
#if !SILVERLIGHT
        /// <summary>
        /// The save failed
        /// </summary>
        public static SaveResult Fail(string errorMessage)
        {
            if (string.IsNullOrEmpty(errorMessage))
                throw new ApplicationException("message must be supplied when save failed!");
            return new SaveResult { Failed = true, ErrorMessage = errorMessage };
        }

        /// <summary>
        /// The save failed
        /// </summary>
        public static SaveResult Fail(Exception ex)
        {
            // for now just use the message
            return Fail(ex.Message);
        }
#endif
    }
}