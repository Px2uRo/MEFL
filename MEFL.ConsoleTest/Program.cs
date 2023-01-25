using MEFL.MinecraftPackage;

namespace MEFL.ConsoleTest;

internal class Program
{
    static void Main(string[] args)
    {
        foreach (var item in AssetIndexHelper.GetAssetIndex("origin/assetIndex.json", "1"))
        {
            Console.WriteLine(item);
        } 
    }
}
