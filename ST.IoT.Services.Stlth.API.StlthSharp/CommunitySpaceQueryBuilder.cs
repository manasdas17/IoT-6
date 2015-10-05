namespace ST.IoT.Services.Stlth.API.StlthSharp
{
    public class CommunitySpaceQueryBuilder
    {
        private readonly CommunityQuery _communityQuery;

        public CommunitySpaceQueryBuilder(CommunityQuery communityQuery)
        {
            _communityQuery = communityQuery;
        }

        public CommunityQuerySpacesAll All => new CommunityQuerySpacesAll(this);
    }
}