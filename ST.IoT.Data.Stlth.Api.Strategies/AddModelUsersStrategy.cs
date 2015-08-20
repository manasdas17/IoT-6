using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ST.IoT.Data.Stlth.Api.NeoExtensions;

namespace ST.IoT.Data.Stlth.Api.Strategies
{
    public interface IStrategy
    {
        Task ExecuteAsync();
    }

    public interface IStrategy<T>
    {
        Task<T> ExecuteAsync();
    }

    public class AddModelUsersStrategy : IStrategy<AddModelUsersStrategy>, IStrategy
    {
        private readonly IStlthDataClient _client;

        public AddModelUsersStrategy(IStlthDataClient client)
        {
            _client = client;
        }

        async Task<AddModelUsersStrategy> IStrategy<AddModelUsersStrategy>.ExecuteAsync()
        {
            _client.Connect();

            var p1 = new JObject();
            p1["Name"] = "Mike";
            var p2 = new JObject();
            p2["Name"] = "Will";

            await _client.NodeAsync(StlthDataOperation.POST, StlthNodeType.Normal, StlthBuiltinNodeLabels.Person, json: p1.ToNeoJson());
            await _client.NodeAsync(StlthDataOperation.POST, StlthNodeType.Normal, StlthBuiltinNodeLabels.Person, json: p2.ToNeoJson());

            _client.Disconnect();

            return this;
        }

        async Task IStrategy.ExecuteAsync()
        {
            await ((IStrategy<AddModelUsersStrategy>) this).ExecuteAsync();
        }
    }
}
