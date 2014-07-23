using System.Collections;
using System.Reflection;
using Agile.Common.UI;

namespace Agile.Common.Reflections
{
    /// <summary>
    /// AgileAssembly is a wrapper for System.Reflection.Assembly.
    /// </summary>
    public class AgileAssembly : IAgileControlDetails
    {
        private readonly Assembly _assembly;

        /// <summary>
        /// Constructor
        /// </summary>
        private AgileAssembly(Assembly assembly)
        {
            _assembly = assembly;
        }

        /// <summary>
        /// Gets the internal System.Reflection.Assembly
        /// </summary>
        public Assembly Assembly
        {
            get { return _assembly; }
        }

        #region IAgileControlDetails Members

        /// <summary>
        /// Gets the value to display in the control
        /// </summary>
        string IAgileControlDetails.DisplayValue
        {
            get
            {
                // TODO:  Add AgileAssembly.DisplayValue getter implementation
                return _assembly.GetName().Name;
            }
        }

        /// <summary>
        /// Gets any child objects.
        /// </summary>
        /// <example>A database table should return its collection of columns.</example>
        /// <remarks>Returns NULL if there are not any child objects.</remarks>
        IList IAgileControlDetails.ChildObjects
        {
            get
            {
                // TODO:  Add AgileAssembly.ChildObjects getter implementation
                return Types.GetAllTypesIn(_assembly);
            }
        }

        /// <summary>
        /// Gets the color to use for the fore color
        /// </summary>
        /// <remarks>Must be an existing color, may be null</remarks>
        public string ForeColor
        {
            get { return null; }
        }

        /// <summary>
        /// Gets the color to use for the back color
        /// </summary>
        /// <remarks>Must be an existing color, may be null</remarks>
        public string BackColor
        {
            get { return null; }
        }

        #endregion
        /// <summary>
        /// override of ToString to show the contained item
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0} {1}", GetType().Name, Assembly);
        }


        /// <summary>
        /// Instantiate a new AgileAssembly with the given Assembly.
        /// </summary>
        /// <param name="assembly">The System.Reflection.Assembly to wrap</param>
        /// <returns></returns>
        public static AgileAssembly Build(Assembly assembly)
        {
            return new AgileAssembly(assembly);
        }


    }
}