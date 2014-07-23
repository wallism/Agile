using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading;
using System.Web;
using Agile.Diagnostics.Logging;
using Agile.Framework.Security;

namespace Agile.Framework.Server
{
    public class MessageSecurityHeaderInspector : IDispatchMessageInspector 
    {
        /// <summary>
        /// Called after an inbound message has been received but before the message is dispatched to the intended operation.
        /// </summary>
        /// <returns>
        /// The object used to correlate state. This object is passed back in the <see cref="M:System.ServiceModel.Dispatcher.IDispatchMessageInspector.BeforeSendReply(System.ServiceModel.Channels.Message@,System.Object)"/> method.
        /// </returns>
        /// <param name="request">The request message.</param><param name="channel">The incoming channel.</param><param name="instanceContext">The current service instance.</param>
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            Logger.Debug("~ReceiveRequest thread:{0}", Thread.CurrentThread.ManagedThreadId);

            return null;
        }

        /// <summary>
        /// Called after the operation has returned but before the reply message is sent.
        /// </summary>
        /// <param name="reply">The reply message. This value is null if the operation is one way.</param><param name="correlationState">The correlation object returned from the <see cref="M:System.ServiceModel.Dispatcher.IDispatchMessageInspector.AfterReceiveRequest(System.ServiceModel.Channels.Message@,System.ServiceModel.IClientChannel,System.ServiceModel.InstanceContext)"/> method.</param>
        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            Logger.Debug("~SendReply thread:{0}", Thread.CurrentThread.ManagedThreadId);
        }
    }


    public class MessageSecurityHeaderBehaviourExtension : BehaviorExtensionElement
    {
        /// <summary>
        /// Creates a behavior extension based on the current configuration settings.
        /// </summary>
        /// <returns>
        /// The behavior extension.
        /// </returns>
        protected override object CreateBehavior()
        {
            return new MessageSecurityHeaderEndpointBehavior();
        }

        /// <summary>
        /// Gets the type of behavior.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Type"/>.
        /// </returns>
        public override Type BehaviorType
        {
            get { return typeof(MessageSecurityHeaderEndpointBehavior); }
        }
    }


    /// <summary>
    /// The purpose of this behaviour is to add the MessageSecurityHeaderInspector.
    /// </summary>
    /// <remarks>used by SmartClientBehaviorExtensionElement</remarks>
    public class MessageSecurityHeaderEndpointBehavior : IEndpointBehavior
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// ApplyClientBehavior
        /// </summary>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        { // don't add code here, this is a server side class
        }

        /// <summary>
        /// ApplyDispatchBehavior
        /// </summary>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher) 
        {
            var inspector = new MessageSecurityHeaderInspector();
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(inspector);
        }

        /// <summary>
        /// Validate
        /// </summary>
        public void Validate(ServiceEndpoint endpoint) { }

    }
}
