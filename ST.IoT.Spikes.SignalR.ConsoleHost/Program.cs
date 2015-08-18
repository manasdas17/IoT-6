using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using ST.IoT.Messaging.Bus.Core;
using ST.IoT.Subscriptions.Agents.PushUpdates.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Spikes.SignalR.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            EndpointParameters.Default = new EndpointParameters(new Uri("rabbitmq://localhost"), "iot", "iot", "stlth");

            var host = new SignalRThingsUpdateHost();
            Console.ReadLine();
        }
    }
    public class SignalRThingsUpdateHost
    {
        public static CorePushUpdateManager<SignalRSubscriptionHandle> _subscriptionManager;
        public SignalRThingsUpdateHost()
        {
            string url = "http://localhost:8081";
            using (WebApp.Start(url))
            {
                _subscriptionManager = new CorePushUpdateManager<SignalRSubscriptionHandle>();

                Console.WriteLine("Server running on {0}", url);
                Console.ReadLine();
            }

        }
    }

    internal class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }

    public class MyHub : Hub
    {
        public void Send(string name, string message)
        {
            Console.WriteLine("name: " + message);
            Clients.All.addMessage(name, message);
        }
    }



    public class ListenHub : Hub
    {
        static ListenHub()
        {
        }

        public class Subscription
        {
            public StatefulSignalProxy Client { get; set; }
            public string ThingID { get; set; }
        }

        public void ThingUpdated(string thing)
        {
            Clients.All.thingUpdated(thing);
        }

        public void ListenForQuotesFrom(string thingID)
        {
            SignalRThingsUpdateHost._subscriptionManager.subscribe(
                thingID,
                new SignalRSubscriptionHandle(Clients, Context.ConnectionId));

            Console.WriteLine("Request to listen for quotes from: " + thingID);
        }

        public override Task OnConnected()
        {
            Console.WriteLine("On Connected");
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            SignalRThingsUpdateHost._subscriptionManager.unsubscribeByHandleID(Context.ConnectionId);

            Console.WriteLine("On Disconnected");
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            Console.WriteLine("On Reconnected");
            return base.OnReconnected();
        }
    }


}
