using Avalonia;
using MEFL.Arguments;
using MEFL.Contract;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel.Composition;

namespace ServerInstaller
{
    [Export(typeof(IBaseInfo))]
    public class BaseInfo : IBaseInfo
    {
        public object Title => "ServerInstaller";

        public object Icon => "Hello";

        public Uri PulisherUri => new Uri("");

        public Uri ExtensionUri => new("");

        public void SettingsChange(SettingArgs args)
        {

        }
    }

    [Export(typeof(IPermissions))]
    public class Permissions : IPermissions
    {
        public bool UseSettingPageAPI => false;

        public bool UsePagesAPI => false;

        public bool UseGameManageAPI => true;

        public bool UseDownloadPageAPI => true;

        public bool UseAccountAPI => false;
    }
    [Export(typeof(IDownload))]
    public class Download : IDownload
    {
        List<LauncherWebVersionContext> _contexts = new List<LauncherWebVersionContext>() {new WDCServer()};
        public LauncherWebVersionContext[] GetDataCotexts(LauncherWebVersionInfo baseInfo, FileInfo[] Javas, string dotMCPath)
        {
            return _contexts.ToArray();
        }

        public MEFLDownloader[] GetDownloaders(SettingArgs args)
        {
            return new MEFLDownloader[0];
        }

        public DownloadSource[] GetDownloadSources(SettingArgs args)
        {
            return new DownloadSource[0];
        }

        public DownloadPageItemPair[] GetPairs(SettingArgs args)
        {
            return new DownloadPageItemPair[0];
        }
    }

    [Export(typeof(IGameTypeManage))]
    public class Game : IGameTypeManage
    {
        public string[] SupportedType => new string[1] { "server"};


        public GameInfoBase Parse(string type, string JsonPath)
        {
            throw new NotImplementedException();
        }
    }
}