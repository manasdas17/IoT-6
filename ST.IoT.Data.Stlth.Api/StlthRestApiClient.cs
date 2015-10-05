using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace ST.IoT.Data.Stlth.Api
{
    public class StlthRestApiClient2
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public HttpResponseMessage Process(HttpRequestMessage message)
        {
            _logger.Info(message);
            return null;
        }
    }
}
