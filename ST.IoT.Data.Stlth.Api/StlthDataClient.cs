using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Neo4jClient;
using Neo4jClient.Cypher;
using Newtonsoft.Json.Linq;
using NLog;
using ST.IoT.Data.Stlth.Model;

namespace ST.IoT.Data.Stlth.Api
{
    public interface IStlthDataClient
    {
        void Connect();
        void Disconnect();
        Task NodeAsync(StlthDataOperation operation, StlthNodeType nodeType, string nodeLabel, string json = "");
        Task LoadMetaNodesAsync();
    }

    public static class StlthBuiltinNodeLabels
    {
        public const string Person = "Person";
        public const string Post = "Post";
        public const string Root = "Root";
    }

    public enum StlthDataOperation
    {
        PUT,
        POST,
        GET,
        DELETE
    }

    public enum StlthNodeType
    {
        Normal,
        Meta,
        MetaRoot
    }

    public class StlthDataClient : IStlthDataClient
    {
        private GraphClient _client;
        private IRawGraphClient _rawclient;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly string _url = "";
        private readonly Dictionary<string, object> _emptyParameters = new Dictionary<string, object>(); 

        private readonly ConcurrentDictionary<string, JObject> _meta = new ConcurrentDictionary<string, JObject>();  

        public StlthDataClient()
        {
            _url = "http://neo4j:stlth@localhost:7474/db/data";
        }

        public void Connect()
        {
            connect();
        }

        public void Disconnect()
        {
            disconnect();
        }

        public async Task LoadMetaNodesAsync()
        {
            var meta = await executeCypherWithResultsAsync("MATCH (mn:stlth:Node:Meta) return mn");

            _meta.Clear();
            foreach (var m in meta)
            {
                _meta[m["Label"].ToString()] = m;
            }

            meta = null;
        }

        public async Task NodeAsync(StlthDataOperation operation, StlthNodeType nodeType, string nodeLabel, string json = "")
        {
            switch (nodeType)
            {
                case StlthNodeType.MetaRoot:
                {
                    var cypher = @"merge (mr:stlth:Node:Meta:{0} {{Label: '{0}'}}) ";
                    await executeCypherAsync(string.Format(cypher, nodeType, nodeLabel));
                }
                break;

                case StlthNodeType.Meta:
                {
                    if (_meta.ContainsKey(nodeLabel)) throw new Exception("Meta node already exists: " + nodeLabel);
                        var id = await allocateIDs();

                        var cypher = @"
match (mr:stlth:Node:Meta:MetaRoot) where mr.Label='MetaRoot'
create (n:stlth:Node:Meta:{0} {{ID: {1}, Label: '{0}' }}) 
create (mr)-[r:META_INSTANCES]->(n)
";
                    await executeCypherAsync(string.Format(cypher, nodeLabel, id));
                }
                break;


                case StlthNodeType.Normal:
                {
                    if (!_meta.ContainsKey(nodeLabel))
                        throw new Exception("The database does not contain a class definition for node class: " +
                                            nodeType);

                        var id = await allocateIDs(3);
                        var cypher = @"
match (m:stlth:Node:Meta:{1})
create (n:stlth:Node:{0}:{1} {{ID: {3}, Label: '{1}'}})
create (d:stlth:Node:Data:{1} {2})
create (n)-[r1:{1}_INSTANCE_DATA {{ID: {5}}}]->(d)
create (m)-[r2:{1}_INSTANCES {{ID: {4}}}]->(n)
";
                   await executeCypherAsync(string.Format(cypher, nodeType, nodeLabel, json, id, id + 1, id + 2));
                }
                break;
            }
        }

        private async Task<long> allocateIDs(int count = 1)
        {
            var result = await executeCypherWithResultsAsStringAsync(
                string.Format(
                    "merge(id:GlobalUniqueIds) on create set id.count = 1 on match set id.count = id.count + {0} with id.count as uid return uid",
                    count));
            return int.Parse(result);
        }

        //merge(id2:GlobalUniqueIds) on create set id2.count = 1 on match set id2.count = id2.count + 1 with id2.count as uid2
        //

        public async Task deleteAllNodesAndEdges()
        {
            await executeCypherAsync(StlthQueryTemplates.DeleteAllNodesAndEdges);
        }

        private async Task executeCypherAsync(string cypherQueryText, Dictionary<string, object> parameters = null, CypherResultMode resultMode = CypherResultMode.Set)
        {
            try
            {
                _logger.Debug("Attempting to execute cypher");
                connect();
                if (parameters == null) parameters = _emptyParameters;
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
        private async Task<string> executeCypherWithResultsAsStringAsync(string cypherQueryText, Dictionary<string, object> parameters = null, CypherResultMode resultMode = CypherResultMode.Set)
        {
            try
            {
                _logger.Debug("Attempting to execute cypher query with results");
                connect();
                if (parameters == null) parameters = _emptyParameters;
                var query = new CypherQuery(cypherQueryText, parameters, CypherResultMode.Set);
                _logger.Info(query.DebugQueryText);
                var raw = _rawclient.ExecuteGetCypherResults<string>(query);
                return await Task.FromResult(raw.First());
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw;
            }
        }

        private async Task<IEnumerable<JObject>> executeCypherWithResultsAsync(string cypherQueryText, Dictionary<string, object> parameters = null, CypherResultMode resultMode = CypherResultMode.Set)
        {
            try
            {
                _logger.Debug("Attempting to execute cypher query with results");
                connect();
                if (parameters == null) parameters = _emptyParameters;
                var query = new CypherQuery(cypherQueryText, parameters, CypherResultMode.Set);
                _logger.Info(query.DebugQueryText);
                var raw = _rawclient.ExecuteGetCypherResults<string>(query);
                var result = raw.Select(s => JObject.Parse(s)).ToArray();
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw;
            }
        }

        private void connect()
        {
            _logger.Info("Connecting");
            if (_client == null)
            {
                _logger.Info("Creating GraphClient");
                _client = new GraphClient(new Uri(_url));
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

        private void disconnect()
        {
        }
    }
}
