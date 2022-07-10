using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MEFL
{
    public static class FastLoadJson
    {
        public static JObject Load(string JsonPath)
        {
            var Content =System.IO.File.ReadAllText(JsonPath);
            return JObject.Parse(Content);
        }
    }
}
