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
    public interface IStlthDataClient
    {
        void Connect();
        void Disconnect();
        Task MetaNodeAsync(StlthDataOperation operation, string nodeClass, string definition = "");
        Task MetaRelAsync(StlthDataOperation operation, string name, string fromNodeClass, string toNodeClass);

        Task<NodeResult> NodePostAsync(string nodeLabel, string json, string context = "", string tenantID = null);
        Task<NodeResult> NodeGetAsync(string nodeID, string tenantID = null);
        Task<NodeResult> NodePutAsync(string nodeLabel, string json = "", string context = "", string tenantID = null);
        Task<NodeResult> NodeDeleteAsync(string nodeID, string tenantID = null);

        Task MetaEdgeAsync(StlthDataOperation operation, string nodeLabel = "", string json = "");
        Task EdgeAsync(StlthDataOperation operation, StlthElementType elementType, string nodeLabel = "", string json = "");

        Task<NodeResult> QueryNodesAsync(string nodeType, string properties, string tenantID = null);

        //Task RelationAsync(StlthDataOperation operation, StlthElementType elementType, string fromNodeLabel = "",string toNodeLabel = "", string relationName = "");
        Task LoadMetaNodesAsync();
        Task LoadMetaEdgesAsync();
        Task LoadMetaRelsAsync();
        Task<bool> IsRelTypeNameAsync(string segment);
        Task ExecuteCypherAsync(string cypher);
        Task<string> ExecuteCypherWithResultAsStringAsync(string cypher);

        Task<Model.Node> GetNodeByIdAsync(string id, string tenantID = null);
        Task<NodeResult> GetNodesOfTypeAsync(string name, int skip = 0, int limit = 100, string tenantID = null);

        //Task<RelateResult> RelateAsync(StlthDataOperation operation, string fromNodeId, string toNodeId, string relationshipTypeName, string relationshipContext);

        string TenantID { get; }

        Task<RelateResult> RelPostAsync(string fromNodeID, string relationshipName, string toNodeID, string tenantID = null);
        Task<RelateResult> RelDeleteAsync(string relID, string tenantID = null);
        Task<RelateResult> RelGetAsync(string relationshipName, string fromNodeID = "", string toNodeID = "", string tenantID = null, bool alsoNodes = false);

        ICypherFluentQuery Cypher { get; }

        Task<long> AllocateIDsAsync(int count = 1);

        Task<bool> IsNodeTypeNameAsync(string nodeTypeName);

        Task InitializeAsync();
    }
}