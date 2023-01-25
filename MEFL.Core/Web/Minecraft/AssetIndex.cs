using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MEFL.MinecraftPackage;

namespace MEFL.Core.Web.Minecraft;

public class AssetIndex
{
    private const string FileName = "assetIndex.json";
    private string _minecraftRootDir;
    public IEnumerable<DownloadURI> Items => _items;
    private List<DownloadURI> _items;

    public AssetIndex(string minecraftRootDir)
    {
        _minecraftRootDir = minecraftRootDir;
        _items = new List<DownloadURI>();
    }

    private void Load(string path)
    {
        var result = AssetIndexHelper.GetAssetIndex(path, _minecraftRootDir);

        _items.Clear();
        foreach (var item in result)
        {
            _items.Add(new DownloadURI(item.RemoteUri, item.LocalPath));
        }

    }

    public static AssetIndex Download(string assetIndexURI, string minecraftRootDir)
    {
        var assetIndex = new AssetIndex(minecraftRootDir);
        var localPath = Path.Combine(minecraftRootDir, FileName);
        var file = new DownloadFile(new DownloadURI(assetIndexURI, localPath));
        file.Download(isAsync: false, isContinue: false);

        assetIndex.Load(localPath);

        return assetIndex;
    }
}
