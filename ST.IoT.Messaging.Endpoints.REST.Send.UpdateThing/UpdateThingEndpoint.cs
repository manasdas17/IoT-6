using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RestSharp;
using ST.IoT.Messaging.Endpoints.Interfaces;

namespace ST.IoT.Messaging.Endpoints.REST.Send.UpdateThing
{
    public class UpdateThingEndpoint : IUpdateThing
    {
        private RestClient _client;
        private string _thingID;

        public UpdateThingEndpoint()
        {
            
        }

        public UpdateThingEndpoint(Uri baseURL, string thingID)
        {
            _client = new RestClient(baseURL);
            _thingID = thingID;
        }
        public string UpdateThing(string thing_json)
        {
            ensureClient();

            var request = new RestRequest("quote/for/" + _thingID, Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", thing_json, ParameterType.RequestBody);
            var result = _client.Execute(request);
            return JObject.Parse(result.Content).ToString();

        }

        private void ensureClient()
        {
            _client = new RestClient();   
        }
    }
}
