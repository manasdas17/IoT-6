using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

    public static class StlthBuiltinRelNames
    {
        public const string Feed = "Feed";
        public const string Friend = "Friend";
        public const string Parent = "Parent";
        public const string Spouse= "Spouse";
        public const string Member = "Member";
    }

    public static class StlthBuiltinNodeLabels
    {
        public const string Root = "Root";
        public const string Node = "Node";
        public const string Meta = "Meta";
        public const string MetaRoot = "MetaRoot";

        public const string Person = "Person";
        public const string Post = "Post";
        public const string Group = "Group";
        public const string Community = "Community";
        public const string Space = "Space";
        public const string Timeline = "Timeline";
        public const string MediaCollection = "MediaCollection";
        public const string Thing = "Thing";
    }

    public static class StlthBuiltinEdgeLabels
    {
        public const string Root = "Root";
        public const string Node = "Node";
        public const string Edge = "Edge";
        public const string Meta = "Meta";
        public const string MetaRoot = "MetaRoot";

        public const string Interest = "Interest";
        public const string Action = "Action";
    }

    public enum StlthDataOperation
    {
        PUT,
        POST,
        GET,
        DELETE
    }

    public enum StlthElementType
    {
        Instance,
        Node,
        Edge,
        Rel,
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

        private readonly ConcurrentDictionary<string, MetaNode> _metaNodes =
            new ConcurrentDictionary<string, MetaNode>();

        private readonly ConcurrentDictionary<string, MetaEdge> _metaEdges =
            new ConcurrentDictionary<string, MetaEdge>();

        private readonly ConcurrentDictionary<string, MetaRel> _metaRels = new ConcurrentDictionary<string, MetaRel>();

        public List<string> NodeLabels
        {
            get { return _metaNodes.Values.Select(n => n.Name).ToList(); }
        }

        public string TenantID { get; private set; }

        public ICypherFluentQuery Cypher
        {
            get { return _client.Cypher; }
        }

        public object Rel { get; private set; }

        public StlthDataClient()
        {
            _url = "http://neo4j:stlth@localhost:7474/db/data";
            TenantID = "Stlth";
        }

        public void Connect()
        {
            connect();
        }

        public void Disconnect()
        {
            disconnect();
        }

        public async Task InitializeAsync()
        {
            Connect();

            await LoadMetaNodesAsync();
            await LoadMetaEdgesAsync();
            await LoadMetaRelsAsync();
        }

        public async Task ExecuteCypherAsync(string cypher)
        {
            await executeCypherAsync(cypher);
        }

        public async Task LoadMetaNodesAsync()
        {
            var query = _client
                .Cypher
                .Match("()-[:META_NODE_CLASS]->(n)-[:META_NODE_CLASS_INSTANCE]->(i)")
                .Return((n, i) => new
                {
                    N = n.As<string>(),
                    I = i.As<string>()
                });


            _logger.Trace(query.Query.DebugQueryText);
            var results = await query.ResultsAsync;
            var meta = results.Select(n => new MetaNode(new StlthNodeInternals(n.N, n.I))).ToList();

            _metaNodes.Clear();
            foreach (var mr in meta)
            {
                _metaNodes[mr.Name] = mr;
            }
        }

        public async Task LoadMetaEdgesAsync()
        {
            var query = _client
                .Cypher
                .Match("()-[:META_EDGE_CLASS]->(mec)-[]->(met)")
                .Return((mec, met) => new
                {
                    MEC = mec.As<string>(),
                    MET = met.As<string>()
                });

            _logger.Trace(query.Query.DebugQueryText);
            var results = await query.ResultsAsync;

            var meta = results.Select(r =>
            {
                var mec = JObject.Parse(r.MEC);
                var met = JObject.Parse(r.MET);

                var name = mec["data"]["Name"].ToString();
                if (_metaEdges.ContainsKey(name))
                {
                    _metaEdges[name].METs.Add(met);
                }
                else
                {
                    _metaEdges[name] = new MetaEdge(mec, met);
                }

                return name;
            }).ToArray();

            /*
            _metaEdges.Clear();
            foreach (var mr in meta)
            {
                var m = JObject.Parse(mr["data"].ToString());
                if (m["FromNode"] != null && m["ToNode"] != null && m["Name"] != null)
                {
                    _metaEdges[m["Label"].ToString()] = m;
                }
            }
            */
        }

        public async Task LoadMetaRelsAsync()
        {
            var query = _client
                .Cypher
                .Match("()-[:META_REL]->(r)")
                .Return(r => new
                {
                    R = r.As<string>()
                });

            _logger.Trace(query.Query.DebugQueryText);

            var results = await query.ResultsAsync;

            _metaRels.Clear();

            results.Select(r =>
            {
                var mrel = new MetaRel(JObject.Parse(r.R));
                _metaRels[mrel.Name] = mrel;
                return mrel;
            }).ToArray();
        }


        public async Task MetaNodeAsync(StlthDataOperation operation, string nodeClass, string definition = "")
        {
            var query = _client
                .Cypher
                .Match("(mrcn:stlth:Global:Meta:Root:Nodes)")
                .Merge(string.Format("(mnc:stlth:Global:Meta:Node:Class:{0} {{Name: '{0}'}})", nodeClass))
                .Create(string.Format("(n:stlth:Global:Meta:Node:{0}:Instance {1})", nodeClass, definition))
                .Merge("(mrcn)-[r1:META_NODE_CLASS]->(mnc)")
                .Create("(mnc)-[r3:META_NODE_CLASS_INSTANCE {CreatedAt: timestamp()}]->(n)")
                .Return(mnc => new
                {
                    MetaNodeJSON = mnc.As<string>()
                });

            try
            {
                _logger.Trace(query.Query.DebugQueryText);

                var results = await query.ResultsAsync;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                if (ex.InnerException != null) _logger.Error(ex.InnerException.Message);
                throw;
            }
        }

        public async Task MetaEdgeAsync(StlthDataOperation operation, string edgeClass, string definition = "")
        {
            var query = _client
                .Cypher
                .Match("(mre:stlth:Global:Meta:Root:Edges)")
                .Merge(string.Format("(me:stlth:Global:Meta:Edge:Class:{0} {{Name: '{0}'}})", edgeClass))
                .Merge("(mre)-[r1:META_EDGE_CLASS]->(me)");


            var types = JArray.Parse(definition);
            var i = 1;
            foreach (var t in types)
            {
                var p1 = string.Format("(et{0}:stlth:Global:Meta:Edge:Class:{1}:EdgeType:{2} {{Name: '{2}'}})", i,
                    edgeClass, t);
                var p2 = string.Format("(me)-[:META_EDGE_TYPE]->(et{0})", i);
                query = query.Create(p1).Create(p2);
                i++;
            }
            try
            {
                _logger.Trace(query.Query.DebugQueryText);

                await query.ExecuteWithoutResultsAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                if (ex.InnerException != null) _logger.Error(ex.InnerException.Message);
                throw;
            }
        }

        public async Task MetaRelAsync(StlthDataOperation operation, string name, string fromClassName,
            string toClassName)
        {
            var query = _client
                .Cypher
                .Match("(mrr:stlth:Global:Meta:Root:Rels)")
                .Merge(
                    string.Format(
                        "(mnc:stlth:Global:Meta:Rel:Class:{0}:{1}:{2} {{Name: '{0}', FromClassName: '{1}', ToClassName: '{2}'}})",
                        name, fromClassName, toClassName))
                .Create(string.Format("(mrr)-[:META_REL]->(mnc)"));

            try
            {
                _logger.Trace(query.Query.DebugQueryText);

                await query.ExecuteWithoutResultsAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                if (ex.InnerException != null) _logger.Error(ex.InnerException.Message);
                throw;
            }
        }

        public async Task EdgeAsync(StlthDataOperation operation, StlthElementType elementType, string nodeLabel,
            string json = "")
        {
            switch (elementType)
            {
                case StlthElementType.MetaRoot:
                {
                    var cypher =
                        @"match (ur:stlth:UberRoot) merge (mr:stlth:Edge:Meta:Root) create (ur)-[r:EDGE_METADATA]->(mr)";
                    await executeCypherAsync(string.Format(cypher, elementType));
                }
                    break;

                case StlthElementType.Meta:
                {
                    var id = await AllocateIDsAsync();

                    var cypher = @"
match (mr:stlth:Edge:Meta:Root)
create (n:stlth:Edge:Meta:{0} {{ID: {1}, Label: '{0}' }}) 
create (mr)-[r:META_INSTANCES]->(n)
";

                    var sb = new StringBuilder();
                    var specializations = JArray.Parse(json);
                    var i = 0;
                    foreach (var s in specializations)
                    {
                        sb.AppendLine(
                            string.Format(
                                "create (s{1}:stlth:Edge:Specialization:{0}) create (n)-[r{1}:EDGE_SPECIALIZATION]->(s{1})",
                                s, i));
                        i++;
                    }
                    var queryText = string.Format(cypher + sb.ToString(), nodeLabel, id);
                    await executeCypherAsync(queryText);
                }
                    break;

            }
        }

        public async Task<NodeResult> NodePostAsync(string nodeLabel, string json = null, string context = "", string tenantID = null)
        {
            string qt = null;

            try
            {
                if (!_metaNodes.ContainsKey(nodeLabel))
                    throw new Exception("The database does not contain a class definition for :" + nodeLabel);

                tenantID = getTenantID(tenantID);

                var neoJson = DescribeAsNeoJSON.describe(JObject.Parse(json));

                var id = await AllocateIDsAsync(1);
                var query = _client
                    .Cypher
                    .Match(string.Format("(r:stlth:{0}:Data:Nodes)", tenantID))
                    .Merge(string.Format("(nlr:stlth:{0}:Data:Node:{1}:Root {{Name: '{1}'}})", tenantID, nodeLabel))
                    .OnCreate()
                    .Set("nlr.CreatedAt=timestamp()")
                    .Merge(string.Format("(r)-[:NODE_CLASS_INSTANCES_ROOT]->(nlr)"))
                    .Create(string.Format("(n:stlth:{0}:Data:Node:{1}:TypeInstances {{ID: '{2}', CreatedAt: timestamp()}})", tenantID, nodeLabel, id))
                    .Create("(nlr)-[:NODE_TYPE_INSTANCES]->(n)")
                    .Create(string.Format("(im:stlth:{0}:Data:Node:{1}:InstancesRoot {{ClassName: '{1}', ID: '{2}', CreatedAt: timestamp(), Context: '{3}'}})",tenantID, nodeLabel, id, context))
                    .Create(string.Format("(n)-[:NODE_INSTANCE_DATA {{CreatedAt: timestamp(), ID: '{0}'}}]->(im)",id + 1))
                    .Create(String.Format("(i:stlth:{0}:Data:Node:{1}:Instance {2})", tenantID, nodeLabel, neoJson))
                    .Create("(im)-[:NODE_DATA_INSTANCES {CreatedAt: timestamp()}]->(i)");

                qt = query.Query.DebugQueryText;
                _logger.Trace(qt);

                await query.ExecuteWithoutResultsAsync();

                return new NodeResult(HttpStatusCode.Created, id.ToString(), nodeLabel);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                if (ex.InnerException != null) _logger.Error(ex.InnerException.Message);
                return new NodeResult(HttpStatusCode.InternalServerError, ex.InnerException.Message);
            }
        }

        public async Task<NodeResult> NodePutAsync(string nodeID, string json = "", string context = "", string tenantID = null)
        {
            try
            {
                var query = _client
                    .Cypher
                    .Match(
                        string.Format("(n:stlth:{0}:Node:Data:InstanceRoot {{ID: '{1}'}})-[r:NODE_DATA_INSTANCES]->()",
                            getTenantID(tenantID), nodeID))
                    .With("n.ClassName as ClassName")
                    .Return<string>(ClassName => ClassName.As<string>());

                var className = (await query.ResultsAsync).First();

                var query2 = _client
                    .Cypher
                    .Match(
                        string.Format("(n:stlth:{0}:Node:Data:InstanceRoot {{ID: '{1}'}})-[r:NODE_DATA_INSTANCES]->()",
                            getTenantID(tenantID), nodeID))
                    .Create(String.Format("(i:stlth:{0}:Data:Node:{1}:Instance {2})", getTenantID(tenantID), className, json))
                    .Create("(n)-[:NODE_DATA_INSTANCES {CreatedAt: timestamp()}]->(i)");

                _logger.Trace(query2.Query.DebugQueryText);

                await query2.ExecuteWithoutResultsAsync();

                return new NodeResult(HttpStatusCode.Accepted);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                if (ex.InnerException != null) _logger.Error(ex.InnerException.Message);
                return new NodeResult(HttpStatusCode.InternalServerError, ex.InnerException.Message);
            }
        }

        public async Task<NodeResult> NodeDeleteAsync(string nodeID, string tenantID = null)
        {
            try
            {
                var query = _client
                    .Cypher
                    .Match(
                        string.Format("()-[r0]-(p)-[r1]->(n:stlth:{0}:Node:Data:InstanceRoot {{ID: '{1}'}})-[r2]->(d)",
                             getTenantID(tenantID), nodeID))
                    .Delete("r0, r1, r2, p, n, d");

                await query.ExecuteWithoutResultsAsync();
                return new NodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new NodeResult(HttpStatusCode.InternalServerError,
                    ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                throw;
            }
        }

        public async Task<NodeResult> NodeGetAsync(string nodeID, string tenantID = null)
        {
            try
            {
                var node = await GetNodeByIdAsync(nodeID, getTenantID(tenantID));
                return new NodeResult(HttpStatusCode.OK, node);
            }
            catch (Exception ex)
            {
                return new NodeResult(HttpStatusCode.InternalServerError, ex.InnerException.Message);
                throw;
            }
        }

        public async Task<NodeResult> QueryNodesAsync(string nodeType, string properties, string tenantID = null)
        {
            string qt = null;

            try
            {
                var query = _client
                    .Cypher
                    .Match(string.Format("(n:stlth:{0}:Node:Data:{1}:InstancesRoot)-[r:NODE_DATA_INSTANCES]->()",
                            getTenantID(tenantID), nodeType))
                    .With("max(r.CreatedAt) as newestTime")
                    .Match(string.Format("(n) -[r: NODE_DATA_INSTANCES {{ CreatedAt: newestTime }}]->(i {0})", DescribeAsNeoJSON.describe(JObject.Parse(properties))))
                    .Return((n, i) =>
                        new
                        {
                            Node = n.As<string>(),
                            Data = i.As<string>()
                        });
                qt = query.Query.DebugQueryText;
                _logger.Trace(qt);

                var results = await query.ResultsAsync;
                var nodes = results.Select(r => NodeFactory.create(new StlthNodeInternals(r.Node, r.Data))).ToArray();
                
                return new NodeResult(HttpStatusCode.OK, nodes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<long> AllocateIDsAsync(int count = 1)
        {
            var query = _client
                .Cypher
                .Merge("(id:stlth:Global:UniqueIDs)")
                .OnCreate()
                .Set("id.count = 1")
                .OnMatch()
                .Set(string.Format("id.count = id.count + {0}", count))
                .With("id.count as uid")
                .Return<long>(uid => uid.As<int>());

            /*
            var result = await executeCypherWithResultsAsStringAsync(
                string.Format(
                    "merge(id:stlth:Global:UniqueIDs) on create set id.count = 1 on match set id.count = id.count + {0} with id.count as uid return uid",
                    count));
                    */

            var result = await query.ResultsAsync;
            return result.First();
        }

        //merge(id2:GlobalUniqueIds) on create set id2.count = 1 on match set id2.count = id2.count + 1 with id2.count as uid2
        //

        public async Task deleteAllNodesAndEdges()
        {
            await executeCypherAsync(StlthQueryTemplates.DeleteAllNodesAndEdges);
        }

        private async Task executeCypherAsync(string cypherQueryText, Dictionary<string, object> parameters = null,
            CypherResultMode resultMode = CypherResultMode.Set)
        {
            try
            {
                _logger.Trace("Attempting to execute cypher");
                connect();
                if (parameters == null) parameters = _emptyParameters;
                var query = new CypherQuery(cypherQueryText, parameters, CypherResultMode.Set);
                _logger.Trace(query.DebugQueryText);
                _rawclient.ExecuteCypher(query);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw;
            }
        }

        private async Task<string> executeCypherWithResultsAsStringAsync(string cypherQueryText,
            Dictionary<string, object> parameters = null, CypherResultMode resultMode = CypherResultMode.Set)
        {
            try
            {
                _logger.Trace("Attempting to execute cypher query with results");
                connect();
                if (parameters == null) parameters = _emptyParameters;
                var query = new CypherQuery(cypherQueryText, parameters, resultMode);
                _logger.Trace(query.DebugQueryText);
                try
                {
                    var raw = await _rawclient.ExecuteGetCypherResultsAsync<string>(query);
                    return raw.First();
                }
                catch (Exception ex2)
                {
                    _logger.Error(ex2.Message);
                    throw;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<IEnumerable<JObject>> executeCypherWithResultsAsync(string cypherQueryText,
            Dictionary<string, object> parameters = null, CypherResultMode resultMode = CypherResultMode.Set)
        {
            try
            {
                _logger.Trace("Attempting to execute cypher query with results");
                connect();
                if (parameters == null) parameters = _emptyParameters;
                var query = new CypherQuery(cypherQueryText, parameters, CypherResultMode.Set);
                _logger.Trace(query.DebugQueryText);
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
            _logger.Trace("Connecting");
            if (_client == null)
            {
                _logger.Trace("Creating GraphClient");
                _client = new GraphClient(new Uri(_url));
                _rawclient = _client;
            }
            try
            {
                if (!_client.IsConnected)
                {
                    _logger.Trace("Not connected: attempting to connect");
                    try
                    {
                        _client.Connect();
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("Hard error");
                        _logger.Error(ex.Message);
                    }
                    _logger.Trace("Connected");
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
                _logger.Trace("Leaving connect");
            }
        }

        private void disconnect()
        {
        }

        /*
        public async Task RelationAsync(StlthDataOperation operation, StlthElementType elementType,
            string fromNodeLabel = "", string toNodeLabel = "", string relationName = "")
        {
            switch (elementType)
            {
                case StlthElementType.MetaRoot:
                {
                    var cypher =
                        @"match (ur:stlth:UberRoot) merge (mr:stlth:Rel:Meta:Root) create (ur)-[r:REL_METADATA]->(mr)";
                    await executeCypherAsync(string.Format(cypher, elementType));
                }
                    break;

                case StlthElementType.Meta:
                {
                    var cypher = @"
match (rr:stlth:Rel:Meta:Root)
create (rn:stlth:Rel:Meta:{0}:{1}:{2} {{FromNode: '{0}', ToNode: '{1}', Name: '{2}'}})
create (rr)-[f:REL_METADATA]->(rn)
";
                    await executeCypherAsync(string.Format(cypher, fromNodeLabel, toNodeLabel, relationName));
                }
                break;
            }
        }
        */

        public async Task<string> ExecuteCypherWithResultAsStringAsync(string cypher)
        {
            var result = await executeCypherWithResultsAsStringAsync(cypher);
            return result;
        }

        /*
// returns the newest class isntance

match(i:stlth:Global:Meta:Node:Person:Instance)-[r]-(p)
with max(r.CreatedAt) as newest
match(i2:stlth:Global:Meta:Node:Person:Instance)-[:META_NODE_CLASS_INSTANCE {CreatedAt: newest}]-(p)
return i2
*/



        public async Task<Model.Node> GetNodeByIdAsync(string nodeId, string tenantID = null)
        {
            try
            {
                var query = _client
                    .Cypher
                    .Match(string.Format("(n:stlth:{0}:Node:Data:InstancesRoot {{ID: '{1}'}})-[r:NODE_DATA_INSTANCES]->()",
                            getTenantID(tenantID), nodeId))
                    .With("max(r.CreatedAt) as newestTime")
                    .Match("(n) -[r: NODE_DATA_INSTANCES { CreatedAt: newestTime}]->(i)")
                    .Return((n, i) =>
                        new
                        {
                            Node = n.As<string>(),
                            Data = i.As<string>()
                        });
                var results = await query.ResultsAsync;
                var rawnode = results.Select(r => new StlthNodeInternals(r.Node, r.Data)).FirstOrDefault();
                dynamic node = NodeFactory.create(rawnode);
                return node;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string getTenantID(string tenantID)
        {
            return tenantID ?? TenantID;
        }

        public async Task<NodeResult> GetNodesOfTypeAsync(string typeName, int skip = 0, int limit = 100, string tenantID = null)
        {
            var query = _client.Cypher
                .Match(string.Format(@"(n:stlth:{0}:Node:{1}:Data:InstanceRoot)", getTenantID(tenantID), typeName))
                .Match(string.Format("(n)-[r:NODE_DATA_INSTANCES]->(d)"))
                .Return((n, d) =>
                    new
                    {
                        Node = n.As<string>(),
                        Data = d.As<string>()
                    })
                .Skip(skip)
                .Limit(limit);

            try
            {
                _logger.Trace(query.Query.DebugQueryText);

                var results = await query.ResultsAsync;
                var rawnode = results.Select(r => new StlthNodeInternals(r.Node, r.Data)).ToArray();

                var nodes = NodeFactory.create(rawnode);

                return new NodeResult(HttpStatusCode.OK, nodes);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                if (ex.InnerException != null) _logger.Error(ex.InnerException.Message);
                return new NodeResult(HttpStatusCode.InternalServerError, ex.InnerException.Message);
            }
        }

        public async Task<RelateResult> RelPostAsync(string fromNodeID, string relationshipName, string toNodeID, string tenantID = null)
        {
            var createNewRelationship = false;
            if (relationshipName == null)
            {
                _logger.Info("Not relationship named, so using default_relationship");
                relationshipName = "default_relationship";
            }
            else
            if (!_metaRels.ContainsKey(relationshipName))
            {
                _logger.Warn("Relationship name does not exist: " + relationshipName);
                _logger.Warn("Creating a new relationship between the types of these nodes with name: " + relationshipName);
                createNewRelationship = true;
            }

            try
            {
                var id = await AllocateIDsAsync(1);

                tenantID = getTenantID(tenantID);

                // should check if these exist and throw exception?
                var fromNode = await GetNodeByIdAsync(fromNodeID, tenantID);
                var toNode = await GetNodeByIdAsync(toNodeID, tenantID);

                if (createNewRelationship)
                {
                    await MetaRelAsync(StlthDataOperation.PUT, relationshipName, fromNode.ClassName, toNode.ClassName);
                }

                var query = _client.Cypher
                    //.Match(string.Format(@"(glblRel:stlth:Global:Rel {{Name: '{0}'}})", relationshipName))  // the global metadata node for the specified relationship name
                    .Match(string.Format(@"(relDataRoot:stlth:{0}:Tenant:Data:Rels)", getTenantID(tenantID))) // root for data in the  tenant for rels
                    .Match(string.Format(@"(fromNodeRoot)-[:NODE_INSTANCE_DATA]-(fromNode:stlth:{0}:Node:Data:InstanceRoot {{ID: '{1}'}})", tenantID, fromNodeID))  // data node for the source
                    .Match(string.Format(@"(toNodeRoot)-[:NODE_INSTANCE_DATA]-(toNode:stlth:{0}:Node:Data:InstanceRoot {{ID: '{1}'}})", tenantID, toNodeID))  // data node for the source
                                                                                                                                                              //.Match(string.Format(@"(toNode:stlth:{0}:Node:Data {{ID: '{1}'}})", TenantID, toNodeID))  // data node for the target
                    .Create(string.Format(@"(relNode:stlth:{0}:Data:Rel:{1}:{2}:Instance {{ID: '{3}', RelName: '{4}', FromID: '{5}', ToID: '{6}'}})", // create a new rel node instance
                                          tenantID, fromNode.ClassName, toNode.ClassName, id, relationshipName, fromNodeID, toNodeID))
                    .Create(string.Format(@"(fromNodeRoot)-[:FROM_RELATION_INSTANCE]->(relNode)"))
                    .Create(string.Format(@"(relNode)-[:TO_RELATION_INSTANCE]->(toNodeRoot)"))
                    .Create(string.Format(@"(relDataRoot)-[:RELATION_INSTANCE]->(relNode)"));

                _logger.Trace(query.Query.DebugQueryText);
                await query.ExecuteWithoutResultsAsync();
                return new RelateResult(HttpStatusCode.OK, id: id.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<RelateResult> RelDeleteAsync(string relID, string tenantID)
        {
            try
            {
                var query = _client.Cypher
                    .Match(string.Format(@"(relDataRoot:stlth:{0}:Tenant:Data:Rels)", getTenantID(tenantID)))
                    .Match(string.Format(@"(relDataRoot)-[r:RELATION_INSTANCE]->(relNode {{ID: '{0}'}})", relID))
                    .Match(string.Format(@"(from)-[fromRel:FROM_RELATION_INSTANCE]->(relNode)-[toRel:TO_RELATION_INSTANCE]->(to)"))
                    .Delete("r, fromRel, toRel, relNode");

                _logger.Trace(query.Query.DebugQueryText);
                await query.ExecuteWithoutResultsAsync();
                return new RelateResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<RelateResult> RelGetAsync(string relationshipName, string fromNodeID = "", string toNodeID = "", string tenantID = null, bool alsoNodes = false)
        {
            return !alsoNodes
                ? await relGetWithNodesAsync(relationshipName, fromNodeID, toNodeID, tenantID)
                : await relGetWithNodesAsync(relationshipName, fromNodeID, toNodeID, tenantID);
        }

        private async Task<RelateResult> relGetWithNodesAsync(string relationshipName, string fromNodeID, string toNodeID, string tenantID)
        {
            /*
MATCH(relDataRoot: stlth:Stlth: Tenant:Data: Rels) -[:RELATION_INSTANCE] - (relNode { RelName: 'Friend', FromID: '2'})
MATCH(relNode) -[:FROM_RELATION_INSTANCE] - (fromNode)
MATCH(relNode) -[:TO_RELATION_INSTANCE] - (toNode)
MATCH(fromNode) -[:NODE_INSTANCE_DATA] - () -[rFrom: NODE_DATA_INSTANCES]->()
MATCH(toNode) -[:NODE_INSTANCE_DATA] - () -[rTo: NODE_DATA_INSTANCES]->()
WITH relNode, fromNode, toNode, max(rFrom.CreatedAt) as fromNewestTime, max(rTo.CreatedAt) as toNewestTime
MATCH(fromNode) -[:NODE_INSTANCE_DATA]-() -[:NODE_DATA_INSTANCES { CreatedAt: fromNewestTime}]->(fromInstanceData)
MATCH(toNode) -[:NODE_INSTANCE_DATA] - () -[:NODE_DATA_INSTANCES { CreatedAt: toNewestTime}]->(toInstanceData)
return relNode, fromNode, fromInstanceData, toNode, toInstanceData
*/

            try
            {

                var query = _client.Cypher
                    .Match(string.Format(@"(relDataRoot:stlth:{0}:Tenant:Data:Rels)-[:RELATION_INSTANCE]-(relNode {1})", getTenantID(tenantID), genRelQueryParams(relationshipName, fromNodeID, toNodeID)))
                    .Match("(relNode) -[:FROM_RELATION_INSTANCE] - (fromNode)")
                    .Match("(relNode) -[:TO_RELATION_INSTANCE] - (toNode)")
                    .Match("(fromNode) -[:NODE_INSTANCE_DATA] - () -[rFrom: NODE_DATA_INSTANCES]->()")
                    .Match("(toNode) -[:NODE_INSTANCE_DATA] - () -[rTo: NODE_DATA_INSTANCES]->()")
                    .With("relNode, fromNode, toNode, max(rFrom.CreatedAt) as fromNewestTime, max(rTo.CreatedAt) as toNewestTime")
                    .Match("(fromNode) -[:NODE_INSTANCE_DATA]-(fni) -[:NODE_DATA_INSTANCES { CreatedAt: fromNewestTime}]->(fromInstanceData)")
                    .Match("(toNode) -[:NODE_INSTANCE_DATA] - (tni) -[:NODE_DATA_INSTANCES { CreatedAt: toNewestTime}]->(toInstanceData)")
                    .Return((relNode, fni, fromInstanceData, tni, toInstanceData) =>
                        new
                        {
                            RelNode = relNode.As<string>(),
                            FromNode = fni.As<string>(),
                            FromInstanceData = fromInstanceData.As<string>(),
                            ToNode = tni.As<string>(),
                            ToInstanceData = toInstanceData.As<string>()
                        });
                
                _logger.Trace(query.Query.DebugQueryText);

                var results = await query.ResultsAsync;

                var relations = results.Select(r => new Rel(JObject.Parse(r.RelNode)["data"].ToString())).ToArray();

                var fromNodes = results.Select(r =>
                {
                    var jo1 = JObject.Parse(r.FromNode).ToString();
                    var jo2 = JObject.Parse(r.FromInstanceData).ToString();
                    return NodeFactory.create(new StlthNodeInternals(jo1.ToString(), jo2.ToString()));
                }).ToArray();

                var toNodes = results.Select(r =>
                {
                    var jo1 = JObject.Parse(r.ToNode).ToString();
                    var jo2 = JObject.Parse(r.ToInstanceData).ToString();
                    return NodeFactory.create(new StlthNodeInternals(jo1.ToString(), jo2.ToString()));
                }).ToArray();

                var ret = new RelateResult(HttpStatusCode.Accepted,
                    relations: relations,
                    fromNodes: fromNodes.DistinctBy(n => n.ID),
                    toNodes: toNodes.DistinctBy(n => n.ID));

                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<RelateResult> relGetAsync(string relationshipName, string fromNodeID, string toNodeID, string tenantID)
        {
            try
            {

                var query = _client.Cypher
                    .Match(string.Format(@"(relDataRoot:stlth:{0}:Tenant:Data:Rels)-[:RELATION_INSTANCE]-(relNode {1})", getTenantID(tenantID), genRelQueryParams(relationshipName, fromNodeID, toNodeID)))
                    .Return(relNode => relNode.As<string>());

                _logger.Trace(query.Query.DebugQueryText);

                var results = await query.ResultsAsync;
                var result = new RelateResult(HttpStatusCode.OK, results.Select(r => new Rel(r)));

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string genRelQueryParams(string relationshipName, string fromNodeID, string toNodeID)
        {
            if (string.IsNullOrEmpty(relationshipName) && string.IsNullOrEmpty(fromNodeID) && string.IsNullOrEmpty(toNodeID)) return "";
            var parts = new[]
            {
                !string.IsNullOrEmpty(relationshipName) ? "RelName: '" + relationshipName + "'" : "",
                !string.IsNullOrEmpty(fromNodeID) ? "FromID: '" + fromNodeID + "'" : "",
                !string.IsNullOrEmpty(toNodeID) ? "ToID: '" + toNodeID + "'" : "",
            }.Where(s => !string.IsNullOrEmpty(s));
            return "{" + string.Join(", ", parts) + "}";
        }

        public async Task<bool> IsNodeTypeNameAsync(string nodeTypeName)
        {
            return await Task.FromResult(_metaNodes.ContainsKey(nodeTypeName));
        }
        public async Task<bool> IsRelTypeNameAsync(string nodeTypeName)
        {
            return await Task.FromResult(_metaNodes.ContainsKey(nodeTypeName));
        }
    }

    public static class LinqExtensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
