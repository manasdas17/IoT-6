using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using ST.IoT.Subscriptions.Agents.PushUpdates.Core;

namespace IoT.Services.SignalR.ThingUpdateConsoleRunner
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // This will *ONLY* bind to localhost, if you want to bind to all addresses
            // use http://*:8080 to bind to all addresses. 
            // See http://msdn.microsoft.com/en-us/library/system.net.httplistener.aspx 
            // for more information.
            string url = "http://localhost:8081";
            using (WebApp.Start(url))
            {
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
        private static CorePushUpdateManager<SignalRSubscriptionHandle> _subscriptionManager;

        static ListenHub()
        {
            _subscriptionManager = new CorePushUpdateManager<SignalRSubscriptionHandle>();
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
            _subscriptionManager.subscribe(
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
            _subscriptionManager.unsubscribeByHandleID(Context.ConnectionId);

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
             