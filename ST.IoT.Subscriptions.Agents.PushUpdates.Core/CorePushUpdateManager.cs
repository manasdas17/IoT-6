using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ST.IoT.Messaging.Bus.Core;
using ST.IoT.Messaging.Messages.Push;

namespace ST.IoT.Subscriptions.Agents.PushUpdates.Core
{
    public class CorePushUpdateManager<T> : ConsumeEndpoint<ThingUpdatedMessage> where T : SubscriptionHandle
    {
        private ConcurrentDictionary<string, ConcurrentDictionary<string, SubscriptionToThing>> _subscriptions
            = new ConcurrentDictionary<string, ConcurrentDictionary<string, SubscriptionToThing>>();

        public CorePushUpdateManager() : base("thing_updated", autoDelete: true)
        {
            Start();
        }

        public async override Task ProcessAsync(ThingUpdatedMessage request)
        {
            var theThing = JObject.Parse(request.Thing);
            var id = theThing["Thing"]["ID"].ToString();

            if (_subscriptions.ContainsKey(id))
            {
                // this is important as it will snapshot the subscriptions for iteration
                var subscriptions = _subscriptions[id].Values.ToList();
                subscriptions.ForEach(s => s.pushUpdate(request.Thing));
            }
        }

        public T subscribe(
            string thingID,
            SubscriptionHandle handler,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ConcurrentDictionary<string, SubscriptionToThing> subscriptions = null;
            if (!_subscriptions.ContainsKey(thingID))
            {
                _subscriptions[thingID] = subscriptions = new ConcurrentDictionary<string, SubscriptionToThing>();
            }
            else
            {
                subscriptions = _subscriptions[thingID];
            }

            var subscription = new SubscriptionToThing(thingID, handler);
            subscriptions[subscription.SubscriptionID] = subscription;

            if (cancellationToken == default(CancellationToken))
                cancellationToken = new CancellationToken();
            subscription.CancellationToken = cancellationToken;
            cancellationToken.Register(cancellationRequested, subscription);

            return (T)subscription.Handle;
        }

        private Dictionary<string, SubscriptionToThing> _subscriptionsByHandleID = new Dictionary<string, SubscriptionToThing>(); 
        public T subscribe(
            string thingID,
            SubscriptionHandle handle)
        {
            ConcurrentDictionary<string, SubscriptionToThing> subscriptions = null;
            if (!_subscriptions.ContainsKey(thingID))
            {
                _subscriptions[thingID] = subscriptions = new ConcurrentDictionary<string, SubscriptionToThing>();
            }
            else
            {
                subscriptions = _subscriptions[thingID];
            }

            var subscription = new SubscriptionToThing(thingID, handle);
            subscriptions[subscription.SubscriptionID] = subscription;

            if (!string.IsNullOrEmpty(subscription.Handle.SubscriptionHandleID))
            {
                _subscriptionsByHandleID[subscription.Handle.SubscriptionHandleID] = subscription;
            }

            return (T)subscription.Handle;
        }

        private void cancellationRequested(object obj)
        {
            var subscription = obj as SubscriptionToThing;

            if (_subscriptions.ContainsKey(subscription.ForThingID))
            {
                var subs = _subscriptions[subscription.ForThingID];
                if (subs.TryRemove(subscription.SubscriptionID, out subscription))
                {
                    subscription.cancel();
                }
            }
        }

        public void unsubscribeByHandleID(string subscriptionHandleID)
        {
            if (_subscriptionsByHandleID.ContainsKey(subscriptionHandleID))
            {
                _subscriptionsByHandleID.Remove(subscriptionHandleID);
            }
        }
    }
}
