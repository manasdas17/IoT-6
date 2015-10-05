namespace ST.IoT.Services.Stlth.API.StlthSharp
{
    public class ThingRequestBuilder
    {
        private StlthRestSocialApi _stlthRestSocialApi;

        public ThingRequestBuilder(StlthRestSocialApi stlthRestSocialApi)
        {
            _stlthRestSocialApi = stlthRestSocialApi;
        }

        public NewThingCommand New(string thingName)
        {
            return new NewThingCommand(this, thingName);
        }


    }
}