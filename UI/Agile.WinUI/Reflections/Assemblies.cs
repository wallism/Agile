using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Agile.Common.Reflections
{
    /// <summary>
    /// Singleton class used to cache and retrieve Assembly info
    /// </summary>
    public class Assemblies
    {
        #region Singleton Implementation

        private static readonly object padlock = new object();

        /// <summary>
        /// The unique instance of the Assemblies object.
        /// </summary>
        private static volatile Assemblies _uniqueInstance;

        /// <summary>
        /// Private constructor to lock down instantiation
        /// </summary>
        private Assemblies()
        {
        }

        /// <summary>
        /// Get the instance of the Assemblies object
        /// </summary>
        /// <returns></returns>
        public static Assemblies Instance
        {
            get
            {
                if (_uniqueInstance == null)
                {
                    lock (padlock)
                    {
                        if (_uniqueInstance == null)
                            _uniqueInstance = new Assemblies();
                    }
                }
                return _uniqueInstance;
            }
        }

        #endregion

        /// <summary>
        /// Cached copy of loaded assemblies
        /// </summary>
        private static List<AgileAssembly> _loadedAssemblies;

        /// <summary>
        /// Cached copy of loaded assemblies
        /// </summary>
        private static List<AgileAssembly> _ourLoadedAssemblies;

        /// <summary>
        /// Gets all of the loaded assemblies.
        /// </summary>
        public List<AgileAssembly> AllLoadedAssemblies
        {
            get
            {
                if (_loadedAssemblies == null || _loadedAssemblies.Count == 0)
                {
                    LoadAllAssemblies();
                }
                return _loadedAssemblies;
            }
        }

        /// <summary>
        /// Gets all of our loaded assemblies.
        /// </summary>
        public List<AgileAssembly> OurAssemblies
        {
            get
            {
                if (_ourLoadedAssemblies == null || _ourLoadedAssemblies.Count == 0)
                {
                    LoadOurAssemblies();
                }
                return _ourLoadedAssemblies;
            }
            set 
            {
                _ourLoadedAssemblies = value;
            }
        }

        /// <summary>
        /// Invalidate the cache and cause everything to be reloaded again
        /// next time it is accessed.
        /// </summary>
        /// <remarks>Make sure you know what you're doing when you call this!</remarks>
        public void InvalidateCache()
        {
            _loadedAssemblies = null;
            _ourLoadedAssemblies = null;
        }

        /// <summary>
        /// Load all of the standard assemblies.
        /// </summary>
        private void LoadAllAssemblies()
        {
            _loadedAssemblies = new List<AgileAssembly>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Debug.Write(string.Format("Adding '{0}' assembly to AllLoadedAssemblies.\n", assembly.FullName));
                _loadedAssemblies.Add(AgileAssembly.Build(assembly));
            }

            // Cannot use Environment.CurrentDirectory. 
            // When running as unit tests it's fine but when
            // running as a WCF server then Environment.CurrentDirectory 
            // returns a windows directory.
            // RelativeSearchPath will be empty for unit tests but when
            // running as WCF server then it will have the required bin dir
            // for the application.
//            string path = AppDomain.CurrentDomain.RelativeSearchPath;
//            if (string.IsNullOrEmpty(path))
//                path = AppDomain.CurrentDomain.BaseDirectory;
//            AddUnloadedAssembliesIn(path);

//            _loadedAssemblies.Sort(); commented out 20140124 mw
        }


        /// <summary>
        /// Loads our assemblies (fills the array of Assemblies)
        /// </summary>
        private void LoadOurAssemblies()
        {
            _ourLoadedAssemblies = new List<AgileAssembly>();
            foreach (AgileAssembly assembly in AllLoadedAssemblies)
            {
                if (IsOneOfOurs(assembly.Assembly))
                    _ourLoadedAssemblies.Add(assembly);
            }
        }

        /// <summary>
        /// Returns true if the assembly is one of our assemblies
        /// </summary>
        /// <param name="assembly">the assembly to check</param>
        public static bool IsOneOfOurs(Assembly assembly)
        {
            return IsOneOfOurs(assembly.FullName);
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
                    || assemblyName.StartsWith("resco", StringComparison.InvariantCultureIgnoreCase)
                    || assemblyName.StartsWith("infragistics", StringComparison.InvariantCultureIgnoreCase)
                    || assemblyName.StartsWith("system", StringComparison.InvariantCultureIgnoreCase)
                    || assemblyName.StartsWith("Wintellect.", StringComparison.InvariantCultureIgnoreCase)
                    || assemblyName.StartsWith("compactformatter", StringComparison.InvariantCultureIgnoreCase)
                    || assemblyName.ToLower().Contains("sharpziplib")
                    || assemblyName.ToLower().Contains("opennetcf")
                    || assemblyName.Equals("smdiagnostics", StringComparison.InvariantCultureIgnoreCase)
                    || assemblyName.Equals("cppcodeprovider", StringComparison.InvariantCultureIgnoreCase)
                    || assemblyName.Equals("SharpBITS.Base", StringComparison.InvariantCultureIgnoreCase)
                    || assemblyName.Equals("Accessibility", StringComparison.InvariantCultureIgnoreCase)
                   );
        }

        /// <summary>
        /// Search all dlls in the path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static void AddUnloadedAssembliesIn(string path)
        {
            foreach (string filePath in Directory.GetFiles(path, "*.dll"))
            {
                AddUnloadedAssemblies(filePath);
            }
        }

        /// <summary>
        /// Adds any already unloaded assemblies from the given path to AllLoadedAssemblies
        /// </summary>
        /// <param name="fullPath"></param>
        public static void AddUnloadedAssemblies(string fullPath)
        {
            try
            {
                //Assembly asm= Assembly.LoadFile(path);
                //FileInfo file = new FileInfo(fullPath);
                //fullPath = fullPath.Replace(file.Extension, "");
                Assembly assemblyToSearch = Assembly.LoadFrom(fullPath); //AppDomain.CurrentDomain.Load(fullPath);

                if (!Instance.AllLoadedAssemblies.Any(a => a.Assembly.FullName == assemblyToSearch.FullName))
                {
                    Instance.AllLoadedAssemblies.Add(AgileAssembly.Build(assemblyToSearch));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                Instance.OurAssemblies = null; 
            }
        }

        #region Static public methods

        /// <summary>
        /// Returns true if an assembly of the given name is in the given set of assemblies.
        /// </summary>
        /// <param name="assembliesToSearch">Collection of assemblies to search through</param>
        /// <param name="nameOfAssemblyToFind">Full Name of the assembly to look for.</param>
        /// <returns></returns>
        public static bool ContainsAssemblyNamed(List<AgileAssembly> assembliesToSearch, string nameOfAssemblyToFind)
        {
            return GetAssemblyNamed(assembliesToSearch, nameOfAssemblyToFind) != null;
        }

        /// <summary>
        /// Gets the assembly of the given name from the given set of assemblies.
        /// </summary>
        /// <remarks>CASE sensitive</remarks>
        /// <param name="assembliesToSearch">Collection of assemblies to search through</param>
        /// <param name="nameOfAssemblyToFind">Full Name of the assembly to look for.</param>
        /// <returns>null if the assembly is not found, otherwise the Assembly.</returns>
        public static Assembly GetAssemblyNamed(List<AgileAssembly> assembliesToSearch, string nameOfAssemblyToFind)
        {
            foreach (AgileAssembly assembly in assembliesToSearch)
            {
                if (assembly.Assembly.GetName().Name.Equals(nameOfAssemblyToFind))
                    return assembly.Assembly;
            }
            return null;
        }

        /// <summary>
        /// Get a file resource as a stream
        /// </summary>
        /// <param name="theAssembly">the assembly we are looking in</param>
        /// <param name="fileName">the filename</param>
        /// <returns>the resource as a stream</returns>
        public static Stream GetFileStream(Assembly theAssembly, string fileName)
        {
            Stream result = theAssembly.GetManifestResourceStream(
                FindResource(theAssembly, fileName));
            Debug.Assert(result != null, "Make sure the result isn't null");
            return result;
        }

        /// <summary>
        /// Goes through the list of resource names, and finds the resource
        /// name corresponding to this file.
        /// </summary>
        /// <remarks>An exception is thrown if the resource is not found.</remarks>
        /// <param name="theAssembly">Assembly to search.</param>
        /// <param name="fileName">Name of the file to find.</param>
        /// <returns>The resource</returns>
        private static string FindResource(Assembly theAssembly, string fileName)
        {
            foreach (string name in theAssembly.GetManifestResourceNames())
            {
                if (name.Contains(fileName))
                    return name;
            }
            throw new ResourceNotFoundException(theAssembly, fileName);
        }

        #endregion
    }
}