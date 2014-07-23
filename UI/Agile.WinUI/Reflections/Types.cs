using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Agile.Common.Reflections
{
    /// <summary>
    /// Summary description for Types.
    /// </summary>
    public class Types
    {
        /// <summary>
        /// Do not contruct
        /// </summary>
        private Types()
        {
        }

        /// <summary>
        /// Returns true if a Type of the given name is in the given Assembly.
        /// </summary>
        /// <param name="assemblyToSearch">Assembly to search through</param>
        /// <param name="nameOfTypeToFind">Name of the type to look for.</param>
        /// <returns>True if there is a Type of the given name in the Assembly.</returns>
        public static bool ContainsTypeNamed(Assembly assemblyToSearch, string nameOfTypeToFind)
        {
            return GetTypeNamed(assemblyToSearch, nameOfTypeToFind) != null;
        }

        /// <summary>        
        /// Gets the Type of the given name from the given Assemblies.
        /// </summary>
        /// <remarks>CASE sensitive and will get the first Type found.</remarks>
        /// <param name="assembliesToSearch">Assemblies to search through</param>
        /// <param name="nameOfTypeToFind">Name of the type to look for.</param>
        /// <returns>null if the type is not found, otherwise the Type.</returns>
        public static Type GetTypeNamed(List<AgileAssembly> assembliesToSearch, string nameOfTypeToFind)
        {
            foreach (AgileAssembly agileAssembly in assembliesToSearch)
            {
                Type type = GetTypeNamed(agileAssembly.Assembly, nameOfTypeToFind);
                if (type != null)
                    return type;
            }
            return null;
        }

        /// <summary>        
        /// Gets the Type of the given name from the given Assembly.
        /// </summary>
        /// <remarks>CASE sensitive</remarks>
        /// <param name="assemblyToSearch">Assembly to search through</param>
        /// <param name="nameOfTypeToFind">Name of the type to look for.</param>
        /// <returns>null if the type is not found, otherwise the Type.</returns>
        public static Type GetTypeNamed(Assembly assemblyToSearch, string nameOfTypeToFind)
        {
            foreach (Type type in assemblyToSearch.GetTypes())
            {
                if (type.Name.Equals(nameOfTypeToFind))
                    return type;
            }
            return null;
        }

        /// <summary>
        /// Returns true if the child types inheritance tree eventually
        /// leads to the base type.
        /// </summary>
        /// <param name="childType">Check the inheritance of this type</param>
        /// <param name="baseType">Is this Type in the Childs inheritance tree</param>
        /// <returns></returns>
        public static bool IsDecendantOf(Type childType, Type baseType)
        {
            if (childType.BaseType == null)
                return false;
            if (IsDirectDecendantOf(childType, baseType))
                return true;
            return IsDecendantOf(childType.BaseType, baseType);
        }

        /// <summary>
        /// Returns true if the child type DIRECTLY inherits from
        /// the base type.
        /// </summary>
        /// <param name="childType">Check the inheritance of this type</param>
        /// <param name="baseType">Check if the child type directly inherits from this type</param>
        /// <returns></returns>
        public static bool IsDirectDecendantOf(Type childType, Type baseType)
        {
            if (childType.BaseType == null)
                return false;
            if (childType.BaseType.Equals(baseType))
                return true;
            return false;
        }

        /// <summary>
        /// Retrieves all sub classes of a particular type. i.e. All classes
        /// that either directly or indirectly inherit from the super type.
        /// </summary>
        /// <param name="assembliesToSearch">Search these assemblies for all sub types.</param>
        /// <param name="baseType">The base (super) type that returned types must inherit from.</param>
        /// <param name="includeAbstractClasses">Set to true if you want to also include Abstract classes.</param>
        /// <returns></returns>
        public static AgileTypeCollection GetAllSubClassesOf(List<AgileAssembly> assembliesToSearch
                                                             , Type baseType
                                                             , bool includeAbstractClasses)
        {
            var subTypes = new AgileTypeCollection();
            foreach (AgileAssembly assembly in assembliesToSearch)
                subTypes.AddRange(GetAllSubClassesOf(assembly.Assembly, baseType, includeAbstractClasses));
            return subTypes;
        }

        /// <summary>
        /// Retrieves all sub classes of a particular type. i.e. All classes
        /// that either directly or indirectly inherit from the super type.
        /// </summary>
        /// <param name="assemblyToSearch">Search this assembly for all sub types.</param>
        /// <param name="baseType">Base the base (super) type that returned types must inherit from.</param>
        /// <param name="includeAbstractClasses">Set to true if you want to also include Abstract classes.</param>
        /// <returns></returns>
        public static AgileTypeCollection GetAllSubClassesOf(Assembly assemblyToSearch
                                                             , Type baseType
                                                             , bool includeAbstractClasses)
        {
            var subTypes = new AgileTypeCollection();
            foreach (Type typeInAssembly in assemblyToSearch.GetTypes())
            {
                if (!includeAbstractClasses && typeInAssembly.IsAbstract)
                    continue;
                if (IsDecendantOf(typeInAssembly, baseType))
                {
                    subTypes.Add(AgileType.Build(typeInAssembly));
                    Debug.Write(string.Format("Added {0} to Type collection of {1}\n"
                                              , typeInAssembly.Name
                                              , baseType.Name));
                }
            }
            subTypes.Sort();
            return subTypes;
        }

        /// <summary>
        /// Retrieves all direct decendants of a particular type. i.e. All classes
        /// that directly inherit from the super type.
        /// </summary>
        /// <param name="assemblyToSearch">Search this assembly.</param>
        /// <param name="baseType">The base (super) type that returned types must inherit from.</param>
        /// <param name="includeAbstractClasses">Set to true if you want to also include Abstract classes.</param>
        /// <returns></returns>
        public static AgileTypeCollection GetDirectDecendantsOf(Assembly assemblyToSearch
                                                                , Type baseType
                                                                , bool includeAbstractClasses)
        {
            var directDecendants = new AgileTypeCollection();
            foreach (Type typeInAssembly in assemblyToSearch.GetTypes())
            {
                if (!includeAbstractClasses && typeInAssembly.IsAbstract)
                    continue;
                if (IsDirectDecendantOf(typeInAssembly, baseType))
                    directDecendants.Add(AgileType.Build(typeInAssembly));
            }
            directDecendants.Sort();
            return directDecendants;
        }

        /// <summary>
        /// Retrieves the direct decendants of a particular type. i.e. All classes
        /// that directly inherit from the super type.
        /// </summary>
        /// <param name="assembliesToSearch">Search these assemblies.</param>
        /// <param name="baseType">The base (super) type that returned types must inherit directly from.</param>
        /// <param name="includeAbstractClasses">Set to true if you want to also include Abstract classes.</param>
        /// <returns></returns>
        public static AgileTypeCollection GetDirectDecendantsOf(List<AgileAssembly> assembliesToSearch
                                                                , Type baseType
                                                                , bool includeAbstractClasses)
        {
            var subTypes = new AgileTypeCollection();
            foreach (AgileAssembly assembly in assembliesToSearch)
                subTypes.AddRange(GetDirectDecendantsOf(assembly.Assembly, baseType, includeAbstractClasses));
            return subTypes;
        }


        /// <summary>
        /// Returns the collection of Types that are in the given assembly.
        /// </summary>
        /// <param name="assembly">Assembly to get the contained types from.</param>
        /// <returns></returns>
        public static AgileTypeCollection GetAllTypesIn(Assembly assembly)
        {
            AgileTypeCollection agileTypes = AgileTypeCollection.Build();
            agileTypes.AddRange(assembly.GetTypes());
            agileTypes.Sort();
            return agileTypes;
        }

        /// <summary>
        /// Returns a collection of Types that implement the given interface
        /// </summary>
        /// <param name="assembliesToSearch">Search these assemblies.</param>
        /// <param name="interfaceImplemented">The interface that Types must implement to be included in the returned collection.</param>
        /// <returns></returns>
        public static AgileTypeCollection GetClassesImpementing(List<AgileAssembly> assembliesToSearch
                                                                , Type interfaceImplemented)
        {
            if (!interfaceImplemented.IsInterface)
                throw new Exception(string.Format("Tried to find classes implementing '{0}' but it is not an interface.",
                                  interfaceImplemented.Name));

            var subTypes = new AgileTypeCollection();
            foreach (AgileAssembly assembly in assembliesToSearch)
                subTypes.AddRange(GetClassesImpementing(assembly.Assembly, interfaceImplemented));
            return subTypes;
        }

        /// <summary>
        /// Returns a collection of Types that implement the given interface
        /// </summary>
        /// <param name="assembly">The assembly to search.</param>
        /// <param name="interfaceImplemented">The interface that Types must implement to be included in the returned collection.</param>
        /// <returns></returns>
        public static AgileTypeCollection GetClassesImpementing(Assembly assembly, Type interfaceImplemented)
        {
            var typesThatImplementTheInterface = new AgileTypeCollection();
            foreach (AgileType typeToCheck in GetAllTypesIn(assembly))
            {
                AgileTypeCollection interfaces = AgileTypeCollection.Build();
                interfaces.AddRange(typeToCheck.SystemType.GetInterfaces());
                foreach (AgileType agileType in interfaces)
                {
                    if (agileType.SystemType == interfaceImplemented)
                        typesThatImplementTheInterface.Add(typeToCheck);
                }
            }
            typesThatImplementTheInterface.Sort();
            return typesThatImplementTheInterface;
        }

        /// <summary>
        /// Returns a collection of Types that declare a specific Attribute
        /// </summary>
        /// <param name="assembliesToSearch">Search these assemblies.</param>
        /// <param name="attributeType">The Type of the attribute that classes must have
        /// declared to be included in the returned collection.</param>
        /// <returns></returns>
        public static AgileTypeCollection GetClassesWithAttribute(List<AgileAssembly> assembliesToSearch
                                                                  , Type attributeType)
        {
            if (!(IsDecendantOf(attributeType, typeof (Attribute))))
                throw new Exception(string.Format("Tried to find classes that declare the Attribute '{0}' but it is not an Attribute!",
                                  attributeType.Name));

            var subTypes = new AgileTypeCollection();
            foreach (AgileAssembly assembly in assembliesToSearch)
                subTypes.AddRange(GetClassesWithAttribute(assembly.Assembly, attributeType));

            return subTypes;
        }

        /// <summary>
        /// Returns a collection of Types that declare a specific Attribute
        /// </summary>
        /// <param name="assembly">The assembly to search.</param>
        /// <param name="attributeType">The Attribute Type that Types must declare 
        /// to be included in the returned collection.</param>
        /// <returns></returns>
        public static AgileTypeCollection GetClassesWithAttribute(Assembly assembly, Type attributeType)
        {
            var typesThatDeclareTheAttribute = new AgileTypeCollection();
            foreach (AgileType type in GetAllTypesIn(assembly))
            {
                object[] attributes = type.SystemType.GetCustomAttributes(attributeType, false);
                if (attributes.Length > 0)
                {
                    typesThatDeclareTheAttribute.Add(type);
                }
            }
            return typesThatDeclareTheAttribute;
        }
    }
}