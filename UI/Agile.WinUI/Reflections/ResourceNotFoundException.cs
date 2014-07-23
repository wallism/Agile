using System;
using System.Reflection;
using Agile.Shared;

namespace Agile.Common.Reflections
{
    /// <summary>
    /// Exception for when a particular resource wasn't found
    /// </summary>
    public class ResourceNotFoundException : Exception
    {
        /// <summary>
        /// Instantiate the exception.
        /// </summary>
        /// <param name="theAssembly">Assembly in which to look for the resource.</param>
        /// <param name="fileName">Name of the resource file.</param>
        public ResourceNotFoundException(Assembly theAssembly, string fileName)
            : base("couldn't find '" + fileName + "' in assembly " + theAssembly.GetName().Name)
        {
            ArgumentValidation.CheckForNullReference(theAssembly, "theAssembly");
            ArgumentValidation.CheckForNullReference(fileName, "fileName");
        }
    }
}