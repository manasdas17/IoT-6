using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Messaging.Messages.REST.Routing
{
    public class RestProxyToRouterMessage
    {
        public string InternalMessageID { get; set; }
        public string SendingProxyID { get; set; }
        public DateTime SentAt { get; set; }
        public byte[] HttpRequest { get; set; }

        public RestProxyToRouterMessage()
        {
            InternalMessageID = Guid.NewGuid().ToString();
            SentAt = DateTime.UtcNow;
        }
    }
}
