using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Ninject;
using NLog;
using ST.IoT.API.REST.Router.Messaging.Endpoints;
using ST.IoT.API.REST.Router.Plugins.Interfaces;
using ST.IoT.Messaging.Bus.Core;
using ST.IoT.Messaging.Messages.REST.Routing;

namespace ST.IoT.API.REST.Router
{
    public interface IRestApiRouterService
    {
        void Start();
        void Stop();
    }

    public class RestRouterService : IRestApiRouterService
    {

        [ImportMany] public IEnumerable<IRestRouterPlugin> _plugins = null;

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private ConsumeRestRequestEndpoint _consumer;

        [ImportingConstructor]
        public RestRouterService(IConsumeRestRequestEndpoint consumer)
        {
            _consumer = consumer as ConsumeRestRequestEndpoint;
            _consumer.Handler = consumer_MessageReceived;
        }

        public void Start()
        {
            _logger.Info("Starting REST router");

            loadPlugins();

            // start plugins
            _plugins.ToList().ForEach(p => p.Start());

            _consumer.Start();

            _logger.Info("REST router started");
        }

        public void Stop()
        {
            _logger.Info("REST router starting");

            // stop plugins
            _plugins.ToList().ForEach(p => p.Stop());

            _consumer.Stop();

            _logger.Info("REST router stopped");
        }


        private void loadPlugins()
        {
            _logger.Info("Loading plugins");
            var aggCatalog = new AggregateCatalog();

            var dc1 = new DirectoryCatalog(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "ST.IoT.API.REST.Router.Plugins.*.dll");

            var dc2 = new DirectoryCatalog(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "ST.IoT.SERVICES.STLTH.*.DLL");

            var dc3 = new DirectoryCatalog(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "ST.IoT.API.REST.Router.Plugins.Minions.DLL");
            var dc4 = new DirectoryCatalog(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "ST.IoT.Services.Minions.Endpoints.DLL");

            aggCatalog.Catalogs.Add(dc1);
            aggCatalog.Catalogs.Add(dc2);
            aggCatalog.Catalogs.Add(dc3);
            aggCatalog.Catalogs.Add(dc4);

            var container = new CompositionContainer(aggCatalog, CompositionOptions.DisableSilentRejection);

            _logger.Info("Composing router plugings");

            container.ComposeParts(this);

            _logger.Info("Found the following plugins:");
            _plugins.ToList().ForEach(p =>_logger.Info(p.GetType().Name));

            _logger.Info("Started router");
        }

        private async Task<RestRouterReplyMessage> consumer_MessageReceived(RestRequestToRouterMessage message)
        {
            var request = message.HttpRequest;

            _logger.Info(message: "Message is addressed to {0}", argument: request.RequestUri.Host);
            _logger.Info(request.ToString());

            HttpResponseMessage http_response = null;

            foreach (var p in _plugins)
            {
                if (p.CanHandle(request))
                {
                    _logger.Info("Received rest request");

                    //await Task.Delay(5000);
                    http_response = await p.HandleAsync(request);

                    _logger.Info("Done processing request");
                    break;
                }
            }

            if (http_response == null)
            {
                _logger.Warn("Didn't recognize how to route: {0}", request.RequestUri.Host);

                http_response = createHttpErrorResponse(HttpStatusCode.InternalServerError,
                    "No services opted to handle this request: " + request.RequestUri);
            }

            _logger.Info("returning reply");

            return new RestRouterReplyMessage(http_response);
        }

        private HttpResponseMessage createHttpErrorResponse(HttpStatusCode code, string message)
        {
            var r = new HttpResponseMessage(code);
            r.Content = new StringContent(message);
            return r;
        }
    }

}
