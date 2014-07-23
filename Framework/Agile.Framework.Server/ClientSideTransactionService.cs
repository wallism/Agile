using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using Agile.DataAccess;
using Agile.Diagnostics.Logging;

namespace Agile.Framework.Services
{
    /// <summary>
    /// Server implementation of ClientSideTransactionService
    /// </summary>
    [ServiceBehavior(ConfigurationName = "ClientSideTransactionService"
        , InstanceContextMode = InstanceContextMode.Single
        , ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ClientSideTransactionService : IClientSideTransactionService
    {
        // when to start the tran on the db?
        // what if no commit or rollback is ever received - default timout which auto rolls back
        private Dictionary<Guid, Timer> timers = new Dictionary<Guid, Timer>();
        public ClientSideTransaction BeginTransaction()
        {
            ActiveTransaction.BeginTesting();
            // currently there isn't really any point returning the object as currently it is testing use only.
            // But with a few changes it should be possible to use in non-test code. Needs to be much more 
            // robust though plus need to consider multithreaded environment, auto rollback etc.
            var transaction = ClientSideTransaction.Build();
            Logger.Debug("BeginTransaction[{0}]", transaction.ID);
            Timer timer = new Timer(AutoRollbackTransaction, transaction, 10000, 60000);
            timers.Add(transaction.ID, timer);
            return transaction;
        }

        /// <summary>
        /// Callback for timers. Automatically rolls back transactions after n seconds.
        /// </summary>
        private void AutoRollbackTransaction(object state)
        {
            ClientSideTransaction transaction = state as ClientSideTransaction;
            if(transaction == null)
                return;
            RollbackTransaction(transaction);
            
        }

        public void RollbackTransaction(ClientSideTransaction transaction)
        {
            Logger.Info("RollbackTransaction[{0}]", transaction.ID);
            ActiveTransaction.FinishTesting();

            // and remove the timer
            if (!timers.ContainsKey(transaction.ID))
                return;
            Timer timer = timers[transaction.ID];
            timers.Remove(transaction.ID);
            timer.Dispose();
        }

        public void CommitTransaction(ClientSideTransaction transaction)
        {
            Logger.Debug("CommitTransaction[{0}]", transaction.ID);
            throw new NotImplementedException("current version of ClientSideTransactions is for testing use only.");
        }

    }
}