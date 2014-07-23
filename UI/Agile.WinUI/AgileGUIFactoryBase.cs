using System;
using System.Windows.Forms;
using Agile.Common;
using Agile.Common.Reflections;
using Agile.Common.UI;
using Agile.Shared;

namespace Agile.WinUI
{
    /// <summary>
    /// Windows forms GUI factory interace.
    /// </summary>
    public abstract class AgileGUIFactoryBase
    {
//        /// <summary>
//        /// Throws an exception if the item is invalid
//        /// </summary>
//        /// <param name="item"></param>
//        protected abstract void ValidateItemForViewer(object item);

        private static AgileTypeCollection _cachedAgileEditors;
        private readonly object _selectedItem;

        /// <summary>
        /// Construct with an object
        /// </summary>
        /// <param name="item"></param>
        public AgileGUIFactoryBase(object item)
        {
            _selectedItem = item;
        }

        /// <summary>
        /// Instantiate without an object
        /// (viewers etc will not be loaded)
        /// </summary>
        public AgileGUIFactoryBase()
        {
        }

        /// <summary>
        /// Gets the cached collection of all classes that are Editor Controls
        /// (Classes that define the AgileEditorAttribute).
        /// </summary>
        public AgileTypeCollection AgileEditors
        {
            get
            {
                if (_cachedAgileEditors == null)
                {
                    _cachedAgileEditors = Types.GetClassesWithAttribute(Assemblies.Instance.AllLoadedAssemblies
                                                                        , typeof (AgileEditorAttribute));
                }
                return _cachedAgileEditors;
            }
        }

        /// <summary>
        /// Gets the object that was selected and made this gui factory active.
        /// </summary>
        public object SelectedItem
        {
            get { return _selectedItem; }
        }

        #region Menu Item Creation

        private static AgileTypeCollection _cachedAgileMenuItems;
        private static AgileTypeCollection _cachedMenuItemFactories;

        /// <summary>
        /// Gets the cached collection of menu item factories.
        /// </summary>
        public AgileTypeCollection MenuItemFactories
        {
            get
            {
                if (_cachedMenuItemFactories == null)
                    _cachedMenuItemFactories = Types.GetClassesImpementing(Assemblies.Instance.OurAssemblies,
                                                                           typeof (IAgileMenuItemsFactory));
                return _cachedMenuItemFactories;
            }
        }


        /// <summary>
        /// Gets the cached collection of all classes that are decendant from AgileMenuItem.
        /// </summary>
        public AgileTypeCollection AgileMenuItemTypes
        {
            get
            {
                if (_cachedAgileMenuItems == null)
                {
                    _cachedAgileMenuItems = Types.GetAllSubClassesOf(Assemblies.Instance.OurAssemblies
                                                                     , typeof (AgileMenuItem)
                                                                     , false);
                }
                return _cachedAgileMenuItems;
            }
        }

        /// <summary>
        /// Gets the menu items for the given object.
        /// </summary>
        /// <param name="details">Details for the factory to use to create the menus</param>
        /// <returns></returns>
        public AgileMenuItemCollection GetMenuItemsFor(params object[] details)
        {
            ArgumentValidation.CheckForNullReference(details, "details");
            MethodParameterDetails parameterDetails = MethodParameterDetails.Build(details);
            AgileMenuItemCollection menuItems = AgileMenuItemCollection.Build();

            // This code gives us all menu items that specifically take the given parameters.
            foreach (AgileType menu in AgileMenuItemTypes)
            {
                if (menu.InstantiateMethodExistsFor(parameterDetails))
                {
                    var menuItem = (AgileMenuItem) menu.InstantiateConcreteClass(parameterDetails);
                    if (menuItem != null)
                        menuItems.Add(menuItem);
                }
            }

            // This code gives us all menu items that require special creation.
            // TODO: Cache the menu item factories somewhere
            AgileTypeCollection menuFactories = MenuItemFactories;
            foreach (AgileType menuFactory in menuFactories)
            {
                if (menuFactory.InstantiateMethodExistsFor(parameterDetails))
                {
                    var factory = (IAgileMenuItemsFactory) menuFactory.InstantiateConcreteClass(parameterDetails);
                    menuItems.AddRange(factory.CreateMenuItems());
                }
            }

            return menuItems;
        }

        #endregion

        /// <summary>
        /// Loads and returns the appropriate viewer for the currently selected object.
        /// </summary>
        /// <returns></returns>
        public virtual Control DisplaySelectedItem()
        {
            if (SelectedItem == null)
                return null;
            // Have to feed in the type here of the selected item becuase that method recursively calls
            // itself, feeding in base types until it hits 'object' Type.
            AgileType agileTypeOfItemToView = AgileType.Build(SelectedItem.GetType());
            return FindEditor(agileTypeOfItemToView);
        }

        /// <summary>
        /// Find the appropriate Viewer for the currently selected item using reflection
        /// </summary>
        /// <param name="type">Type to search for an editor for.</param>
        /// <returns></returns>
        private Control FindEditor(AgileType type)
        {
            foreach (AgileType agileEditor in AgileEditors)
            {
                object[] attributes = agileEditor.SystemType.GetCustomAttributes(typeof (AgileEditorAttribute), false);

                // In order for it to be an AgileEditorControl it must have the attribte so this should be safe without testing for # items in the array
                var editor = (AgileEditorAttribute) attributes[0];

                if (editor.EditType.Name == type.SystemType.Name)
                {
                    MethodParameterDetails parameters = MethodParameterDetails.Build(SelectedItem);
                    return (Control) agileEditor.InstantiateConcreteClass(parameters);
                }
            }

            if (type.SystemType.BaseType == null)
                throw new Exception(
                    "Failed to find the ObjectViewer in the loaded assemblies. [AgileGUIFactoryBase.FindEditor]");

            return FindEditor(AgileType.Build(type.SystemType.BaseType));
        }
    }
}