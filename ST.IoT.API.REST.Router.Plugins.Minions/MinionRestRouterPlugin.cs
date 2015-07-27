using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;
using ST.IoT.API.REST.Router.Plugins.Interfaces;
using ST.IoT.Services.Minions.Endpoints.Send.MTRMQ;
using ST.IoT.Services.Minions.Messages;

namespace ST.IoT.API.REST.Router.Plugins.Minions
{
    [Export(typeof(IRestRouterPlugin))]
    [ExportMetadata("Services", "minions")]
    public class MinionRestRouterPlugin : IRestRouterPlugin
    {
        private MinionsSendEndpointMTRMQ _minionsServiceFacade;

        public MinionRestRouterPlugin()
        {
            _minionsServiceFacade = new MinionsSendEndpointMTRMQ();
            _minionsServiceFacade.Start();
        }

        public async Task<HttpResponseMessage> HandleAsync(HttpRequestMessage request)
        {
            var content = await request.Content.ReadAsStringAsync();

            var reply = await _minionsServiceFacade.ProcessRequestAsync(new MinionsRequestMessage(content));

            // TODO: need to analyze the minions response and put it in the HttpResponseMessage

            var response = new HttpResponseMessage(reply.StatusCode);
            response.Content = new StringContent(reply.Response);
            return response;
        }
    }
}
