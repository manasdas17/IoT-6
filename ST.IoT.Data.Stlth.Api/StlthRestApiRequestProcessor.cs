using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Data.Stlth.Api
{
    public class StlthRestApiRequestProcessor
    {
        private StlthRouter _router;

        public StlthRestApiRequestProcessor(StlthDataClient dataClient)
        {
            _router = new StlthRouter(dataClient);
        }

        public async Task<HttpResponseMessage> handle(HttpRequestMessage request)
        {
            var r = await _router.process(request);
            return r;
        }
    }
}
