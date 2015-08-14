using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.API.REST.Router.Plugins.Interfaces
{
    public interface IRestRouterPlugin
    {
        bool CanHandle(HttpRequestMessage request);
        Task<HttpResponseMessage> HandleAsync(HttpRequestMessage request);
    }
}
