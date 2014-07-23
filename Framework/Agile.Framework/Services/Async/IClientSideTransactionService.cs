using System;
using System.ServiceModel;

namespace Agile.Framework.Services.Async
{
    /// <summary>
    /// Async Service Contract.
    /// </summary>
    [ServiceContract]
    public interface IClientSideTransactionService
    {
        /// <summary>
        /// Asynchronously begin a transaction
        /// </summary>
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginBeginTransaction(AsyncCallback callback, object asyncState);

        ClientSideTransaction EndBeginTransaction(IAsyncResult result);



        /// <summary>
        /// Begin Rollback client side transaction
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void RollbackTransaction(ClientSideTransaction transaction);

        


        /// <summary>
        /// Begin Commit client side transaction
        /// </summary>
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginCommitTransaction(ClientSideTransaction transaction, AsyncCallback callback, object asyncState);

        void EndCommitTransaction(IAsyncResult result);
    }
}
