using System.ServiceModel;

namespace Agile.Framework.Clients
{
    public static class ServiceHelper
    {
        public static void DisposeService(ICommunicationObject service)
        {
            // first, need to close the channel
            if (service.State == CommunicationState.Faulted)
                service.Abort();
            else if (service.State == CommunicationState.Closed
                || service.State == CommunicationState.Closing)
                return;

            service.Close();
        }
    }
}
