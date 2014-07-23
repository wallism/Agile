using System;

namespace Agile.Common.Reflections
{
    /// <summary>
    /// When we invoke methods using reflection, we need to know the parameter
    /// types to get the method and we need the actual parameter values when
    /// we call Invoke. 
    /// This class encapsulates those parameter details.
    /// </summary>
    public class MethodParameterDetails
    {
        /// <summary>
        /// Stores the parameter details
        /// </summary>
        private readonly object[] _parameters;

        /// <summary>
        /// Construct with the parameters
        /// </summary>
        private MethodParameterDetails(params object[] parameters)
        {
            _parameters = parameters;
        }

        /// <summary>
        /// Gets the number of parameters that have been provided for the Instantiation method.
        /// </summary>
        private int NumberOfParameters
        {
            get { return _parameters.Length; }
        }

        /// <summary>
        /// Returns true if there are no parameters defined.
        /// </summary>
        public bool HasNoParameters
        {
            get { return _parameters == null; }
        }

        /// <summary>
        /// Gets the signature (not return type tho) details.
        /// Essentially the Types of the parameters that were provided on
        /// instantiation.
        /// </summary>
        public Type[] Signature
        {
            get
            {
                if (HasNoParameters)
                    return new Type[0];

                var instantiateMethodParameters = new Type[NumberOfParameters];
                for (int i = 0; i < NumberOfParameters; i++)
                {
                    instantiateMethodParameters[i] = _parameters[i].GetType();
                }
                return instantiateMethodParameters;
            }
        }

        /// <summary>
        /// Gets the parameter values.
        /// </summary>
        public object[] Parameters
        {
            get { return _parameters; }
        }

        /// <summary>
        /// Instantiate the required parameters. 
        /// </summary>
        /// <param name="parameters">All of the parameters required for the method.
        /// NOTE: Must be in the correct order!</param>
        /// <returns></returns>
        public static MethodParameterDetails Build(params object[] parameters)
        {
            return new MethodParameterDetails(parameters);
        }
    }
}