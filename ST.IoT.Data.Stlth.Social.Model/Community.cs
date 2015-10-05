using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ST.IoT.Data.Stlth.Social.Model
{
    public class Community : SocialEntityBase
    {
        public Community()
        {
        }

        public string Name
        {
            get { return _obj["RelName"].ToString(); }
            set { ((dynamic)this).RelName = ""; }
        }
    }
}
