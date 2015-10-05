using System;
using System.Net;
using System.Net.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using ST.IoT.Data.Stlth.Model;
using Community = ST.IoT.Data.Stlth.Social.Model.Community;

namespace ST.IoT.Services.Stlth.API.StlthSharp
{

    public class NewCommunityCommandResult : CommandResult
    {
        public Community Community { get; private set; }

        public NewCommunityCommandResult()
        {
        }
        public override bool Success
        {
            get { return _statusCode == HttpStatusCode.Created; }
        }

        internal NewCommunityCommandResult(IRestResponse response)
        {
            _statusCode = response.StatusCode;
            _resultJson = response.Content;

            if (Success)
            {
                var jo = JObject.Parse(_resultJson);
                Community = new Community()
                {
                    ID = jo["ID"].Value<string>()
                };
            }
        }
    }
}