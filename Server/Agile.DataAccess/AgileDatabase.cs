
using System;
using System.Configuration;
using Agile.Diagnostics.Logging;
using Agile.Environments;

namespace Agile.DataAccess
{
	using System.Collections;
	using Microsoft.Practices.EnterpriseLibrary.Data;

	/// <summary>
	/// Singleton class to manage which database we are connected to.
	/// </summary>
	public class AgileDatabase
	{
		/// <summary>
		/// Constructor
		/// </summary>
		private AgileDatabase()
		{
		}

        /// <summary>
        /// The unique instance of the database
        /// </summary>
        private static volatile AgileDatabase uniqueInstance;
        private static readonly object Padlock = new object();
        private static readonly Hashtable Databases = new Hashtable();

		/// <summary>
		/// Get the instance of the singleton class
		/// </summary>
		/// <returns></returns>
		public static AgileDatabase GetInstance()
		{
			if (uniqueInstance == null)
			{
				lock (Padlock)
				{
					if (uniqueInstance == null)
						uniqueInstance = new AgileDatabase();
				}
			}
			return uniqueInstance;
		}


		/// <summary>
		/// Gets the initialised database.
		/// </summary>
		/// <param name="databaseName">Name of the database (excluding the environment, e.g. Development)</param>
		public Database GetDatabase(string databaseName)
		{
			var database = FindExistingDatabase(databaseName);
            return database ?? InstantiateTheDatabase(databaseName, ApplicationEnvironment.Name);
		}

        /// <summary>
        /// Gets the initialised database.
        /// </summary>
        /// <param name="databaseInstanceName">Full Name of the database instance (INCLUDING the environment, e.g. Development)</param>
        public Database GetDatabaseFromFullInstanceName(string databaseInstanceName)
        {
            var database = FindExistingDatabase(databaseInstanceName);
            return database ?? InstantiateTheDatabase(databaseInstanceName);
        }

        /// <summary>
        /// Gets the connection string for the given database.
        /// </summary>
        /// <param name="databaseName">Name of the database (excluding the environment, e.g. Development)</param>
        public string GetConnectionString(string databaseName)
        {
            var database = GetDatabase(databaseName);
            return database.CreateConnection().ConnectionString;
        }

        /// <summary>
        /// Gets the database from the hashtable if it has been previously set.
        /// </summary>
        private Database FindExistingDatabase(string name)
        {
            object database = Databases[name];
            if (database == null)
                return null;

            return (Database) database;
        }

        /// <summary>
        /// Instantiates the database from the database name and the current environment
        /// then adds it to the hashtable and returns it.
        /// </summary>
        private Database InstantiateTheDatabase(string databaseName, string environment = "")
        {
            try
            {
                var factory = new DatabaseProviderFactory();
                var database = factory.Create(string.Format("{0}{1}", databaseName, environment));
                Databases[databaseName] = database;
                return database;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "InstantiateTheDatabase");
                throw;
            }
        }

	}
}