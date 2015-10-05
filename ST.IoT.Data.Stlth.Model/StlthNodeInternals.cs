using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ST.IoT.Data.Stlth.Model
{
    public class StlthNodeInternals
    {
        public JObject Node { get; private set; }
        public JObject Data { get; private set; }

        public StlthNodeInternals(string nodeJson, string instanceJson)
        {
            Node = JObject.Parse(nodeJson);
            Data = JObject.Parse(instanceJson);
        }
    }
}
