using System;

namespace Agile.Common.UI
{
    /// <summary>
    /// Summary description for GeneratorDescriptionAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class GUIDetailsAttribute : Attribute
    {
        private readonly string _display = string.Empty;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="display">Value to display in things such as context menu's</param>
        public GUIDetailsAttribute(string display)
        {
            _display = display;
        }

        /// <summary>
        /// Value to display in things such as context menu's
        /// </summary>
        public string Display
        {
            get { return _display; }
        }
    }
}