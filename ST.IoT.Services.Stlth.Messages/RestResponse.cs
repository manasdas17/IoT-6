using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ST.IoT.Messaging.Utils.HttpMessages;

namespace ST.IoT.Services.Stlth.Messages
{
    public class RestResponse
    {
        public byte[] AsByteArray { get; set; }
        public HttpResponseMessage Response
        {
            get
            {
                return AsByteArray.DeserializeAsHttpResponseMessage();
            }
        }

        public RestResponse()
        {
            
        }

        public RestResponse(HttpResponseMessage response)
        {
            AsByteArray = response.SerializeAsByteArray();
        }
    }
}
