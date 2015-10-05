using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace ST.IoT.Services.Stlth.API.StlthSharp
{
    public class NewGroupCommand
    {
        private string _groupName;
        private GroupRequestBuilder _groupRequestBuilder;

        public NewGroupCommand(GroupRequestBuilder groupRequestBuilder, string groupName)
        {
            _groupRequestBuilder = groupRequestBuilder;
            _groupName = groupName;
        }

        public Task<NewGroupCommandResult> ResultAsync => this.executeAsync();
        public NewGroupCommandResult Result => this.execute();

        public NewGroupCommandResult execute()
        {
            return executeAsync().Result;
        }

        private async Task<NewGroupCommandResult> executeAsync()
        {
            var body = JObject.Parse("{ 'Name': '" + _groupName + "'}");

            var response = await StlthRestRequestExecutor.executeAsync(
                method: Method.POST,
                requestUrl: "/Group",
                body: "{'Name': '" + _groupName + "'}");

            var result = new NewGroupCommandResult(response);
            return result;
        }
    }
}