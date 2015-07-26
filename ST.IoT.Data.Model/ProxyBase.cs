using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Data.Model
{
    public class ProxyBase
    {
        public Dynamic _dynamic;

        public object getPropertyValue(string propertyName)
        {
            object result;
            return _dynamic.TryGetMember(null, out result);
        }

        public void setPropertyValue(string propertyName, object value)
        {
            
        }
    }
}
