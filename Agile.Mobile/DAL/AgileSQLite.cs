using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Diagnostics.Logging;
using SQLite.Net;
using SQLite.Net.Interop;

namespace Agile.Mobile.DAL
{
    public abstract class AgileSQLite : IAgileSQLite
    {
        /// <summary>
        /// ctor
        /// </summary>
        public AgileSQLite()
        {
            DefineTables();
        }

        protected SQLiteConnection Db { get; set; }

        protected void Initialize(ISQLitePlatform platform
            , string fullPath)
        {
            Logger.Info("Db Path:{0}", fullPath);
            try
            {
                Db = new SQLiteConnection(platform, fullPath);
                var tables = GetAllTables();
                Logger.Debug("{0} existing tables:", tables.Count);
                // log all existing if there are some
                if (tables.Count > 0)
                {
                    foreach (var masterTableRecord in tables)
                        Logger.Debug("  {0}", masterTableRecord);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to initialize database");
                throw; // still throw, just want it logged for now
            }
            Logger.Debug("INIT COMPELTE\r\n");
        }

        /// <summary>
        /// All local db tables for Acoustie
        /// </summary>
        public static readonly List<Type> Tables = new List<Type>();

        /// <summary>
        /// This method is mainly for testing. Just use CreateAllTables in normal usage.
        /// </summary>
        public void DropThenCreateAllTables()
        {
            DropAllTables();
            CreateAllTables();
        }

        /// <summary>
        /// Add tables (DAL classes) that make up the tables of the database
        /// </summary>
        protected abstract void DefineTables();
        /// <summary>
        /// Drop all tables that have been defined in DefineTables
        /// (needs to be manually done at concrete level because DropTable needs to be call individually)
        /// </summary>
        public abstract void DropAllTables();

        /// <summary>
        /// Create all of the required tables 
        /// Does nothing if the table already exists.
        /// </summary>
        public void CreateAllTables()
        {
            Logger.Debug("CREATE ALL tables");
            var existing = GetAllTables();

            // we want to always call create table, if the table already exists, existing data 
            // is preserved but calling Create ensures any new columns are added
            foreach (var table in Tables)
            {
                CreateTable(table);
                // uncomment when debugging issue with the db...otherwise don't, it slows down startup
//                Logger.Debug(" {0} exists {1} records", table.Name, GetRecordCount(table));
            }
        }

        private void CreateTable(Type table)
        {
            var tableName = table.Name;
//            Logger.Debug("Create Table: {0}", table);
            var result = Db.CreateTable(table);
            // usually is 0 but sometimes returns 1
            if (result >= 0)
                return;

            // Failed...
            var message = string.Format("Failed to create table: {0}", tableName);
            Logger.Warning(message);
#if DEBUG
            throw new Exception(message);
#endif
        }

        /// <summary>
        /// Gets all Tables that already exist excluding 'sqlite_sequence'
        /// </summary>
        public List<MasterTableRecord> GetAllTables()
        {
            return Query<MasterTableRecord>(@"SELECT * FROM sqlite_master 
WHERE type='table' and tbl_name != 'sqlite_sequence'");
        }

        
        public int GetRecordCount(Type type)
        {
            var tableName = type.Name;
            var count = ExecuteScalar<int>(string.Format("SELECT COUNT(*) FROM {0}", tableName));
//            Logger.Debug("{0} records in {1}", count, tableName);
            return count;
        }

        /// <summary>
        /// All server side id's start at 10000, so we have up to 10000
        /// records we can create client side before saving.
        /// As soon as the record is saved to the server and we get the realId value
        /// then we update that (using the altId) in the table and all child records
        /// </summary>
        public long GetNextLocalId<T>(string idFieldNameOverride = null)
            where T : LocalDbRecord
        {
            return GetNextLocalId(typeof (T), idFieldNameOverride);
        }

        /// <summary>
        /// Gets the first record in the table (the one with the lowest id)
        /// </summary>
        public T GetFirstRecord<T>() where T : LocalDbRecord, new()
        {
            return Query<T>(string.Format("SELECT * FROM {0} ORDER BY 1 ASC LIMIT 1", typeof (T).Name)).FirstOrDefault();
        }

        /// <summary>
        /// Gets the LAST record in the table (the one with the highest id)
        /// </summary>
        public T GetLastRecord<T>() where T : LocalDbRecord, new()
        {
            return Query<T>(string.Format("SELECT * FROM {0} ORDER BY 1 DESC LIMIT 1", typeof(T).Name)).FirstOrDefault();
        }

        /// <summary>
        /// All server side id's start at 10000, so we have up to 10000
        /// records we can create client side before saving.
        /// As soon as the record is saved to the server and we get the realId value
        /// then we update that (using the altId) in the table and all child records
        /// </summary>
        private long GetNextLocalId(Type type, string idFieldNameOverride = null)
        {
            var tableName = type.Name;
            var idFieldName = string.IsNullOrEmpty(idFieldNameOverride)
                                  ? string.Format("{0}Id", tableName.Replace("Record", ""))
                                  : idFieldNameOverride;

            // for these queries we only care about ids < 10000
            var localRecordCount = ExecuteScalar<int>(string.Format("SELECT COUNT(*) FROM {1} WHERE {0} < 10000"
                , idFieldName, tableName));
            // if there are zero records, the select max throws an exception
            if (localRecordCount == 0)
                return 1;

            var max = ExecuteScalar<long>(string.Format(@"SELECT MAX({0}) FROM {1} WHERE {0} < 10000"
                , idFieldName, tableName));

            var nextId = max + 1;
            Logger.Debug("Next id:{0} for:{1}", nextId, tableName);
            return nextId;
        }



        /// <summary>
        /// Wrapper to log exceptions
        /// </summary>
        public int Execute(string query, params object[] args)
        {
            try
            {
                return Db.Execute(query, args);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, string.Format("Execute: {0}", query));
                throw;
            }
        }

        /// <summary>
        /// Wrapper to log exceptions
        /// </summary>
        public T ExecuteScalar<T>(string query, params object[] args) 
        {
            try
            {
                return Db.ExecuteScalar<T>(query, args);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, string.Format("ExecuteScalar: {0}", query));
                throw;
            }
        }

        /// <summary>
        /// Get a specific record (returns null if not found)
        /// </summary>
        /// <remarks>could make the type object so is the same as the wrapped method...not sure if that's better...</remarks>
        public T Find<T>(long id) where T : LocalDbRecord, new()
        {
            // using Find because Get throws an ex if the record does not exist
            try
            {
                return Db.Find<T>(id);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, string.Format("Find: {0}", id));
                throw;
            }
        }

        /// <summary>
        /// Get a specific record using the AltId (returns null if not found)
        /// </summary>
        /// <remarks>could make the type object so is the same as the wrapped method...not sure if that's better...</remarks>
        public T FindByAltId<T>(Guid id) where T : LocalDbRecord, new()
        {
            // using Find because Get throws an ex if the record does not exist
            try
            {
                return Db.Find<T>(record => record.AltId == id);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, string.Format("FindByAltId: {0}", id));
                throw;
            }
        }

        public List<T> GetAll<T>() where T : LocalDbRecord, new()
        {
            return Db.Query<T>(string.Format("SELECT * FROM {0}", typeof(T).Name));
        }

        public int DeleteAllRecords<T>() where T : LocalDbRecord
        {
            var result = Db.DeleteAll<T>();
            Logger.Debug("delete result:{0}", result);
            return result;
        }

        /// <summary>
        /// Wrapper to log exceptions
        /// </summary>
        public List<T> Query<T>(string query, params object[] args)
            where T : LocalDbRecord, new()
        {
            try
            {
                return Db.Query<T>(query, args);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, string.Format("Query: {0}", query));
                throw;
            }
        }

        private object padlock = new object();

        /// <summary>
        /// Wrapper to log exceptions
        /// </summary>
        public int Insert(object item, string extra = null)
        {
            try
            {
                // this is CRITICAL! sqlite crashes if multi threads try to insert at the same time!
                lock (padlock)
                {
                    return string.IsNullOrEmpty(extra) 
                        ? Db.Insert(item) 
                        : Db.Insert(item, extra);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, string.Format("Insert: {0}", item));
                throw;
            }
        }

        /// <summary>
        /// Wrapper to log exceptions
        /// </summary>
        public int InsertOrReplace(object item)
        {
            try
            {
                // this is CRITICAL! sqlite crashes if multi threads try to insert at the same time!
                lock (padlock)
                {
                    return Db.InsertOrReplace(item);
                }
                
            }
            catch (Exception ex)
            {
                Logger.Error(ex, string.Format("InsertOrReplace: {0}", item));
                throw;
            }
        }
        
        /// <summary>
        /// Wrapper to log exceptions
        /// </summary>
        public int Update(object item, Type type = null)
        {
            try
            {
                // this is CRITICAL! sqlite crashes if multi threads try to insert at the same time!
                lock (padlock)
                {
                    return type == null
                        ? Db.Update(item)
                        : Db.Update(item, type);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, string.Format("Update: {0}", item));
                throw;
            }
        }

        /// <summary>
        /// Wrapper to log exceptions
        /// </summary>
        public int Delete(object item)
        {
            try
            {
                // this is CRITICAL! sqlite crashes if multi threads try to insert at the same time!
                lock (padlock)
                {
                    Logger.Debug("Delete: {0}", item);
                    return Db.Delete(item);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, string.Format("Delete: {0}", item));
                throw;
            }
        }

    }
    /// <summary>
    /// SQLite_master table record schema
    /// </summary>
    public class MasterTableRecord : LocalDbRecord
    {
        public override string ToString()
        {
            return string.Format("Table:{0} type:{1}   NAME:{2}", tbl_name, type, name);
        }

        public override long GetId()
        {
            throw new NotImplementedException();
        }

        public override void SetId(long id)
        {
            throw new NotImplementedException();
        }

        public string type { get; set; }
        public string name { get; set; }
        public string tbl_name { get; set; }
        public int rootpage { get; set; }
        public string sql { get; set; }
    }
}
