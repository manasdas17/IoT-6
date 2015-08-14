using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ST.IoT.Messaging.Endpoints.Interfaces;

namespace ST.IoT.API.REST.Proxy.Interfaces
{
    public interface IRestApiProxyHost
    {
        string BaseAddress { get; set; }
        //ISendRESTRequestToRESTRouterEndpoint RestRouterEndpoint { get; }

        void Start();
        void Stop();
    }
}
