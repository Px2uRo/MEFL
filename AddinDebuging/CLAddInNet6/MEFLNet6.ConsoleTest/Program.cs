using MEFL.CLAddIn.Downloaders;
using MEFL.Contract;
using MEFL.Contract.Web;
using System.Diagnostics;

namespace MEFLNet6.ConsoleTest
{

    internal class Program
    {
        static void Main(string[] args)
        {
            var source = @"https://piston-meta.mojang.com/v1/packages/6607feafdb2f96baad9314f207277730421a8e76/1.19.3.json";
            var targetName = Path.GetFileName(source);
            var target = Path.Combine(Environment.CurrentDirectory, targetName);
            var manifest = new ManifestFile(new NativeLocalPair(source, target), Path.Combine(Environment.CurrentDirectory, "download"));
            var fandown = new FandownProgress(manifest, null, null);
            fandown.Start();
            Console.Read();
        }
    }
}