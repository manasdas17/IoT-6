using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4jClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ST.IoT.Data.Interfaces;

namespace ST.IoT.Data.Neo
{
    public class Neo4jThingsDataFacade : IThingsDataFacade
    {
        private GraphClient _client;
        public class User
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
            public string Email { get; set; }
        }

        public void Put(ST.IoT.Data.Model.Thing theThing)
        {
            connect();

            var json = JsonConvert.SerializeObject(theThing);

            var create = string.Format("(n:{0} {{newThing}})", theThing.GetType().Name);

            _client.Cypher
                .Create(create)
                .WithParam("newThing", theThing)
                .ExecuteWithoutResults();
        }

        public IEnumerable<ST.IoT.Data.Model.Thing> GetMostRecentVersionsOfThing(string thingID, int count = 1)
        {
            throw new NotImplementedException();
        }

        private void connect()
        {
            if (_client == null)
            {
                _client = new GraphClient(new Uri("http://neo4j:rush2112@localhost:7474/db/data"));
            }
            if (!_client.IsConnected)
            {
                _client.Connect();
            }
        }
    }
}
