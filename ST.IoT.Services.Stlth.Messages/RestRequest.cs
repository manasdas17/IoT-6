using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ST.IoT.Messaging.Utils.HttpMessages;

namespace ST.IoT.Services.Stlth.Messages
{
    public class RestRequest
    {
        public byte[] AsByteArray { get; set; }

        public HttpRequestMessage Request
        {
            get
            {
                return AsByteArray.DeserializeAsHttpRequestMessage();
            }
        }

        public RestRequest()
        {
        }

        public RestRequest(HttpRequestMessage request)
        {
            AsByteArray = request.SerializeAsByteArray();
        }
    }
}
