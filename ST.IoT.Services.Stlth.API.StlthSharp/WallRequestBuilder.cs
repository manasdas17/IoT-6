using ST.IoT.Data.Stlth.Social.Model;

namespace ST.IoT.Services.Stlth.API.StlthSharp
{
    public class WallRequestBuilder
    {
        public WallRequestForCommunityCommand For(Community community)
        {
            return new WallRequestForCommunityCommand();
        }
    }
}

