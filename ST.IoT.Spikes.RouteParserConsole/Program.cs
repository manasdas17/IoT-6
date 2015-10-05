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
            string sTable = "static class BinaryTable\r\n{";
            string stemp = "";
            for (int i = 0; i < 256; i++)
            {
                stemp = System.Convert.ToString(i, 2);
                while (stemp.Length < 8) stemp = "0" + stemp;
                sTable += "\tconst byte nb" + stemp + "=" + i.ToString() + ";\r\n";
            }
            sTable += "}";


            //var parser = new RouteParser("{protocol}://{domain}/{action}/{remainder}");
            //var values = parser.ParseRouteInstance("http://minions.bseamless.com/quote/for/minion-1");

            var url = new Uri("http://minions.bseamless.com/quote/for/minion-1");
        }
    }
}
