using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Agile.Diagnostics.Logging;
using Agile.Framework.Web;
using Agile.Mobile.DAL;
using Agile.Shared;
using Agile.Shared.IoC;
using AutoMapper;

namespace Agile.Mobile.Web
{
    public interface ISendService
    {
        bool IsStarted { get; }
        bool IsSending { get; }
        SendQueue GetNext();
        void Start(int intervalSeconds = 15);
        void Stop();

    }

    /// <summary>
    /// All posts should be made through here from mobile apps.
    /// The service serializes the call and puts it into the sendQ (fifo), 
    /// then when it has a connection is deserializes and posts,
    /// then when successful it deletes from the queue and publishes success.
    /// </summary>
    /// <remarks>because we are serializing then deserializing, we need to have concrete
    /// versions of the service so we can still use the HttpServiceBase post with the ServiceCallResult,
    /// the problem being the generic type parameter, if we are deserializing, without
    /// some kind of reflection we can't pass T.</remarks>
    public abstract class SendService<TDB> : ISendService
        where TDB : class, IAgileSQLite
    {
        private TDB db;
        /// <summary>
        /// Gets the SQLite database
        /// </summary>
        protected TDB Db
        {
            get { return db ?? (db = Container.Resolve<TDB>()); }
        }

        private INetworkConnectionManager connectionManager;
        private INetworkConnectionManager ConnectionManager {
            get { return connectionManager 
                ?? (connectionManager = Container.Resolve<INetworkConnectionManager>());
            }
        }

        private static int count;
        private int version;
        /// <summary>
        /// ctor
        /// </summary>
        protected SendService()
        {
            version = count++;
            IsSending = false;
            Logger.Debug("         version {0}", version);
        }

        protected string GetDataTypeName(Type type)
        {
            return type.Name;
        }

        protected SendQueue BuildQueueRecord<T>(T value
            , string url = "")
        {
            var record = new SendQueue();
            record.Data = HttpServiceBase.Create(value);
            record.Url = url;

            record.Method = HttpServiceBase.POST;
            record.ContentType = ContentTypes.JSON;
            record.DataType = GetDataTypeName(typeof (T));
            record.Created = AgileDateTime.UtcNow;
            return record;
        }

        private IDisposable disposable;
        private bool isSending;

        /// <summary>
        /// Start sending every n seconds
        /// </summary>
        /// <param name="intervalSeconds"></param>
        public void Start(int intervalSeconds = 5)
        {
            if(IsStarted)
                return;

            Logger.Debug("Set interval to {0}", intervalSeconds);
            disposable = Observable.Interval(TimeSpan.FromSeconds(intervalSeconds))
                      .Subscribe(next => SendAllInQueue()
                    , Logger.Error
                    , () => Logger.Debug("Interval Send Subscription completed"));
        }

        /// <summary>
        /// Stop sending every n seconds
        /// </summary>
        /// <remarks>note that it is REALLY important to call stop 
        /// in unit tests if you call start!!!</remarks>
        public void Stop()
        {
            if(disposable == null)
                return;
            disposable.Dispose();
            disposable = null;
        }

        /// <summary>
        /// Get the next item that is to be sent
        /// </summary>
        public SendQueue GetNext()
        {
            return Db.GetFirstRecord<SendQueue>();
        }


        protected void AddToQueue(SendQueue record)
        {
            Db.Insert(record);
        }

        protected void RemoveFromQueue(SendQueue record)
        {
            Db.Delete(record);
        }


        public bool IsStarted {
            get { return disposable != null; }
        }

        // a little hacky but a simple quick way of ensuring that sending does not kick off again before the first send finishes
        public bool IsSending
        {
            get { return isSending; }
            private set { 
                isSending = value; 
                Logger.Debug("isSending setTo:{0}", isSending);
            }
        }

        private async Task SendAllInQueue()
        {
            if (IsSending)
                return;
            IsSending = true;

            // todo: check users settings for wifi
            // for now, always/only allow if wifi or mobile or Ethernet
            if (!ConnectionManager.CanSend)
            {
                IsSending = false; // leave this and the above set to true, just makes it easier to test to have the IsSending change...
                Logger.Debug("Cannot send with the Current Connection");
                return;
            }

            try
            {
                var count = Db.GetRecordCount(typeof (SendQueue));
                if (count == 0)
                    return;
                while (count > 0)
                {
                    var next = Db.GetFirstRecord<SendQueue>();
                    try
                    {
                        // allow the concrete implementation process the Post (it knows types)
                        var result = await PostFromQueue(next);
                        if (!string.IsNullOrEmpty(result))
                        {
                            FlagAsFailed(next, result);
//                            break; // failed, stop processing
                            // for v1 we only do fifo as long as there are no errors. 
                            // if a record has a problem sending we don't want to stop others from going through.
                            // if it is a connectivity issue, the call to FlagAsFailed refreshed the NetworkConnectionManager.
                        }
                        else
                        {
                            if (next.Succeeded)
                                Db.Delete(next);
                        }
                        count--;
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex, "PostFromQueue");
                        FlagAsFailed(next);
                        // want to stop this send, queue is fifo, if we don't break then the next one could get processed
//                        break;   see not above about v1
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "SendAllInQueue");
            }
            finally
            {
                IsSending = false; // essential this gets set back to false
            }
        }

        protected const string InvalidDatatype = "Invalid DataType, no implementation for POST of the items DataType";

        private void FlagAsFailed(SendQueue next, string result = "")
        {
            // todo: investigate causes of errors and take more appropriate action based on what the hell just happened
            // if the error is some kind of connectivity issue do not save
            // for now, just ensure connection manager is up to date with latest
            var changed = connectionManager.CheckConnection();
            if(changed && !connectionManager.CanSend)
                return; // don't flag it as an error if we now do not have a connection

            if (result == InvalidDatatype)
            {
                Logger.Testing("this record will never succeed so removing it");
                Db.Delete(next);
                return;
            }

            next.Attempts++;

            if (next.Attempts >= 5)
            {
                // for now we are just going to drop from the table
                // todo: wire up handling of errors - at the moment the SendError table gets populated but we do nothing with it.
                Logger.Warning("ADD TO SENDERROR because failed 5 or more times (also deleting from SendQueue)");
                var sendError = Mapper.DynamicMap<SendQueue, SendError>(next);
                Db.Insert(sendError);
                Db.Delete(next);
                return;
            }
            Db.Update(next);
        }

        protected virtual async Task<string> PostFromQueue(SendQueue record)
        {
            // has to be virtual so can apply async keyword...
            throw new Exception("need to implement!!!");
        }


    }


}