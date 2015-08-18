using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using NLog;
using ST.IoT.API.REST.Proxy.Interfaces;
using ST.IoT.API.REST.Proxy.OWIN;
using ST.IoT.Data.Neo;
using ST.IoT.Hosts.Interfaces;
using ST.IoT.Hosts.Minions;
using ST.IoT.Hosts.RestProxyService;
using ST.IoT.Hosts.RestRouter;
using ST.IoT.Messaging.Busses.Factories.MTRMQ;
using ST.IoT.Services.Minions;
using ST.IoT.Services.Minions.Data.STNeo;

namespace ST.IoT.Aggregates.Services.AggreateServiceHost
{
    class Program
    {
        private Logger _logger = LogManager.GetCurrentClassLogger();

        [ImportMany(typeof(IHostableService))] public IEnumerable<IHostableService> _services = new List<IHostableService>();

        private static void Main(string[] args)
        {
            new Program().run();
        }

        private void run()
        {
            _logger.Info("Starting aggregate services host");

            /*
            var aggCatalog = new AggregateCatalog();
            var directoryCatalog = new DirectoryCatalog(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ST.IOT.HOSTS.RESTPROXYSERVICE.dll");
             * */

            try
            {
                var kernel = new StandardKernel();

                kernel.Bind<IMassTransitRabbitMQFactory>().To<MassTransitRabbitMQFactory>();
                //kernel.Bind<ISendRESTRequestToRESTRouterEndpoint>().To<RabbitMQViaMassTransit>();
                kernel.Bind<IRestApiProxyHost>().To<OwinRestApiProxyHost>();
                kernel.Bind<IRestProxyServiceHost>().To<RestProxyServiceHost>();

                //var proxy = kernel.Get<IRestProxyServiceHost>();

                var f = new MassTransitRabbitMQFactory();

                //var proxy = new RestProxyServiceHost(new OwinRestApiProxyHost(new RabbitMQViaMassTransit(f)));
                /*
                var router = new RestRouterHost(new FromRabbitMQViewMassTransit(f));
                var minions =
                    new MinionsHost(new MinionsService(new MinionsMTRMQReceiveEndpoint(f),
                        new MinionsSeamlessThingiesNeo4JDataFacade(new Neo4jThingsDataFacade(new NullThingUpdatedSink()))));

                //proxy.Start();
                router.Start();
                minions.Start();


                Console.WriteLine("Up and running.  Press enter to exit...");
                Console.ReadLine();

                f.Stop();
                minions.Stop();
                //proxy.Stop();
                router.Stop();

//                (proxy as IHostableService).Stop();

                Console.WriteLine("stopped completely");
                var allTargets = LogManager.Configuration.AllTargets;

                foreach (var target in allTargets)
                    target.Dispose();
                    */

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);
                }
            }

        }
    }
}
