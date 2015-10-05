using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RestSharp;
using ST.IoT.Data.Stlth.Social.Model;

namespace ST.IoT.Services.Stlth.API.StlthSharp
{
    public class AddPersonToCommunityCommand : StlthCommand
    {
        private Person _person;
        private WithCommunityRequestBuilder _communityRequest;

        
        public AddPersonToCommunityCommand(WithCommunityRequestBuilder community, Person person) : base()
        {
            _communityRequest = community;
            _person = person;
        }

        public Task<AddPersonToCommunityResult> ResultAsync => this.executeAsync();
        public AddPersonToCommunityResult Result => this.execute();


        protected override string createCommandBody()
        {
            throw new NotImplementedException();
        }

        public AddPersonToCommunityResult execute()
        {
            return executeAsync().Result;
        }

        private async Task<AddPersonToCommunityResult> executeAsync()
        {
            var response = await StlthRestRequestExecutor.executeAsync(
                method: Method.POST,
                requestUrl: string.Format("/Community/{0}/Member/Person/{1}", _communityRequest.Community.ID, _person.ID));

            var result = new AddPersonToCommunityResult(response);
            return result;
        }
    }
}