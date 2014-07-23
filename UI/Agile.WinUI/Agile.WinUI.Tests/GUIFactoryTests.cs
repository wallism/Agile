using Agile.Common.Testing;
using NUnit.Framework;

namespace Agile.WinUI.Tests
{
    /// <summary>
    /// Tests the functionality of GUIFactory
    /// </summary>
    [TestFixture]
    public class GUIFactoryTests
    {
        /// <summary>
        /// Verifies we can get a menu item using reflection
        ///     - Check the GetMenuItemsFor on the WidgetGUIFactory returns WidgetMenuItem and WidgetOtherMenuItem when given a widget as parameters
        ///         - Create the widget gui factory
        ///         - Call GetMenuItemsFor for a standard widget item.
        ///         - Check the returned menus contains WidgetMenuItem
        ///         - Check the returned menus contains WidgetOtherMenuItem
        ///         - Check the returned menus does not contain WidgetBogusMenuItem
        ///         
        ///         - Call GetMenuItemsFor for a standard widget item and a string
        ///         - Check the returned menus does not contain WidgetMenuItem
        ///         - Check the returned menus contains WidgetOtherMenuItem
        ///         - Check the returned menus does not contain WidgetBogusMenuItem         
        /// </summary>
        [Test, Ignore("Problem with type loading.")]
        public void GetMenuItemsUsingFactoryTests()
        {
            // Create the widget gui factory
            WidgetGUIFactory factory = WidgetGUIFactory.Build();
            // Call GetMenuItemsFor for a standard widget item.
            AgileMenuItemCollection widgetMenus = factory.GetMenuItemsFor(Widget.Build("TestWidget"));

            // Check the returned menus contains WidgetMenuItem
            Assert.IsNotNull(widgetMenus.FindMenuItemByText("WidgetMenuItem"),
                             "Check the returned menus contains WidgetMenuItem.");
            // Check the returned menus contains WidgetOtherMenuItem
            Assert.IsNotNull(widgetMenus.FindMenuItemByText("WidgetOtherMenuItem"),
                             "Check the returned menus contains WidgetOtherMenuItem.");
            // Check the returned menus does not contain WidgetBogusMenuItem
            Assert.IsNull(widgetMenus.FindMenuItemByText("WidgetBogusMenuItem"),
                          "Check the returned menus contains WidgetBogusMenuItem.");

            // Call GetMenuItemsFor for a standard widget item and a string
            widgetMenus = factory.GetMenuItemsFor(Widget.Build("TestWidget"), "Somestring");
            // Check the returned menus does not contain WidgetMenuItem
            Assert.IsNull(widgetMenus.FindMenuItemByText("WidgetMenuItem"),
                          "Check the returned menus does not contain WidgetMenuItem");
            // Check the returned menus contains WidgetOtherMenuItem
            Assert.IsNotNull(widgetMenus.FindMenuItemByText("WidgetOtherMenuItem"),
                             "Check the returned menus contains WidgetOtherMenuItem.");
            // Check the returned menus does not contain WidgetBogusMenuItem
            Assert.IsNull(widgetMenus.FindMenuItemByText("WidgetBogusMenuItem"),
                          "Check the returned menus contains WidgetBogusMenuItem.");
        }
    }
}