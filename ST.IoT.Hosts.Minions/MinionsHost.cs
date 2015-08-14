using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using ST.IoT.Hosts.Interfaces;
using ST.IoT.Services.Minions.Interfaces;

namespace ST.IoT.Hosts.Minions
{
    public class MinionsHost : IHostableService
    {
        private IMinionsService _service;
        private Logger _logger = LogManager.GetCurrentClassLogger();

        [ImportingConstructor]
        public MinionsHost(IMinionsService minionsService)
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
    }
}
