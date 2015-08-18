using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ST.IoT.Messaging.Utils.HttpMessages;

namespace ST.IoT.Messaging.Messages.REST.Routing
{
    public class RestRouterReplyMessage
    {
        public string InternalMessageID { get; set; }
        public string CorrelatedRequestInternalMessageID { get; set; }
        public string TargetRouterID { get; set; }
        public DateTime SentAt { get; set; }

        public byte[] HttpResponseAsBytes { get; set; }

        public HttpResponseMessage HttpResponse { get { return HttpResponseAsBytes.DeserializeAsHttpResponseMessage(); } }

        public RestRouterReplyMessage()
        {
            
        }

        public RestRouterReplyMessage(HttpResponseMessage response)
        {
            InternalMessageID = Guid.NewGuid().ToString();
            SentAt = DateTime.UtcNow;

            HttpResponseAsBytes = response.SerializeAsByteArray();
        }

    }
}
