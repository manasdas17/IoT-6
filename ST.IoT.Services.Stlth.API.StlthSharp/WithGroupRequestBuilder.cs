using System;
using ST.IoT.Data.Stlth.Model;
using Group = ST.IoT.Data.Stlth.Social.Model.Group;
using Thing = ST.IoT.Data.Stlth.Social.Model.Thing;

namespace ST.IoT.Services.Stlth.API.StlthSharp
{
    public class WithGroupRequestBuilder
    {
        private Group _group;
        public Group Group { get { return _group; } }

        public WithGroupRequestBuilder(Group group)
        {
            _group = group; 
        }

        public AddThingToGroupCommand Add(Thing thing)
        {
            return new AddThingToGroupCommand(this, thing);
        }

        public void Subscribe(Action<Node> p)
        {
            throw new NotImplementedException();
        }

        public void Update(string thingData)
        {
            throw new NotImplementedException();
        }
    }
}