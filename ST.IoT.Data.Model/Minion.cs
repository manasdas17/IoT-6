using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ST.IoT.Data.Model
{
    public class Minion : Thing
    {
        public string Content { get; set; }
        public Minion()
        {
            this.Content = "";
        }
    }
}
