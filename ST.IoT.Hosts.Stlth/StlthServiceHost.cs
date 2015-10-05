using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using NLog;
using ST.IoT.Data.Stlth.Api;
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

    [Export(typeof(IHostableService))]

    public class StlthServiceHost : IStlthServiceHost
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IStlthService _stlthService;

        private static IKernel _kernel;

        private static IStlthServiceHost _host;
        private readonly StlthRestApiEndpoint _restEndpoint;

        public StlthServiceHost(
            IStlhRestMessageEndpoint consumer,
            IStlthService stlthService)
        {
            _stlthService = stlthService;
            _restEndpoint = consumer as StlthRestApiEndpoint;
        }
         
        public void Start()
        {
            _logger.Info("Starting");
            _restEndpoint.Start();
            _stlthService.Start();
            _logger.Info("Started");
        }

        public void Stop()
        {
            _logger.Info("Stopping");
            _stlthService.Stop();
            _restEndpoint.Stop();
            _logger.Info("Stopped");
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
            _kernel.Bind<IStlthDataClient>().To<StlthDataClient>();
            _kernel.Bind<IStlthService>().To<StlthService>().InSingletonScope();
            _kernel.Bind<IStlhRestMessageEndpoint>().To<StlthRestApiEndpoint>();

            _logger.Info("Finished wiring");
        }
    }
}
