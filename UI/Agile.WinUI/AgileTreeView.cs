using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Agile.Common;
using Agile.Common.Reflections;
using Agile.Common.UI;
using Agile.Shared;

namespace Agile.WinUI
{
    /// <summary>
    /// Wrapper for the TreeView control.
    /// </summary>
    public class AgileTreeView : AgileUserControl
    {
        private TreeView _treeView;

        /// <summary>
        /// Constructor
        /// </summary>
        public AgileTreeView()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            _treeView.AfterSelect += _treeView_AfterSelect;
        }

        /// <summary>
        /// The encapsulated Tree view control
        /// </summary>
        public TreeView Tree
        {
            get { return _treeView; }
        }

        /// <summary>
        /// Gets the parameters for invoking a method (using reflection), from the node
        /// </summary>
        public override object[] ParametersForInvoke
        {
            get
            {
                if (SelectedNode == null)
                    return null;
                return SelectedNode.ParametersForInvoke;
            }
        }

        #region Node Functionality

        /// <summary>
        /// Gets the node that is currently selected.
        /// </summary>
        public AgileTreeNode SelectedNode
        {
            get { return _treeView.SelectedNode as AgileTreeNode; }
        }

        /// <summary>
        /// Create nodes for the given collection
        /// </summary>
        /// <remarks>The items in the collection must implement IAgileControlDetails.</remarks>
        /// <param name="items"></param>
        /// <returns></returns>
        private AgileTreeNode[] CreateNodesFrom(IList items)
        {
            if (items == null) return new AgileTreeNode[0];

            var nodes = new AgileTreeNode[items.Count];
            int i = 0;
            foreach (IAgileControlDetails details in items)
            {
                nodes[i] = AgileTreeNode.Build(details
                                               , CreateNodesFrom(details.ChildObjects));
                i++;
                Application.DoEvents();
            }
            return nodes;
        }

        /// <summary>
        /// Handles when a node is selected.
        /// Sets the GUI factory for the Type of object in the node.
        /// </summary>
        private void _treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SetGuiFactory(AgileType.Build(SelectedNode.Details.GetType()));
        }

        private void _treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                _treeView.SelectedNode = e.Node;
        }

        #endregion


        /// <summary>
        /// Initializes the tree view with the given set of items.
        /// </summary>
        /// <param name="items">The collection of items to display in the tree.</param>
        /// <remarks>The items MUST implement IAgileControlDetails.</remarks>
        public void InitializeWith(IList items)
        {
            ArgumentValidation.CheckForNullReference(items, "items");
            _treeView.Nodes.Clear();
            _treeView.Nodes.AddRange(CreateNodesFrom(items));
        }

        /// <summary>
        /// Adds the items as extra nodes in the tree view.
        /// </summary>
        /// <param name="items">The collection of items to add to the tree.</param>
        /// <remarks>The items MUST implement IAgileControlDetails.</remarks>
        public void AddNodes(IList items)
        {
            ArgumentValidation.CheckForNullReference(items, "items");
            _treeView.Nodes.AddRange(CreateNodesFrom(items));
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            _treeView = new TreeView();
            SuspendLayout();
            // 
            // _treeView
            // 
            _treeView.Dock = DockStyle.Fill;
            _treeView.ImageIndex = -1;
            _treeView.Location = new Point(0, 0);
            _treeView.Name = "_treeView";
            _treeView.SelectedImageIndex = -1;
            _treeView.Size = new Size(150, 150);
            _treeView.TabIndex = 0;
            this._treeView.NodeMouseClick +=
                new System.Windows.Forms.TreeNodeMouseClickEventHandler(this._treeView_NodeMouseClick);
            // 
            // AgileTreeView
            // 
            Controls.Add(_treeView);
            Name = "AgileTreeView";
            ResumeLayout(false);
        }

        #endregion
    }
}