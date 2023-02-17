using MEFL.Core.Web;
using MEFL.Core.Web.Minecraft;

namespace MEFL.ConsoleTest;

internal class Program
{
    static void Main(string[] args)
    {
        var actions = new List<Action<int>>();

        var list = new int[10];
        for (int i = 0; i < 10; i++)
        {
            list[i] = i;

        }

        Parallel.ForEach(list, (i) =>
        {
            Thread.Sleep(i * 100);
            if (i > 6) throw new Exception($"{i}{i}{i}{i}");
            Console.WriteLine(i);
        });
        Console.WriteLine("111");
        Console.ReadKey();
        //var uri = @"https://piston-meta.mojang.com/v1/packages/6607feafdb2f96baad9314f207277730421a8e76/1.19.3.json";
        //var mcdir = Environment.CurrentDirectory;
        //var manifest =  PackageManifest.Download(uri, mcdir);
        //var assetIndex = manifest.DownloadAssetIndex();

        //List<DownloadURI> urls = new List<DownloadURI>();
        //urls.AddRange(manifest.Items);
        //urls.AddRange(assetIndex.Items);
        //new DownloadQueue(urls.ToArray()) . Download();
        //Console.ReadKey();
    }
}
