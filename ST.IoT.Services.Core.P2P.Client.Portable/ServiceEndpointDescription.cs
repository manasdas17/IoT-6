using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Services.Core.P2P.Client.Portable
{
    public class ServiceEndpointDescription
    {
        public string ServiceName { get; set; }
        public EndpointDescription[] Endpoints { get; set; }
    }
}
