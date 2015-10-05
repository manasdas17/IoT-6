using System;

namespace ST.IoT.Services.Stlth.API.StlthSharp
{
    public class CommunityQuerySpacesAll : StlthQuery
    {
        private readonly CommunitySpaceQueryBuilder _communitySpaceQueryBuilder;

        public CommunityQuerySpacesAll(CommunitySpaceQueryBuilder communitySpaceQueryBuilder)
        {
            _communitySpaceQueryBuilder = communitySpaceQueryBuilder;
        }

        public override string createQueryBody()
        {
            throw new NotImplementedException();
        }
    }
}