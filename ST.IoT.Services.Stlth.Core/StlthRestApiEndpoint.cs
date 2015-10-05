using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using ST.IoT.API.REST.Util.UrlPatterns;
using ST.IoT.Data.Stlth.Api;
using ST.IoT.Data.Stlth.Model;
using ST.IoT.Messaging.Bus.Core;
using ST.IoT.Services.Stlth.Endpoints;
using ST.IoT.Services.Stlth.Messages;

namespace ST.IoT.Services.Stlth.Core
{
    public class StlthRestApiEndpoint : StlthConsumeRestMessageEndpoint, IStlhRestMessageEndpoint
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private StlthRequestProcessor[] _processors;
        private IStlthService _service;

        public StlthRestApiEndpoint(IStlthService service)
        {
            _service = service;

            _processors = new StlthRequestProcessor[]
            {
                new CoreRequestProcessor(_service),
                new SocialRequestProcessor(_service),
            };
        }

        public async override Task<RestResponse> ProcessAsync(RestRequest request)
        {
            _logger.Info("Got request");

            HttpResponseMessage response = null;

            var processor = _processors.FirstOrDefault(p => p.willHandle(request.Request));
                response = processor != null ? await processor.handleAsync(request.Request) :
                                               new HttpResponseMessage(HttpStatusCode.BadRequest);

            _logger.Info("Returning from processing request");

            return new RestResponse(response);
        }
    }
}