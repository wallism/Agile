// WARNING: This file has been generated. Any manual changes must be made within preserved regions or they will be lost.

//===============================================================================
//
// AgileMenuItemCollectionTests
//
// PURPOSE: 
// Tests the functionality of AgileMenuItemCollection.
//
// NOTES: 
// 
//
//===============================================================================
//
// Copyright (C) 2003 Wallis Software Solutions
// All rights reserved.
//
// THIS CODE AND INFORMATION IS PROVIDED 'AS IS' WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR
// FITNESS FOR A PARTICULAR PURPOSE.
//
//===============================================================================

using Agile.Common.Testing;
using Agile.Testing;
using Agile.WinUI.Tests.Testing;
using NUnit.Framework;

namespace Agile.WinUI.Tests
{
    /// <summary>
    /// Tests the functionality of AgileMenuItemCollection.
    /// </summary>
    [TestFixture]
    public class AgileMenuItemCollectionTests : AgileTestBase
    {
        private AgileMenuItem _holden;
        private AgileMenuItem _ford;
        private AgileMenuItem _car;

        /// <summary>
        /// Setup some basic menu items for testing
        ///     - Create two child menu items
        ///     - Create a 'parent'(car) menu item, with the children
        ///     - Create a collection
        ///     - Add the car menu items to the collection
        /// </summary>
        /// <returns></returns>
        private AgileMenuItemCollection SetupTestCarMenuItems()
        {
            // Create two child menu items
            _holden = new AgileMenuItem("Holden");
            _ford = new AgileMenuItem("Ford");
            var childItems = new AgileMenuItem[2];
            childItems[0] = _holden;
            childItems[1] = _ford;

            // Create a 'parent' menu item, with the children
            _car = new AgileMenuItem("Car", childItems);
            AgileMenuItemCollection collection = AgileMenuItemCollection.Build();
            collection.Add(_car);
            return collection;
        }

        /// <summary>
        /// Verifies ContainsMenuItemIncludingChildItems returns the expected results
        ///     - Setup menu items for testing
        ///     - Check ContainsMenuItemIncludingChildItemsTests returns true for car
        ///     - Check ContainsMenuItemIncludingChildItemsTests returns true for holden
        /// </summary>
        [Test]
        public void ContainsMenuItemIncludingChildItemsTests()
        {
            // Setup menu items for testing
            AgileMenuItemCollection collection = SetupTestCarMenuItems();
            // Check ContainsMenuItemIncludingChildItemsTests returns true for car
            Assert.IsTrue(collection.ContainsMenuItemIncludingChildItems(_car)
                          , "Check ContainsMenuItemIncludingChildItemsTests returns true for car");
            // Check ContainsMenuItemIncludingChildItemsTests returns true for holden
            Assert.IsTrue(collection.ContainsMenuItemIncludingChildItems(_holden)
                          , "Check ContainsMenuItemIncludingChildItemsTests returns true for holden");
        }

        /// <summary>
        /// Verifies the Contains function works as expected.
        ///     - Setup menu items for testing
        ///     - Check the collection contains the car item
        ///     - Check the collection doesn't Contain holden
        /// </summary>
        [Test]
        public void ContainsTests()
        {
            // Setup menu items for testing
            AgileMenuItemCollection collection = SetupTestCarMenuItems();
            // Check the collection contains the car item
            Assert.IsTrue(collection.Contains(_car), "Check the collection contains the car item");
            // Check the collection doesn't Contain holden
            Assert.IsFalse(collection.Contains(_holden), "Check the collection doesn't Contain holden");
        }

        /// <summary>
        /// Verifies we can get a specific menu item out of a collection by looking
        /// for the text of the item
        ///     - Setup menu items for testing
        ///     - Check FindMenuItemByText returns null for "toyota"
        ///     - Check FindMenuItemByText returns a menu item for car
        ///     - Check FindMenuItemByText returns a menu item for holden
        /// </summary>
        [Test]
        public void FindMenuItemByTextTests()
        {
            // Setup menu items for testing
            AgileMenuItemCollection collection = SetupTestCarMenuItems();
            // Check FindMenuItemByText returns null for "toyota"
            Assert.IsNull(collection.FindMenuItemByText("toyota"), "Check FindMenuItemByText returns null for 'toyota'");
            // Check FindMenuItemByText returns a menu item for car
            Assert.IsNotNull(collection.FindMenuItemByText("Car"),
                             "Check FindMenuItemByText returns a menu item for car");
            // Check FindMenuItemByText returns a menu item for holden
            Assert.IsNotNull(collection.FindMenuItemByText("Holden"),
                             "Check FindMenuItemByText returns a menu item for holden");
            // Check FindMenuItemByText returns a menu item for ford
            Assert.IsNotNull(collection.FindMenuItemByText("Ford"),
                             "Check FindMenuItemByText returns a menu item for ford");

            // Check case sensitivity
            Assert.IsNull(collection.FindMenuItemByText("car"), "Check case sensitivity");
        }

        /// <summary>
        /// Verifies AgileMenuItems are cached.
        ///     - Check the Assemblies.AgileMenuItems has at least 2 items
        ///     - Check the Assemblies.AgileMenuItems contains WidgetMenuItem
        ///     - Check the Assemblies.AgileMenuItems contains WidgetOtherMenuItem
        ///     - Check it doesnt contain widget
        /// </summary>
        [Test]
        public void MenuItemCachingTests()
        {
            AgileGUIFactoryTester factoryTester = AgileGUIFactoryTester.Build();
            // Check the Assemblies.AgileMenuItems has at least 2 items
            Assert.IsTrue(factoryTester.AgileMenuItemTypes.Count > 1
                          , "Check the Assemblies.AgileMenuItems has at least 2 items");
            // Check the Assemblies.AgileMenuItems contains WidgetMenuItem
            Assert.IsTrue(factoryTester.AgileMenuItemTypes.Contains(typeof (WidgetMenuItem))
                          , "Check the Assemblies.AgileMenuItems contains WidgetMenuItem");
            // Check the Assemblies.AgileMenuItems contains WidgetOtherMenuItem
            Assert.IsTrue(factoryTester.AgileMenuItemTypes.Contains(typeof (WidgetOtherMenuItem))
                          , "Check the Assemblies.AgileMenuItems contains WidgetOtherMenuItem");
            // Check it doesnt contain widget
            Assert.IsFalse(factoryTester.AgileMenuItemTypes.Contains(typeof (Widget))
                           , "Check it doesnt contain widget");
        }
    }
}