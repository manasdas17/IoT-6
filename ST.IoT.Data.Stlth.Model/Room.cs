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
    public class Room : Node
    {
        public string Name
        {
            get { return _dict["Name"]; }
            set { _dict["Name"] = value; }
        }

        public Room(StlthNodeInternals internals) : base(internals)
        {
        }
    }
}
