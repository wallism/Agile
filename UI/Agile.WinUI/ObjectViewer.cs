using System.ComponentModel;
using System.Windows.Forms;
using Agile.Common;
using Agile.Common.UI;
using Agile.Shared;

namespace Agile.WinUI
{
    /// <summary>
    /// Summary description for ObjectViewer.
    /// </summary>
    [AgileEditor(typeof (object))]
    public class ObjectViewer : AgileEditorControl
    {
        private Label _tostringLabel;
        private TextBox _toStringValue;

        /// <summary>
        /// Constructor
        /// </summary>
        public ObjectViewer()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
        }

        /// <summary>
        /// Construct with an object to view
        /// </summary>
        public static ObjectViewer Build(object item)
        {
            ArgumentValidation.CheckForNullReference(item, "item");

            var viewer = new ObjectViewer();
            viewer.Populate(item);
            return viewer;
        }

        /// <summary>
        /// Initialize the viewer with any object.
        /// </summary>
        /// <param name="item"></param>
        public override void Populate(object item)
        {
            base.Populate(item);
            _toStringValue.Text = item.ToString();
        }

       

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._toStringValue = new System.Windows.Forms.TextBox();
            this._tostringLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _toStringValue
            // 
            this._toStringValue.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                   | System.Windows.Forms.AnchorStyles.Right)));
            this._toStringValue.Location = new System.Drawing.Point(8, 24);
            this._toStringValue.Multiline = true;
            this._toStringValue.Name = "_toStringValue";
            this._toStringValue.Size = new System.Drawing.Size(184, 88);
            this._toStringValue.TabIndex = 0;
            this._toStringValue.Text = "";
            // 
            // _tostringLabel
            // 
            this._tostringLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F,
                                                               System.Drawing.FontStyle.Bold,
                                                               System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
            this._tostringLabel.Location = new System.Drawing.Point(8, 8);
            this._tostringLabel.Name = "_tostringLabel";
            this._tostringLabel.Size = new System.Drawing.Size(100, 16);
            this._tostringLabel.TabIndex = 1;
            this._tostringLabel.Text = "To String:";
            // 
            // ObjectViewer
            // 
            this.Controls.Add(this._tostringLabel);
            this.Controls.Add(this._toStringValue);
            this.Name = "ObjectViewer";
            this.Size = new System.Drawing.Size(200, 120);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}