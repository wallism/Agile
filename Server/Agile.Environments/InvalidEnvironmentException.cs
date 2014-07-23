using System;
using System.Runtime.Serialization;

namespace Agile.Environments
{
    /// <summary>
    /// Exception for when the database updater is given an invalid environment.
    /// </summary>
    /// <remarks>Doesnt currently do anything extra, but is required anyway because
    /// of fxCop rule - DoNotRaiseReservedExceptionTypes</remarks>
    [Serializable]
    public class InvalidEnvironmentException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the InvalidEnvironmentException class.
        /// </summary>
        public InvalidEnvironmentException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the InvalidEnvironmentException class with a specified error message. 
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public InvalidEnvironmentException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the InvalidEnvironmentException class with a specified error message and a reference to the inner exception that is the cause of this exception.  
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. 
        /// If the innerException parameter is not a null reference, 
        /// the current exception is raised in a catch block that handles the inner exception.</param>
        public InvalidEnvironmentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the InvalidEnvironmentException class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected InvalidEnvironmentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}