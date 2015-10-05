using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

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
                    try
                    {
                        if (p.Name == "Internals") return "";
                        var property = t.GetProperty(p.Name);
                        var r = property.GetValue(o, null).ToString();
                        return p.Name + ": '" + r + "'";
                    }
                    catch (Exception ex)
                    {
                        return "";
                    }
                }).Where(s => !string.IsNullOrEmpty(s)));

            var result = "{" + props + "}";
            return result;
        }

        public static string describe(JObject jo, bool loose = false)
        {
            
            var inner = string.Join(",", jo.Properties().Select(p => p.Name + ": '" + p.Value.ToString() + "'"));
            return !loose ? "{" + inner + "}" : inner;
        }

        public static string describe<T>(IEnumerable<T> objects) where T : Node
        {
            var json = "[" + string.Join(",", objects.Select(o => DescribeAsNeoJSON.describe(o))) + "]";
            return json;
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
    public static class DescribeAsJSON
    {
        public static string describe(object o, bool pretty = true)
        {
            var t = o.GetType();
            var properties = t.GetProperties();
            var props = string.Join(",",
                properties.Select(p =>
                {
                    try
                    {
                        if (p.Name == "Internals") return "";
                        var property = t.GetProperty(p.Name);
                        var r = property.GetValue(o, null).ToString();
                        return "'" + p.Name + "': '" + r + "'";
                    }
                    catch (Exception ex)
                    {
                        return "";
                    }
                }).Where(s => !string.IsNullOrEmpty(s)));

            var result = "{" + props + "}";
            return !pretty ? result : JObject.Parse(result).ToString();
        }

        public static string describe<T>(IEnumerable<T> objects, bool pretty = true) where T : Node
        {
            var json = "[" + string.Join(",", objects.Select(o => DescribeAsJSON.describe(o, false))) + "]";
            return !pretty ? json : JArray.Parse(json).ToString();
        }
    }

    public static class DescribeAsJSON<T> where T : Node
    {
        public static string describe(bool pretty = true)
        {
            var properties = typeof(T).GetProperties();
            var props = string.Join(",",
                properties.Select(p =>
                {
                    return p.Name != "Internals" ? p.Name + ": '" + p.PropertyType.Name + "'" : "";
                }).Where(s => !string.IsNullOrEmpty(s)));

            var result = "{" + props + "}";
            return !pretty ? result : JObject.Parse(result).ToString();
        }
    }
}
