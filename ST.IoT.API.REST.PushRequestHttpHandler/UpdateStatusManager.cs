using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ST.IoT.Messaging.Endpoints.MTRMQ.Receive.ThingUpdated;

namespace ST.IoT.API.REST.PushRequestHttpHandler
{
    public class UpdateStatusManager
    {
        private ThingUpdatedReceiveEndpoint _updateReceiver;

        private ConcurrentDictionary<string, ConcurrentDictionary<string, Subscription>> _subscriptions
            = new ConcurrentDictionary<string, ConcurrentDictionary<string, Subscription>>();

        private class Subscription : IDisposable
        {
            public string ForThingID { get; set; }
            public DateTime LastUpdatedAt { get; set; }
            public PushStreamContent PushStream { get; set; }
            public string SubscriptionID { get; set; }

            private StreamWriter _writer;
            private Stream _stream;

            public Subscription(string forThingID)
            {
                SubscriptionID = Guid.NewGuid().ToString();
                ForThingID = forThingID;

                PushStream = new PushStreamContent(
                    (stream, content, context) =>
                    {
                        _stream = stream;

                        try
                        {
                            _writer = new StreamWriter(_stream);
                        }
                        catch (Exception ex)
                        {
                        }
                    });
            }

            public void pushUpdate(string thing)
            {
                if (_writer == null) throw new Exception("Response channel not open");
                _writer.Write(thing);
                _writer.Flush();
            }


            public void Dispose()
            {
                if (_writer != null)
                {
                    _writer.Close();
                }
                if (_stream != null)
                {
                    _stream.Close();
                }
            }
        }

        public UpdateStatusManager()
        {
            _updateReceiver = new ThingUpdatedReceiveEndpoint(thingUpdated);
        }

        private void thingUpdated(string thing)
        {
            var theThing = JObject.Parse(thing);
            var id = theThing["ID"].ToString();

            if (_subscriptions.ContainsKey(id))
            {
                // this is important as it will snapshot the subscriptions for iteration
                var subscriptions = _subscriptions[id].Values.ToList();
                subscriptions.ForEach(s => s.pushUpdate(thing));
            }
        }

        public PushStreamContent subscribe(string thingID, CancellationToken cancellationToken)
        {
            ConcurrentDictionary<string, Subscription> subscriptions = null;
            if (!_subscriptions.ContainsKey(thingID))
            {
                _subscriptions[thingID] = subscriptions = new ConcurrentDictionary<string, Subscription>();
            }
            else
            {
                subscriptions = _subscriptions[thingID];
            }
            var subscription = new Subscription(thingID);
            subscriptions[subscription.SubscriptionID] = subscription;

            cancellationToken.Register(cancellationRequested, subscription);

            return subscription.PushStream;
        }

        private void cancellationRequested(object obj)
        {
            var subscription = obj as Subscription;

            if (_subscriptions.ContainsKey(subscription.ForThingID))
            {
                var subs = _subscriptions[subscription.ForThingID];
                Subscription outs;
                subs.TryRemove(subscription.SubscriptionID, out outs);
            }
        }
    }
}
