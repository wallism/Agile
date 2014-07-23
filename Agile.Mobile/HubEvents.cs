using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Mobile
{
    /// <summary>
    /// Generic HubEvents for all clients
    /// </summary>
    public static class HubEvents
    {
        public static string LoginCompletedSuccessfully = "LoginCompletedSuccessfully";
        public static string ServiceCallException = "ServiceCallException";

        public static class Network
        {
            public static string ConnectionChanged = "NetworkConnectionChanged";
            /// <summary>
            /// Should do this when a view is Resumed (or similar) or when we detect a change in the network (e.g. on Android there is a receiver that publishes this event when it is notified of a network change)
            /// </summary>
            public static string CheckNetworkConnection = "CheckNetworkConnection";
        }
    }
}
