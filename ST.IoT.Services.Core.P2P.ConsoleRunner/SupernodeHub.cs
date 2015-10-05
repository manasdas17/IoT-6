using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace ST.IoT.Services.Core.P2P.ConsoleRunner
{
    public class SupernodeHub : Hub, ISupernodeAPI
    {
        public void Announce(string msg)
        {
            Console.WriteLine("Got announce");

            SupernodeService.Announce();
        }

        public override Task OnConnected()
        {
            Console.WriteLine("On Connected");
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {

            Console.WriteLine("On Disconnected");
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            Console.WriteLine("On Reconnected");
            return base.OnReconnected();
        }

        private ISupernodeService SupernodeService => ObjectKernel.Instance.SupernodeService;
    }
}
