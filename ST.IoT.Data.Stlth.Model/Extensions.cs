using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Data.Stlth.Model
{
    public static class DescribeAsNeoJSON
    {
        public static string describe(object o)
        {
            var t = o.GetType();
            var properties = t.GetProperties();
            var props = string.Join(",",
                properties.Select(p =>
                {
                    return p.Name != "Internals" ? p.Name + ": '" + t.GetProperty(p.Name).GetValue(t, null) : "";
                }).Where(s => !string.IsNullOrEmpty(s)));

            var result = "{" + props + "}";
            return result;
        }

    }

    public static class DescribeAsNeoJSON<T> where T : Node
    {
        public static string describe()
        {
            var properties = typeof(T).GetProperties();
            var props = string.Join(",",
                properties.Select(p =>
                {
                    return p.Name != "Internals" ? p.Name + ": '" + p.PropertyType.Name + "'" : "";
                }).Where(s => !string.IsNullOrEmpty(s)));

            var result = "{" + props + "}";
            return result;
        }
    }
}
