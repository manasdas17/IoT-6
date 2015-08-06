using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;

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
        public class Subscription
        {
            
        }

        public void ThingUpdated(string thing)
        {
            Clients.All.thingUpdated(thing);
        }

        public void ListenForQuotesFrom(string thingID)
        {
            Console.WriteLine("Request to listen for quotes from: " + thingID);

            Task.Delay(5000).ContinueWith(
                a =>
                {
                    ThingUpdated("leave my thing alone!");
                });
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
    }
}
