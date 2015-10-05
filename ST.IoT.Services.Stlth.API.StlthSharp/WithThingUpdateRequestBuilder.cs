
using ST.IoT.Data.Stlth.Social.Model;

namespace ST.IoT.Services.Stlth.API.StlthSharp
{
    public class WithThingUpdateRequestBuilder
    {
        public WithThingUpdateResult Result { get; private set; }

        public WithThingUpdateWithinGroupRequestBuilder Within(Community community)
        {
            return new WithThingUpdateWithinGroupRequestBuilder();
        }
    }
}