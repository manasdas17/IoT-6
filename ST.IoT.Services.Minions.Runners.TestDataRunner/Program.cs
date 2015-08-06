using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using ST.IoT.Data.Interfaces;
using ST.IoT.Data.Neo;
using ST.IoT.Services.Minions.Data.Interfaces;
using ST.IoT.Services.Minions.Data.STNeo;
using ST.IoT.Services.Minions.Messages;

namespace ST.IoT.Services.Minions.Runners.TestDataRunner
{
    class Program
    {
        private static IKernel _kernel;

        static void Main(string[] args)
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IThingsDataFacade>().To<Neo4jThingsDataFacade>().InSingletonScope();
            _kernel.Bind<IMinionsDataService>().To<MinionsSeamlessThingiesNeo4JDataFacade>();

            new Program().run();
        }

        private void run()
        {
            var neo = _kernel.Get<IThingsDataFacade>() as Neo4jThingsDataFacade;
            neo.reset();

            var data = _kernel.Get<IMinionsDataService>();
            data.PutMinion(new MinionsRequestMessage(File.ReadAllText("messages/createaminion.json")));
            var minion = data.GetMinion(new MinionsRequestMessage(File.ReadAllText("messages/GetLatestStatusForMinion.json")));

            Console.WriteLine(minion);
        }
    }
}
