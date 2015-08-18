using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NLog;
using ST.IoT.Messaging.Utils.HttpMessages;

namespace ST.IoT.Messaging.Messages.REST.Routing
{
    public class RestRequestToRouterMessage
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public string InternalMessageID { get; set; }
        public string SendingProxyID { get; set; }
        public DateTime SentAt { get; set; }
        public byte[] HttpRequestAsBytes { get; set; }

        public HttpRequestMessage HttpRequest
        {
            get
            {
                return HttpRequestAsBytes.DeserializeAsHttpRequestMessage();
            }
        }

        public RestRequestToRouterMessage()
        {
            
        }

        public RestRequestToRouterMessage(HttpRequestMessage request)
        {
            InternalMessageID = Guid.NewGuid().ToString();
            SentAt = DateTime.UtcNow;
            HttpRequestAsBytes = request.SerializeAsByteArray();
        }
    }
}
