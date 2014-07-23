using System;
using System.Collections.Generic;
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
        /// <summary>
        /// Gets the name of the persitence store (databse name) that the table is in.
        /// </summary>
        private const string PERSISTENCESTORENAME = "Acoustie";

        private Database database;

		/// <summary>
		/// Gets ALL records from the table.
		/// </summary>
        public List<T> GetAll(params DeepLoader[] deepLoaders)
		{
		    return GetAll(null, deepLoaders);
		}

        /// <summary>
        /// Gets ALL records from the table.
        /// </summary>
        public List<T> GetAll(DbTransaction transaction, params DeepLoader[] deepLoaders)
        {
			database = AgileDatabase.GetInstance().GetDatabase(PERSISTENCESTORENAME);
			var storedProcedureName = typeof(T).Name + "SelectAll";
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

    }
}
