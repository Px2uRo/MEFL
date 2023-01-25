using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MEFL.MinecraftPackage;

namespace MEFL.Core.Web.Minecraft;

public class PackageManifest
{
    private const string FileName = "manifest.json";
    private string _minecraftRootDir;

    public string AssetIndexUri
    {
        get; private set;
    }

    public IEnumerable<DownloadURI> Items => _items;
    private List<DownloadURI> _items;

    private PackageManifest(string minecraftRootDir)
    {
        _minecraftRootDir = minecraftRootDir;
        _items = new List<DownloadURI>();
    }

    public void Load(DownloadFile file)
    {
        var package = file.ToObject<PackageRoot>();
        AssetIndexUri = package.assetIndex.url;
        var clientURI = GetClientURI(package);
        _items.Add(clientURI);

        foreach (var downloadItem in package.libraries)
        {
            var isAllowed = DownloadItemFilter(downloadItem);
            if (isAllowed)
            {
                var uri = GetDownloadURI(downloadItem);
                if (uri != null)
                    _items.Add(uri);
            }

        }
    }

    private DownloadURI GetClientURI(PackageRoot package)
    {
        var localPath = PackageHelper.GetDownloadsLocalPath(_minecraftRootDir, package.id, $"{package.id}.jar");
        var remoteURI = package.downloads.client.url;
        return new DownloadURI(remoteURI, localPath);
    }

    private DownloadURI GetDownloadURI(Library downloadItem)
    {
        if (downloadItem.downloads == null)
            return null;
        if (downloadItem.downloads.artifact == null)
            return null;
        if (downloadItem.downloads.artifact.url == null)
            return null;
        if (downloadItem.downloads.artifact.path == null)
            return null;
        var uri = new DownloadURI(downloadItem.downloads.artifact.url, Path.Combine(_minecraftRootDir, downloadItem.downloads.artifact.path));
        return uri;
    }

    private bool DownloadItemFilter(Library downloadItem)
    {
        if (downloadItem.rules == null)
            return true;
        if (downloadItem.rules.Length == 0)
            return true;

        var hasWindowsOS = false;
        foreach (var rule in downloadItem.rules)
        {
            if (rule.os == null)
                continue;
            if (rule.os.name == null)
                continue;
            if (rule.os.name.Contains("windows") == false)
                continue;
            if (rule.action == null)
                continue;

            hasWindowsOS = rule.action == "allow";
        }

        return hasWindowsOS;
    }


    public static PackageManifest Download(string manifestUri, string minecraftRootDir)
    {
        var manifest = new PackageManifest(minecraftRootDir);
        var localPath = Path.Combine(minecraftRootDir, FileName);
        var file = new DownloadFile(new DownloadURI(manifestUri, localPath));
        file.Download(isAsync: false, isContinue: false);

        manifest.Load(file);

        return manifest;
    }
}
