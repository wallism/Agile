using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Agile.Common;
using Agile.Framework.UI;

namespace Agile.WinUI.Common
{
    /// <summary>
    /// Common combo box for displaying lookup records.
    /// </summary>
    public partial class LookupComboBox : UserControl
    {
        /// <summary>
        /// Common combo box for displaying lookup records.
        /// </summary>
        public LookupComboBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the selected item in the combo box
        /// </summary>
        public object SelectedItem
        {
            get { return comboBox.SelectedItem; }
            set { comboBox.SelectedItem = value; }
        }

        /// <summary>
        /// OnLoad override
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            comboBox.DisplayMember = "Display";
        }

        /// <summary>
        /// Populate the combo with a List of objects that implement IAgileLookup
        /// </summary>
        public void Populate<T>(List<T> lookup) where T : IAgileLookup
        {
            comboBox.DataSource = lookup;
        }
    }
}