using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using NLog;
using Owin;
using ST.IoT.API.REST.Proxy.OWIN;
using ST.IoT.API.REST.PushRequestHttpHandler;
using ST.IoT.Messaging.Bus.Core;
using ST.IoT.Messaging.Messages.REST.Routing;
using ST.IoT.Messaging.Security;
using ILogger = Microsoft.Owin.Logging.ILogger;

[assembly: OwinStartup(typeof(Proxy))]


namespace ST.IoT.API.REST.Proxy.OWIN
{
    public class Proxy
    {
        static List<IDisposable> _apps = new List<IDisposable>();
        public static DelegatingHandler HttpRequestGateway { get; set; }

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private static IRequestReplySendEndpoint<RestRequestToRouterMessage, RestRouterReplyMessage> _endpoint;
        private static IRestAuthorizer _authorizer;

        public Proxy()
        {
        }

        public static void Start(
            string proxyAddress, 
            IRequestReplySendEndpoint<RestRequestToRouterMessage, RestRouterReplyMessage> endpoint,
            IRestAuthorizer restAuthorizer)
        {
            _authorizer = restAuthorizer;
            _endpoint = endpoint;

            try
            {
                _logger.Trace("Starting proxy");

                // Start OWIN proxy host 
                _apps.Add(WebApp.Start<Proxy>(proxyAddress));

                _logger.Trace("Proxy server is running at :" + proxyAddress);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                if (ex.InnerException != null)
                    message += ":" + ex.InnerException.Message;

                _logger.Error(message);
            }
        }

        public static void Stop()
        {
            _logger.Trace("Stopping proxy");
            foreach (var app in _apps)
            {
                if (app != null)
                    app.Dispose();
            }
            _logger.Trace("Proxy stopped");

            _endpoint.Stop();
        }

        public void Configuration(IAppBuilder appBuilder)
        {
            _logger.Trace("Configuring OWIN");

            appBuilder.MapSignalR();

            // Configure Web API for self-host. 
            var httpconfig = new HttpConfiguration();

            httpconfig.Routes.MapHttpRoute(
                name: "Proxy",
                routeTemplate: "{*path}",
                handler: HttpClientFactory.CreatePipeline
                    (
                        innerHandler: new HttpClientHandler(), 
                        handlers: new [] 
                                    { 
                                        //new MinionsChunkedWebRequestHandler(),
                                        new RestRequestForwarder(_endpoint, _authorizer) as DelegatingHandler
                                    }
                    ),
                defaults: new { path = RouteParameter.Optional },
                constraints: null);
            
            appBuilder.UseWebApi(httpconfig);

            _logger.Trace("OWIN configured");
        }
    }
}
