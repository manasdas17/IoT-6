using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;

namespace ST.IoT.Subscriptions.Agents.PushUpdates.Core
{
    public class SignalRSubscriptionHandle : SubscriptionHandle
    {
        private IHubCallerConnectionContext<dynamic> _clients;

        public SignalRSubscriptionHandle(IHubCallerConnectionContext<dynamic> clients, string connectionID) : base(connectionID)
        {
            _clients = clients;
        }

        public override void pushUpdate(string thing)
        {
            try
            {
                _clients.Client(SubscriptionHandleID).thingUpdated(thing);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
