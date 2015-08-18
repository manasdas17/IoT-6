using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ST.IoT.API.REST.Router.Plugins.Interfaces;

namespace ST.IoT.API.REST.Router.Plugins.Auth
{
    [Export(typeof(IRestRouterPlugin))]
    [ExportMetadata("Services", "authorization")]
    public class AuthRestRouterPlugin : IRestRouterPlugin
    {
        public bool CanHandle(HttpRequestMessage request)
        {
            return false;
        }

        public Task<HttpResponseMessage> HandleAsync(HttpRequestMessage request)
        {
            throw new NotImplementedException();
        }

        public void Start()
        {

        }

        public void Stop()
        {

        }

    }
}
