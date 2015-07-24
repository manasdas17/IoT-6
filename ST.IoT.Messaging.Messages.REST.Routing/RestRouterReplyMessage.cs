using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Messaging.Messages.REST.Routing
{
    public class RestRouterReplyMessage
    {
        public string InternalMessageID { get; set; }
        public string CorrelatedRequestInternalMessageID { get; set; }
        public string SendingRouterID { get; set; }
        public DateTime SentAt { get; set; }

        public byte[] HttpResponse { get; set; }

    }
}
