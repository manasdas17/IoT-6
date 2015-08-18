using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using NLog;
using ST.IoT.Data.Interfaces;
using ST.IoT.Data.Neo;
using ST.IoT.Hosts.Interfaces;
using ST.IoT.Services.Minions;
using ST.IoT.Services.Minions.Data.Interfaces;
using ST.IoT.Services.Minions.Data.STNeo;
using ST.IoT.Services.Minions.Endpoints;
using ST.IoT.Services.Minions.Interfaces;

namespace ST.IoT.Hosts.Minions
{
    public interface IMinionsServiceHost : IHostableService
    {
    }

    [Export(typeof(IHostableService))]
    public class MinionsServiceHost : IMinionsServiceHost
    {
        private readonly IMinionsService _service;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private static IKernel _kernel;
        private static IMinionsServiceHost _host;

        [ImportingConstructor]
        public MinionsServiceHost([Import] IMinionsService minionsService)
        {
            _service = minionsService;
        }
        
        public void Start()
        {
            _logger.Info("Starting");
            _service.Start();
            _logger.Info("Started");
        }

        public void Stop()
        {
            _logger.Info("Stopping");
            _service.Stop();
            _logger.Info("Stopped");
        }

        public static void wire(StandardKernel kernel)
        {
            _kernel = kernel;
            //_kernel.Bind<IThingUpdated>().To<NullThingUpdatedSink>();
            _kernel.Bind<IThingsDataFacade>().To<Neo4jThingsDataFacade>();
            _kernel.Bind<IMinionsDataService>().To<MinionsSeamlessThingiesNeo4JDataFacade>();
            _kernel.Bind<IConsumeMinionsRequestEndpoint>().To<ConsumeMinionsRequestEndpoint>();
            _kernel.Bind<IMinionsServiceHost>().To<MinionsServiceHost>();
            _kernel.Bind<IMinionsService>().To<MinionsService>();
        }

        public static void start(IKernel kernel = null)
        {
            var k = kernel ?? _kernel;
            if (k == null) throw new Exception("Not wired!");

            _host = _kernel.Get<IMinionsServiceHost>();
            _host.Start();
        }

        public static void stop()
        {
            _host.Stop();
        }

    }
}
