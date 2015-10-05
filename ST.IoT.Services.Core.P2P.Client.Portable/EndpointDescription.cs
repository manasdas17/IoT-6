using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Services.Core.P2P.Client.Portable
{
    public class EndpointDescription
    {
        public string Protocol { get; set; }
        public string Address { get; set; }
        public string Topic { get; set; }
    }
}
