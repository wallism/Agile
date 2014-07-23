using System;
using System.Collections.Generic;

namespace Agile.Common.Reflections
{
    /// <summary>
    /// AgileTypeCollection
    /// </summary>
    public class AgileTypeCollection : List<AgileType>
    {

        #region Constructors and Factories

        /// <summary>
        /// Constructor
        /// </summary>
        internal AgileTypeCollection()
        {
        }

        /// <summary>
        /// Instantiate a new TypeCollection
        /// </summary>
        public static AgileTypeCollection Build()
        {
            return new AgileTypeCollection();
        }

        #endregion

        /// <summary>
        /// Add an array of system types to the list (a new AgileType will be created for each)
        /// </summary>
        public void AddRange(params Type[] types)
        {
            foreach (Type type in types)
            {
                AgileType agile = AgileType.Build(type);
                AddUnique(agile);
            }
        }

        /// <summary>
        /// Returns true if the given system type has a corresponding AgileType in the list.
        /// </summary>
        public bool Contains(Type systemType)
        {
            foreach (AgileType agileType in this)
            {
                if (agileType.SystemType.Equals(systemType))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Add a new Type to the collection, ensures that duplicates are not added.
        /// </summary>
        /// <param name="type">type to add to the collection.</param>
        public void AddUnique(AgileType type)
        {
            if (!Contains(type))
                Add(type);
        }
    }
}