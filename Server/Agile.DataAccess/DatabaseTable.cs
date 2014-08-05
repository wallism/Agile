using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using Agile.Diagnostics.Logging;
using Agile.Shared;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;

namespace Agile.DataAccess
{
    /// <summary>
    /// Summary description for DALBase.
    /// </summary>
    public abstract class DatabaseTable
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        protected DatabaseTable()
        {
        }

        /// <summary>
        /// The primary key of the record.
        /// </summary>
        private IPrimaryKey _primaryKey;

        /// <summary>
        ///  Gets the (enterprise library) database object that this table is in.
        /// </summary>
        protected Database Database
        {
            get { return AgileDatabase.GetInstance().GetDatabase(PersistenceStoreName); }
        }


        /// <summary>
        /// Gets the primary key of the record.
        /// </summary>
        public IPrimaryKey PrimaryKey
        {
            get
            {
                if (_primaryKey == null)
                {
                    InitialisePrimaryKey();
                }
                return _primaryKey;
            }
            set
            {
                // setter needs to be exposed because this needs to be initialised in sub classes.
                _primaryKey = value;
            }
        }

        /// <summary>
        /// Load the data from the database using the primary key info.
        /// </summary>
        protected AgileActionResult InternalLoad()
        {
            return InternalLoad(null);
        }

        /// <summary>
        /// Load the data from the database using the primary key info.
        /// </summary>
        /// <param name="transaction">Load the item using this transaction.
        /// May be null, if it is the record is loaded without a transaction.</param>
        /// <returns></returns>
        protected AgileActionResult InternalLoad(DbTransaction transaction)
        {
            var result = Load(transaction, false);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadWith(long id, DbTransaction transaction = null)
        {
            SetId(id);
            Load(transaction);
        }

        /// <summary>
        /// Set the id 
        /// </summary>
        public abstract void SetId(long id);

        /// <summary>
        /// Load the data from the database using the primary key info.
        /// </summary>
        /// <param name="transaction">Load the item using this transaction.
        /// May be null, if it is the record is loaded without a transaction.</param>
        /// <param name="dodgyForceTestTransactionToNOTBeUsed">don't use, to be removed</param>
        public AgileActionResult Load(DbTransaction transaction, bool dodgyForceTestTransactionToNOTBeUsed = false)
        {
            var result = InternalExecuteDataReader(Database, GetSelectCommand(), transaction, dodgyForceTestTransactionToNOTBeUsed, LoadFromDb);
            if (result == null)
                LastLoadResult = AgileActionResult.Failed("Error in Load");
            return result;
        }

        /// <summary>
        /// Create a generic list from the reader
        /// </summary>
        protected AgileActionResult LoadFromDb(IDataReader reader, DbTransaction transaction)
        {
            if (reader.Read())
            {
                try
                {
                    FillFromReader(reader);
                    LastLoadResult = AgileActionResult.Success;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "FromDb");
                    LastLoadResult = AgileActionResult.Failed(ex.Message);
                }
            }
            else
            {
                LastLoadResult = AgileActionResult.Failed(CreateRecordNotFoundMessage(PrimaryKey, TableName));
            }
            return LastLoadResult;
        }


        /// <summary>
        /// Get the actionResult for the last load. i.e. did it fail and why
        /// </summary>
        public AgileActionResult LastLoadResult { get; set; }
        /// <summary>
        /// Create a message from these
        /// </summary>
        /// <param name="primaryKey">Primary key of the record we failed to find.</param>
        /// <param name="tableName">Name of the table we looked in.</param>
        /// <returns>the error message</returns>
        private static string CreateRecordNotFoundMessage(IPrimaryKey primaryKey, string tableName)
        {
            ArgumentValidation.CheckForNullReference(primaryKey, "primaryKey");
            ArgumentValidation.CheckForNullReference(tableName, "tableName");

            return string.Format(
                "Failed to find a record in '{0}' with primary key details: {1}"
                , tableName
                , primaryKey.WhereClause);
        }

        /// <summary>
        /// Create a message from these
        /// </summary>
        /// <param name="primaryKey">Primary key of the record we failed to find.</param>
        /// <param name="tableName">Name of the table we looked in.</param>
        /// <param name="rowCount">Number of rows actually found</param>
        /// <returns>the error message</returns>
        private static string CreateMultipleRecordMessage(IPrimaryKey primaryKey, string tableName, int rowCount)
        {
            ArgumentValidation.CheckForNullReference(primaryKey, "primaryKey");
            ArgumentValidation.CheckForNullReference(tableName, "tableName");

            return string.Format(
                "Found more than one {0} records in '{1}' with primary key details: {2}"
                , rowCount
                , tableName
                , primaryKey.WhereClause);
        }

        /// <summary>
        /// DELETE the record from the database using the primary key info.
        /// </summary>
        public bool Delete()
        {
            return Delete(null);
        }

        /// <summary>
        /// DELETE the record from the database using the primary key info.
        /// </summary>
        /// <param name="transaction">DELETE the item using this transaction.
        /// May be null, if it is the record is loaded without a transaction.</param>
        /// <returns></returns>
        public bool Delete(DbTransaction transaction)
        {
            try
            {
                DeleteBefore(transaction);

                InternalExecuteNonQuery(Database, GetDeleteCommand(), transaction);

                DeleteAfter(transaction);
                return true;
            }
            catch (SqlException sqlEx)
            {
                if (TableName != "Logging")
                    Logger.Debug("SQL EXCEPTION: " + sqlEx.Message);
                throw;
            }
        }

        /// <summary>
        /// A chance to do something after a Delete()
        /// </summary>
        /// <remarks>E.g. Delete the Party record after deleting a Trader or Person.
        /// NOTE: make sure you document in the comments of Delete() what else gets deleted when delete gets called on an item.</remarks>
        protected virtual void DeleteAfter(DbTransaction transaction)
        {
            // this is meant to be implemented in the base class, but only if required (thus virtual and not abstract)
        }

        /// <summary>
        /// A chance to do something before a Delete()
        /// </summary>
        /// <remarks>E.g. Delete all children records (i.e. developer implemented cascade delete.)
        /// NOTE: make sure you document in the comments of Delete() what else gets deleted when delete gets called on an item.</remarks>
        protected virtual void DeleteBefore(DbTransaction transaction)
        {
            // this is meant to be implemented in the base class, but only if required (thus virtual and not abstract)
        }

        /// <summary>
        /// Return the details of the object as a string, i.e. the value 
        /// of all public properties.
        /// </summary>
        /// <returns>Data in the object as a string.</returns>
        public override string ToString()
        {
            // should this include private properties??
            throw new NotImplementedException();
        }


        /// <summary>
        /// Determines if this record exists in the database.
        /// </summary>
        /// <remarks>Actively queries the database each time it is called.</remarks>
        public bool ExistsInTheDatabase()
        {
            return InternalExecuteScalar(Database, GetSelectCommand(), null) != null;
        }

        /// <summary>
        /// Determines if this record exists in the database.
        /// </summary>
        /// <remarks>Actively queries the database each time it is called.</remarks>
        public bool ExistsInTheDatabase(DbTransaction transaction)
        {
            return InternalExecuteScalar(Database, GetSelectCommand(), transaction) != null;
        }


        //		private static string _databaseInstanceNamePrefix = System.Configuration.ConfigurationSettings.AppSettings["DatabaseInstanceNamePrefix"];

        /// <summary>
        /// A chance to do something before a Save()
        /// This saves everything that this object depends on.
        /// This is used typically when you have common types that
        /// are grouped together in a table, but used by a number of
        /// other tables.
        /// For instance - if you had an addresss record in a customer
        /// record, then you'd first save off the address record
        /// </summary>
        /// <param name="transaction">The Save transaction</param>
        /// <remarks>REMEMBER, their id's need to get back into this record! somehow</remarks>
        protected virtual void SaveBefore(DbTransaction transaction)
        {
            // this is meant to be implemented in the base class, but only if required (thus virtual and not abstract)
        }

        /// <summary>
        /// Save the record.
        /// </summary>
        public virtual void Save(DbTransaction transaction)
        {
            var validation = Validate();
            if (!validation.WasSuccessful)
                throw new InvalidDataException(validation.FailureReason);

            try
            {
                SaveBefore(transaction);

                if (! ExistsInTheDatabase())
                    CreateNew(transaction);
                else
                {
                    var rowsAffected = UpdateExisting(transaction);
                    if(rowsAffected < 1)
                        throw new Exception("UPDATE FAILED. zero rows affected");
                }
            }
            catch (SqlException sqlEx)
            {
                Logger.Debug("SQL EXCEPTION: " + sqlEx.Message);
                if (TableName != "ErrrorLog") 
                    throw;
            }
        }

        /// <summary>
        /// Gets the PK Id value
        /// </summary>
        /// <remarks>assumes id field is a long</remarks>
        public abstract long GetId();

        /// <summary>
        /// Save the record.
        /// </summary>
        /// <remarks>Save is performed outside of a transactiion.</remarks>
        public virtual void Save()
        {
            Save(null);
        }

        /// <summary>
        /// Reloads the data from the database.
        /// </summary>
        public void Refresh()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the count of the items in the table
        /// </summary>
        /// <returns></returns>
        protected static int GetRecordCount(string databaseName
                                            , string tableName
                                            , DbTransaction transaction)
        {
            Database database = AgileDatabase.GetInstance().GetDatabase(databaseName);
            string selectCount = string.Format("SELECT COUNT(*) FROM {0}", tableName);
            DbCommand command = database.GetSqlStringCommand(selectCount);

            var scalar = InternalExecuteScalar(database, command, transaction);

            return Safe.Int(scalar);
        }


        #region Abstract Methods

        /// <summary>
        /// Gets the name of the table this object reflects.
        /// </summary>
        protected abstract string TableName { get; }

        /// <summary>
        /// Gets the name of the database the table is in.
        /// </summary>
        protected abstract string PersistenceStoreName { get; }

        /// <summary>
        /// Initialises the primary key object
        /// </summary>
        protected abstract void InitialisePrimaryKey();

        /// <summary>
        /// Fill the data from the data row into the properties.
        /// </summary>
        /// <param name="reader">The datareader that contains the data.</param>
        public abstract void FillFromReader(IDataReader reader);

        /// <summary>
        /// Verifies if the data is valid and 
        /// therefore OK to save.
        /// </summary>
        /// <returns>'Successful' if the data is valid or an error message if not.</returns>
        public abstract AgileActionResult Validate();

        /// <summary>
        /// Fill all of the output parameter values returned from the UPDATE stored procedure
        /// </summary>
        /// <param name="command"></param>
        protected virtual void FillUpdateOutputParameters(DbCommand command)
        {
        }

        /// <summary>
        /// Fill all of the output parameter values returned from the INSERT stored procedure
        /// </summary>
        /// <param name="command"></param>
        protected virtual void FillInsertOutputParameters(DbCommand command)
        {
        }

        /// <summary>
        /// Insert a new record.
        /// </summary>
        /// <remarks>Implementation needs some work but is basically functional atm.</remarks>
        protected void CreateNew(DbTransaction transaction)
        {
            Logger.Debug("CREATE: {0}", GetId());
            var command = GetInsertCommand();
            InternalExecuteNonQuery(Database, command, transaction);
            FillInsertOutputParameters(command);
        }

        /// <summary>
        /// Insert a new record.
        /// </summary>
        /// <remarks>Implementation needs some work but is basically functional atm.</remarks>
        protected void CreateNew()
        {
            CreateNew(null);
        }

        /// <summary>
        /// Update an existing record.
        /// </summary>
        /// <remarks>Implementation needs some work but is basically functional atm.</remarks>
        protected int UpdateExisting(DbTransaction transaction)
        {
            Logger.Debug("UPDATE: {0}", GetId());
            var command = GetUpdateCommand();
            try
            {
                var rowsAffected = InternalExecuteNonQuery(Database, command, transaction);
                FillUpdateOutputParameters(command);
                return rowsAffected;
            }
            catch (Exception ex)
            {
                if (TableName != "Logging")
                    Logger.Error(ex, "UpdateExisting");
                throw;
            }
        }

        /// <summary>
        /// Update an existing record.
        /// </summary>
        /// <remarks>Implementation needs some work but is basically functional atm.</remarks>
        protected int UpdateExisting()
        {
            return UpdateExisting(null);
        }

        /// <summary>
        /// Executes the given command as a non query, within the given transaction
        /// (if the transaction is not null). Executed without a transaction if it is null.
        /// </summary>
        /// <param name="database">database that this is to be run against (enterprise library)</param>
        /// <param name="command">The dbCommand to execute</param>
        /// <param name="transaction">may be null, otherwise, the transaction the command is to be executed within.</param>
        private static int InternalExecuteNonQuery(Database database
                                                   , DbCommand command, DbTransaction transaction)
        {
            var testTransaction = ActiveTransaction.GetTestTransaction(database);
            // Use the test transaction if one is there
            if (testTransaction != null)
                transaction = testTransaction;

            return transaction == null 
                ? database.ExecuteNonQuery(command) 
                : database.ExecuteNonQuery(command, transaction);
        }

        /// <summary>
        /// Executes the given command as a Scalar query, within the given transaction
        /// (if the transaction is not null). Executed without a transaction if it is null.
        /// </summary>
        /// <param name="database">database that this is to be run against (enterprise library)</param>
        /// <param name="command">The dbCommand to execute</param>
        /// <param name="transaction">may be null, otherwise, the transaction the command is to be executed within.</param>
        public static object InternalExecuteScalar(Database database
                                                   , DbCommand command, DbTransaction transaction)
        {
            DbTransaction testTransaction = ActiveTransaction.GetTestTransaction(database);
            // Use the test transaction if one is there
            if (testTransaction != null)

                transaction = testTransaction;

            if (transaction == null)
                return database.ExecuteScalar(command);
            
            return database.ExecuteScalar(command, transaction);
        }

        /// <summary>
        /// Executes the given command as an ExecuteDataSet, within the given transaction
        /// (if the transaction is not null). Executed without a transaction if it is null.
        /// </summary>
        /// <param name="database">database that this is to be run against (enterprise library)</param>
        /// <param name="command">The dbCommand to execute</param>
        /// <param name="transaction">may be null, otherwise, the transaction the command is to be executed within.</param>
        /// <param name="func"></param>
        public static T InternalExecuteDataReader<T>(Database database
            , DbCommand command
            , DbTransaction transaction
            , Func<IDataReader, DbTransaction, T> func) where T : class
        {
            return InternalExecuteDataReader(database, command, transaction, false, func);
        }

        /// <summary>
        /// Executes the given command as an ExecuteDataSet, within the given transaction
        /// (if the transaction is not null). Executed without a transaction if it is null.
        /// </summary>
        /// <param name="database">database that this is to be run against (enterprise library)</param>
        /// <param name="command">The dbCommand to execute</param>
        /// <param name="transaction">may be null, otherwise, the transaction the command is to be executed within.</param>
        /// <param name="dodgyForceTestTransactionToBeNOTUsed"></param>
        /// <param name="func"></param>
        public static T InternalExecuteDataReader<T>(Database database
            , DbCommand command
            , DbTransaction transaction
            , bool dodgyForceTestTransactionToBeNOTUsed
            , Func<IDataReader, DbTransaction, T> func) where T : class
        {
            DbTransaction testTransaction = null;
            if (!dodgyForceTestTransactionToBeNOTUsed)
                testTransaction = ActiveTransaction.GetTestTransaction(database);

            // Use the test transaction if one is there
            if (testTransaction != null)
                transaction = testTransaction;

            try
            {
                var reader = (transaction == null)
                                 ? database.ExecuteReader(command)
                                 : database.ExecuteReader(command, transaction);

                try
                {
                    return func(reader, transaction);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
                finally
                {
                    if(!reader.IsClosed)
                        reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        #endregion

        #region Stored Procedure Methods

        /// <summary>
        /// Get the DB command command for selecting a record from the table.
        /// </summary>
        /// <returns></returns>
        protected virtual DbCommand GetSelectCommand()
        {
            var dbCommand = Database.GetStoredProcCommand(GetStoredProcedureName("Select"));
            AddParametersForPrimaryKey(dbCommand);

            return dbCommand;
        }


        /// <summary>
        /// Get the DB command command for updating a record in the table.
        /// </summary>
        /// <returns></returns>
        private DbCommand GetUpdateCommand()
        {
            var dbCommand = Database.GetStoredProcCommand(GetStoredProcedureName("Update"));
            AddParametersForUpdate(dbCommand);

            return dbCommand;
        }

        /// <summary>
        /// Get the DB command command for DELETING a record in the table.
        /// </summary>
        /// <returns></returns>
        protected virtual DbCommand GetDeleteCommand()
        {
            var dbCommand = Database.GetStoredProcCommand(GetStoredProcedureName("Delete"));
            AddParametersForPrimaryKey(dbCommand);

            return dbCommand;
        }

        /// <summary>
        /// Get the DB command command for updating a record in the table.
        /// </summary>
        /// <returns></returns>
        private DbCommand GetInsertCommand()
        {
            var dbCommand = Database.GetStoredProcCommand(GetStoredProcedureName("Insert"));
            AddParametersForInsert(dbCommand);

            return dbCommand;
        }

        /// <summary>
        /// Gets the stored procedure name for the given action.
        /// </summary>
        /// <param name="type">The 'type' of stored procedure to run, e.g. Insert, Update or ForDropDown</param>
        /// <returns></returns>
        private string GetStoredProcedureName(string type)
        {
            return CleanTableName(TableName) + type;
        }

        /// <summary>
        /// Returns the table in a 'clean' format.
        /// </summary>
        /// <remarks>All non alpha numeric characters (including underscores) are removed.</remarks>
        public static string CleanTableName(string tableName)
        {
            var name = tableName.RemoveNonAlphanumericCharacters();
            name = name.Replace("_", string.Empty);

            return name.ToPascalCase();
        }

        /// <summary>
        /// Add 'In' parameters to the command using details from the primary key.
        /// </summary>
        /// <param name="command"></param>
        private void AddParametersForPrimaryKey(DbCommand command)
        {
            foreach (Column column in PrimaryKey.Columns)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = "@" + column.Name;
                parameter.Value = column.Value;

                if (PrimaryKey is PrimaryKeyInt)
                    parameter.DbType = DbType.Int32;
                if (PrimaryKey is PrimaryKeyBigint)
                    parameter.DbType = DbType.Int64;
                else if (PrimaryKey is PrimaryKeyVarchar)
                    parameter.DbType = DbType.String;
                else if (PrimaryKey is PrimaryKeyUniqueidentifier)
                    parameter.DbType = DbType.Guid;

                command.Parameters.Add(parameter);
            }
        }

        /// <summary>
        /// Adds the parameters for each field in the table to the command.
        /// </summary>
        /// <remarks>Used only for inserting a record into the table.</remarks>
        /// <param name="command">The command command, should be for a stored procedure.</param>
        protected abstract void AddParametersForInsert(DbCommand command);

        /// <summary>
        /// Adds the parameters for each field in the table to the command.
        /// </summary>
        /// <remarks>Used only for inserting a record into the table.</remarks>
        /// <param name="command">The command command, should be for a stored procedure.</param>
        protected abstract void AddParametersForUpdate(DbCommand command);

        #endregion


        /// <summary>
        /// Create a generic list from the reader
        /// </summary>
        public static List<T> CreateList<T>(IDataReader reader, DbTransaction transaction) 
            where T : DatabaseTable, new()
        {
            var items = new List<T>();
            while (reader.Read())
            {
                var record = new T();
                record.FillFromReader(reader);
                items.Add(record);
            }
            return items;
        }

        /// <summary>
        /// Create a generic list from the reader
        /// </summary>
        public static T CreateFromDb<T>(IDataReader reader, DbTransaction transaction)
            where T : DatabaseTable, new()
        {
            if (reader == null || !reader.Read()) 
                return null; // just return null, nothing found
            var record = new T();
            record.FillFromReader(reader);

            return record;
        }
    }
}