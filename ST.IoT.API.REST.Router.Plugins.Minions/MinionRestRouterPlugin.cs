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

namespace ST.IoT.API.REST.Router.Plugins.Minions
{
    [Export(typeof(IRestRouterPlugin))]
    [ExportMetadata("Services", "minions")]
    public class MinionRestRouterPlugin : IRestRouterPlugin
    {
        public async Task<HttpResponseMessage> HandleAsync(HttpRequestMessage request)
        {

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            return response;
        }
    }
}
