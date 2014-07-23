using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Linq;

namespace Agile.Common.Reflections
{
    /// <summary>
    /// Summary description for PluginFinder.
    /// </summary>
    [Serializable]
    public class PluginFinder
    {
        private readonly ArrayList _goodTypes = new ArrayList();

        /// <summary>
        /// Search all dlls in the path.
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public ArrayList SearchPath(string path)
        {
            _goodTypes.Clear();
            foreach (string file in Directory.GetFiles(path, "*.dll"))
            {
                TryLoadingPlugin(file);
            }

            return _goodTypes;
        }


        private void AddToGoodTypesCollection(Type goodType)
        {
            _goodTypes.Add(goodType);
        }

        private void TryLoadingPlugin(string path)
        {
            try
            {
                //Assembly asm= Assembly.LoadFile(path);
                var file = new FileInfo(path);
                path = file.Name.Replace(file.Extension, "");
                Assembly assemblyToSearch = AppDomain.CurrentDomain.Load(path);

                if (Assemblies.Instance.AllLoadedAssemblies.Any(a => a.Assembly.FullName == assemblyToSearch.FullName))
                    return;
                Assemblies.Instance.AllLoadedAssemblies.Add(AgileAssembly.Build(assemblyToSearch));
//				foreach(Type type in  assemblyToSearch.GetTypes())
//				{
//					foreach(Type iface in  type.GetInterfaces())		
//					{
//						if(iface.Equals(typeof(IPlugin)))
//						{
//							AddToGoodTypesCollection(type);
//							break; 
//						}
//					}
//				}
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool IsAgilePluginType(Type type)
        {
            AppDomain.CurrentDomain.Load("Agile.WinUI");
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            int i = assemblies.Length;
            return false;
        }
    }
}