using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace ST.IoT.Services.Core.P2P.Client.Portable
{
    public class SignalRChannel : PeerChannel, IDisposable
    {
        private HubConnection _connection;
        private IHubProxy _hub;

        public SignalRChannel()
        {
            
        }

        public SignalRChannel(string url)
        {
            _connection = new HubConnection(url);
            _hub = _connection.CreateHubProxy("SupernodeHub");
            _connection.Start().Wait();

        }

        public void Dispose()
        {
            _connection.Stop();
        }

        public override void Send(object message)
        {
            var method = message.GetType().Name;
            var msg = JsonConvert.SerializeObject(message);
            _hub.Invoke(method, msg).Wait();
        }
    }
}
