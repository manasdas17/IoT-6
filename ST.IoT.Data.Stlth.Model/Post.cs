using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Data.Stlth.Model
{
    public class Post : Node
    {
        public string Content
        {
            get { return _dict["Content"]; }
            set { _dict["Content"] = value; }
        }

        public Post(StlthNodeInternals internals) : base(internals)
        {
        }
    }
}
