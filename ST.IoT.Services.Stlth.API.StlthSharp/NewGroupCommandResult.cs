
using RestSharp;
using ST.IoT.Data.Stlth.Social.Model;

namespace ST.IoT.Services.Stlth.API.StlthSharp
{
    public class NewGroupCommandResult
    {
        private IRestResponse response;

        public NewGroupCommandResult()
        {
        }

        public NewGroupCommandResult(IRestResponse response)
        {
            this.response = response;
        }

        public Group Group { get; set; }
    }
}