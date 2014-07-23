using System;
using System.ServiceModel;
using Agile.Diagnostics.Logging;
using Agile.SAL;

namespace Agile.Framework
{
    public abstract class ClientBizServiceBase<T> : ClientBase<T>
        where T : class
    {
        /// <summary>
        /// Ctor
        /// </summary>
        protected ClientBizServiceBase(Type contract)
            : base(new BasicHttpBinding()
            , new EndpointAddress(string.Format("{0}", BizWebServiceInterfaceHelper.GetServiceAddress(contract))))
        {
            var basic = Endpoint.Binding as BasicHttpBinding;
#if SILVERLIGHT
            if(basic != null)
                basic.EnableHttpCookieContainer = true;
#else
            if (basic != null)
            {
                basic.AllowCookies = true;
                
                Endpoint.Behaviors.Add(new WriteToLogCustomBehavior());
//                basic.Security.Mode
            }
#endif
        }
        
#if WINDOWS_PHONE
        /// <summary>
        /// Creates and returns a new channel factory for the service
        /// </summary>
        private T GetServiceChannel()
        {
            var address = new EndpointAddress(string.Format("{0}", BizWebServiceInterfaceHelper.GetServiceAddress(typeof(T))));
            var binding = new BasicHttpBinding();
            T serviceChannel = new ChannelFactory<T>(binding, address).CreateChannel(address);
            Logger.Debug(serviceChannel.ToString());
           return null;
        }

        /// <summary>
        /// Returns a new channel from the client to the service.
        /// </summary>
        /// <returns>
        /// A channel of type that identifies the type of service contract encapsulated by this client object (proxy).
        /// </returns>
        protected override T CreateChannel()
        {
            // not sure about this, put in because is abstract in Mobile.
            // GetServiceChannel method was there for ages but commented out so that's what I'm not sure works right. mw 26032011
            return GetServiceChannel();
        }
#endif
        
    }

    


}
