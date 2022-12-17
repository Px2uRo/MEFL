using MEFL.Arguments;
using MEFL.Contract;
using System;
using System.ComponentModel.Composition;

namespace OctaneForMEFL
{
    [Export(typeof(IBaseInfo))]
    public class BaseInfo : IBaseInfo
    {
        public object Title => "OctaneForMEFL";

        public object Icon => "OK";

        public Uri PulisherUri => new Uri("https://github.com/gregyjames/OctaneDownloader");

        public Uri ExtensionUri => new Uri("https://github.com/gregyjames/OctaneDownloader");

        public void SettingsChange(SettingArgs args)
        {

        }
    }

    [Export(typeof(IDownload))]
    public class Download : IDownload
    {
        public MEFLDownloader[] GetDownloaders(SettingArgs args)
        {
            return new[]{new OctaneDownloader()};
        }

        public DownloadSource[] GetDownloadSources(SettingArgs args)
        {
            throw new NotImplementedException();
        }

        public DownloadPageItemPair[] GetPairs(SettingArgs args)
        {
            return new DownloadPageItemPair[0];
        }
    }

    [Export(typeof(IPermissions))]
    public class Permissions : IPermissions
    {
        public bool UseSettingPageAPI => false;

        public bool UsePagesAPI => false;

        public bool UseGameManageAPI => false;

        public bool UseDownloadPageAPI => true;

        public bool UseAccountAPI => false;
    }
}