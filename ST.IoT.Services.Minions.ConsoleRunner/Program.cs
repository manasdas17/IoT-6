using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using NLog;
using ST.IoT.Data.Interfaces;
using ST.IoT.Data.Neo;
using ST.IoT.Hosts.Interfaces;
using ST.IoT.Hosts.Minions;
using ST.IoT.Messaging.Busses.Factories.MTRMQ;
using ST.IoT.Messaging.Endpoints.Interfaces;
using ST.IoT.Messaging.Endpoints.MTRMQ;
using ST.IoT.Messaging.Endpoints.MTRMQ.Receive.ThingUpdated;
using ST.IoT.Messaging.Endpoints.MTRMQ.Send.ThingUpdated;
using ST.IoT.Services.Interfaces;
using ST.IoT.Services.Minions.Data.Interfaces;
using ST.IoT.Services.Minions.Data.STNeo;
using ST.IoT.Services.Minions.Interfaces;
using ST.IoT.Services.Minions.Messages;
using ST.IoT.Services.Minions.Messaging.Endpoints.Receive.MTRMQ;

namespace ST.IoT.Services.Minions.ConsoleRunner
{
    class Program
    {
        private static IKernel _kernel;

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            _kernel = new StandardKernel();
            /*
            _kernel.Bind<IThingUpdated>().To<ThingUpdatedSendEndpoint>();
            _kernel.Bind<IThingsDataFacade>().To<Neo4jThingsDataFacade>();
            _kernel.Bind<IMinionsDataService>().To<MinionsSeamlessThingiesNeo4JDataFacade>();
            _kernel.Bind<IIoTService>().To<MinionsService>().InSingletonScope();
            _kernel.Bind<IMinionsReceiveRequestEndpoint>().To<MinionsRabbitMQMassTransitReceiveEndpoint>();
             * */

            var factory = new MassTransitRabbitMQFactory();
            /*
            _kernel.Bind<IMTRMQRequestReplySendEndpoint>()
                .ToMethod(_ =>
                {
                    return new MTRMQRequestReplySendEndpoint<MinionsRequestMessage, MinionsResponseMessage>(factory);
                })
                .WhenInjectedInto<MinionsHost>();
             * */

            /*
            _kernel.Bind<IMinionsReceiveRequestEndpoint>().ToMethod(
                _ =>
                {
                    return null;
                })
                .WhenInjectedInto<MinionsService>();
        */

            _kernel.Bind<IThingUpdated>().To<NullThingUpdatedSink>();
            _kernel.Bind<IThingsDataFacade>().To<Neo4jThingsDataFacade>();
            _kernel.Bind<IMinionsDataService>().To<MinionsSeamlessThingiesNeo4JDataFacade>();
            _kernel.Bind<IMassTransitRabbitMQFactory>().To<MassTransitRabbitMQFactory>();
            _kernel.Bind<IRequestReplyReceiveEndpoint<MinionsRequestMessage, MinionsResponseMessage>>().To<MinionsMTRMQReceiveEndpoint>();
            _kernel.Bind<IHostableService>().To<MinionsHost>();
            _kernel.Bind<IMinionsService>().To<MinionsService>();

            new Program().run();
        }

        private void run()
        {
            try
            {
                var host = _kernel.Get<IHostableService>();

                host.Start();
                Console.ReadLine();
                host.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null) Console.WriteLine(ex.InnerException.Message);
            }
            /*
            _logger.Info("Starting minions service");
            var service = _kernel.Get<IIoTService>();
            service.Start();
            _logger.Info("Started service, prees enter to exit");
            
            Console.ReadLine();
            
            _logger.Info("Stopping service");
            service.Stop();
            _logger.Info("Stoppes");
             * */


        }
    }
}
