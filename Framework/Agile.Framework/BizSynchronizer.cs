using System.Collections.Generic;
using Agile.Framework.UI;

namespace Agile.Framework
{
    /// <summary>
    /// Handler for find a matching in a list (ie non reference type match)
    /// </summary>
    public delegate M FindMatchHandler<M>(List<M> list, M itemToMatch);

    /// <summary>
    /// Synchronizes biz objects
    /// </summary>
    public class BizSynchronizer
    {
        private readonly Dictionary<object, object> objectsSynchronizing = new Dictionary<object, object>();
        /// <summary>
        /// Synchronise the data in the master object with the provided data.
        /// </summary>
        public T Sync<T, D>(T master, T data)
            where T : BaseBiz, new()
            where D : class, IModelInterface
        {
            if (master == null && data != null)
                master = new T();
            if (master == null || data == null) return null;

            if (objectsSynchronizing.ContainsKey(data))
                return objectsSynchronizing[data] as T;

            objectsSynchronizing.Add(data, master);

            master.FillShallow(data as D);
            master.FillDeep(data as D, this);
            // make sure the BaseBiz properties are also included
            master.AlternateCreationID = data.AlternateCreationID;
            master.IsDirty = data.IsDirty;

            // do not sync the SaveResult...the value we need is on the master
            //            master.SaveResult = data.SaveResult;
            return master;
        }

        /// <summary>
        /// Synchronize the masterList with the provided syncWithList, applying changes to matching items,
        /// adding new items. Does NOT remove items from the masterList if they don't exist in the syncWithList.
        /// </summary>
        /// <remarks>This method is useful when retrieving a delta of changes from the server, ie the returned list
        /// will only contain items that have been changed or added.</remarks>
        public List<T> SyncDelta<T, D>(List<T> masterList, List<T> syncWithList)
            where T : BaseBiz, new()
            where D : class, IModelInterface
        {
            return SyncDelta<T, D>(masterList, syncWithList, FindMatch);
        }

        /// <summary>
        /// Synchronize the masterList with the provided syncWithList, applying changes to matching items,
        /// adding new items. Does NOT remove items from the masterList if they don't exist in the syncWithList.
        /// </summary>
        /// <remarks>This method is useful when retrieving a delta of changes from the server, ie the returned list
        /// will only contain items that have been changed or added.</remarks>
        public List<T> SyncDelta<T, D>(List<T> masterList, List<T> syncWithList, FindMatchHandler<T> findMatch)
            where T : BaseBiz, new()
            where D : class, IModelInterface
        {
            return SyncInternal<T, D>(masterList, syncWithList, findMatch, true);
        }

        /// <summary>
        /// Synchronize the list syncWithList in the masterList object with the provided syncWithList.
        /// This is used for synchronizing items returned from the server with the client side item.
        /// </summary>
        public List<T> Sync<T, D>(List<T> masterList, List<T> syncWithList)
            where T : BaseBiz, new()
            where D : class, IModelInterface
        {
            return Sync<T, D>(masterList, syncWithList, FindMatch);
        }


        /// <summary>
        /// Synchronize the masterList object with the provided syncWithList, applying changes to matching items,
        /// adding new items and REMOVING items from the masterList that do not exist in the the syncWithList.
        /// </summary>
        public List<T> Sync<T, D>(List<T> masterList, List<T> syncWithList, FindMatchHandler<T> findMatch)
            where T : BaseBiz, new()
            where D : class, IModelInterface
        {
            return SyncInternal<T, D>(masterList, syncWithList, findMatch, false);
        }

        /// <summary>
        /// Synchronize the masterList object with the provided syncWithList, applying changes to matching items,
        /// adding new items and REMOVING items from the masterList that do not exist in the the syncWithList.
        /// </summary>
        private List<T> SyncInternal<T, D>(List<T> masterList, List<T> syncWithList, FindMatchHandler<T> findMatch, bool delta)
            where T : BaseBiz, new()
            where D : class, IModelInterface
        {
            if (masterList == null && syncWithList != null)
                masterList = new List<T>();

            if (masterList == null || syncWithList == null) return null;

            var itemsToBeDeleted = new List<T>();
            var itemsToBeAdded = new List<T>();

            // we want to keep the same masterList list in memory and just update with the provided syncWithList.
            // handles the easy cases where changes to existing items have been made.
            for (int i = 0; i < masterList.Count; i++)
            {
                T masterItem = masterList[i];
                T dataItem = findMatch(syncWithList, masterItem);

                if (dataItem == null && !delta)
                {
                    // this is the case where items are deleted on the server and this
                    // list is returned to the client, minus those items
                    // i.e. masterList items that do not have matching item in syncWithList
                    // when this happens we delete the item from masterList
                    itemsToBeDeleted.Add(masterItem);
                    continue;
                }
                Sync<T, D>(masterItem, dataItem);
            }
            // now we need to consider records that may have been added
            // i.e. syncWithList items that do not have matching item in masterList

            for (int i = 0; i < syncWithList.Count; i++)
            {
                T dataItem = syncWithList[i];
                // if there is a match then we updated it above
                if (findMatch(masterList, dataItem) != null)
                    continue;
                itemsToBeAdded.Add(GetNewItemToBeAdded<T, D>(dataItem));
            }
            // Just add the items.
            masterList.AddRange(itemsToBeAdded);

            // then remove any items that need to be deleted locally.
            for (int i = 0; i < itemsToBeDeleted.Count; i++)
                masterList.Remove(itemsToBeDeleted[i]);
            return masterList;
        }

        protected virtual T GetNewItemToBeAdded<T, D>(T item)
            where T : BaseBiz, new()
            where D : class, IModelInterface
        {
            return item;
        }

        /// <summary>
        /// Find and return the matchine item from the list.
        /// Matches on AlternateID (handles for all cases, id is flaky)
        /// </summary>
        /// <param name="list">List to search in</param>
        /// <param name="item">item to search for</param>
        private static T FindMatch<T>(List<T> list, T item)
            where T : BaseBiz, new()
        {
            return list.Find(match => match.AlternateCreationID == item.AlternateCreationID);

        }

        /// <summary>
        /// Find and return the matchine item from the list.
        /// Matches on the lookup Identifier.
        /// </summary>
        /// <param name="list">List to search in</param>
        /// <param name="item">item to search for</param>
        public static T FindMatchByLookupID<T>(List<T> list, T item)
            where T : BaseBiz, IAgileLookup, new()
        {
            return list.Find(match => match.Identifier == item.Identifier);
        }

    }
}
