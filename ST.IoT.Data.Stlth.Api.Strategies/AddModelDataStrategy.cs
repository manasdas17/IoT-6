using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ST.IoT.Data.Stlth.Api.NeoExtensions;
using ST.IoT.Data.Stlth.Model;

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

    public class AddModelDataStrategy : IStrategy<AddModelDataStrategy>, IStrategy
    {
        private readonly IStlthDataClient _client;

        public AddModelDataStrategy(IStlthDataClient client)
        {
            _client = client;
        }

        async Task<AddModelDataStrategy> IStrategy<AddModelDataStrategy>.ExecuteAsync()
        {
            _client.Connect();

            var people = new []
            {
                JObject.FromObject(new { Name = "Mike" }),
                /*JObject.FromObject(new { Name = "Marcia" }),
                JObject.FromObject(new { Name = "Mikael" }),
                JObject.FromObject(new { Name = "Wiliam" }),
                JObject.FromObject(new { Name = "Dan" }),*/
            };
            
            var results = new List<NodeResult>();
            foreach (var person in people)
            {
                var result = await _client.NodePostAsync(StlthBuiltinNodeLabels.Person, json: person.ToString());
                results.Add(result);
            }
            
            //var who = await _client.NodeGetAsync(results[0].ID);
            //Console.WriteLine(who.Node);

            var who = await _client.QueryNodesAsync(StlthBuiltinNodeLabels.Person, "{'Name': 'Mike'}");
            Console.WriteLine(who);
            /*
            var posts = new[]
            {
                JObject.FromObject(new { Content = "HI!" })
            };
            
            foreach (var post in posts)
            {
                var result = await _client.NodePostAsync(StlthBuiltinNodeLabels.Post, json: post.ToNeoJson());
                results.Add(result);
            }

            //await _client.NodeDeleteAsync(results[0].ID.ToString());
            //await _client.NodePutAsync(results[0].ID.ToString(), "{Foo: 'Bar'}");

            var r1 = await _client.RelPostAsync(results[0].ID, StlthBuiltinRelNames.Friend, results[3].ID);
            var r2 = await _client.RelPostAsync(results[0].ID, StlthBuiltinRelNames.Friend, results[4].ID);
            var r3 = await _client.RelPostAsync(results[0].ID, StlthBuiltinRelNames.Spouse, results[1].ID);
            var r4 = await _client.RelPostAsync(results[0].ID, StlthBuiltinRelNames.Parent, results[2].ID);
            var r5 = await _client.RelPostAsync(results[1].ID, StlthBuiltinRelNames.Parent, results[2].ID);
            //await _client.RelDeleteAsync(f1r.ID.ToString());

            Console.WriteLine("Friends of mike");
            var relGetResult = await _client.RelGetAsync(StlthBuiltinRelNames.Friend, results[0].ID, alsoNodes: true);
            relGetResult.ToNodes.ToList().ForEach(r => Console.WriteLine(r.Value));

            Console.WriteLine("People who are parents");
            relGetResult = await _client.RelGetAsync(StlthBuiltinRelNames.Parent);
            relGetResult.Relations.ToList().ForEach(r => Console.WriteLine(r));

            Console.WriteLine("People who are spouses");
            relGetResult = await _client.RelGetAsync(StlthBuiltinRelNames.Spouse);
            relGetResult.Relations.ToList().ForEach(r => Console.WriteLine(r));
            */
            _client.Disconnect();

            return this;
        }

        async Task IStrategy.ExecuteAsync()
        {
            await ((IStrategy<AddModelDataStrategy>) this).ExecuteAsync();
        }
    }
}
