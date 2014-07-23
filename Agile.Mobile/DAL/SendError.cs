using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Diagnostics.Logging;
using Agile.Mobile.Web;
using Agile.Shared;
using SQLite.Net.Attributes;

namespace Agile.Mobile.DAL
{
    /// <summary>
    /// Records in the SendQueue that fail to send more than n times are
    /// moved into this table.
    /// Plan is to try to resend this data on app startup. but this requires 
    /// some thought because the sendQueue is fifo, if send from here succeeds we could overwrite 
    /// a more recent change that worked. Maybe best to leave errors be...unless they are
    /// new records, i.e. ones that have never been sent to the server. It's ok to lose
    /// an update but losing the whole orginal record could be unacceptable.
    /// 
    /// Leaving this for now...
    /// </summary>
    public class SendError : LocalDbRecord
    {
        [PrimaryKey, AutoIncrement]
        public int SendErrorId { get; set; }
        
        public int SendQueueId { get; set; }
        /// <summary>
        /// Type of the serialized object (so know where to send)
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// Serialized object (usually json)
        /// </summary>
        public byte[] Data { get; set; }
        /// <summary>
        /// POST (usually)
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// usually Application/json
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// extra to the url path
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// once it fails n (3?) times the clear from the Q,
        /// move to the FailedQueue (then we can retry from there another n times) 
        /// (maybe try sending to server for analysis)
        /// Why move? because this table is fifo, which means we want to NOT process
        /// the other messages until the current one is complete.
        /// </summary>
        /// <remarks>network connection failures should not
        /// increase this value. So 'not found' errors should be handled differently.
        /// 
        /// TODO: how to handle failures needs to be mapped out or at least tested with common scenarios (internal server error, not found, no connection etc).</remarks>
        public int Attempts { get; set; }
        

        public override long GetId()
        {
            return SendQueueId;
        }

        public override void SetId(long id)
        {
            Logger.Debug("Ignore SetId because for SendQueue the id is AutoIncrement");
        }
    }
}
