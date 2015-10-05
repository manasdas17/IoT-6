using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ST.IoT.Services.Core.P2P.Client.Portable;

namespace ST.IoT.Spike.MQTT.JustSendMeMessagesThroughServer
{
    public class JustSendToMeMqttMessageListener
    {
        private PeerCoordinator _coordinator;
        private PeerServiceManager _serviceManager;

        public JustSendToMeMqttMessageListener()
        {
            _coordinator = new PeerCoordinator();
            _serviceManager = new PeerServiceManager(
                new PeerService[]
                {
                    new PeerService(new PeerMessageHandler[]
                    {
                        new PeerMessageHandler(new EndpointDescription[]
                        {
                            new EndpointDescription()
                        },
                        receivedMessage)
                    })
                });
        }

        private void receivedMessage(PeerChannel channel, string message)
        {
        }

        public void Start()
        {
            _coordinator.Start();
            _serviceManager.Start();
        }
    }
}
