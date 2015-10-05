using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace ST.IoT.Services.Stlth.API.StlthSharp
{
    public class NewCommunityCommand : StlthCommand
    {
        private readonly string _communityName;

        public NewCommunityCommand(string communityName)
        {
            _communityName = communityName;
        }

        protected override string createCommandBody()
        {
            throw new NotImplementedException();
        }

        public Task<NewCommunityCommandResult> ResultAsync => this.executeAsync();
        public NewCommunityCommandResult Result => this.execute();


        public NewCommunityCommandResult execute()
        {
            return executeAsync().Result;
        }

        private async Task<NewCommunityCommandResult> executeAsync()
        {
            var response = await StlthRestRequestExecutor.executeAsync(
                method: Method.POST,
                requestUrl: "/Community",
                body: "{'Name': '" + _communityName + "'}");

            var result = new NewCommunityCommandResult(response);
            return result;
        }
    }
}