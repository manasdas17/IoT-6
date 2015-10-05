using System;
using ST.IoT.Data.Stlth.Social.Model;

namespace ST.IoT.Services.Stlth.API.StlthSharp
{
    public class WithThingRequestBuilder 
    {
        private Thing thing;

        public WithThingRequestBuilder(Thing thing)
        {
            this.thing = thing;
        }

        public void Subscribe(Action<Thing> handler)
        {
        }

        public WithThingUpdateRequestBuilder Update(string thingData)
        {
            throw new NotImplementedException();
        }
    }
}