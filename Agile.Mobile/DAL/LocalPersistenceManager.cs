using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Diagnostics.Logging;
using Agile.Framework;
using Agile.Mobile.DAL;
using Agile.Shared;
using Agile.Shared.IoC;
using AutoMapper;

namespace Acoustie.Mobile.DAL
{
    public interface ILocalPersistenceManager
    {
        long GetNextLocalId<T>() where T : LocalDbRecord;

        int InsertOrReplace<T, TR>(T source)
            where T : BaseBiz
            where TR : LocalDbRecord;

        int InsertOrReplace<TR>(TR source)
            where TR : LocalDbRecord;

        int Update<T, TR>(T source)
            where T : BaseBiz
            where TR : LocalDbRecord;

        int Update<TR>(TR source)
            where TR : LocalDbRecord;

        T Find<T, TR>(long id)
            where T : class, new()
            where TR : LocalDbRecord, new();

        TR Find<TR>(long id)
            where TR : LocalDbRecord, new();

        T FindFor<T, TR>(long id, string idFieldName)
            where T : class, new()
            where TR : LocalDbRecord, new();

        List<T> FindAllFor<T, TR>(long id, string idFieldName)
            where T : class, new()
            where TR : LocalDbRecord, new();

        List<T> FindAll<T, TR>(string where)
            where T : class, new()
            where TR : LocalDbRecord, new();

        List<T> GetAll<T, TR>()
            where T : class, new()
            where TR : LocalDbRecord, new();

        List<TR> GetAll<TR>()
            where TR : LocalDbRecord, new();

        int Delete<T, TR>(T source)
            where T : class, new()
            where TR : LocalDbRecord, new();

        int DeleteAll<TR>()
            where TR : LocalDbRecord, new();

        void RecreateCleanDb();
    }

    /// <summary>
    /// Singleton class to encapsulate logic of saving biz items to the local store
    /// (auto maps to and from biz types to record types)
    /// </summary>
    public class LocalPersistenceManager<TDB> : ILocalPersistenceManager
        where TDB : class, IAgileSQLite
    {
        /// <summary>
        /// ctor
        /// </summary>
        public LocalPersistenceManager()
        {
            Db.CreateAllTables();
        }

        public void RecreateCleanDb()
        {
            // just drop and create
            Db.DropAllTables();
            Db.CreateAllTables();
        }

        public long GetNextLocalId<T>() where T : LocalDbRecord
        {
            return Db.GetNextLocalId<T>();
        }

        private static object padlock = new object();

        /// <summary>
        /// Pass in a biz object, auto maps to Record (db) object
        /// and saves to the database (calls InsertOrReplace on SQLite)
        /// </summary>
        /// <remarks>safe to pass in nulls, just ignored.</remarks>
        public int InsertOrReplace<T, TR>(T source) 
            where T : BaseBiz
            where TR : LocalDbRecord
        {
            if (source == null)
                return 0; // nothing to save, not necessarily an error...
                // lock is required because it 'seems' AutoMapper can get its nickers in a knot with multi threading...putting the lock here stopped ALOT exs that were being thrown after I opened up session to load all data at once.
            lock (padlock)
            {
                MakeSureIdIsNotZero<T, TR>(source);

                // map to a 'Record' 
                try
                {
                    var record = Mapper.DynamicMap<T, TR>(source);
                    var result = Db.InsertOrReplace(record);
                    // only uncomment if needed
//                    Logger.Info("--- Save result={0} [{1}]", result, source.ToString());
                    return result;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    return -1;
                }
            }
        }

        /// <summary>
        /// Pass in a biz object, auto maps to Record (db) object
        /// and saves to the database (calls InsertOrReplace on SQLite)
        /// </summary>
        /// <remarks>safe to pass in nulls, just ignored.</remarks>
        public int InsertOrReplace<TR>(TR source)
            where TR : LocalDbRecord
        {
            if (source == null)
                return 0; // nothing to save, not necessarily an error...
            MakeSureIdIsNotZero(source);

            // map to a 'Record' 
            var result = Db.InsertOrReplace(source);
            Logger.Debug("Save result={0} [{1}]", result, source.ToString());
            return result;
        }

        private void MakeSureIdIsNotZero<T, TR>(T source) where T : BaseBiz where TR : LocalDbRecord
        {
            // Don't save anything with an id of 0!
            if (source.GetId() == 0)
                source.SetId(GetNextLocalId<TR>());
        }

        private void MakeSureIdIsNotZero<TR>(TR source)
            where TR : LocalDbRecord
        {
            // Don't save anything with an id of 0!
            if (source.GetId() == 0)
                source.SetId(GetNextLocalId<TR>());
        }

        /// <summary>
        /// Pass in a biz object, auto maps to Record (db) object
        /// and updates the existing record
        /// </summary>
        /// <remarks>safe to pass in nulls, just ignored.</remarks>
        public int Update<T, TR>(T source)
            where T : BaseBiz
            where TR : LocalDbRecord
        {
            if (source == null)
                return 0; // nothing to save, not necessarily an error...
            if (source.GetId() == 0) // throw an exception if we are doing an Update, Id can't be 0 if we we're updating!
                throw new Exception("Id cannot be 0! If it is a new record make sure Id is set using GetNextLocalId first.");

            // map to a 'Record' 
            var record = Mapper.DynamicMap<T, TR>(source);
            lock (padlock)
            {
                var result = Db.Update(record);
                Logger.Debug("Update result={0} [{1}]", result, source.ToString());
                return result;
            }
        }

        public int Update<TR>(TR source) where TR : LocalDbRecord
        {
            if (source == null)
                return 0; // nothing to save, not necessarily an error...
            if (source.GetId() == 0) // throw an exception if we are doing an Update, Id can't be 0 if we we're updating!
                throw new Exception("Id cannot be 0! If it is a new record make sure Id is set using GetNextLocalId first.");
            lock (padlock)
            {
                var result = Db.Update(source);
                Logger.Debug("Update result={0} [{1}]", result, source.ToString());
                return result;
            }
        }

        /// <summary>
        /// Load the object from the db
        /// </summary>
        public T Find<T, TR>(long id) 
            where T : class, new()
            where TR : LocalDbRecord, new()
        {
            var record = Db.Find<TR>(id);
            return record == null 
                ? null 
                : Mapper.DynamicMap<TR, T>(record);
        }

        /// <summary>
        /// Load the object from the db
        /// </summary>
        public TR Find<TR>(long id)
            where TR : LocalDbRecord, new()
        {
            return Db.Find<TR>(id);
        }

        /// <summary>
        /// Delete the object from the db
        /// </summary>
        public int Delete<T, TR>(T source)
            where T : class, new()
            where TR : LocalDbRecord, new()
        {
            lock (padlock)
            {
                var record = Mapper.DynamicMap<T, TR>(source);
                var result = Db.Delete(record);

                Logger.Debug("Delete result={0} [{1}]", result, source.ToString());
                return result;
            }
        }

        /// <summary>
        /// Delete the object from the db
        /// </summary>
        public int DeleteAll<TR>()
            where TR : LocalDbRecord, new()
        {
            lock (padlock)
            {
                var result = Db.DeleteAllRecords<TR>();

                Logger.Debug("DeleteAll result={0} [{1}]", result, typeof (TR).Name);
                return result;
            }
        }

        /// <summary>
        /// Find all records in a table matching on the given id (can only search by ids atm)
        /// on the given field name. 
        /// E.g. find all Photos for a given CaptureId matching on fieldName CaptureId
        /// </summary>
        public List<T> FindAllFor<T, TR>(long id, string idFieldName)
            where T : class, new()
            where TR : LocalDbRecord, new()
        {
            var list = new List<T>();
            var result = Db.Query<TR>(string.Format("SELECT * FROM {0} WHERE {1} = {2}"
                , typeof(TR).Name, idFieldName, id));

            if (result == null || result.Count == 0)
                return list;

            result.ForEach(record => list.Add(Mapper.DynamicMap<TR, T>(record)));
            return list;
        }

        /// <summary>
        /// Find ONE (or none) records in a table matching on the given id (can only search by ids atm)
        /// on the given field name. 
        /// </summary>
        public T FindFor<T, TR>(long id, string idFieldName)
            where T : class, new()
            where TR : LocalDbRecord, new()
        {
            var result = Db.Query<TR>(string.Format("SELECT * FROM {0} WHERE {1} = {2}"
                , typeof(TR).Name, idFieldName, id));

            if (result == null || result.Count == 0)
                return null;

            if (result.Count > 1)
                Logger.Warning("FindFor expected ONE record returned, got {0}. {1} {2}. Will return first.", result.Count, idFieldName, id);

            var item = result.First();
            return Mapper.DynamicMap<TR, T>(item);
        }

        /// <summary>
        /// Find all records in a table matching on the given where clause.
        /// </summary>
        public List<T> FindAll<T, TR>(string where)
            where T : class, new()
            where TR : LocalDbRecord, new()
        {
            var list = new List<T>();
            var result = Db.Query<TR>(string.Format("SELECT * FROM {0} WHERE {1}"
                , typeof(TR).Name, where));

            if (result == null || result.Count == 0)
                return list;

            result.ForEach(record => list.Add(Mapper.DynamicMap<TR, T>(record)));
            return list;
        }

        public List<T> GetAll<T, TR>() 
            where T : class, new()
            where TR : LocalDbRecord, new()
        {
            var list = new List<T>();
            var result = Db.GetAll<TR>();
            if (result == null || result.Count == 0)
                return list;
            
            result.ForEach(record => list.Add(Mapper.DynamicMap<TR, T>(record)));
            return list;
        }

        public List<TR> GetAll<TR>()
            where TR : LocalDbRecord, new()
        {
            var result = Db.GetAll<TR>();
            return result;
        }

        private TDB db;
        /// <summary>
        /// Gets the SQLite database
        /// </summary>
        protected TDB Db
        {
            get { return db ?? (db = Container.Resolve<TDB>()); }
        }
    }
}
