using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Agile.WinUI
{
    /// <summary>
    /// Summary description for AgileForm.
    /// </summary>
    public class AgileForm : Form
    {
        private AgileGUIFactoryBase _activeGUIFactory;

        /// <summary>
        /// Gets the currently active GUI factory for the form.
        /// </summary>
        public AgileGUIFactoryBase ActiveGUIFactory
        {
            get { return _activeGUIFactory; }
        }

        /// <summary>
        /// Set the GUI factory that is to be used.
        /// </summary>
        /// <param name="factory">GUI factory to use. May be null.</param>
        protected void SetActiveGuiFactory(AgileGUIFactoryBase factory)
        {
            // May be null
            _activeGUIFactory = factory;

            string factoryName = (factory == null) ? "null" : factory.GetType().Name;
            Debug.Write(string.Format(@"Set GUI factory to: {0}, on the form: {1}
"
                                      , factoryName, GetType().Name));

            if (_activeGUIFactory == null)
                return;

            //_activeGUIFactory.GetViewer();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // AgileForm
            // 
            ClientSize = new Size(297, 266);
            Name = "AgileForm";
            ResumeLayout(false);
        }
    }
}