using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Services.Core.P2P.Client.Portable
{
    public class PeerCoordinator
    {
        private PeerClientFactory _peerClientFactory;


        public PeerCoordinator()
        {
            _peerClientFactory = new PeerClientFactory();
        }

        public void Start()
        {
            // default is to get a new SignalR connection to a fixed address
            var snc = new SupernodeClient(new SignalRChannel("http://localhost:8081/signalr"));
            snc.Announce("HI!");
        }

        public void Stop()
        {
            
        }
    }
}
