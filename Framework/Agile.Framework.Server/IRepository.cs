using System.Collections.Generic;
using System.Data.Common;

namespace Agile.Framework.Server
{
    /// <summary>
    /// Base interface for all Biz server services
    /// </summary>
    public interface IRepository<T> where T : IBaseBiz
    {
        /// <summary>
        /// Load the T
        /// </summary>
        T Load(long id, IList<DeepLoader> deepLoaders = null);

        /// <summary>
        /// Load the T
        /// </summary>
        T Load(long id, DbTransaction transaction, IList<DeepLoader> deepLoaders = null);

        /// <summary>
        /// Delete from the database.
        /// </summary>
        bool Delete(T item, DbTransaction transaction = null);

        T Save(T item);

        /// <summary>
        /// Gets a list of items that represents ALL records in the underlying table.
        /// </summary>
        List<T> GetAll(IList<DeepLoader> deepLoaders);

        /// <summary>
        /// Gets a list of items that represents ALL records in the underlying table.
        /// </summary>
        List<T> GetAll(DbTransaction transaction, IList<DeepLoader> deepLoaders = null);

        /// <summary>
        /// Returns true if the record exists in the db
        /// </summary>
        bool Exists(T item);

        /// <summary>
        /// Returns true if the record exists in the db
        /// </summary>
        bool Exists(T item, DbTransaction trans);
    }
}