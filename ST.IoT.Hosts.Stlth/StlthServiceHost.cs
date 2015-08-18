using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using NLog;
using ST.IoT.Hosts.Interfaces;
using ST.IoT.Services.Stlth.Core;
using ST.IoT.Services.Stlth.Endpoints;

namespace ST.IoT.Hosts.Stlth
{
    public interface IStlthServiceHost : IHostableService
    {
        void Start();
        void Stop();
    }

    public class StlthServiceHost : IStlthServiceHost
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private IStlthService _stlthService;

        private static IKernel _kernel;

        private static IStlthServiceHost _host;

        public StlthServiceHost(IStlthService stlthService)
        {
            _stlthService = stlthService;
        }

        public void Start()
        {
            _stlthService.Start();
        }

        public void Stop()
        {
            _stlthService.Stop();
        }

        public static void start(IKernel kernel = null)
        {
            var k = kernel ?? _kernel;
            if (k == null) throw new Exception("Not wired!");

            _host = _kernel.Get<IStlthServiceHost>();
            _host.Start();

        }

        public static void stop()
        {
            _host.Stop();
        }

        public static void wire(IKernel kernel)
        {
            _logger.Info("Starting wiring");

            _kernel = kernel;
            _kernel.Bind<IStlthServiceHost>().To<StlthServiceHost>();
            _kernel.Bind<IStlthService>().To<StlthService>();
            _kernel.Bind<IConsumeRestMessageEndpoint>().To<ConsumeRestMessageEndpoint>();

            _logger.Info("Finished wiring");
        }
    }
}
