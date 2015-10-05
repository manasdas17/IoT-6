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
    public class NodeResult
    {
        public HttpStatusCode StatusCode { get; private set; }
        public Model.Node Node { get; private set; }
        public string Message { get; private set; }
        //public string Rel { get; private set; }
        public string ID { get; private set; }
        public string Path { get; private set; }
        public IEnumerable<Model.Node> ResultSet { get; private set; }

        public bool HasResult
        {
            get { return Node != null || (ResultSet != null && ResultSet.Any()); }
        }

        public NodeResult(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
        public NodeResult(HttpStatusCode statusCode, Model.Node node)
        {
            StatusCode = statusCode;
            Node = node;
        }
        public NodeResult(HttpStatusCode statusCode, string id, string labelName = "")
        {
            StatusCode = statusCode;
            ID = id;
            Path = "/" + labelName + "/" + id.ToString();
        }
        public NodeResult(HttpStatusCode statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
        public NodeResult(HttpStatusCode statusCode, IEnumerable<Model.Node> resultSet)
        {
            StatusCode = statusCode;
            ResultSet = resultSet;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            append(sb, string.Format("'StatusCode': '{0}'", StatusCode));
            append(sb, string.Format("'HasResult': '{0}'", HasResult));
            if (ID != null) append(sb, string.Format("'ID': '{0}'", ID));
            if (Message != null) append(sb, string.Format("'Message': '{0}'", Message));
            if (Node != null) append(sb, string.Format("'Node': {0}'", Node.ToString()));
            //if (Rel != null) append(sb, string.Format("'Rel': {0}'", Rel.ToString()));
            if (Path != null) append(sb, string.Format("Path: '{0}'", Path));
            if (ResultSet != null)
            {
                append(sb, "'ResultSet': [" + string.Join(",", ResultSet.Select(r => r.ToString())) + "]");
            }
            var ret = JObject.Parse("{" + sb.ToString() + "}").ToString();
            return ret;
        }

        private void append(StringBuilder sb, string v)
        {
            if (sb.Length > 0) sb.Append(",");
            sb.Append(v);
        }
    }
}