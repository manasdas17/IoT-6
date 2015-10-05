using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ST.IoT.Data.Stlth.Api;
using ST.IoT.Data.Stlth.Model;

namespace ST.IoT.Services.Stlth.Core
{
    internal abstract class StlthRequestProcessor
    {
        public abstract bool willHandle(HttpRequestMessage request);
        public abstract Task<HttpResponseMessage> handleAsync(HttpRequestMessage request);


        protected StringContent getContentForResult(NodeResult result, bool collection = false)
        {
            var s = "";

            switch (result.StatusCode)
            {
                case HttpStatusCode.Created:
                {

                }
                    break;

                case HttpStatusCode.OK:
                    {
                        if (!collection)
                        {
                            s = DescribeAsJSON.describe(result.Node);
                        }
                        else
                        {
                            //var text = "[" + String.Join(",", result.ResultSet.Select(n => n.ToJson())) + "]";
                            s = DescribeAsJSON.describe(result.ResultSet);
                        }
                    }
                    break;

                default:
                    {
                        s = result.Message;
                    }
                    break;

            }

            return new StringContent(s);
        }

        public string getTenantId(HttpRequestMessage request)
        {
            return request.Headers.Contains("TenantID") != null ? request.Headers.GetValues("TenantID").First() : "";
        }

    }
}