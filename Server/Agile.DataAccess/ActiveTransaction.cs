using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Agile.Diagnostics.Logging;
using Agile.Environments;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Agile.Shared;

namespace Agile.DataAccess
{
    /// <summary>
    /// Class to manage transactions across multiple databases plus
    /// considers testing.
    /// </summary>
    public static class ActiveTransaction
    {
        private static Hashtable activeTransactions;
        private static bool isTestingInProgress;

        /// <summary>
        /// Gets the hashtable that contains all open test transactions
        /// </summary>
        private static Hashtable ActiveTransactions
        {
            get { return activeTransactions ?? (activeTransactions = new Hashtable()); }
        }

        /// <summary>
        /// Returns true if tests are in progress.
        /// </summary>
        public static bool IsTestingInProgress
        {
            get { return isTestingInProgress; }
        }

        /// <summary>
        /// Check the environment is ok to run tests against.
        /// </summary>
        private static void CheckEnvironment()
        {
            if(ApplicationEnvironment.IsProduction)
                throw new Exception(string.Format("Cannot run tests against production"));
        }


        /// <summary>
        /// Sets up Active transaction for testing.
        /// </summary>
        /// <remarks>For test transactions it just uses the Configuration Name of the database 
        /// plus 'TEST' for the name of the transaction. This is fine for TESTING use only, anything
        /// else it's just too dangerous if the name of the transaction is not unique.</remarks>
        public static void BeginTesting()
        {
            CheckEnvironment();
            isTestingInProgress = true;
        }

        /// <summary>
        /// Gets the test transaction, if one is active.
        /// If testing is in progress and there isn't a transaction for the database we create one.
        /// </summary>
        /// <returns>Returns null if testing is not in progress, otherwise a testing transaction</returns>
        private static DbTransaction InternalGetTestingTransaction(Database database)
        {
            if (!IsTestingInProgress)
                return null;
            // we don't use transactions for Oracle.
//            if (database is OracleDatabase)
//                return null;

            var transaction = FindActiveTransaction(GetTestTransactionName(database)) 
                ?? CreateAndBeginActiveTransaction(database, GetTestTransactionName(database));
            return transaction;
        }

        /// <summary>
        /// Gets the test transaction if there is one.
        /// </summary>
        /// <param name="database">The database the test is being run against (one test may go over multiple db's)</param>
        /// <remarks>Returns null if testing is not in progress, otherwise a testing transaction</remarks>
        public static DbTransaction GetTestTransaction(Database database)
        {
            return InternalGetTestingTransaction(database);
        }

        /// <summary>
        /// Gets the initialised database.
        /// </summary>
        /// <param name="database">The database to start the transaction against.</param>
        /// <param name="transactionName">Very important that the name of the transaction will
        /// be unique across sessions because of the web envirnment.</param>
        public static DbTransaction GetActiveTransaction(Database database, string transactionName)
        {
            // If a test transaction exists we have to use that! 
            if (IsTestingInProgress)
                return InternalGetTestingTransaction(database);

            // If testing is not in progress, check for the specified transactionName
            DbTransaction transaction = FindActiveTransaction(transactionName);

            if (transaction != null)
                return transaction;

            return CreateAndBeginActiveTransaction(database, transactionName);
        }


        /// <summary>
        /// Gets the testTransaction from the hashtable if it has been previously set.
        /// </summary>
        /// <returns></returns>
        private static DbTransaction FindActiveTransaction(string transactionName)
        {
            object testTransaction = ActiveTransactions[transactionName];
            if (testTransaction == null)
                return null;

            return (DbTransaction) testTransaction;
        }

        /// <summary>
        /// Start a transaction for the given database.
        /// Call 'FindExistingTestTransaction' first to ensure a transaction doesn't exist.
        /// </summary>
        private static DbTransaction CreateAndBeginActiveTransaction(Database database, string transactionName)
        {
            var connection = (SqlConnection) database.CreateConnection();
            connection.Open();
            SqlTransaction transactionToBeRolledBack = connection.BeginTransaction(IsolationLevel.RepeatableRead,
                                                                                   transactionName);
            ActiveTransactions[transactionName] = transactionToBeRolledBack;
            Logger.Debug("Opened Connection and started transaction on " + connection.Database);
            return transactionToBeRolledBack;
        }

        /// <summary>
        /// Commit an Active Transaction
        /// </summary>
        /// <param name="transactionName">Name of the transaction to COMMIT.</param>
        public static void CommitActiveTransaction(string transactionName)
        {
            if (IsTestingInProgress)
                return;

            var transaction = FindActiveTransaction(transactionName);
            if (transaction == null)
                throw new ActiveTransactionException(string.Format("Tried to COMMIT {0}, but could not find any ActiveTransaction with that name."
                                  , transactionName));

            var connection = transaction.Connection;
            Logger.Debug("COMMIT active transaction {0} on {1}"
                , transactionName
                , transaction.Connection.Database);

            try
            {
                transaction.Commit();
                Logger.Debug("Commit successful");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "committing {0} active transaction", transactionName);
				throw;
            }
            finally
            {
                connection.Dispose();
                activeTransactions.Remove(transactionName);
            }
        }

        /// <summary>
        /// Commit an Active Transaction
        /// </summary>
        /// <param name="transactionName">Name of the transaction to COMMIT.</param>
        public static void RollbackActiveTransaction(string transactionName)
        {
            if (IsTestingInProgress)
                return;

            var transaction = FindActiveTransaction(transactionName);
            if (transaction == null)
                throw new ActiveTransactionException(string.Format("Tried to ROLLBACK {0}, but could not find any ActiveTransaction with that name."
                                  , transactionName));

            var connection = transaction.Connection;
            if (connection != null)
            {
                Logger.Debug("Rolling back transaction on " + transaction.Connection.Database);
                transaction.Rollback();
                connection.Close();
            }

            activeTransactions.Remove(transactionName);
        }


        /// <summary>
        /// Rollback all transactions and clear the cache.
        /// </summary>
        public static void FinishTesting()
        {
            CheckEnvironment();
            // may already be rolled back.
            if (!IsTestingInProgress)
                return;   // throw new ActiveTransactionException("FinishTesting method called but testing is NOT in progress.");

            foreach (DictionaryEntry entry in ActiveTransactions)
            {
                var transaction = (DbTransaction) entry.Value;
                var connection = transaction.Connection;

                transaction.Rollback();
                if (connection != null)
                {
                    Logger.Testing("Rolling back testing transaction on " + connection.Database);
                    connection.Close();
                }
                else
                {
                    Logger.Testing("FAILED to Rollback testing transaction because connection was null.");
                }
            }
            activeTransactions = null;
            isTestingInProgress = false;
        }

        /// <summary>
        /// Get the name to use for a TESTING ONLY transaction.
        /// </summary>
        /// <param name="database">Database where we want to run tests in a transaction.</param>
        /// <returns></returns>
        internal static string GetTestTransactionName(Database database)
        {
            return database.ConnectionStringWithoutCredentials.GetStringBetween("database=", ";") + "TEST";
        }


        /// <summary>
        /// Gets a unique name for the transaction
        /// </summary>
        /// <returns></returns>
        public static string GetUniqueName()
        {
            return Guid.NewGuid().ToString().RemoveNonAlphanumericCharacters();
        }
    }
}