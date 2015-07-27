using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Data.Neo.Console.Runner
{
    class Program
    {
        private static void Main(string[] args)
        {
            new Program().run();
        }

        private void run()
        {
            //doCreateUpdate();
            doGetMostRecentState();
            //doGetAllStates();
        }

        private void doCreateUpdate()
        {
            var df = new Neo4jThingsDataFacade();
            df.reset();

            df.Put(File.ReadAllText("Message Samples/CreateAMinion.json"));
            df.Put(File.ReadAllText("Message Samples/UpdateAMinion.json"));
        }

        private void doGetMostRecentState()
        {
            var df = new Neo4jThingsDataFacade();
            //df.reset();
            
            //doCreateUpdate();

            var results = df.Get(File.ReadAllText("Message Samples/GetLatestStatusForMinion.json"));
            
        }

        private void doGetAllStates()
        {
            var df = new Neo4jThingsDataFacade();
            //df.reset();
            
            //doCreateUpdate();

            var results = df.Get(File.ReadAllText("Message Samples/GetAllStatusesForMinion.json"));

        }
    }
}
