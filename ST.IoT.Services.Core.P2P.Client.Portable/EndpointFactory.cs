using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Services.Core.P2P.Client.Portable
{
    public class EndpointFactory
    {
        private Dictionary<string, Func<EndpointDescription, PeerChannel>> _factories = new Dictionary
            <string, Func<EndpointDescription, PeerChannel>>()
        {
            { "MQTT", createMqttChannel}
        };

        private static PeerChannel createMqttChannel(EndpointDescription arg)
        {
            throw new NotImplementedException();
        }

        public PeerChannel create(EndpointDescription endpointDescription)
        {
            return null;
        }
    }
}
