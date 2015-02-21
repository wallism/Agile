using System;
using Agile.Diagnostics.Logging;
using Agile.Shared.PubSub;

namespace Agile.Mobile
{
    public interface INetworkConnectionManager
    {
        ConnectionState CurrentConnection { get; set; }
        bool CanSend { get; }
        bool CheckConnection();
    }

    /// <summary>
    /// Goto place for checking the current connection we have to the big wide world
    /// </summary>
    public class NetworkConnectionManager : INetworkConnectionManager
    {

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="getPlatformNetworkState">Register the platform specific method that returns the current network state</param>
        public NetworkConnectionManager(Func<ConnectionState> getPlatformNetworkState)
        {
            GetPlatformNetworkState = getPlatformNetworkState;
            // check immediately so CurrentConnection is known
            CheckConnection();

            Hub.Subscribe(HubEvents.Network.CheckNetworkConnection, () => CheckConnection(), "NetworkConnectionManager");
        }

        public ConnectionState CurrentConnection { get; set; }
        private Func<ConnectionState> GetPlatformNetworkState { get; set; }
        

        public bool CanSend {
            get
            {
                return CurrentConnection == ConnectionState.WiFi
                       || CurrentConnection == ConnectionState.Ethernet
                       || CurrentConnection == ConnectionState.Mobile;
            }
        }

        /// <summary>
        /// Actively check the connection
        /// </summary>
        /// <remarks>Call this if you get a connectivity exception</remarks>
        /// <returns>true if the connection changed</returns>
        public bool CheckConnection()
        {
            if (GetPlatformNetworkState == null)
                throw new Exception("need to register a method that returns the network connection state (platform specific)");
            var newConnectionState = GetPlatformNetworkState();
            var changed = newConnectionState != CurrentConnection;
            CurrentConnection = newConnectionState;

            Logger.Debug("CheckConnection={0} (changed:{1})", newConnectionState.ToString(), changed.ToString());
            if(changed)
                Hub.Publish(HubEvents.Network.ConnectionChanged, newConnectionState);
            return changed;
        }
    }
}