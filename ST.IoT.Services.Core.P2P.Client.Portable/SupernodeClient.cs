using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Services.Core.P2P.Client.Portable
{
    public class SupernodeClient : ISupernodeClient
    {
        private PeerChannel _channel;

        public SupernodeClient(PeerChannel channel)
        {
            _channel = channel;
        }

        public void Announce(string msg)
        {
            _channel.Send(new Announce(msg));
        }
    }
}
