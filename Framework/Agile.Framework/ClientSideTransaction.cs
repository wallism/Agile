using System;
using System.Runtime.Serialization;

namespace Agile.Framework
{
    /// <summary>
    /// Client initiated transaction through a web service.
    /// 
    /// The goal of this class is to enable clients connecting via
    /// a webservice to begin, commit or rollback a transaction as if the 
    /// call was happneing
    /// </summary>
    /// <remarks>Currently ClientSideTransactions are testing use only, 
    /// once you start the trans you don't need to feed it into any other methods...</remarks>
    public class ClientSideTransaction
    {
        /// <summary>
        /// Ctor
        /// </summary>
        private ClientSideTransaction()
        {
            ID = Guid.NewGuid();
        }

        /// <summary>
        /// Unique identifier for the transaction
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// Simple factory
        /// </summary>
        public static ClientSideTransaction Build()
        {
            return new ClientSideTransaction();    
        }

    }
}
