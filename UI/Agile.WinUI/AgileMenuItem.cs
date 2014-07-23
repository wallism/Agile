using System;
using System.Windows.Forms;
using Agile.Common;
using Agile.Common.UI;

namespace Agile.WinUI
{
    /// <summary>
    /// Summary description for AgileMenuItem.
    /// </summary>
    public class AgileMenuItem : MenuItem
    {
        private Action _runOnClick;

        #region Constructors

        /// <summary>
        /// Default constructor required for designer
        /// </summary>
        public AgileMenuItem()
        {
        }

        /// <summary>
        /// Instantiate a new agile menu item.
        /// </summary>
        /// <param name="text">Dispaly text of the menu item.</param>
        /// <remarks>Only use when you really want to override or manually set the 
        /// display text.</remarks>
        public AgileMenuItem(string text)
            : base(text)
        {
            base.Click += AgileMenuItem_Click;
        }

        /// <summary>
        /// Instantiate a new agile menu item.
        /// </summary>
        /// <param name="text">DisplayValue text of the menu item.</param>
        /// <param name="childItems">Child menu items.</param>
        public AgileMenuItem(string text, AgileMenuItem[] childItems)
            : this(text)
        {
            MenuItems.AddRange(childItems);
        }

        /// <summary>
        /// Instantiate a new agile menu item.
        /// </summary>
        /// <param name="type">System Type we are instantiating a menu item for.</param>
        /// <remarks>This constructor (and other overloads that have Type as the parameter,
        /// is the best one to use when creating menu's that inherit from AgileMenuItem.
        /// The reason is, if you provide the Type then if the Type defines the GUIDetailsAttribute
        /// then it will get it's display value from that attribute.</remarks>
        public AgileMenuItem(Type type)
            : this(type, new AgileMenuItem[0])
        {
        }

        /// <summary>
        /// Instantiate a new agile menu item.
        /// </summary>
        public AgileMenuItem(Type type, AgileMenuItem[] childItems)
            : this(GetDisplayValue(type), childItems)
        {
        }


        /// <summary>
        /// Instantiate a new agile menu item with an object 
        /// that implements IAgileControlDetails.
        /// </summary>
        /// <param name="details">Any object that implements IAgileControlDetails, provide
        /// required display details.</param>
        /// <param name="childItems">Child menu items.</param>
        public AgileMenuItem(IAgileControlDetails details, AgileMenuItem[] childItems)
            : this(details.DisplayValue, childItems)
        {
        }

        /// <summary>
        /// Instantiate a new agile menu item with an object 
        /// that implements IAgileControlDetails.
        /// </summary>
        /// <param name="details">Any object that implements IAgileControlDetails, provide
        /// required display details.</param>
        public AgileMenuItem(IAgileControlDetails details)
            : this(details.DisplayValue)
        {
        }

        #endregion

        /// <summary>
        /// Set the function that is to be run when the on click event is triggered.
        /// </summary>
        /// <param name="runOnClick"></param>
        public void SetOnClickEventHandler(Action runOnClick)
        {
            _runOnClick = runOnClick;
        }

        /// <summary>
        /// Handler for the on click event
        /// </summary>
        private void AgileMenuItem_Click(object sender, EventArgs e)
        {
            if (_runOnClick == null)
                throw new NullReferenceException("Function to run on click has not been set.");
            _runOnClick();
        }

        /// <summary>
        /// Get the text to use as the display value.
        /// </summary>
        /// <returns></returns>
        private static string GetDisplayValue(Type type)
        {
            string displayValue = type.Name;
            object[] attributes = type.GetCustomAttributes(typeof (GUIDetailsAttribute), true);
            foreach (object attribute in attributes)
            {
                if (attribute.GetType().Name == "GUIDetailsAttribute")
                {
                    var details = (GUIDetailsAttribute) attribute;
                    displayValue = details.Display;
                }
            }
            return displayValue;
        }
    }
}