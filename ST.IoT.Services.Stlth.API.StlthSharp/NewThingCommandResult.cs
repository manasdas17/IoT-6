using RestSharp;
using ST.IoT.Data.Stlth.Social.Model;

namespace ST.IoT.Services.Stlth.API.StlthSharp
{
    public class NewThingCommandResult
    {
        private IRestResponse response;

        public NewThingCommandResult(IRestResponse response)
        {
            this.response = response;
        }

        public Thing Thing { get; set; }
    }
}