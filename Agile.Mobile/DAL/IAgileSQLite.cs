using System;
using System.Collections.Generic;

namespace Agile.Mobile.DAL
{
    public interface IAgileSQLite
    {

        /// <summary>
        /// for testing
        /// </summary>
        void DropThenCreateAllTables();
        /// <summary>
        /// create all tables
        /// </summary>
        void CreateAllTables();
        /// <summary>
        /// Drop all existing tables
        /// </summary>
        void DropAllTables();

        /// <summary>
        /// Select a record by Id.
        /// (shortcut instead of using db.Query)
        /// </summary>
        T Find<T>(long id) where T : LocalDbRecord, new();

        T FindByAltId<T>(Guid id) where T : LocalDbRecord, new();

        /// <summary>
        /// Get all of the records in the table
        /// </summary>
        List<T> GetAll<T>() where T : LocalDbRecord, new();

        int GetRecordCount(Type type);
        /// <summary>
        /// Get the next id to use for saving the record locally (before saving to server and getting the actual id)
        /// </summary>
        long GetNextLocalId<T>(string idFieldNameOverride = null)
            where T : LocalDbRecord;

        T GetFirstRecord<T>() where T : LocalDbRecord, new();

        /// <summary>
        /// Gets the LAST record in the table (the one with the highest id)
        /// </summary>
        T GetLastRecord<T>() where T : LocalDbRecord, new();

        int DeleteAllRecords<T>() where T : LocalDbRecord;
        List<MasterTableRecord> GetAllTables();

        /// <summary>
        /// Use this wrapper where possible because it logs errors
        /// </summary>
        T ExecuteScalar<T>(string query, params object[] args);
        int Execute(string query, params object[] args);
        List<T> Query<T>(string query, params object[] args) where T : LocalDbRecord, new();
        int Insert(object item, string extra = null);
        int InsertOrReplace(object item);
        int Update(object item, Type type = null);
        int Delete(object item);
    }
}