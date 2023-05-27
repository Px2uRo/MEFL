using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerInstaller
{

    public class Root
    {
        [JsonProperty("arguments")]
        public Arguments Arguments { get; set; }
        [JsonProperty("downloads")]
        public Downloads Downloads { get; set; }
        [JsonProperty("upaoption")]
        public UpaOption UpaOption { get; set; }
        [JsonProperty("servertype")]
        public string ServerType { get; set; }
        [JsonProperty("baseversion")]
        public string BaseVersion { get; set; }
        [JsonProperty("javamajor")]
        public int JavaMajor { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class Arguments
    {
        public Arguments(bool useTemplate=true)
        {
            if (useTemplate)
            {
                
            }
        }

        [JsonProperty("jvm")]
        [JsonConverter(typeof(ArgsConverter))]
        public object[] Jvm { get; set; }
        [JsonProperty("server")]
        public object[] Server { get; set; }
    }

    public class Downloads
    {
        [JsonProperty("server")]
        public MCFile Server { get; set; }
    }

    public class MCFile
    {
        public string sha1 { get; set; }
        public int size { get; set; }
        public string url { get; set; }
    }

    public class UpaOption
    {
        [JsonProperty("server_id")]
        public string Server_Id { get; set; }
    }


    public class PropertyResultPair
    {
        [JsonProperty("targetproperty")]
        public string TargetProperty { get; set; }
        [JsonProperty("results")]
        public Result[] Results { get; set; }
    }

    public class Result
    {
        [JsonProperty("condition")]
        public string Condition { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }

}
