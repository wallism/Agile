using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Runtime.Serialization;
using Agile.Diagnostics.Logging;
using Agile.Framework.Helpers;

namespace Agile.Framework.Clients.Silverlight.CacheProviders
{
    /// <summary>
    /// Local Storage (caching) provider for silverlight projects
    /// </summary>
    public class LocalStorageProvider : LocalStorageProviderBase, ILocalStorageProvider
    {
        /// <summary>
        /// ctor
        /// </summary>
        public LocalStorageProvider(IDictionary<string, object> stateDictionary)
            : base(stateDictionary)
        {
        }

        /// <summary>
        /// ensure we do not try to save and load at the same time from different threads
        /// </summary>
        private object padlock = new object();

        public object LoadFromLocalStorage<T>(string key) where T : class
        {
            T loaded = null;

            lock (padlock)
            {

                bool errorOccurred = false;
                using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (var stream = new IsolatedStorageFileStream(GetFileName(key),
                                                                      FileMode.OpenOrCreate, isf))
                    {
                        if (stream.Length > 0)
                        {
                            try
                            {
                                var dcs = new DataContractSerializer(typeof (T));
                                loaded = dcs.ReadObject(stream) as T;
                            }
                            catch (Exception ex)
                            {
                                Logger.Error(ex);
                                // clear the cache if we got an error when loading it
                                errorOccurred = true;
                            }
                        }
                    }
                }

                // need to perform the Clear outside of the using statement where the error occurred or get a 'file in use' error.
                if (errorOccurred)
                    ClearLocalStorage(key);
            }
            // if not found we want null to be returned
            // the only job the provider does is save and load stuff in state and storage...not creating new objects if not found etc
            return loaded;
        }

        public void SaveToLocalStorage<T>(string key, T value) where T : class
        {
            Logger.Debug(@"Saving '{0}' to LocalStorage'", key);
            lock (padlock)
            {
                try
                {
                    using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        using (var stream = new IsolatedStorageFileStream(GetFileName(key), FileMode.Create, isf))
                        {
                            var dcs = new DataContractSerializer(value.GetType());
                            dcs.WriteObject(stream, value);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "SaveToLocalStorage failed");
                }
            }
        }
    }
}
