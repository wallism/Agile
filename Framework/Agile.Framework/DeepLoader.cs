using System;
using System.Runtime.Serialization;
using System.Text;

namespace Agile.Framework
{
    /// <summary>
    /// Description of DeepLoader...
    /// </summary>
    public class DeepLoader
    {
        /// <summary>
        /// Name of the property to be loaded
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// for loading delta changes
        /// </summary>
        public DateTimeOffset? LastUpdated { get; set; }

        /// <summary>
        /// Load strategies for the property
        /// </summary>
        public DeepLoader[] DeepLoaders { get; set; }
        
        
        /// <summary>
        /// Use the Build method externally.
        /// (public for deserialization)
        /// </summary>
        public DeepLoader(){}

        public override string ToString()
        {
            var builder = new StringBuilder("[P]" +PropertyName);
            foreach (DeepLoader loader in DeepLoaders)
                builder.Append(string.Format("[S]{0}", loader));
            builder.AppendLine();
            return builder.ToString();
        }
        
        /// <summary>
        /// Instantiate a new DeepLoader
        /// </summary>
        public static DeepLoader Build(string propertyName, params DeepLoader[] loaders)
        {
            return new DeepLoader
            {
                PropertyName = propertyName,
                DeepLoaders = loaders
            };
        }

        /// <summary>
        /// Instantiate a new DeepLoader
        /// </summary>
        public static DeepLoader Build(string propertyName, DateTimeOffset? lastUpdated, params DeepLoader[] loaders)
        {
            return new DeepLoader
            {
                PropertyName = propertyName,
                LastUpdated =  lastUpdated,
                DeepLoaders = loaders
            };
        }
        
    }
}