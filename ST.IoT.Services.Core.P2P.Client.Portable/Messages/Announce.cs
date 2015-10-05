using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Services.Core.P2P.Client.Portable
{
    public class Announce 
    {
        public string Message { get; set; }

        public EndpointDescription[] Endpoints { get; set; }

        public Announce()
        {
        }

        public Announce(string message)
        {
            Message = message;
        }
    }
}
