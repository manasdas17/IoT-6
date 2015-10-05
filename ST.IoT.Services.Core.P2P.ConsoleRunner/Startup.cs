using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Cors;
using Owin;

namespace ST.IoT.Services.Core.P2P.ConsoleRunner
{
    internal class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Console.WriteLine("Startup.Configuration - in");
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
            Console.WriteLine("Startup.Configuration - out");
        }
    }

}
