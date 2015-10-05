using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace ST.IoT.Services.Stlth.API.StlthSharp
{
    public class StlthRestRequestExecutor
    {
        public static async Task<IRestResponse> executeAsync(RestSharp.Method method, string requestUrl, string body = null)
        {
            var client = new RestClient("http://social.api.stlth.io:8080");
            var request = new RestRequest(requestUrl, method);
            if (body != null)
            {
                request.AddParameter("Application/Json", body, ParameterType.RequestBody);
            }

            request.AddHeader("Authorization", "Token token='48a3b0c8-aed7-4ced-99ef-ddbd46e12a14'");

            var response = await client.ExecuteTaskAsync(request);

            return response;
        }
    }
}
