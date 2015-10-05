using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Data.Stlth.Model
{
    public class Group : Node
    {
        public string Name
        {
            get { return _dict["Name"]; }
            set { _dict["Name"] = value; }
        }

        public Group(StlthNodeInternals internals) : base(internals)
        {
        }
    }
}
