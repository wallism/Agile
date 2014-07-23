using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Forms;
using Agile.Common;
using Agile.Shared;


//===============================================================================
//
// AgileMenuItemCollection
//
// PURPOSE: 
// 
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

namespace Agile.WinUI
{
    /// <summary>
    /// AgileMenuItemCollection
    /// </summary>
    public class AgileMenuItemCollection : List<AgileMenuItem>
    {
        #region Preserved Region - Developer Hand Written Code

        /// <summary>
        /// Adds a collection of Agile menu items to the collection
        /// </summary>
        /// <param name="menus">the collection of menu items to add.</param>
        public void AddRange(AgileMenuItemCollection menus)
        {
            foreach (AgileMenuItem menu in menus)
                Add(menu);
        }


        /// <summary>
        /// Searches this collection for the menu item AND also
        /// seaches any child menu items of each menu item (and their child items etc). 
        /// </summary>
        /// <param name="itemToFind">Search for this menu item.</param>
        /// <returns>True if the item is found.</returns>
        public bool ContainsMenuItemIncludingChildItems(AgileMenuItem itemToFind)
        {
            ArgumentValidation.CheckForNullReference(itemToFind, "itemToFind");

            Debug.Write(string.Format("Searching for {0}.\n"
                                      , itemToFind.Text));

            foreach (AgileMenuItem item in this)
            {
                if (item.Equals(itemToFind))
                    return true;
                if (ContainsMenuItem(item.MenuItems, itemToFind))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Searches the given collection for the menu item AND also
        /// seaches any child menu items of each menu item (and their child items etc). 
        /// </summary>
        /// <param name="itemsToSearch">Array of items to search</param>
        /// <param name="itemToFind">Search for this menu item.</param>
        /// <returns>True if the item is found.</returns>
        private static bool ContainsMenuItem(Menu.MenuItemCollection itemsToSearch
                                             , AgileMenuItem itemToFind)
        {
            foreach (AgileMenuItem item in itemsToSearch)
            {
                Debug.Write(string.Format("Searching in {0}.\n"
                                          , item.Text));
                if (item.Equals(itemToFind))
                    return true;
                if (ContainsMenuItem(item.MenuItems, itemToFind))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Searches this collection for a menu item with the given Text value.
        /// NOTE: Also seaches any child menu items of each menu item 
        /// (and their child items etc). 
        /// </summary>
        /// <remarks>Case Sensitive!</remarks>
        /// <param name="text">Search for a menu item that has this text.</param>
        /// <returns>The menu item that has the given text or null if not found.</returns>
        public AgileMenuItem FindMenuItemByText(string text)
        {
            ArgumentValidation.CheckForNullReference(text, "text");

            Debug.Write(string.Format("Searching for menu item with Text: {0}.\n"
                                      , text));

            foreach (AgileMenuItem item in this)
            {
                if (item.Text.Equals(text))
                    return item;

                AgileMenuItem foundItem = FindMenuItemByText(item.MenuItems, text);
                if (foundItem != null)
                    return foundItem;
            }
            return null;
        }

        /// <summary>
        /// Searches the given collection for the menu item AND also
        /// seaches any child menu items of each menu item (and their child items etc). 
        /// </summary>
        /// <remarks>Case Sensitive!</remarks>
        /// <param name="itemsToSearch">Array of items to search</param>
        /// <param name="text">Search for a menu item that has this text.</param>
        /// <returns>The menu item that has the given text or null if not found.</returns>
        private static AgileMenuItem FindMenuItemByText(Menu.MenuItemCollection itemsToSearch
                                                        , string text)
        {
            foreach (AgileMenuItem item in itemsToSearch)
            {
                Debug.Write(string.Format("Searching in {0}.\n"
                                          , item.Text));
                if (item.Text.Equals(text))
                    return item;

                AgileMenuItem foundItem = FindMenuItemByText(item.MenuItems, text);
                if (foundItem != null)
                    return foundItem;
            }
            return null;
        }

        #endregion // Preserved Region - Developer Hand Written Code

        #region Constructors and Factories

        /// <summary>
        /// Instantiate a new AgileMenuItemCollection
        /// </summary>
        public static AgileMenuItemCollection Build()
        {
            return new AgileMenuItemCollection();
        }

        #endregion

    }
}