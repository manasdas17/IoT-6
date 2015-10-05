using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ST.IoT.Data.Stlth.Social.Model;

namespace ST.IoT.Services.Stlth.API.StlthSharp
{
    public class CommunityRequestBuilder : StlthRequestBuilder
    {
        //public WallRequestBuilder Wall { get; set; }


        public CommunityRequestBuilder(StlthRestSocialApi client)
        {
            
        }
        
        public NewCommunityCommand New(string communityName)
        {
            return new NewCommunityCommand(communityName);
        }

        /*
        public AddPersonToCommunityCommand Add(Person person)
        {
            return new AddPersonToCommunityCommand(this, person);
        }

        public AddGroupToCommunityCommand Add(Group group)
        {
            throw new NotImplementedException();
        }
        */

        public WithCommunityRequestBuilder With(Community community)
        {
            return new WithCommunityRequestBuilder(community);
        }
    }
}
