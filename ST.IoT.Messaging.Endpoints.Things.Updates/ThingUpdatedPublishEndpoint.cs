using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ST.IoT.Messaging.Bus.Core;
using ST.IoT.Messaging.Messages.Push;

namespace ST.IoT.Messaging.Endpoints.Things.Updates
{
    public interface IThingUpdatedPublishEndpoint
    {
        
    }

    public class ThingUpdatedPublishEndpoint : PublishEndpoint<ThingUpdatedMessage>,  IThingUpdatedPublishEndpoint
    {
        public ThingUpdatedPublishEndpoint() : base("thing_updated")
        {
            Start();
        }
    }
}
