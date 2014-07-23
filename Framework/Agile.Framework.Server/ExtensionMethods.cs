using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;

namespace Agile.Framework
{
    /// <summary>
    /// Common useful extension methods
    /// </summary>
    public static class ExtensionMethods
    {
#if SILVERLIGHT
        /// <summary>
        /// Create a full (deep) clone of the object
        /// </summary>
        public static T Clone<T>(this T item) where T : BaseBiz
        {
            using (var ms = new MemoryStream())
            {
                var serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(T));
                serializer.WriteObject(ms, item);
                ms.Position = 0;
                return serializer.ReadObject(ms) as T;
            }
        }
#else
        /// <summary>
        /// Create a full (deep) clone of the object
        /// </summary>
        /// <remarks>This is better as an extension method, rather than 
        /// a method on the base class, because the generics works better
        /// when the item in passed in as a parameter (with my understanding 
        /// of generics anyway...maybe I'm missing something)</remarks>
        public static T Clone<T>(this T item) where T : BaseBiz
        {
            using (MemoryStream stream = new MemoryStream())
            {
                // keep the full namespace here (don't add to usings) for Silverlight
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(stream, item);
                stream.Position = 0;
                return formatter.Deserialize(stream) as T;
            }
        }

#endif  
    }
}
