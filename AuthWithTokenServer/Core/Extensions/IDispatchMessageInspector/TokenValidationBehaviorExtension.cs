using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace AuthWithTokenServer.Core.Extensions.IDispatchMessageInspector
{
    public class TokenValidationBehaviorExtension : BehaviorExtensionElement, IServiceBehavior
    {
        public override Type BehaviorType => 
            typeof(TokenValidationBehaviorExtension);

        protected override object CreateBehavior() => 
            new TokenValidationBehaviorExtension();

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcherBase item in serviceHostBase.ChannelDispatchers)
            {
                if (!(item is ChannelDispatcher channelDispatcher))
                    continue;

                foreach (EndpointDispatcher endpointDispatcher in channelDispatcher.Endpoints)
                {
                    endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new TokenValidationInspector());
                }
            }
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase,
            Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        { }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        { }
    }
}