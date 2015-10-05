using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace ST.IoT.Data.Stlth.Api
{
    public class StlthRestRequestFactory
    {
        public static HttpRequestMessage getNodeById(string id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, string.Format("http://localhost/{0}", id));
            return request;
        }

        public static HttpRequestMessage getNodesByType(string nodeType)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, string.Format("http://localhost/{0}", nodeType));
            return request;
        }

        public static HttpRequestMessage getNodeByTypeAndId(string nodeType, string nodeId)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, string.Format("http://localhost/{0}/{1}", nodeType, nodeId));
            return request;
        }
    }
}
