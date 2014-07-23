using System;
using Agile.Diagnostics.Logging;
using Agile.Framework.Services.Async;
using Microsoft.Practices.Composite.Presentation.Events;

namespace Agile.Framework.Clients
{
    /// <summary>
    /// Client implementation of ClientSideTransactionService implementation
    /// </summary>
    /// <remarks>Currently ClientSideTransactions are testing use only, 
    /// once you start the trans you don't need to feed it into any other methods...</remarks>
    public class ClientSideTransactionService : ClientBizServiceBase<IClientSideTransactionService>, IClientSideTransactionService
    {        
        /// <summary>
        /// Instantiate an empty PartyWebSiteService object.
        /// </summary>
        public ClientSideTransactionService() : base(typeof(IClientSideTransactionService)){}

        /// <summary>
        /// BeginBeginTransaction
        /// </summary>
        public IAsyncResult BeginBeginTransaction(AsyncCallback callback, object asyncState)
        {
            IAsyncResult result = Channel.BeginBeginTransaction(callback, this);
            return result;
        }
        /// <summary>
        /// EndBeginTransaction
        /// </summary>
        public ClientSideTransaction EndBeginTransaction(IAsyncResult result)
        {
            try
            {
                return Channel.EndBeginTransaction(result);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw; // always want to 'bubble up' the message from here                
            } 
        }


        #region Rollback Async Methods

        /// <summary>
        /// This method has been made public to allow it to be used when using
        /// the PowerThreading library from Wintellect. If you're not using that
        /// then use RollbackAsync instead and subscribe to the Rollback completed event.
        /// </summary>
        public void RollbackTransaction(ClientSideTransaction transaction)
        {
            Channel.RollbackTransaction(transaction);
        }

        #endregion



        #region Commit Async Methods

        /// <summary>
        /// Commit asynchronously. 
        /// The caller must Subscribe to transactionCommitCompletedEvent to get the return value.
        /// </summary>
        public void CommitAsync(ClientSideTransaction transaction)
        {
            BeginCommitTransaction(transaction, CommitTransactionCallback, null);
        }

        /// <summary>
        /// This method has been made public to allow it to be used when using
        /// the PowerThreading library from Wintellect. If you're not using that
        /// then use CommitAsync instead and subscribe to the Commit completed event.
        /// </summary>
        public IAsyncResult BeginCommitTransaction(ClientSideTransaction transaction, AsyncCallback callback, object state)
        {
            IAsyncResult result = Channel.BeginCommitTransaction(transaction, callback, this);
            return result;
        }

        /// <summary>
        /// Callback handler for the Commit method. Publishes the results
        /// </summary>
        private void CommitTransactionCallback(object state)
        {
            EndCommitTransaction(state as IAsyncResult);

        }

        /// <summary>
        /// Calls the EndCommit method on the channel method.
        /// </summary>
        public void EndCommitTransaction(IAsyncResult result)
        {
            try
            {
                Channel.EndCommitTransaction(result);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// Prism Publish/Subscribe event
        /// </summary>
        public class ClientSideTransactionCommitCompletedEvent : CompositePresentationEvent<object> { }

        #endregion
    }
}