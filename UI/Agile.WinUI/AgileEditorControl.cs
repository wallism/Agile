using System.ComponentModel;
using Agile.Common.Reflections;

namespace Agile.WinUI
{
    /// <summary>
    /// Base class for editor controls
    /// </summary>
    public class AgileEditorControl : AgileUserControl
    {
        private object _itemToEdit;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private Container components;

        /// <summary>
        /// Constructor
        /// </summary>
        public AgileEditorControl()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
        }

        /// <summary>
        /// Gets the parameters for invoking a method (using reflection), from the node
        /// </summary>
        public override object[] ParametersForInvoke
        {
            get
            {
                var items = new object[1];
                items[0] = _itemToEdit;
                return items;
            }
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Initialize the control with the specific type of object
        /// </summary>
        /// <remarks>Must be called by any concrete classes</remarks>
        public virtual void Populate(object item)
        {
            _itemToEdit = item;
            SetGuiFactory(AgileType.Build(item.GetType()));
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion
    }
}