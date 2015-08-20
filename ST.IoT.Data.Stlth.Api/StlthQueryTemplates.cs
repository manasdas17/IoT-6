using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Data.Stlth.Api
{
    public static class StlthQueryTemplates
    {
        public const string
            DeleteAllNodesAndEdges =
@"
MATCH (n)
OPTIONAL MATCH (n)-[r]-()
DELETE n,r
";
    }
}
