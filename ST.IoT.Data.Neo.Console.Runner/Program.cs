using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ST.IoT.Data.Model;

namespace ST.IoT.Data.Neo.Console.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var df = new Neo4jThingsDataFacade();

            var thing = new Minion();
            thing.ID = "thing-1";
            thing.Content = "{\"hi\": \"there!\"}";
            df.Put(thing);
        }
    }
}
