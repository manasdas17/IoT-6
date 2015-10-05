using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Services.Core.P2P.Client.Portable
{
    public class PeerServiceManager
    {
        private PeerService[] _services;

        public PeerServiceManager(PeerService[] services)
        {
            _services = services;
        }

        public void Start()
        {
            foreach (var s in _services) s.Start();
        }

        public void Stop()
        {
            foreach (var s in _services) s.Stop();
        }
    }
}
