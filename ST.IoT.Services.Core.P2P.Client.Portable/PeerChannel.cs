using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Services.Core.P2P.Client.Portable
{
    public interface IPeerChannel
    {
        void Send(object msg);
    }

    public abstract class PeerChannel : IPeerChannel
    {
        public PeerChannel()
        {
        }

        public abstract void Send(object message);
    }
}
