using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace ST.IoT.Data.Model
{
    public abstract class State : Dynamic
    {
        public string ForThingID { get; set; }
        public long Version { get; set; }
        public long AtEpochGMT { get; set; }
    }
}
