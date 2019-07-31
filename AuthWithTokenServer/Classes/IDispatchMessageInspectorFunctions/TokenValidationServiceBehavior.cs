using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace AuthWithTokenServer.Classes.IDispatchMessageInspectorFunctions
{
    /// <summary>
    ///     A viselkedési szolgáltatás-t definiáló osztály
    /// </summary>
    public class TokenValidationServiceBehavior : IServiceBehavior
    {
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {

        }

        /// <summary>
        ///     Lehetőséget biztosít a futási idejű proberty értékek
        ///     megváltoztatására, vagy egyéni kiterjesztésű objektumok használatára:
        ///     pl.: hibakezelők, üzenetek vagy paraméter
        ///     bővítmények, biztonsági bővítmények használata stb..
        /// </summary>
        /// <param name="serviceDescription">A szolgáltatás leírása</param>
        /// <param name="serviceHostBase">A jelenleg felépült kiszolgáló állomás</param>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            /// Lekérdezzük a kiszolgáló állomás által használt ChannelDispatcher-jeit
            foreach (var item in serviceHostBase.ChannelDispatchers)
            {
                if (item is ChannelDispatcher channelDispatcher)
                {
                    /// Lekérdezzük azokat az EndpointDispatcher-eket amelyek
                    /// továbbítják az üzeneteket a csatorna végpontjaihoz
                    foreach (var endpointDispatcher in channelDispatcher.Endpoints)
                    {
                        endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new TokenValidationInspector());
                    }
                }
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {

        }
    }
}