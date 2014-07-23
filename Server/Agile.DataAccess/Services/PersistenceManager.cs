using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Agile.Framework.Services;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Agile.DataAccess.Services
{
    /// <summary>
    /// Implementation of IPersistenceManager for server side persistence
    /// </summary>
    /// <remarks>this should evolve to have an SQL and Oracle etc Manager...</remarks>
    public class PersistenceManager : IPersistenceManager<Database>
    {
        private const string DatabaseNameKey = "PersistenceManagerDatabaseName";

        /// <summary>
        /// ctor
        /// </summary>
        public PersistenceManager()
        {
            DatabaseName = ConfigurationManager.AppSettings[DatabaseNameKey];
            // for now just set the persistence manager on the base class from here.
            DatabaseTable.PersistenceManager = this;
        }

        /// <summary>
        /// Database object, initialised with the default database.
        /// </summary>
        private Database _database;

        /// <summary>
        /// Database object, initialised with the default database.
        /// </summary>
        public Database Database
        {
            get
            {
                if (_database == null)
                    _database = AgileDatabase.GetInstance().GetDatabase(PersistenceStoreName);
                return _database;
            }
        }

        /// <summary>
        /// Gets the name of the database this table in is.
        /// Used to create the Database object.
        /// </summary>
        protected string DatabaseName { get; set; }

        /// <summary>
        /// Gets the name of the persistence store
        /// e.g. the database name.
        /// </summary>
        public string PersistenceStoreName
        {
            get
            {
                if (DatabaseName == null)
                    throw new ApplicationException(@"PersistenceManager database name has not been set.
Try adding something like this to your app.config:-
  <appSettings>
    <add key =""PersistenceManagerDatabaseName"" value=""YourDatabaseName"" />
  </appSettings>
");
                return DatabaseName; 
            }
        }

    }
}
