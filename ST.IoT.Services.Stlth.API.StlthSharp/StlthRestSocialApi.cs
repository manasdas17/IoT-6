using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using ST.IoT.Data.Stlth.Social.Model;

namespace ST.IoT.Services.Stlth.API.StlthSharp
{
    public class StlthRestSocialApi
    {
        public string Token { get; private set; }
        public string BaseUrl { get; set; }

        private RestClient _client = null;

        public StlthRestSocialApi(string appToken, string userToken)
        {
            // use cached credentials
        }

        public StlthRestSocialApi(string token)
        {
            Token = "48a3b0c8-aed7-4ced-99ef-ddbd46e12a14";
            BaseUrl = "http://stlth.local:8080";

            _client = new RestClient(BaseUrl);
        }
        
        public void execute(StlthCommand command)
        {
            var request = new RestRequest("/Community", Method.POST);
            request.AddHeader("Authorization", "Token token='" + Token + "'");
            request.AddHeader("Content-Type", "application/json");
            //request.AddBody("{'Name': '{0}'}", command.createCommandBody());

            _client.Execute(request);
        }

        /*
public void execute(StlthQuery command)
{
   var request = new RestRequest("/Community", Method.POST);
   request.AddHeader("Authorization", "Token token='" + Token + "'");
   request.AddHeader("Content-Type", "application/json");
   request.AddBody("{'Name': '{0}'}", command.createQueryBody());

   _client.Execute(request);
}
*/
        public CommunityRequestBuilder Community => new CommunityRequestBuilder(this);
        public PersonRequestBuilder Person => new PersonRequestBuilder(this);
        public ThingRequestBuilder Thing => new ThingRequestBuilder(this);

        public WithThingRequestBuilder With(Thing thing)
        {
            return new WithThingRequestBuilder(thing);
        }
        public WithGroupRequestBuilder With(Group group)
        {
            return new WithGroupRequestBuilder(group);
        }

        public WithCommunityRequestBuilder With(Community community)
        {
            return new WithCommunityRequestBuilder(this, community);
        }

        public GroupRequestBuilder Group => new GroupRequestBuilder(this);

        public WallRequestBuilder Wall => new WallRequestBuilder();
    }
}
