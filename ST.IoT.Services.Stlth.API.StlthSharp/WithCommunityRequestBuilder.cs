using System;
using ST.IoT.Data.Stlth.Social.Model;

namespace ST.IoT.Services.Stlth.API.StlthSharp
{
    public class WithCommunityRequestBuilder
    {
        public StlthRestSocialApi Client { get; private set; }

        public Data.Stlth.Social.Model.Community Community { get; private set; }

        public WithCommunityRequestBuilder(Community community)
        {
            Community = community;
        }

        public WithCommunityRequestBuilder(StlthRestSocialApi stlthRestSocialApi, Community community)
        {
            this.Client = stlthRestSocialApi;
            this.Community = community;
        }

        public AddPersonToCommunityCommand Add(Person person)
        {
            return new AddPersonToCommunityCommand(this, person);
        }
        public AddGroupToCommunityCommand Add(Data.Stlth.Social.Model.Group group)
        {
            return new AddGroupToCommunityCommand(this, group);
        }
    }
}