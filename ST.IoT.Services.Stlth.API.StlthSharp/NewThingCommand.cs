using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace ST.IoT.Services.Stlth.API.StlthSharp
{
    public class NewThingCommand : StlthCommand
    {
        private ThingRequestBuilder _thingRequestBuilder;
        private string _thingName;

        public NewThingCommand(ThingRequestBuilder thingRequestBuilder, string thingName)
        {
            _thingRequestBuilder = thingRequestBuilder;
            _thingName = thingName;
        }

        protected override string createCommandBody()
        {
            throw new System.NotImplementedException();
        }

        public Task<NewThingCommandResult> ResultAsync => this.executeAsync();
        public NewThingCommandResult Result => this.execute();

        public NewThingCommandResult execute()
        {
            return executeAsync().Result;
        }

        private async Task<NewThingCommandResult> executeAsync()
        {
            var body = JObject.Parse("{ 'Name': '" + _thingName + "'}");

            var response = await StlthRestRequestExecutor.executeAsync(
                method: Method.POST,
                requestUrl: "/Thing",
                body: "{'Name': '" + _thingName + "'}");

            var result = new NewThingCommandResult(response);
            return result;
        }

    }
}