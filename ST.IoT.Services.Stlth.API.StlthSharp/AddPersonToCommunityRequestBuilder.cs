

using ST.IoT.Data.Stlth.Social.Model;

namespace ST.IoT.Services.Stlth.API.StlthSharp
{
    public class AddPersonToCommunityRequestBuilder
    {
        private Person person;
        private WithCommunityRequestBuilder withCommunityRequestBuilder;

        public AddPersonToCommunityRequestBuilder(WithCommunityRequestBuilder withCommunityRequestBuilder, Person person)
        {
            this.withCommunityRequestBuilder = withCommunityRequestBuilder;
            this.person = person;
        }

        public AddPersonToCommunityResult Result { get; set; }
    }
}