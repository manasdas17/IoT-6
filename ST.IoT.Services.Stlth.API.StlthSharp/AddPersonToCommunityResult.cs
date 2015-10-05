using RestSharp;

namespace ST.IoT.Services.Stlth.API.StlthSharp
{
    public class AddPersonToCommunityResult
    {
        private IRestResponse response;

        public AddPersonToCommunityResult(IRestResponse response)
        {
            this.response = response;
        }

        public bool Success { get; set; }
    }
}