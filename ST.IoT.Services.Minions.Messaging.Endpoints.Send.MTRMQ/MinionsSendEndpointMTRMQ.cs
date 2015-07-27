using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.RabbitMqTransport;
using ST.IoT.Services.Minions.Interfaces;
using ST.IoT.Services.Minions.Messages;

namespace ST.IoT.Services.Minions.Endpoints.Send.MTRMQ
{
    public class MinionsSendEndpointMTRMQ : IMinionsServiceFacade
    {
        private string _address = "rabbitmq://localhost/minions_virtual_host";
        private IBusControl _bus;
        private IRabbitMqHost _host;
        private BusHandle _handle;
        private IRequestClient<MinionsRequestMessage, MinionsResponseMessage> _client;
 
        public async Task<MinionsResponseMessage> ProcessRequestAsync(MinionsRequestMessage request)
        {
            var response = await _client.Request(request);
            return response;
        }

        public void Start()
        {
            _bus = Bus.Factory.CreateUsingRabbitMq(x =>
            {
                _host = x.Host(new Uri(_address), h =>
                {
                    h.Username("minion_boss");
                    h.Password("minion_boss");
                });
            });

            _handle = _bus.Start();

            var address = _address + "/" + "minion_requests";
            _client = _bus.CreateRequestClient<MinionsRequestMessage, MinionsResponseMessage>(new Uri(address), TimeSpan.FromSeconds(500));
        }

        public void Stop()
        {
            if (_handle != null)
            {
                _handle.Stop();
            }
        }
    }
}
