using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ST.IoT.API.REST.Router.Plugins.Interfaces;
using ST.IoT.Services.Minions.Messages;
using ST.IoT.Services.Minions.ServiceFacade.RabbitMQ;

namespace ST.IoT.API.REST.Router.Plugins.Minions
{
    [Export(typeof(IRestRouterPlugin))]
    [ExportMetadata("Services", "minions")]
    public class MinionRestRouterPlugin : IRestRouterPlugin
    {
        private MinionsServiceFacadeForRabbitMQMassTransit _minionsServiceFacade;

        public MinionRestRouterPlugin()
        {
            _minionsServiceFacade = new MinionsServiceFacadeForRabbitMQMassTransit();
            _minionsServiceFacade.Start();
        }

        public async Task<HttpResponseMessage> HandleAsync(HttpRequestMessage request)
        {
            var content = await request.Content.ReadAsStringAsync();

            var reply = await _minionsServiceFacade.ProcessRequestAsync(new MinionsRequestMessage(content));

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            return response;
        }
    }
}
