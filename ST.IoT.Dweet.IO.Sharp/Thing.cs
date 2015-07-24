using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace ST.IoT.Dweet.IO.Sharp
{
    public interface IThing
    {
        JObject Dweet(string content);
    }

    public class Thing
    {
        private const string _baseAddress = "https://dweet.io";
        public string Name { get; private set; }
        public string Key { get; private set; }

        public Thing(string name, string key = "")
        {
            Name = name;
            Key = key;
        }

        //{"this":"succeeded","by":"dweeting","the":"dweet","with":{"thing":"seamless-thingies-thing-1","created":"2015-07-23T04:04:59.638Z","content":{"this":"is"}}}

        public JObject Dweet(string content)
        {
            var client = new RestClient(_baseAddress);
            var request = new RestRequest("dweet/for/" + Name, Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", content, ParameterType.RequestBody);
            var result = client.Execute(request);
            return JObject.Parse(result.Content);
        }
    }
}
