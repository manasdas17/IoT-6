using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Services.Minions.ConsoleRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().run();
        }

        private void run()
        {
            var service = new MinionsService();
            service.Start();
            Console.ReadLine();
            service.Stop();
        }
    }
}
