using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using MassTransit;

namespace ST.IoT.API.REST.Router
{
    public interface IRestApiRouterService
    {
        void Start();
        void Stop();
    }
    /*
    public class RestApiRouterService : IRestApiRouterService
    {
        private IRabbitBusFactory _busFactory;

        public RestApiRouterService(IRabbitBusFactory busFactory)
        {
            _busFactory = busFactory;
        }

        public void Start()
        {
            _busFactory.Start();
        }

        public void Stop()
        {
            _busFactory.Stop();
        }
    }
     * */
}
