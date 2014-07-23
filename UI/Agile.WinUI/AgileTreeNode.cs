using System.Drawing;
using System.Windows.Forms;
using Agile.Common;
using Agile.Common.UI;
using Agile.Shared;

namespace Agile.WinUI
{
    /// <summary>
    /// Tree nodes for use in the AgileTreeView.
    /// </summary>
    public class AgileTreeNode : TreeNode
    {
        private readonly IAgileControlDetails _details;

        /// <summary>
        /// Constructor
        /// </summary>
        private AgileTreeNode(IAgileControlDetails details)
            : this(details, new AgileTreeNode[0])
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private AgileTreeNode(IAgileControlDetails details, AgileTreeNode[] children)
            : base(details.DisplayValue, children)
        {
            ArgumentValidation.CheckForNullReference(details, "details");
            _details = details;
            if (details.ForeColor != null)
                ForeColor = Color.FromName(details.ForeColor);
        }

        /// <summary>
        /// Gets the object containing the details associated with
        /// the node.
        /// </summary>
        public object Details
        {
            get { return _details; }
        }

        /// <summary>
        /// Gets the parameters for invoking a method (using reflection), from the node
        /// </summary>
        public virtual object[] ParametersForInvoke
        {
            get
            {
                var parameters = new object[1];
                parameters[0] = Details;
                return parameters;
            }
        }


        /// <summary>
        /// Returns true if this node is a Leaf node, 
        /// i.e. it does not have any child nodes.
        /// </summary>
        public bool IsLeafNode
        {
            get { return !IsParentNode; }
        }

        /// <summary>
        /// Returns true if this node is a Parent node, 
        /// i.e. it DOES have child nodes.
        /// </summary>
        public bool IsParentNode
        {
            get
            {
                if ((Nodes == null) || (Nodes.Count == 0))
                    return false;
                return true;
            }
        }

        /// <summary>
        /// Returns true if this node is a Root node, 
        /// i.e. it does not have any Parent nodes.
        /// </summary>
        public bool IsRootNode
        {
            get
            {
                if (Parent == null)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Instantiates a new AgileTreeNode.
        /// </summary>
        /// <param name="details">Details for the node</param>
        /// <param name="children">Child nodes</param>
        /// <returns></returns>
        public static AgileTreeNode Build(IAgileControlDetails details
                                          , AgileTreeNode[] children)
        {
            return new AgileTreeNode(details, children);
        }
    }
}