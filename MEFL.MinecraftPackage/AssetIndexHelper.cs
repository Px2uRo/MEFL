using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace MEFL.MinecraftPackage;
public static class AssetIndexHelper
{
    public static string CreateRemoteURI(string hash)
    {
        return $"http://resource.download.minecraft.net/{hash.Substring(0, 2)}/{hash}";
    }

    ///<summary>minecraftRootDir一般是指 .minecraft 文件夹。该方法用于在 资源索引文件中创建指定文件。</summary>
    public static string CreateLocalPath(string minecraftRootDir, string name)
    {
        return System.IO.Path.Combine(minecraftRootDir, "assets", name);
    }

    public static IEnumerable<DownloadURI> GetAssetIndex(string path, string minecraftRootDir)
    {
        using (var filestream = new FileStream(path, FileMode.Open))
        {
            return GetAssetIndex(filestream, minecraftRootDir);
        }

    }

    public static IEnumerable<DownloadURI> GetAssetIndex(Stream stream, string minecraftRootDir)
    {
        using (var jsonDoc = JsonDocument.Parse(stream))
        {
            return GetAssetIndex(jsonDoc, minecraftRootDir);
        }

    }

    private static IEnumerable<DownloadURI> GetAssetIndex(JsonDocument jsondoc, string minecraftRootDir)
    {
        var root = jsondoc.RootElement;
        var list = new List<DownloadURI>();

        var objs = root.GetProperty("objects");
        foreach (var obj in objs.EnumerateObject())
        {
            var hash = obj.Value.GetProperty("hash").GetString();
            var uri = new DownloadURI(CreateRemoteURI(hash), CreateLocalPath(minecraftRootDir, obj.Name));
            list.Add(uri);

        }

        return list;

    }

}
