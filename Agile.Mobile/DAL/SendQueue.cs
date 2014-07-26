using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Diagnostics.Logging;
using Agile.Framework.Web;
using Agile.Mobile.Web;
using Agile.Shared;
using SQLite.Net.Attributes;

namespace Agile.Mobile.DAL
{
    /// <summary>
    /// Serialized post messages used by the Send Service
    /// Client side only!
    /// </summary>
    public class SendQueue : LocalDbRecord
    {
        /// <summary>
        /// ctor (be default, the Method is POST and ContentType is Json)
        /// </summary>
        public SendQueue()
        {
            Method = HttpHelper.POST;
            ContentType = ContentTypes.JSON;
            Succeeded = false;
        }

        [PrimaryKey, AutoIncrement]
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

        [Ignore]
        public bool Succeeded { get; set; }

        /// <summary>
        /// once it fails n (3?) times  clear from the Q,
        /// move to the FailedQueue (then we can retry from there another n times) 
        /// (maybe try sending to server for analysis)
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
