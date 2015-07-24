using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;
using ST.IoT.Common;

namespace ST.IoT.Spikes.RouteParserConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //var parser = new RouteParser("{protocol}://{domain}/{action}/{remainder}");
            //var values = parser.ParseRouteInstance("http://minions.bseamless.com/quote/for/minion-1");

            var url = new Uri("http://minions.bseamless.com/quote/for/minion-1");
        }
    }
}
