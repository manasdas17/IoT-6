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
using NLog;
using ST.IoT.Data.Interfaces;
using ST.IoT.Messaging.Endpoints.Things.Updates;
using ST.IoT.Messaging.Messages.Push;

namespace ST.IoT.Data.Neo
{
    public class Neo4jThingsDataFacade : IThingsDataFacade
    {
        private GraphClient _client;
        private IRawGraphClient _rawclient;

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private ThingUpdatedPublishEndpoint _thingUpdated = null;

        public Neo4jThingsDataFacade(IThingUpdatedPublishEndpoint thingUpdated)
        {
            _thingUpdated = thingUpdated as ThingUpdatedPublishEndpoint;
        }

        public void reset()
        {
            connect();

            var cypher = @"
MATCH (state:State)<-[s_2_si]-()
MATCH (si:StateItem)<-[si_2_sc]-()
MATCH (thing:Thing)-[t_others]->()
MATCH (state_collection:StateCollection)-[sc_context]->()
MATCH (c:Context)
DELETE s_2_si, state, si_2_sc, si, t_others, thing, sc_context, state_collection, c
            ";

            executeCypher(cypher, null, CypherResultMode.Set);
        }

        public void Put(string json)
        {
            _logger.Info(json);

            try
            {
                var jo = JObject.Parse(json);

                // TODO: check all properties available in json

                var thing = jo["Thing"];
                var state = jo["State"];
                var meta = jo["Meta"];

                var time = DateTime.UtcNow.ToString();

                var cypher = @"
MERGE (thing:Thing {ID: {thing_id}}) ON CREATE SET thing = {thing_props} 
MERGE (context:Context {Name: {context_name}}) ON CREATE SET context = {context_props} 
CREATE UNIQUE (thing)-[r_thing_state:HAS_STATE]->(state_collection:StateCollection) 
CREATE (state_item:StateItem)<-[r_states_has_state_items:THE_STATES]-(state_collection) 
CREATE (state:State)<-[r_state_item_state:STATE_ITEM]-(state_item) 
SET state = {state_data} 
SET state_item = {state_props}
CREATE UNIQUE thing-[r_thing_context:IN_CONTEXT]->(context)
CREATE UNIQUE state_collection-[r_state__collection_context:IN_CONTEXT]->(context) 
                ";

                var thing_id = thing["ID"].ToString();
                var context_name = meta["Context"].ToString();
                var class_name = meta["Class"].ToString();

                var parameters = buildDict(
                    "thing_id", thing_id,
                    "thing_props", buildDict("ID", thing_id, "Name", thing_id),
                    "state_data", state,
                    "context_name", context_name,
                    "class_name", class_name,
                    "context_props", buildDict("Name", context_name),
                    "class_props", buildDict("Name", class_name),
                    "state_props", buildDict("CreateAtUTC", time)
                    );

                executeCypher(cypher, parameters);

                if (_thingUpdated != null)
                {
                    _thingUpdated.PublishAsync(new ThingUpdatedMessage(json));
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw;
            }
            finally
            {
                _logger.Info("Done PUT");
            }
        }

        public string Get(string json)
        {
            try
            {
                var jo = JObject.Parse(json);

                // TODO: check all properties available in json

                var thing = jo["Thing"];
                var meta = jo["Meta"];

                var cypher = @"
MATCH (t:Thing{Name:{thing_id} })-[]->(c:Context{Name:{context_name} })
MATCH (t)-[]->(sc:StateCollection)-[]->(c)
MATCH (sc)-[]->(si:StateItem)-[]-(s:State)
RETURN s
ORDER BY ID(s) DESC
";

                var thing_id = thing["ID"].ToString();
                var context_name = meta["Context"].ToString();

                var parameters = buildDict(
                    "thing_id", thing_id,
                    "context_name", context_name);

                if (meta["Paging"] != null)
                {
                    var paging = meta["Paging"];
                    if (paging["Skip"] != null)
                    {
                        parameters["skip"] = paging["Skip"].ToObject<int>();
                        cypher += "SKIP {skip}";

                    }
                    if (paging["Limit"] != null)
                    {
                        parameters["limit"] = paging["Limit"].ToObject<int>();
                        cypher += "LIMIT {limit}";

                    }
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
            catch (Exception ex)
            {
                _logger.Info(ex.Message);
                throw;
            }
            finally
            {
                _logger.Info("Done GET");
            }
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
            _logger.Info("Connecting");
            if (_client == null)
            {
                _logger.Info("Creating GraphClient");
                _client = new GraphClient(new Uri("http://neo4j:rush2112@localhost:7474/db/data"));
                _rawclient = _client;
            }
            try
            {
                if (!_client.IsConnected)
                {
                    _logger.Info("Not connected: attempting to connect");
                    try
                    {
                        _client.Connect();
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("Hard error");
                        _logger.Error(ex.Message);
                    }
                    _logger.Info("Connected");
                }
            }
            catch (AggregateException aex)
            {
                _logger.Error(aex.InnerExceptions.First().Message);
                throw aex.InnerExceptions.First();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw;
            }
            finally
            {
                _logger.Info("Leaving connect");
            }
        }

        private void executeCypher(string cypherQueryText, Dictionary<string, object> parameters, CypherResultMode resultMode = CypherResultMode.Set)
        {
            try
            {
                _logger.Debug("Attempting to execute cypher");
                connect();
                var query = new CypherQuery(cypherQueryText, parameters, CypherResultMode.Set);
                _logger.Info(query.DebugQueryText);
                _rawclient.ExecuteCypher(query);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw;
            }
        }

        private IEnumerable<T> executeCypherWithResults<T>(string cypherQueryText, Dictionary<string, object> parameters, CypherResultMode resultMode = CypherResultMode.Set)
        {
            try
            {
                _logger.Debug("Attempting to execute cypher with results");
                connect();
                var query = new CypherQuery(cypherQueryText, parameters, CypherResultMode.Set);
                _logger.Info(query.DebugQueryText);
                var results = _rawclient.ExecuteGetCypherResults<T>(query);
                _logger.Info(results);
                return results;

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw;
            }
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
