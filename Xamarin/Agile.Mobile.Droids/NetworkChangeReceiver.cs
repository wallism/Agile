using Agile.Mobile;
using Agile.Shared.IoC;
using Agile.Shared.PubSub;
using Android.Content;
using Android.Widget;

namespace Acoustie.Androids
{
    public class NetworkChangeReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            Hub.Publish(HubEvents.Network.CheckNetworkConnection);
        }
    }
}