using System.Collections;

namespace Agile.Common.UI
{
    /// <summary>
    /// Make your class implement this interface to fit in the
    /// 'Agile' GUI architecture.
    /// </summary>
    /// <remarks>Provides details from your class that are commonly used
    /// by GUI components</remarks>
    public interface IAgileControlDetails
    {
        /// <summary>
        /// Gets the value to display in the control
        /// </summary>
        string DisplayValue { get; }

        /// <summary>
        /// Gets any child objects.
        /// </summary>
        /// <example>A database table should return its collection of columns.</example>
        /// <remarks>Returns NULL if there are not any child objects.</remarks>
        IList ChildObjects { get; }

        /// <summary>
        /// Gets the color to use for the fore color
        /// </summary>
        /// <remarks>Must be an existing color, may be null</remarks>
        string ForeColor { get; }

        /// <summary>
        /// Gets the color to use for the back color
        /// </summary>
        /// <remarks>Must be an existing color, may be null</remarks>
        string BackColor { get; }
    }
}