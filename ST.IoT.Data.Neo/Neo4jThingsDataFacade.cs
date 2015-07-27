using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Neo4jClient;
using Neo4jClient.Cypher;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using ST.IoT.Data.Interfaces;

namespace ST.IoT.Data.Neo
{
    public class Neo4jThingsDataFacade : IThingsDataFacade
    {
        private GraphClient _client;
        private IRawGraphClient _rawclient;

        public void reset()
        {
            connect();

            var cypher = @"
                MATCH (thing:Thing)-[r1]->()
                MATCH ()-[r2]->(state:State)
                MATCH (state_collection:StateCollection)-[r3]->(n)
                DELETE r1, thing, r2, state,  r3, n, state_collection
            ";

            executeCypher(cypher, null, CypherResultMode.Set);
        }

        public void Put(string json)
        {
            var jo = JObject.Parse(json);

            // TODO: check all properties available in json

            var thing = jo["Thing"];
            var state = jo["State"];
            var meta = jo["Meta"];

            var cypher = @"
                MERGE (thing:Thing {ID: {thing_id}}) ON CREATE SET thing = {thing_props} 
                MERGE (context:Context {Name: {context_name}}) ON CREATE SET context = {context_props} 
                CREATE UNIQUE (thing)-[r_thing_state:HAS_STATE]->(state_collection:StateCollection) 
                CREATE (state:State)<-[r_states_has_states:THE_STATES]-(state_collection) 
                SET state = {state_props} 
                CREATE UNIQUE thing-[r_thing_context:IN_CONTEXT]->(context)
                CREATE UNIQUE state_collection-[r_state__collection_context:IN_CONTEXT]->(context) 
            ";

            var thing_id = thing["ID"].ToString();
            var context_name = meta["Context"].ToString();
            var class_name = meta["Class"].ToString();

            var parameters = buildDict(
                "thing_id", thing_id,
                "thing_props", buildDict("ID", thing_id, "Name", thing_id),
                "state_props", state.asProperties(),
                "context_name", context_name,
                "class_name", class_name,
                "context_props", buildDict("Name", context_name),
                "class_props", buildDict("Name", class_name)
                );

            executeCypher(cypher, parameters);
        }

        public string Get(string json)
        {
            var jo = JObject.Parse(json);

            // TODO: check all properties available in json

            var thing = jo["Thing"];
            var meta = jo["Meta"];

            var cypher = @"
                MATCH most_recent_state = (thing:Thing)-[:IN_CONTEXT]->(context:Context)
                MATCH (thing)-[:HAS_STATE]->(state_collection:StateCollection)
                MATCH (state_collection)-[:THE_STATES]->(state:State)
                WHERE thing.ID = {thing_id} AND context.Name = {context_name}
                RETURN state
                ORDER BY ID(state) DESC
            ";

            var thing_id = thing["ID"].ToString();
            var context_name = meta["Context"].ToString();

            var parameters = buildDict(
                "thing_id", thing_id,
                "context_name", context_name);

            if (meta["Limit"] != null)
            {
                parameters["limit"] = meta["Limit"].ToObject<int>();
                cypher += "LIMIT {limit}";
            }

            var results = executeCypherWithResults<string>(cypher, parameters);

            var foo = "[" + string.Join(",", results) + "]";

            var rb = new StringBuilder();
            rb.AppendLine("{");
            rb.AppendLine("  \"Request\":");
            rb.AppendLine(json + ",");
            rb.AppendLine("  \"Results\":");
            rb.AppendLine(foo);
            rb.AppendLine("}");

            var rs = rb.ToString();

            var jobject_result = JObject.Parse(rs);

            var return_value = jobject_result.ToString();
            return return_value;
        }

        private Dictionary<string, object> buildDict(params object[] parms)
        {
            if (parms.Length %2 != 0) throw new Exception("Needs to be even number of parameters");

            var result = new Dictionary<string, object>();
            for (var i=0; i<parms.Length; i+=2)
            {
                result[(string) parms[i]] = parms[i + 1];
            }
            return result;
        }

        private void connect()
        {
            if (_client == null)
            {
                _client = new GraphClient(new Uri("http://neo4j:rush2112@localhost:7474/db/data"));
                _rawclient = _client;
            }
            if (!_client.IsConnected)
            {
                _client.Connect();
            }
        }

        private void executeCypher(string cypherQueryText, Dictionary<string, object> parameters, CypherResultMode resultMode = CypherResultMode.Set)
        {
            connect();
            var query = new CypherQuery(cypherQueryText, parameters, CypherResultMode.Set);
            _rawclient.ExecuteCypher(query);
        }

        private IEnumerable<T> executeCypherWithResults<T>(string cypherQueryText, Dictionary<string, object> parameters, CypherResultMode resultMode = CypherResultMode.Set)
        {
            connect();
            var query = new CypherQuery(cypherQueryText, parameters, CypherResultMode.Set);
            var results = _rawclient.ExecuteGetCypherResults<T>(query);
            return results;
        }
    }

    public static class JsonDotNetExtensionsForCypher
    {
        public static IDictionary<string, object> asProperties(this JObject @object)
        {
            var result = @object.ToObject<Dictionary<string, object>>();

            var JObjectKeys = (from r in result
                               let key = r.Key
                               let value = r.Value
                               where value.GetType() == typeof(JObject)
                               select key).ToList();

            var JArrayKeys = (from r in result
                              let key = r.Key
                              let value = r.Value
                              where value.GetType() == typeof(JArray)
                              select key).ToList();

            JArrayKeys.ForEach(key => result[key] = ((JArray)result[key]).Values().Select(x => ((JValue)x).Value).ToArray());
            JObjectKeys.ForEach(key => result[key] = asProperties(result[key] as JObject));

            return result;
        }

        public static IDictionary<string, object> asProperties(this JToken token)
        {
            return ((JObject) token).asProperties();
        }
    }
}
