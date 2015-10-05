namespace ST.IoT.Services.Stlth.API.StlthSharp
{
    public class PersonRequestBuilder : StlthRequestBuilder
    {
        private StlthRestSocialApi _stlthRestSocialApi;

        public PersonRequestBuilder(StlthRestSocialApi stlthRestSocialApi)
        {
            _stlthRestSocialApi = stlthRestSocialApi;
        }

        public NewPersonCommand New()
        {
            return new NewPersonCommand(this);
        }

        public NewPersonCommand New(string emailAddress, string password)
        {
            return new NewPersonCommand(this, emailAddress, password);
        }

        public NewPersonCommand Register(string emailAddress, string password)
        {
            return new NewPersonCommand(this, emailAddress, password);
        }

    }
}