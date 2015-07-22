using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Owin;
using ST.IoT.API.REST.Proxy.OWIN;
using ST.IoT.Messaging.HttpRequestGateway.Interfaces;

[assembly: OwinStartup(typeof(Proxy))]


namespace ST.IoT.API.REST.Proxy.OWIN
{
    public class Proxy
    {
        static List<IDisposable> _apps = new List<IDisposable>();
        public static IHttpRequestGateway HttpRequestGateway { get; set; }

        public Proxy()
        {
        }

        public static void Start(string proxyAddress)
        {
            try
            {
                // Start OWIN proxy host 
                _apps.Add(WebApp.Start<Proxy>(proxyAddress));

                Trace.WriteLine("Proxy server is running at " + proxyAddress);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                if (ex.InnerException != null)
                    message += ":" + ex.InnerException.Message;

                Trace.TraceInformation(message);
            }
        }

        public static void Stop()
        {
            foreach (var app in _apps)
            {
                if (app != null)
                    app.Dispose();
            }
        }

        public void Configuration(IAppBuilder appBuilder)
        {
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
                                        HttpRequestGateway as DelegatingHandler, 
                                    }
                    ),
                defaults: new { path = RouteParameter.Optional },
                constraints: null);
            
            appBuilder.UseWebApi(httpconfig);
        }
    }
}
