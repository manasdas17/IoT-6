using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ST.IoT.Data.Stlth.Model
{
    public class Rel : DynamicObject
    {
        private readonly dynamic _obj;

        public Rel(string json)
        {
            _obj = JObject.Parse(json);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var name = binder.Name;
            if (_obj[name] != null)
            {
                result = _obj[name].ToString();
                return true;
            }
            result = null;
            return false;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (_obj[binder.Name] == null)
            {
                _obj.Add(binder.Name, value); 
            }
            else
            {
                _obj[binder.Name] = value;
            }
            return true;
        }

        public string FromID
        {
            get { return _obj["FromID"].ToString(); }
        }

        public string ToID
        {
            get { return _obj["ToID"].ToString(); }
        }

        public string Name
        {
            get { return _obj["RelName"].ToString(); }
            set { ((dynamic)this).RelName = ""; }
        }

        public string ID
        {
            get { return _obj["ID"].ToString(); }
        }

        public override string ToString()
        {
            return _obj.ToString();
        }
    }
}
