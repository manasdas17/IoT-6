using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ST.IoT.Data.Stlth.Model
{
    public class MetaRel
    {
        public JObject Rel { get; private set; }

        public string Name { get { return Rel["data"]["Name"].ToString(); }}
        public string FromClassName { get { return Rel["data"]["FromClassName"].ToString(); } }
        public string ToClassNam { get { return Rel["data"]["ToClassName"].ToString(); } }

        public MetaRel(JObject mrel)
        {
            Rel = mrel;
        }
    }
}
