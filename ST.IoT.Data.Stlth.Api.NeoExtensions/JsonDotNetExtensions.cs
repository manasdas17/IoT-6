using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ST.IoT.Data.Stlth.Api.NeoExtensions
{
    public static class NeoJsonDotNetExtensions
    {
        public static string ToNeoJson(this JObject jo)
        {
            var sb = new StringBuilder();

            foreach (var p in jo.Properties())
            {
                if (sb.Length > 0) sb.AppendLine(",");
                sb.Append("  " + p.Name + ": ");
                if (p.Value.Type == JTokenType.String)
                {
                    sb.Append("'" + p.Value.ToString() + "'");
                }
                else
                {
                    sb.Append(" " + p.Value);
                }
            }
            sb.AppendLine();

            return "{\n" + sb.ToString() + "}";
        }
    }
}
