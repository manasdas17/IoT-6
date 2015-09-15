using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using NLog;
using ST.IoT.Hosts.Interfaces;
using ST.IoT.Hosts.Stlth;
using ST.IoT.Services.Stlth.Core;

namespace ST.IoT.Services.Stlth.Runners.ConsoleRunner
{
    class Program
    {
        private static IKernel _kernel;

        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)

        {
            _kernel.Bind<IHostableService>().To<StlthServiceHost>();
            _kernel.Bind<IStlthService>().To<StlthService>();

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

        }
    }
}
