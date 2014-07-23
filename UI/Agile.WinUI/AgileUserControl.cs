using System;
using System.Diagnostics;
using System.Windows.Forms;
using Agile.Common;
using Agile.Common.Reflections;
using Agile.Shared;

namespace Agile.WinUI
{
    /// <summary>
    /// Summary description for AgileUserControl.
    /// </summary>
    public class AgileUserControl : UserControl
    {
        #region Delegates

        /// <summary>
        /// Delegate for when the GUI factory is changed
        /// </summary>
        public delegate void GuiFactoryChangeEventHandler(AgileGUIFactoryBase factory);

        #endregion

        private AgileGUIFactoryBase _activeGUIFactory;

        /// <summary>
        /// Classes that implement AgileGUIFactoryBase
        /// </summary>
        private AgileTypeCollection _factories;

        /// <summary>
        /// Constructor
        /// </summary>
        public AgileUserControl()
        {
            ContextMenu = _contextMenu;
            _contextMenu.Popup += _contextMenu_Popup;
        }

        /// <summary>
        /// Gets and sets the gui factory for the tree view.
        /// </summary>
        protected AgileGUIFactoryBase GuiFactory
        {
            get
            {
                if (_activeGUIFactory == null)
                    throw new ApplicationException("GUI factory has not been set");
                return _activeGUIFactory;
            }
        }

        /// <summary>
        /// Gets all classes that implement AgileGUIFactoryBase
        /// </summary>
        /// <remarks>Lazy loading used.</remarks>
        private AgileTypeCollection GUIFactories
        {
            get
            {
                if (_factories == null)
                    _factories = Types.GetAllSubClassesOf(Assemblies.Instance.OurAssemblies
                                                          , typeof (AgileGUIFactoryBase), false);

                return _factories;
            }
        }

        /// <summary>
        /// Gets the parameters for invoking a method (using reflection), from the node
        /// </summary>
        /// <remarks>because this is a user control it can't be abstract (at least without 
        /// doing so affecting the designer), otherwise this would be abstract
        /// because it should be implemented by all concrete derivates</remarks>
        public virtual object[] ParametersForInvoke
        {
            get { return null; }
        }

        #region Menu Item Click Events

        #region Delegates

        /// <summary>
        /// Handler for when a context menu item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void MenuItemClickHandler(object sender, EventArgs e);

        #endregion

        /// <summary>
        /// Event for when any menu item is clicked.
        /// </summary>
        public event MenuItemClickHandler OnMenuItemClick;

        private void MenuItem_ClickHandler(object sender, EventArgs e)
        {
            if (OnMenuItemClick != null)
                OnMenuItemClick(sender, e);
        }

        #endregion

        #region Context Menus

        /// <summary>
        /// The context menu 
        /// </summary>
        private readonly ContextMenu _contextMenu = new ContextMenu();

        /// <summary>
        /// Handler for the context menu Popup event.
        /// </summary>
        private void _contextMenu_Popup(object sender, EventArgs e)
        {
            _contextMenu.MenuItems.Clear();

            if ((_activeGUIFactory != null) && (ParametersForInvoke != null))
            {
                AgileMenuItemCollection menus = _activeGUIFactory.GetMenuItemsFor(ParametersForInvoke);
                if (menus != null)
                {
                    foreach (MenuItem menu in menus)
                    {
                        menu.Click += MenuItem_ClickHandler;
                        _contextMenu.MenuItems.Add(menu);
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Set the GUI factory that is to be used.
        /// </summary>
        /// <param name="factory">GUI factory to use. May be null.</param>
        protected void SetGuiFactory(AgileGUIFactoryBase factory)
        {
            // May be null
            _activeGUIFactory = factory;
            if (GuiFactoryBeingSet != null)
            {
                GuiFactoryBeingSet(factory);
            }

            string factoryName = (factory == null) ? "null" : factory.GetType().Name;
            Debug.Write("Set GUI factory to: " + factoryName + "\n");
        }

        /// <summary>
        /// Uses reflection to find the GUI factory for the 
        /// given type.
        /// </summary>
        /// <param name="factoryForThisType">Find the appropriate GUI factory for this
        /// type and set it to that factory.</param>
        protected void SetGuiFactory(AgileType factoryForThisType)
        {
            AgileGUIFactoryBase factory = FindGuiFactory(factoryForThisType);
            SetGuiFactory(factory);
        }


        /// <summary>
        /// Event for when the GUI factory is changed
        /// </summary>
        public event GuiFactoryChangeEventHandler GuiFactoryBeingSet;


//        private AgileGUIFactoryBase _defaultGUIFactory;
//
//        /// <summary>
//        /// The default GUI factory to use when one is not found in the AppDomain.
//        /// </summary>
//        protected AgileGUIFactoryBase DefaultGUIFactory
//        {
//            get { return _defaultGUIFactory; }
//        }
//
//        /// <summary>
//        /// Sets the default GUI factory.
//        /// </summary>
//        /// <param name="factory"></param>
//        protected void SetDefaultGUIFactory(AgileGUIFactoryBase factory)
//        {
//            _defaultGUIFactory = factory;
//        }


        /// <summary>
        /// Find the GUI factory for the given Type using reflection
        /// </summary>
        /// <remarks>Currently this is name based, i.e. string based.
        /// The name of the GUI Factories must be in the format:
        /// <para> ClassName + GUIFactory</para>   
        /// If we fail to find a factory for the given type we try to find one
        /// for its base type, then the base types base type and so on.
        /// </remarks>
        /// <param name="forThisType">Find the appropriate GUI factory for this type.</param>
        /// <returns></returns>
        private AgileGUIFactoryBase FindGuiFactory(AgileType forThisType)
        {
            ArgumentValidation.CheckForNullReference(forThisType, "forThisType");

            // First check if  there is one specifically for the given type
            foreach (AgileType factory in GUIFactories)
            {
                string existingFactory = factory.Name.Remove(factory.Name.IndexOf("GUIFactory"), 10).ToLower();
                if (existingFactory.Equals(forThisType.Name.ToLower()))
                    return
                        (AgileGUIFactoryBase)
                        factory.InstantiateConcreteClass(MethodParameterDetails.Build(ParametersForInvoke));
                        // was done with null
            }

            // If it doesnt have one, try to find one for classes this inherits from.
            if (forThisType.SystemType.BaseType != null)
                return FindGuiFactory(AgileType.Build(forThisType.SystemType.BaseType));

            // Should never hit this unless the ObjectGuiFactory is not found.
//            if (DefaultGUIFactory != null)
//                return DefaultGUIFactory;
            return null;
        }
    }
}