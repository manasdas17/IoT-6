using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Services.Core.P2P.Client.Portable
{
    public class PeerMessageHandler
    {
        private EndpointDescription[] _endpoints;
        private Action<PeerChannel, string> _messageHandler;

        public PeerMessageHandler(EndpointDescription[] endpoints, Action<PeerChannel, string> messageHendler)
        {
            _endpoints = endpoints;
            _messageHandler = messageHendler;
        }

        public void Start()
        {
            
        }
    }
}
