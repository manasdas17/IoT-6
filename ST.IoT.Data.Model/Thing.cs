using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ST.IoT.Data.Model
{
    public class Thing : JObject
    {
        public string ID
        {
            get { return this["ID"].ToString(); }
            set { this["ID"] = value; }
        }

        public Thing()
        {
            ID = Guid.NewGuid().ToString();
        }
    }
}
