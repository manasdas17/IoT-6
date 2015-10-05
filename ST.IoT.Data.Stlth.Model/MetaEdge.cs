using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ST.IoT.Data.Stlth.Model
{
    public class MetaEdge
    {
        public JObject MEC { get; private set; }

        public List<JObject> METs { get; private set; }


        public string Name
        {
            get { return MEC["data"]["Name"].ToString(); }
        }

        public Dictionary<string, string> Types
        {
            get { return METs.Select(m => m["data"]["Name"].ToString()).ToDictionary(i => i); }
        }

        public MetaEdge(JObject mec, JObject met)
        {
            MEC = mec;
            METs = new List<JObject> () { met };
        }
    }
}
