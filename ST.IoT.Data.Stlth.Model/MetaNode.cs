using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Data.Stlth.Model
{
    public class MetaNode
    {
        public StlthNodeInternals Internals { get; private set; }

        public string Name
        {
            get { return Internals.Node["data"]["Name"].ToString(); }
        }

        public MetaNode(StlthNodeInternals internals)
        {
            Internals = internals;
        }
    }
}
