using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Data.Model
{
    public class Dynamic : DynamicObject
    {
        private Dictionary<string, object> _props = new Dictionary<string, object>();

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return _props.TryGetValue(binder.Name, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _props[binder.Name] = value;
            return true;
        }

        public override DynamicMetaObject GetMetaObject(Expression parameter)
        {
            var meta = base.GetMetaObject(parameter);
            return meta;
        }
        
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            foreach (var kvp in _props)
            {
                info.AddValue(kvp.Key, kvp.Value);
            }
        }
    }
}
