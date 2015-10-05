using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ST.IoT.API.REST.Util.UrlPatterns;

namespace ST.IoT.Services.Stlth.API.REST
{
    public class RestApiMapper
    {
        private readonly UrlPatternDispatcher _patternDispatcher;

        public RestApiMapper()
        {
 
        }

        public async Task<HttpResponseMessage> ProcessAsync(HttpRequestMessage request)
        {
            return null;
        }
    }
}
