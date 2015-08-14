using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Subscriptions.Agents.PushUpdates.Core
{
    public class SubscriptionToThing : IDisposable 
    {
        public string ForThingID { get; set; }
        public string SubscriptionID { get; set; }
        public DateTime LastUpdatedAt { get; set; }

        public SubscriptionHandle Handle { get; set; }

        public SubscriptionToThing(string thingID, SubscriptionHandle handle)
        {
            SubscriptionID = Guid.NewGuid().ToString();
            ForThingID = thingID;
            Handle = handle;
        }

        public void pushUpdate(string thing)
        {
            Handle.pushUpdate(thing);
        }

        public virtual void Dispose()
        {
        }

        public void start()
        {
            
        }

        public void cancel()
        {
            Handle.cancel();
        }

        public System.Threading.CancellationToken CancellationToken { get; set; }
    }
}
