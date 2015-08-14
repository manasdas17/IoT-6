using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Ninject;
using NLog;
using ST.IoT.API.REST.Proxy.Interfaces;
using ST.IoT.API.REST.Proxy.OWIN;
using ST.IoT.Messaging.Bus.Core;

namespace ST.IoT.Aggregates.Stlth
{
    public class foo
    {
        public string Message { get; set; }
        public override string ToString()
        {
            return Message;
        }
    }

    class Program
    {
        private Logger _logger = LogManager.GetCurrentClassLogger();

        private static void Main(string[] args)
        {
            new Program().run();
        }

        private void run()
        {
            EndpointParameters.Default = new EndpointParameters(new Uri("rabbitmq://localhost"), "iot", "iot", "stlth");
            var sender = new RequestReplySendEndpoint<foo, foo>("rest_api_requests");
            var consumer = new RequestReplyConsumeEndpoint<foo, foo>("rest_api_requests");
            consumer.MessageReceived += (s, args) =>
            {
                Console.WriteLine("Got: " + args.Message);
                args.Response = new foo() {Message = "back at ya"};
            };

            try
            {
                //var sender = new RequestReplySendEndpoint<HttpRequestMessage, HttpResponseMessage>();
                //var consumer = new RequestReplyConsumeEndpoint<HttpRequestMessage, HttpResponseMessage>("rest_api_requests");
                /*
                 * */
                sender.Start();
                consumer.Start();

                Console.WriteLine("Started");

                var response = sender.SendAsync(new foo() { Message = "HI"}).Result;

                Console.WriteLine(response);
                Console.ReadLine();

                Console.WriteLine("Shutting down");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null) Console.WriteLine(ex.InnerException.Message);
            }
            finally
            {
                sender.Stop();
                consumer.Stop();
            }

            Console.WriteLine("done");

            /*
            var busControl = Bus.Factory.CreateUsingRabbitMq(x =>
            {
                _logger.Info("Creating host");

                var host = x.Host(new Uri("rabbitmq://localhost/stlth"), h =>
                {
                    h.Username("iot");
                    h.Password("iot");
                });
            });

            _logger.Info("Starting bus");
            var busHandle = busControl.Start();
            _logger.Info("Started");

            Console.ReadLine();

            busHandle.Stop();
            */

            var kernel = new StandardKernel();
/*
            EndpointParameters.Default = new EndpointParameters(new Uri("rabbitmq://localhost"), "iot", "iot", "stlth");

            kernel.Bind<IRestApiProxyHost>().To<OwinRestApiProxyHost>();
            kernel.Bind<IRequestReplySendEndpoint<HttpRequestMessage, HttpResponseMessage>>()
                .ToMethod(c =>
                {
                    return new RequestReplySendEndpoint<HttpRequestMessage, HttpResponseMessage>();
                })
                .WhenInjectedInto<OwinRestApiProxyHost>();
                    */
            /*
            try
            {
                var proxy = kernel.Get<IRestApiProxyHost>();
                proxy.Start();

                _logger.Info("Up and running!");

                Console.ReadLine();

                proxy.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null) Console.WriteLine(ex.InnerException.Message);
            }
             * */
        }
    }
}
