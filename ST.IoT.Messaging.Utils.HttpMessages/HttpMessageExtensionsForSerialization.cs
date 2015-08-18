using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Messaging.Utils.HttpMessages
{
    public static class HttpMessageExtensionsForSerialization
    {
        public static byte[] SerializeAsByteArray(this HttpRequestMessage request)
        {
            var content = new HttpMessageContent(request);
            return content.ReadAsByteArrayAsync().Result;

        }

        public static byte[] SerializeAsByteArray(this HttpResponseMessage response)
        {
            var content = new HttpMessageContent(response);
            return content.ReadAsByteArrayAsync().Result;
        }

        public static HttpRequestMessage DeserializeAsHttpRequestMessage(this byte[] data)
        {
            var request = new HttpRequestMessage();
            request.Content = new ByteArrayContent(data);
            request.Content.Headers.Add("Content-Type", "application/http;msgtype=request");
            return request.Content.ReadAsHttpRequestMessageAsync().Result;

        }

        public static HttpResponseMessage DeserializeAsHttpResponseMessage(this byte[] data)
        {
            var response= new HttpResponseMessage();
            response.Content = new ByteArrayContent(data);
            response.Content.Headers.Add("Content-Type", "application/http;msgtype=response");
            return response.Content.ReadAsHttpResponseMessageAsync().Result;

        }
    }
}
