using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Agile.Diagnostics.Logging;
using Microsoft.Practices.Unity;

namespace Agile.Framework.Services
{
    /// <summary>
    /// Helper to automatically register types in the Unity Container using reflection.
    /// Saves manually registering things like web services where there will be many.
    /// </summary>
    public static class UnityRegisterHelper
    {
        /// <summary>
        /// Ctor for Unity 
        /// </summary>
        public static void RegisterContainer(IUnityContainer unityContainer)
        {
            container = unityContainer;
        }

        private static IUnityContainer container;
        /// <summary>
        /// Returns the unity container
        /// </summary>
        public static IUnityContainer Container
        {
            get { return container; }
        }
        
        /// <summary>
        /// Registers all services implementing the given interface, in the given assembly.
        /// </summary>
        /// <param name="implementing">name of the interface that types must implement for them to be registered.</param>
        /// <param name="searchAssemblyNames">if the interface and concrete types are in the same assembly
        /// just provide that assembly name, otherwise you'll need to provide both assembly names.</param>
        /// <remarks>Use this overload if GetSearchAssemblies doesn't contain the assembly(s) that contain 
        /// the interfaces or types.
        /// This will happen if the running project doesn't have any in code reference to the other assembly.</remarks>
        public static List<Type> RegisterTypesThatImplement(Type implementing, List<string> searchAssemblyNames)
        {
            var searchAssemblies = new List<Assembly>();
            foreach (string assemblyName in searchAssemblyNames)
            {
                try
                {
                    var searchAssembly = Assembly.Load(assemblyName);
                    searchAssemblies.Add(searchAssembly);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    throw;
                }
            }

            return RegisterTypesThatImplement(implementing, searchAssemblies);
        }

        /// <summary>
        /// Registers all services implementing the given interface, in the given assembly.
        /// </summary>
        /// <returns>List of interfaces that implement the interface</returns>
        public static List<Type> RegisterTypesThatImplement(Type implementing, List<Assembly> searchAssemblies)
        {
            if (container == null) throw new Exception("Register the UnityContainer before trying to register types.");

            Logger.Setup(string.Format("Registering types that are that implement {0}", implementing.Name));

            var interfaces = GetInterfacesThatImplement(implementing, searchAssemblies);
            foreach (Type inter in interfaces)
            {
                var types = GetConcreteTypesThatImplement(inter, searchAssemblies);
                if (types.Count == 0)
                {
                    Logger.Warning("Failed to find a concrete type for {0}", inter.Name);
                    continue;
                }
                // should only be one but may be more if one is a Mock, remove any with a name ending in Mock
                if (types.Count > 1)
                {
#if SILVERLIGHT
                    for (int i = types.Count; i > 0; i--)
                    {
                        if (types[i].Name.EndsWith("Mock"))
                            types.RemoveAt(i);
                    }
#else
                    types.RemoveAll(type => type.Name.EndsWith("Mock")); 
#endif
                }

                // beyond that, for now just register the first item.
                Logger.Setup(string.Format("Registering {0} for {1}", types[0].Name, inter.Name));
                container.RegisterType(inter, types[0], new ContainerControlledLifetimeManager());
            }
            return interfaces;

        }
        
        /// <summary>
        /// Gets all interfaces that implement the given interface
        /// </summary>
        public static List<Type> GetInterfacesThatImplement(Type implementing, List<Assembly> assemblies)
        {
            var interfaces = (from assembly in assemblies          
                    from type in assembly.GetTypes()
                    where type.IsInterface
                    from implements in type.GetInterfaces()
                    where implements == implementing
                    select type).ToList();
            Logger.Setup("{0} interfaces found that implement {1}", interfaces.Count, implementing.Name);
            return interfaces;
        }

        /// <summary>
        /// Gets all concrete types that implement the given interface
        /// </summary>
        /// <remarks>Prob need to add an exclude later to ensure coded mocks don't get included</remarks>
        public static List<Type> GetConcreteTypesThatImplement(Type implementing, List<Assembly> assemblies)
        {
            var types = (from assembly in assemblies
                    from type in assembly.GetTypes()
                    where !type.IsInterface && ! type.IsAbstract
                    from implements in type.GetInterfaces()
                    where implements == implementing
                    select type).ToList();
            Logger.Setup("{0} concrete types found that implement {1}", types.Count, implementing.Name);
            return types;
        }

#if !SILVERLIGHT
        public static List<Assembly> GetSearchAssemblies()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(IsOneOfOurs).ToList();

            var assembliesString = new StringBuilder("Search Assemblies \r\n");

            assemblies.ForEach(item => assembliesString.AppendLine("    " + item.GetName().Name));
            Logger.Setup(assembliesString.ToString());
            return assemblies;
        }
        
        /// <summary>
        /// Gets all interfaces that implement the given interface
        /// </summary>
        public static List<Type> GetInterfacesThatImplement(Type implementing)
        {
            return GetInterfacesThatImplement(implementing, GetSearchAssemblies());
        }
        
        /// <summary>
        /// Registers all services implementing the given interface, in the given assembly.
        /// </summary>
        public static void RegisterTypesThatImplement(Type implementing)
        {
            RegisterTypesThatImplement(implementing, GetSearchAssemblies());
        }
#endif

        /// <summary>
        /// Returns true if the assembly is one of our assemblies
        /// </summary>
        public static bool IsOneOfOurs(Assembly assembly)
        {
            return IsOneOfOurs(assembly.GetName().Name);
        }

        /// <summary>
        /// Returns true if the assembly is one of our assemblies
        /// </summary>
        /// <param name="assemblyName">Name of the assembly to check</param>
        public static bool IsOneOfOurs(string assemblyName)
        {
            return !
                   (assemblyName.StartsWith("microsoft", StringComparison.InvariantCultureIgnoreCase)
                    || assemblyName.StartsWith("mscorlib", StringComparison.InvariantCultureIgnoreCase)
                    || assemblyName.StartsWith("nunit", StringComparison.InvariantCultureIgnoreCase)
                    || assemblyName.StartsWith("jetbrains", StringComparison.InvariantCultureIgnoreCase)
                    || assemblyName.StartsWith("system", StringComparison.InvariantCultureIgnoreCase)
                    || assemblyName.StartsWith("Wintellect.", StringComparison.InvariantCultureIgnoreCase)
                    || assemblyName.StartsWith("anonymous", StringComparison.InvariantCultureIgnoreCase)
                    || assemblyName.Equals("Accessibility", StringComparison.InvariantCultureIgnoreCase)
                   );
        }
    }
}