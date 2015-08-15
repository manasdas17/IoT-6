using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Subscriptions.Agents.PushUpdates.Core
{
    public abstract class SubscriptionHandle
    {
        public string SubscriptionHandleID { get; private set; }

        public SubscriptionHandle(string subscriptionHandleID = null)
        {
            SubscriptionHandleID = subscriptionHandleID;
        }

        public abstract void pushUpdate(string thing);

        public virtual void start()
        {
            
        }

        public virtual void cancel()
        {
            
        }
    }
}
