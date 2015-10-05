using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ST.IoT.Services.Stlth.API.StlthSharp;

namespace St.IoT.Spikes.StlthSharpRunner
{
    class Program
    {
        public static string _token = "48a3b0c8-aed7-4ced-99ef-ddbd46e12a14";
        static void Main(string[] args)
        {
            var stlth = new StlthRestSocialApi(token: _token);

        }
    }
}
