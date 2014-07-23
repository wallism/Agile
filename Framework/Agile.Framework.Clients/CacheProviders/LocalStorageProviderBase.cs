using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Net;
using Agile.Diagnostics.Logging;

namespace Agile.Framework.Clients.Silverlight.CacheProviders
{
    public abstract class LocalStorageProviderBase
    {
        protected IDictionary<string, object> State { get; private set; }

        /// <summary>
        /// ctor
        /// </summary>
        protected LocalStorageProviderBase(IDictionary<string, object> state)
        {
            State = state;
        }

        /// <summary>
        /// Load data from the phone State
        /// </summary>
        public object LoadFromState(string key)
        {
            return State.ContainsKey(key)
                ? State[key] : null;
        }

        /// <summary>
        /// Save data to the phone State
        /// </summary>
        public void SaveToState(string key, object value)
        {
            Logger.Debug(@"Saving '{0}' to State", key);
            State[key] = value;
        }

        protected void ClearLocalStorage(string fileName)
        {
            Logger.Debug(@"CLEAR '{0}' LocalStorage'", fileName);
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                try
                {
                    isf.DeleteFile(fileName);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
        }

        protected string GetFileName(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new Exception("key cannot be null when saving or retrieving from cache");
            // just remove any spaces (probably not necessary, just being overly safe)
            // and append the .dat
            return string.Format("bizzy{0}.dat", key.Replace(" ", string.Empty));
        }

        protected string GetBackupFileName(string key)
        {
            return string.Format("BK_{0}", GetFileName(key));
        }

    }
}
