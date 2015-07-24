using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Dweet.IO.Sharp.Runner
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            new Program().run();
        }

        private void run()
        {
            var thing = new Thing("seamless-thingies-thing-1");
            thing.Dweet("{\"this\": \"is\"3}");
        }
    }
}
