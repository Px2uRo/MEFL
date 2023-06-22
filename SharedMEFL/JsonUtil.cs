using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL
{
    internal static class JsonUtil
    {
        internal static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        internal static T? ToObject<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value, (JsonSerializerSettings?)null);
        }
    }
}
