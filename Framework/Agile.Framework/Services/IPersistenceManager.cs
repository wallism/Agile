using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agile.Framework.Services
{
    /// <summary>
    /// Details for where data is stored and how etc.
    /// </summary>
    public interface IPersistenceManager<T>
    {
        /// <summary>
        /// Gets the Persistence 'entity' eg database, file etc
        /// </summary>
        /// <remarks>TODO: change the name of this later to Entity or something</remarks>
        T Database { get; }

        /// <summary>
        /// Gets the name of the persistence store
        /// e.g. the database name.
        /// </summary>
        string PersistenceStoreName { get; }
    }
}
