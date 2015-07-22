using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Common
{
    public static class RabbitMQConstants
    {
        public static string BaseAddress = "rabbitmq://API.IOT.BSEAMLESS.COM";
        public static string VirtualHost = "IoT";

        public static Uri FullBaseUrl
        {
            get { return new Uri(BaseAddress + "/" + VirtualHost); }
        }

        public static string Username = "iot";
        public static string Password = "iot";
    }
}
