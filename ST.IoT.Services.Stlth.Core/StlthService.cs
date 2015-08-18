using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NLog;
using ST.IoT.Messaging.Bus.Core;
using ST.IoT.Services.Stlth.API.REST;
using ST.IoT.Services.Stlth.Endpoints;
using ST.IoT.Services.Stlth.Messages;

namespace ST.IoT.Services.Stlth.Core
{
    public interface IStlthService
    {
        void Start();
        void Stop();
    }

    public class StlthService : IStlthService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly ConsumeRestMessageEndpoint _consumer;

        private RestApiMapper _mapper = new RestApiMapper();

        public StlthService(IConsumeRestMessageEndpoint consumer)
        {
            _consumer = consumer as ConsumeRestMessageEndpoint;
            _consumer.Handler = consumer_MessageReceived;
        }

        private async Task<RestResponse> consumer_MessageReceived(RestRequest request)
        {
            _logger.Info("Got request");

            var response = await _mapper.ProcessAsync(request.Request);

            _logger.Info("Returning from processing request");

            return new RestResponse(response);
        }

        public void Start()
        {
            _logger.Info("Starting");
            _consumer.Start();
            _logger.Info("Started");
        }

        public void Stop()
        {
            _logger.Info("Stopping");
            _consumer.Stop();
            _logger.Info("Stopped");
        }
    }
}
