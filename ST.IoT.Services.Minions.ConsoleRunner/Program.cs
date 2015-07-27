using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using ST.IoT.Data.Interfaces;
using ST.IoT.Data.Neo;
using ST.IoT.Services.Interfaces;
using ST.IoT.Services.Minions.Data.Interfaces;
using ST.IoT.Services.Minions.Data.STNeo;
using ST.IoT.Services.Minions.Interfaces;
using ST.IoT.Services.Minions.Messaging.Endpoints.Receive.MTRMQ;

namespace ST.IoT.Services.Minions.ConsoleRunner
{
    class Program
    {
        private static IKernel _kernel;

        static void Main(string[] args)
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IThingsDataFacade>().To<Neo4jThingsDataFacade>();
            _kernel.Bind<IMinionsDataService>().To<MinionsSeamlessThingiesNeo4JDataFacade>();
            _kernel.Bind<IIoTService>().To<MinionsService>().InSingletonScope();
            _kernel.Bind<IMinionsReceiveRequestEndpoint>().To<MinionsRabbitMQMassTransitReceiveEndpoint>();

            new Program().run();
        }

        private void run()
        {
            var service = _kernel.Get<IIoTService>();
            service.Start();
            Console.ReadLine();
            service.Stop();
        }
    }
}
