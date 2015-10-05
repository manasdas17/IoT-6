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
    public class RelateResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ID { get; set; }
        public IEnumerable<Rel> Relations { get; set; }
        public Dictionary<string, Model.Node> FromNodes { get; private set; }
        public Dictionary<string, Model.Node> ToNodes { get; private set; }

        public RelateResult(HttpStatusCode statusCode, string id)
        {
            StatusCode = statusCode;
            ID = id;
        }

        public RelateResult(HttpStatusCode statusCode, IEnumerable<Rel> relations = null, IEnumerable<Model.Node> fromNodes = null, IEnumerable<Model.Node> toNodes = null)
        {
            StatusCode = statusCode;
            Relations = relations;
            if (fromNodes != null) FromNodes = fromNodes.ToDictionary(n => n.ID);
            if (toNodes != null) ToNodes = toNodes.ToDictionary(n => n.ID);
        }
    }
}