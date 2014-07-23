namespace Agile.Framework
{
    /// <summary>
    /// Status a record can go through being send via the send queue
    /// </summary>
    public enum SendStatus
    { 
        Unknown=0,
        /// <summary>
        /// Not yet added to the Q, because the user is not yet registered
        /// </summary>
        UnRegistered=3,
        /// <summary>
        /// Not yet added to the Q
        /// </summary>
        /// <remarks>maybe it failed to be added...or is simply still pending a save</remarks>
        NotAddedToQ=5,
        /// <summary>
        /// It is in the Send Q ready to be sent
        /// </summary>
        AddedToSendQ=10,
        /// <summary>
        /// Either loaded from server or 
        /// Send Q has done it's job and successfully sent the record to the server.
        /// (at this stage the record will have a valid Id, i.e. > 10000)
        /// </summary>
        Done=15,
    }
}