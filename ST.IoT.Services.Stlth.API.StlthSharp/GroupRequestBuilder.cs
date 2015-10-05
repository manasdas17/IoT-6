namespace ST.IoT.Services.Stlth.API.StlthSharp
{
    public class GroupRequestBuilder
    {
        private StlthRestSocialApi _stlthRestSocialApi;

        public GroupRequestBuilder(StlthRestSocialApi stlthRestSocialApi)
        {
            _stlthRestSocialApi = stlthRestSocialApi;
        }

        public NewGroupCommand New(string groupName)
        {
            return new NewGroupCommand(this, groupName);
        }
    }
}