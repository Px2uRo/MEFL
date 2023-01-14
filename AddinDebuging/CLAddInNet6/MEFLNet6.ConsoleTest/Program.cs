using MEFL.CLAddIn.Downloaders;
using MEFL.Contract;

namespace MEFLNet6.ConsoleTest
{
    public class DownloadSourceJsonStruct
    {
        public List<NativeLocalPair> Sources { get; set; } = new List<NativeLocalPair>();
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var jsonPath = Path.Combine(Environment.CurrentDirectory, "test.json");
            var dotMCFolder = Path.Combine(Environment.CurrentDirectory, "dotmc");
            DownloadSourceJsonStruct jsonResult = null;
            using (var fileStream = new FileStream(jsonPath, FileMode.Open))
            {
               
                jsonResult = System.Text.Json.JsonSerializer.Deserialize<DownloadSourceJsonStruct>(fileStream);
            }

            //var source = 
            //var fandown = new FandownProgress(jsonResult.Sources, dotMCFolder, null);
            //fandown.Start();


            //Console.WriteLine("Hello, World!");
            var sourceJosnUri = @"https://piston-meta.mojang.com/v1/packages/6607feafdb2f96baad9314f207277730421a8e76/1.19.3.json";
            var targetJsonPath = Path.Combine(Environment.CurrentDirectory, "") ;
            var mani = new PackageManifest();

            Console.ReadKey();
        }
    }
}