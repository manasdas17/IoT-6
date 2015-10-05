using System.Net;
using Newtonsoft.Json.Linq;
using RestSharp;

using ST.IoT.Data.Stlth.Social.Model;

namespace ST.IoT.Services.Stlth.API.StlthSharp
{
    public class NewPersonCommandResult : CommandResult
    {
        private IRestResponse _response;
        public Person Person { get; private set; }
        public override bool Success
        {
            get { return _statusCode == HttpStatusCode.Created; }
        }


        public NewPersonCommandResult(IRestResponse response)
        {
            _response = response;
            _statusCode = response.StatusCode;
            _resultJson = response.Content;

            if (Success)
            {
                var jo = JObject.Parse(_resultJson);
                Person = new Person()
                {
                    ID = jo["ID"].Value<string>()
                };
            }
        }
    }
}