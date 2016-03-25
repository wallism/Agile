using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using Agile.Diagnostics.Logging;

namespace Agile.Framework.Server
{
    public interface ICache
    {
        void Remove<T>(string key)
            where T : class;

        T GetCached<T>(string key, Func<T> loadDataFunc = null, int expiryMins = 60)
            where T : class;

        IList<T> GetCachedAll<T>(Func<IList<T>> loadDataFunc, int expiryMins = 30)
            where T : class;

        T GetCachedItemFromAll<T>(Func<T, bool> matcher, Func<IList<T>> loadDataFunc, int expiryMins = 60)
            where T : class;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>taken from: https://github.com/wallism/Agile/blob/master/Framework/Agile.Framework.Server/Cache.cs</remarks>
    public class Cache : ICache
    {
        public void Remove<T>(string key) where T : class
        {
            Logger.Debug("Cache.Remove: {0}", key);
            if (!MemoryCache.Default.Contains(key))
                return;

            MemoryCache.Default.Remove(key);
        }

        private readonly object padlock = new object();

        public T GetCached<T>(string key, Func<T> loadDataFunc = null, int expiryMins = 60)
            where T : class
        {
            lock (padlock)
            {
                var cached = MemoryCache.Default.Get(key) as T;
                if (cached != null)
                    return cached;

                // whatever is calling doesn't want (or know) how to load
                if (loadDataFunc == null)
                    return null;

                try
                {
                    // load from db
                    var loaded = loadDataFunc();
                    if (loaded == null)
                        return null;
                    Logger.Debug("[{0}] Adding new {1} to cache", key, typeof(T).Name);
                    MemoryCache.Default.Add(key, loaded, DateTimeOffset.Now.AddMinutes(expiryMins));

                    return loaded;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "GetCached: {0}", typeof(T).Name);
                    return null;
                }
            }
        }


        public T GetCachedItemFromAll<T>(Func<T, bool> matcher, Func<IList<T>> loadDataFunc, int expiryMins = 60)
            where T : class
        {
            var all = GetCachedAll(loadDataFunc, expiryMins);
            if (all == null || !all.Any())
            {
                Logger.Warning("GetCachedAll has 0 items for key:{1}", typeof(T).FullName);
                return null;
            }
            return all.FirstOrDefault(matcher);
        }

        /// <summary>
        /// Gets items from cache, if not found, loads using the given func.
        /// </summary>
        public IList<T> GetCachedAll<T>(Func<IList<T>> loadDataFunc = null, int expiryMins = 60)
            where T : class
        {
            var listKey = string.Format("List:{0}", typeof(T).FullName);
            var cached = MemoryCache.Default.Get(listKey) as List<T>;
            if (cached != null && cached.Any())
                return cached;

            // whatever is calling doesn't want (or know) how to load
            if (loadDataFunc == null)
                return null;

            try
            {
                // load from db
                var loaded = loadDataFunc();
                if (loaded == null || !loaded.Any())
                    return new List<T>();
                Logger.Debug("(key:{0}) Adding {1} new {2} to cache", listKey, loaded.Count, typeof(T).Name);
                MemoryCache.Default.Add(listKey, loaded, DateTimeOffset.Now.AddMinutes(expiryMins));

                return loaded;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetCachedAll: {0}", typeof(T).Name);
                return new List<T>();
            }
        }


    }
}