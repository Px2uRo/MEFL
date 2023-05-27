using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ServerInstaller
{
    internal class ArgsConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var res = new List<object>();
            var j = JArray.Load(reader);
            foreach (var t in j)
            {
                try
                {
                    var item = JsonConvert.DeserializeObject<PropertyResultPair>(t.ToString());
                    res.Add(item);
                }
                catch
                {
                    res.Add(t.ToString());
                }
            }
            return res.ToArray();
        }

        public override bool CanWrite => false;
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {

        }
    }
}