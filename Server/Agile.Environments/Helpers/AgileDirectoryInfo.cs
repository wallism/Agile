using System.Collections;
using System.IO;
using Agile.Common.UI;

namespace Agile.Common
{
    /// <summary>
    /// Wrapper for DirectoryInfo.
    /// </summary>
    public class AgileDirectoryInfo : IAgileControlDetails
    {
        private readonly DirectoryInfo _directoryInfo;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="directoryInfo">The DirectoryInfo to wrap</param>
        private AgileDirectoryInfo(DirectoryInfo directoryInfo)
        {
            _directoryInfo = directoryInfo;
        }

        /// <summary>
        /// The 'wrapped' Directory info object.
        /// </summary>
        public DirectoryInfo DirectoryInfo
        {
            get { return _directoryInfo; }
        }

        #region IAgileControlDetails Members

        /// <summary>
        /// Gets the value to display in the control
        /// </summary>
        string IAgileControlDetails.DisplayValue
        {
            get { return _directoryInfo.Name; }
        }

        /// <summary>
        /// Gets any child objects.
        /// </summary>
        /// <example>A database table should return its collection of columns.</example>
        /// <remarks>Returns NULL if there are not any child objects.</remarks>
        IList IAgileControlDetails.ChildObjects
        {
            get { return GetDirectories(); }
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
        /// Instantiate a new AgileDirectoryInfo with the given DirectoryInfo.
        /// </summary>
        /// <param name="directoryInfo">The DirectoryInfo to wrap</param>
        /// <returns></returns>
        public static AgileDirectoryInfo Build(DirectoryInfo directoryInfo)
        {
            return new AgileDirectoryInfo(directoryInfo);
        }

        /// <summary>
        /// Gets the 'child' directories as an Agile DirectoryInfo Collection.
        /// </summary>
        /// <returns></returns>
        public AgileDirectoryInfoCollection GetDirectories()
        {
            return AgileDirectoryInfoCollection.Build(DirectoryInfo);
        }
    }
}