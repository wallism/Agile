using System.ServiceModel;

namespace Agile.Framework.Services
{
    /// <summary>
    /// ClientSideTransaction Service Contract.
    /// </summary>
    [ServiceContract]
    public interface IClientSideTransactionService
    {
        /// <summary>
        /// Begin a ClientSideTransaction
        /// </summary>
        [OperationContract]
        ClientSideTransaction BeginTransaction();

        /// <summary>
        /// Rollback a ClientSideTransaction
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void RollbackTransaction(ClientSideTransaction transaction);

        /// <summary>
        /// Commit a ClientSideTransaction
        /// </summary>
        [OperationContract]
        void CommitTransaction(ClientSideTransaction transaction);
    }
}
