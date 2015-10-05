using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ST.IoT.Data.Stlth.Model
{
    public class Node : DynamicObject
    {
        //public StlthNodeInternals Internals { get; private set; }
        protected  Dictionary<string, string> _dict = new Dictionary<string, string>(); 
        public StlthNodeInternals Internals { get; private set; }

        public string ID
        {
            get { return _dict["ID"]; }
            set { _dict["ID"] = value; }
        }


        public Node(StlthNodeInternals internals)
        {
            Internals = internals;

            var nodeData = internals.Node["data"];
            foreach (JProperty t in nodeData)
            {
                _dict[t.Name] = t.Value.ToString();
            }
            var dataData = internals.Data["data"];
            foreach (JProperty t in dataData)
            {
                _dict[t.Name] = t.Value.ToString();
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var name = binder.Name;
            if (Internals.Node["data"][name] != null)
            { 
                result = Internals.Node["data"][name];
                return true;
            }
            if (Internals.Data["data"][name] != null)
            {
                result = Internals.Data["data"][name];
                return true;
            }

            result = null;
            return false;
        }
        /*
        public string AsResultJSON
        {
            get
            {
                return "{\n " +
                    string.Join(",\n ",_dict.Keys.Select(k => string.Format("\"{0}\": \"{1}\"", k, _dict[k]))) +
                       ",\n \"Internals\": { \"Node\": " + Internals.Node.ToString() + ", \n " +
                       "      \"Data\": " + Internals.Data.ToString() + " }\n"
                       + "}";
            }
        }
        */
        public string ClassName
        {
            get { return _dict["ClassName"]; }
            set { _dict["ClassName"] = value; }
        }

        public virtual string ToJson()
        {
            var sb = new StringBuilder();
            sb.Append("{");
            foreach (var key in _dict.Keys)
            {
                if (sb.Length > 1) sb.Append(",");
                sb.Append(string.Format("'{0}': '{1}'", key, _dict[key]));
            }
            sb.Append("}");
            return sb.ToString();
        }

        public override string ToString()
        {
            return DescribeAsJSON.describe(this);
        }
    }
    /*
    public static class NodeExtensions
    {
        public static string AsResultJSON(this IEnumerable<Node> nodes)
        {
            return "{" + string.Join(",\n", nodes.Select(n => n.AsResultJSON)) + "}";
        }
    }
    */
}
