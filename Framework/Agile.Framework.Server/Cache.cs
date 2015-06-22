using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using Agile.Diagnostics.Logging;
using Agile.Shared;

namespace Agile.Framework.Server
{
    public interface ICache
    {
        T Get<T>(string key) 
            where T : class;

        IList<T> GetCachedAll<T>(Func<IList<T>> loadFromDbFunc, int expiryMins = 30) 
            where T : class;

        T GetCachedItemFromAll<T>(Func<T, bool> matcher, Func<IList<T>> loadFromDbFunc)
            where T : class;
    }

    public class Cache : ICache
    {

        public T Get<T>(string key) 
            where T : class
        {
            return MemoryCache.Default.Get(key) as T;
        }

        public T GetCachedItemFromAll<T>(Func<T, bool> matcher, Func<IList<T>> loadFromDbFunc)
            where T : class
        {
            var all = GetCachedAll(loadFromDbFunc);
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
        public IList<T> GetCachedAll<T>(Func<IList<T>> loadFromDbFunc = null, int expiryMins = 60) 
            where T : class
        {
            var listKey = string.Format("List:{0}", typeof(T).FullName);
            var cached = MemoryCache.Default.Get(listKey) as List<T>;
            if (cached != null && cached.Any())
                return cached;

            // whatever is calling doesn't want or know how to load
            if(loadFromDbFunc == null) 
                return null;

            try
            {
                // load from db
                var loaded = loadFromDbFunc();
                if (loaded == null || !loaded.Any()) 
                    return new List<T>();
                Logger.Debug("Adding {0} new {1} to cache", loaded.Count, typeof(T).Name);
                MemoryCache.Default.Add(listKey, loaded, AgileDateTime.Now.AddMinutes(expiryMins));

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
