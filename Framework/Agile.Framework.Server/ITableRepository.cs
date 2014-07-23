using System;
using System.Collections.Generic;
using System.Data.Common;
using Agile.DataAccess;

namespace Agile.Framework.Server
{
    /// <summary>
    /// Base interface for get data from a db table
    /// </summary>
    public interface ITableRepository<T> where T : DatabaseTable, new()
    {
        /// <summary>
        /// Load the T
        /// </summary>
        T Load(long id, DbTransaction transaction, params DeepLoader[] deepLoaders);
        
        /// <summary>
        /// Delete from the database.
        /// </summary>
        bool Delete(long id, DbTransaction transaction = null);
        
		/// <summary>
		/// Gets a list of items that represents ALL records in the underlying table.
		/// </summary>
        List<T> GetAll(params DeepLoader[] deepLoaders);

        /// <summary>
        /// Gets a list of items that represents ALL records in the underlying table.
        /// </summary>
        List<T> GetAll(DbTransaction transaction, params DeepLoader[] deepLoaders);
    }
}