using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Services.Core.P2P.Client.Portable
{
    public class PeerService
    {
        private PeerMessageHandler[] _messageHandlers;

        public PeerService(PeerMessageHandler[] mesageHandlers)
        {
            _messageHandlers = mesageHandlers;
        }

        public void Start()
        {
            foreach (var mh in _messageHandlers)
            {
                mh.Start();
            }
        }

        public void Stop()
        {
            
        }
    }
}
