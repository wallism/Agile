using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Mobile
{
    /// <summary>
    /// How are we connected to the big wide world
    /// </summary>
    public enum ConnectionState
    {
        NotConnected = 0,
        WiFi = 5,
        Ethernet = 10,
        Mobile = 15,
        /// <summary>
        /// By default Roaming will be treated like NO CONNECTION
        /// </summary>
        Roaming = 20,
        Unknown = 99
    }

}
