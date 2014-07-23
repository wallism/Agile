using System;

namespace Agile.Common.UI
{
    /// <summary>
    /// Summary description for AgileEditorAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AgileEditorAttribute : Attribute
    {
        private readonly Type _editType;

        /// <summary>
        /// Constructor
        /// </summary>
        public AgileEditorAttribute(Type editType)
        {
            _editType = editType;
        }

        /// <summary>
        /// Gets the Type that this Editor edits
        /// </summary>
        public Type EditType
        {
            get { return _editType; }
        }
    }
}