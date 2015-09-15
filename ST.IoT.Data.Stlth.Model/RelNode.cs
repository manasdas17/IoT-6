using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Data.Stlth.Model
{
    public class RelNode : Node
    {
        public string RelNaame
        {
            get { return _dict["RelNaame"]; }
            set { _dict["RelNaame"] = value; }
        }
        public string FromID
        {
            get { return _dict["FromID"]; }
            set { _dict["FromID"] = value; }
        }
        public string ToID
        {
            get { return _dict["ToID"]; }
            set { _dict["ToID"] = value; }
        }

        public RelNode(StlthNodeInternals internals) : base(internals)
        {
            
        }
    }
}
