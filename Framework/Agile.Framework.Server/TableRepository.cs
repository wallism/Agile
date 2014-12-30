using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.DataAccess;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Agile.Framework.Server
{
    public class TableRepository<T> : ITableRepository<T> where T : DatabaseTable, new()
    {
        private Database database;

		/// <summary>
		/// Gets ALL records from the table.
		/// </summary>
        public List<T> GetAll(params DeepLoader[] deepLoaders)
		{
		    return GetAll(null, deepLoaders);
		}

        private string persistenceStoreName;

        private string PersistenceStoreName
        {
            get { return persistenceStoreName 
                ?? (persistenceStoreName = ConfigurationManager.AppSettings["PersistenceStoreName"]); }
        }


        /// <summary>
        /// Gets ALL records from the table.
        /// </summary>
        public List<T> GetAll(DbTransaction transaction, params DeepLoader[] deepLoaders)
        {
			database = AgileDatabase.GetInstance().GetDatabase(PersistenceStoreName);
            // all of the Types will likely end with 'Table' but the procs names do not include table...so remove it if the name ends with it.

            var name = typeof(T).Name;
            var start = name.EndsWith("Table")
                ? name.Remove(name.LastIndexOf("Table"), 5)
                : name;
            var storedProcedureName = start + "SelectAll";
			var command  = database.GetStoredProcCommand(storedProcedureName);

			return DatabaseTable.InternalExecuteDataReader(database, command, transaction, DatabaseTable.CreateList<T>);
        }


        public T Load(long id, DbTransaction transaction, params DeepLoader[] deepLoaders)
        {
            var item = new T();
            item.LoadWith(id, transaction);
            return item;
        }

        public bool Delete(long id, DbTransaction transaction = null)
        {
            var item = new T();
            item.SetId(id);
            return item.Delete(transaction);
        }

        public bool Exist(long id, DbTransaction transaction = null)
        {
            var item = new T();
            item.SetId(id);
            return item.ExistsInTheDatabase(transaction);
        }

    }
}
