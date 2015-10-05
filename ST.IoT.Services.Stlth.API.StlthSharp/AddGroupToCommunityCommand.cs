using System;
using System.Threading.Tasks;
using RestSharp;
using ST.IoT.Data.Stlth.Social.Model;

namespace ST.IoT.Services.Stlth.API.StlthSharp
{
    public class AddGroupToCommunityCommand : StlthCommand
    {
        private Group _group;
        private WithCommunityRequestBuilder _communityRequest;

        public AddGroupToCommunityCommand(WithCommunityRequestBuilder community, Group group) : base()
        {
            _communityRequest = community;
            _group = group;
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
                requestUrl: string.Format("/Community/{0}/Member/Group/{1}", _communityRequest.Community.ID, _group.ID));

            var result = new AddPersonToCommunityResult(response);
            return result;
        }

    }
}