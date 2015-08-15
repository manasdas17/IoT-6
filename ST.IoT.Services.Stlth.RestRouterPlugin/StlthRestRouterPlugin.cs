using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ST.IoT.API.REST.Router.Plugins.Interfaces;

namespace ST.IoT.Services.Stlth.RestRouterPlugin
{
    public class StlthRestRouterPlugin : IRestRouterPlugin
    {
        public StlthRestRouterPlugin()
        {
            
        }

        public bool CanHandle(HttpRequestMessage request)
        {
            var uri = request.RequestUri.ToString().ToLower();
            return uri.StartsWith("stlth.") || uri.StartsWith("command.stlth.") || uri.StartsWith("reply.stlth.");
        }

        public Task<HttpResponseMessage> HandleAsync(HttpRequestMessage request)
        {
            return null;
        }
    }
}
