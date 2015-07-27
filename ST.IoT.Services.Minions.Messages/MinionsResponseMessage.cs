using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Services.Minions.Messages
{
    public class MinionsResponseMessage
    {
        public string Response { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public MinionsResponseMessage()
        {
            
        }

        public MinionsResponseMessage(HttpStatusCode statusCode, string response = "")
        {
            StatusCode = statusCode;
            Response = response;
        }
    }
}
