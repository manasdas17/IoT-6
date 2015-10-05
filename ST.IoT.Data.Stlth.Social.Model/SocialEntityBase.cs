using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ST.IoT.Data.Stlth.Social.Model
{
    public abstract class SocialEntityBase : DynamicObject
    {
        protected readonly dynamic _obj;

        public SocialEntityBase()
        {
            _obj = new JObject();
        }

        public SocialEntityBase(string json)
        {
            _obj = JObject.Parse(json);
        }

        public string ID
        {
            get { return _obj["ID"].ToString(); }
            set { _obj["ID"] = value; }
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

    }
}
