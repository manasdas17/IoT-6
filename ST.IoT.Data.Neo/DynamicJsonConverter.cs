using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ST.IoT.Data.Model;

namespace ST.IoT.Data.Neo
{
    internal class DynamicJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            var result = objectType.IsAssignableFrom(typeof (Dynamic));
            return result;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
