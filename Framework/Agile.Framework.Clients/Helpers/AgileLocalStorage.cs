using System;
using Agile.Diagnostics.Logging;

namespace Agile.Framework.Helpers
{
    /// <summary>
    /// Abstracts away the logic of looking an item up in state and if not there
    /// then look in the Local Storage.
    /// To work this class requires an implementation for the device type.
    /// </summary>
    public class AgileLocalStorage
    {
        /// <summary>
        /// ctor
        /// </summary>
        public AgileLocalStorage(ILocalStorageProvider provider)
        {
            Provider = provider;
        }

        private ILocalStorageProvider Provider { get; set;}

        /// <summary>
        /// Load the item from the cache.
        /// Tries State first, then if not found in state it loads from
        /// LocalStorage and if found adds into the state for subsequent loads
        /// </summary>
        public T Load<T>(string key) where T : class 
        {
            Logger.Debug(@"Try to load [key:{0}] from State", key);
            try
            {
                var stateValue = Provider.LoadFromState(key);
                if (stateValue != null)
                {
                    Logger.Debug(@"Found [key:{0}] in State'", key);
                    return stateValue as T;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                Logger.Debug(@"Just continuing after exception in load from provider State");
            }

            Logger.Debug(@"Try to load [key:{0}] from LocalStorage", key);
            try
            {
                var localStorageValue = Provider.LoadFromLocalStorage<T>(key);
                Logger.Debug(@"[key:{0}] was {1}found in LocalStorage", key, (localStorageValue == null) ? "not " : string.Empty);
                if(localStorageValue != null)
                    Provider.SaveToState(key, localStorageValue);
                return localStorageValue as T;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                Logger.Debug(@"Just continuing after exception in load from provider LocalStorage");
            }
            return null;
        }

        private static object  _lock = new object();

        /// <summary>
        /// Save data to the phone State
        /// </summary>
        public void SaveToStateOnly(string key, object value)
        {
            Logger.Debug("SaveToState START {0}", key);
            Provider.SaveToState(key, value);
            Logger.Debug("SaveToState END {0}", key);
        }

        /// <summary>
        /// Save the key value pair to State and Local Storage
        /// </summary>
        public void Save<T>(string key, T value) where  T : class
        {
            lock (_lock)
            {
                SaveInternal(key, value);
            }
        }

        private void SaveInternal<T>(string key, T value) where  T : class
        {
//            if (string.IsNullOrEmpty(key) || value == null)
//            {
//                const string message = "key or value is null but trying to save to Cache!";
//#if DEBUG
//                throw new Exception(message);
//#else
//                Logger.Warning(message);
//                return;
//#endif
//            }

            try
            {
                SaveToStateOnly(key, value);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                Logger.Debug(@"Just continuing after exception in save to provider State");
            }

            try
            {
                Logger.Debug("SaveToStorage START {0}", key);
                Provider.SaveToLocalStorage(key, value);
                Logger.Debug("SaveToStorage END {0}", key);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                Logger.Debug(@"Just continuing after exception in save2 to provider LocalStorage");
            }
        }
    }
}
