using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Ninject;
using ST.IoT.Services.Core.P2P.Client.Portable;
using ST.IoT.Spike.MQTT.JustSendMeMessagesThroughServer;

namespace ST.IoT.Services.Core.P2P.ConsoleRunner
{
    class Program
    {

        static void Main(string[] args)
        {
            string url = "http://localhost:8081";
            using (WebApp.Start(url))
            {
                Console.WriteLine("Server running on {0}", url);

                new Program().run();
            }
        }

        public Program()
        {
            var kernel = new StandardKernel();
            kernel.Bind<ISupernodeService>().To<SupernodeService>().InSingletonScope();

            new ObjectKernel(kernel);
        }

        public void run()
        {
            var superNode = ObjectKernel.Instance.SupernodeService;
            superNode.Initialize();

            Task.Delay(1000).Wait();

            var coordinator = new PeerCoordinator();
            coordinator.Start();

            var listener = new JustSendToMeMqttMessageListener();
            listener.Start();

            Console.ReadLine();

            coordinator.Stop();
        }
    }
}
