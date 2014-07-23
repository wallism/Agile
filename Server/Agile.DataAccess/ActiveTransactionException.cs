using System;
using System.Runtime.Serialization;

namespace Agile.DataAccess
{
    /// <summary>
    /// Summary description for ActiveTransactionException.
    /// </summary>
    public class ActiveTransactionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the ActiveTransactionException class.
        /// </summary>
        public ActiveTransactionException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the ActiveTransactionException class with a specified error message. 
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public ActiveTransactionException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ActiveTransactionException class with a specified error message and a reference to the inner exception that is the cause of this exception.  
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. 
        /// If the innerException parameter is not a null reference, 
        /// the current exception is raised in a catch block that handles the inner exception.</param>
        public ActiveTransactionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ActiveTransactionException class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected ActiveTransactionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}