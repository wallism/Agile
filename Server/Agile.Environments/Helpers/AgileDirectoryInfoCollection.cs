// WARNING: This file has been generated. Any manual changes must be made within preserved regions or they will be lost.

//===============================================================================
//
// AgileDirectoryInfoCollection
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

using System.IO;

namespace Agile.Common
{
    /// <summary>
    /// AgileDirectoryInfoCollection
    /// </summary>
    public class AgileDirectoryInfoCollection : AgileCollection
    {
        #region Preserved Region - Developer Hand Written Code

        /// <summary>
        /// Adds a collection of AgileDirectoryInfos to the collection.
        /// </summary>
        /// <param name="directories">Collection of directories to add to this collection.</param>
        public void AddRange(AgileDirectoryInfoCollection directories)
        {
            foreach (AgileDirectoryInfo directoryInfo in directories)
                Add(directoryInfo);
        }

        /// <summary>
        /// Adds an arrya of AgileDirectoryInfos to the collection.
        /// </summary>
        /// <param name="directories">Array of directories to add to this collection.</param>
        public void AddRange(AgileDirectoryInfo[] directories)
        {
            foreach (AgileDirectoryInfo DirectoryInfo in directories)
                Add(DirectoryInfo);
        }

        /// <summary>
        /// Adds an array of AgileDirectoryInfos to the collection.
        /// </summary>
        /// <param name="directoryInfos">Array of directories to add to this collection.</param>
        public void AddRange(DirectoryInfo[] directoryInfos)
        {
            foreach (DirectoryInfo directoryInfo in directoryInfos)
                Add(AgileDirectoryInfo.Build(directoryInfo));
        }

        /// <summary>
        /// Adds an array of AgileDirectoryInfos to the collection.
        /// </summary>
        /// <param name="directoryInfos">Array of directories to add to this collection.</param>
        /// <param name="excludeMatches">Excludes an directory that matches or partially matches strings provided here.</param>
        public void AddRange(DirectoryInfo[] directoryInfos, params string[] excludeMatches)
        {
            foreach (DirectoryInfo directoryInfo in directoryInfos)
            {
                // Don't add the directory if its name contains an excludeMatches string.
                if(! ShouldDirectoryBeExcluded(directoryInfo, excludeMatches))
                    continue;
                
                Add(AgileDirectoryInfo.Build(directoryInfo));
            }
        }

        private static bool ShouldDirectoryBeExcluded(DirectoryInfo directory, params string[] excludeMatches)
        {
            if (excludeMatches == null || excludeMatches.Length == 0)
                return true;
                
            foreach (string excludeMatch in excludeMatches)
            {
                if (directory.Name.Contains(excludeMatch))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Checks if the given DirectoryInfo is in the collection.
        /// </summary>
        /// <param name="directoryInfo">DirectoryInfo to look for in the collection.</param>
        /// <returns>True if the item is in the collection.</returns>
        public virtual bool Contains(DirectoryInfo directoryInfo)
        {
            ArgumentValidation.CheckForNullReference(directoryInfo, "directoryInfo");

            foreach (AgileDirectoryInfo item in List)
            {
                if (item.DirectoryInfo.Equals(directoryInfo))
                    return true;
            }
            return false;
        }

        #endregion // Preserved Region - Developer Hand Written Code

        #region Constructors and Factories

        /// <summary>
        /// Constructor
        /// </summary>
        internal AgileDirectoryInfoCollection()
        {
        }

        /// <summary>
        /// Instantiate a new AgileDirectoryInfoCollection
        /// </summary>
        public static AgileDirectoryInfoCollection Build()
        {
            return new AgileDirectoryInfoCollection();
        }

        /// <summary>
        /// Instantiate a new AgileDirectoryInfoCollection with the given directory.
        /// Returns a collection of its 'child' directories
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="excludeMatches">Excludes an directory that matches or partially matches strings provided here.</param>
        public static AgileDirectoryInfoCollection Build(DirectoryInfo directory, params string[] excludeMatches)
        {
            var directories = new AgileDirectoryInfoCollection();
            directories.AddRange(directory.GetDirectories(), excludeMatches);
            
            return directories;
        }

        /// <summary>
        /// Instantiate a new AgileDirectoryInfoCollection with the given directory.
        /// Returns a collection of its 'child' directories
        /// </summary>
        public static AgileDirectoryInfoCollection Build(DirectoryInfo directory)
        {
            return Build(directory, null);
        }
        #endregion

        #region Strongly Typed IList Implementations

        /// <summary>
        /// Returns the AgileDirectoryInfo that is at the given index in the collection.
        /// </summary>
        public AgileDirectoryInfo this[int index]
        {
            get { return InnerList[index] as AgileDirectoryInfo; }
        }

        /// <summary>
        /// Add a new AgileDirectoryInfo to the collection
        /// </summary>
        /// <param name="agileDirectoryInfo">agileDirectoryInfo to add to the collection.</param>
        public void Add(AgileDirectoryInfo agileDirectoryInfo)
        {
            if (Contains(agileDirectoryInfo))
            {
                string message =
                    string.Format("Tried adding a AgileDirectoryInfo to the collection but it is already in it.");
                throw new AgileCommonException(message);
            }
            List.Add(agileDirectoryInfo);
        }

        /// <summary>
        /// Remove a AgileDirectoryInfo from the collection
        /// </summary>
        /// <param name="agileDirectoryInfo">agileDirectoryInfo to remove from the collection.</param>
        public void Remove(AgileDirectoryInfo agileDirectoryInfo)
        {
            InnerList.Remove(agileDirectoryInfo);
        }

        /// <summary>
        /// Checks if the given AgileDirectoryInfo is in the collection
        /// </summary>
        /// <param name="agileDirectoryInfo">agileDirectoryInfo to look for in the collection.</param>
        /// <returns>True if the item is in the collection.</returns>
        public bool Contains(AgileDirectoryInfo agileDirectoryInfo)
        {
            return InnerList.Contains(agileDirectoryInfo);
        }

        /// <summary>
        /// Searches for the specified AgileDirectoryInfo and returns the zero-based index
        /// of the first occurrence within the section of the
        /// collection that starts at the
        /// specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="agileDirectoryInfo">agileDirectoryInfo to get the index of from the collection.</param>
        /// <returns>The index of the item in the collection.</returns>
        public int IndexOf(AgileDirectoryInfo agileDirectoryInfo)
        {
            return InnerList.IndexOf(agileDirectoryInfo);
        }

        /// <summary>
        /// Searches for the specified AgileDirectoryInfo and returns the zero-based index
        /// of the first occurrence within the section of the
        /// collection that starts at the
        /// specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="agileDirectoryInfo">agileDirectoryInfo to get the index of from the collection.</param>
        /// <param name="startIndex">The zero-based starting index of the search.</param>
        /// <returns>The index of the item in the collection.</returns>
        public int IndexOf(AgileDirectoryInfo agileDirectoryInfo, int startIndex)
        {
            return InnerList.IndexOf(agileDirectoryInfo, startIndex);
        }

        /// <summary>
        /// Searches for the specified AgileDirectoryInfo and returns the zero-based index
        /// of the first occurrence within the section of the
        /// collection that starts at the
        /// specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="agileDirectoryInfo">agileDirectoryInfo to get the index of from the collection.</param>
        /// <param name="startIndex">The zero-based starting index of the search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <returns>The index of the item in the collection.</returns>
        public int IndexOf(AgileDirectoryInfo agileDirectoryInfo, int startIndex, int count)
        {
            return InnerList.IndexOf(agileDirectoryInfo, startIndex, count);
        }

        /// <summary>
        /// Inserts the AgileDirectoryInfo into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which value should be inserted.</param>
        /// <param name="agileDirectoryInfo">agileDirectoryInfo to insert into the collection.</param>
        public void Insert(int index, AgileDirectoryInfo agileDirectoryInfo)
        {
            InnerList.Insert(index, agileDirectoryInfo);
        }

        /// <summary>
        /// Copies a range of elements from the collection of AgileDirectoryInfo's to a compatible
        /// one-dimensional System.Array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="agileDirectoryInfos">The one-dimensional System.Array that is the destination of the elements copied from the collection. The System.Array must have zero-based indexing.</param>
        /// <param name="index">The zero-based index at which copying begins.</param>
        public void CopyTo(AgileDirectoryInfo[] agileDirectoryInfos, int index)
        {
            List.CopyTo(agileDirectoryInfos, index);
        }

        #endregion
    }
}