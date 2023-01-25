using MEFL.Core.Web.Minecraft;
using MEFL.MinecraftPackage;

namespace MEFL.ConsoleTest;

internal class Program
{
    static void Main(string[] args)
    {
        var uri = @"https://piston-meta.mojang.com/v1/packages/6607feafdb2f96baad9314f207277730421a8e76/1.19.3.json";
        var mcdir = Environment.CurrentDirectory;
        var manifest =  PackageManifest.Download(uri, mcdir);
        var assetIndex = manifest.DownloadAssetIndex();
        foreach (var item in manifest.Items)
        {
            Console.WriteLine(item);
        }

        foreach (var item in assetIndex.Items)
        {
            Console.WriteLine(item);
        }
        Console.ReadKey();
    }
}
