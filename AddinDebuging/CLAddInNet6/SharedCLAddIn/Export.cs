using MEFL.Arguments;
using MEFL.CLAddIn;
using MEFL.CLAddIn.Pages;
using MEFL.CLAddIn.WebVersion;
using MEFL.Contract;
using MEFL.Contract.Controls;
using MEFL.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace MEFL.CLAddIn.Export
{
    [Export(typeof(IBaseInfo))]
    public class BaseInfo : IBaseInfo
    {
        public object Title => "CoreLaunching fo MEFL";

        public object Icon => "MEFL.jpg";

        public Uri PulisherUri => new Uri("https://space.bilibili.com/283605961");

        public Uri ExtensionUri => throw new NotImplementedException("https://space.bilibili.com/283605961");

        public void SettingsChange(SettingArgs args)
        {
            //throw new NotImplementedException();
        }
    }

    [Export(typeof(IPermissions))]
    public class Permissions : IPermissions
    {
        public bool UseSettingPageAPI =>false;

        public bool UsePagesAPI => false;

        public bool UseGameManageAPI => true;

        public bool UseDownloadPageAPI => true;

        public bool UseAccountAPI => true;
    }

    [Export(typeof(IDownload))]
    public class Download : IDownload
    {
        static string website = "https://launchermeta.mojang.com/mc/game/version_manifest.json";
        static WebRequest req;
        public MEFLDownloader[] GetDownloaders(SettingArgs args)
        {
            throw new NotImplementedException();
        }

        public DownloadSource[] GetDownloadSources(SettingArgs args)
        {
            throw new NotImplementedException();
        }

        public DownloadPageItemPair[] GetPairs(SettingArgs args)
        {
            DownloadPageItemPair RealsePair = new("realse", realret, "realse");
            DownloadPageItemPair SnapsortPair = new("snapsort", snapret, "snapsort");
            DownloadPageItemPair[] ret = new DownloadPageItemPair[] { RealsePair,SnapsortPair};
            foreach (var pair in ret)
            {
                pair.ListRefreshEvent += Pair_ListRefreshEvent;
                pair.WebRefreshEvent += Pair_WebRefreshEvent;
            }
            return ret;
        }

        private void Pair_WebRefreshEvent(object sender, string tmpFolderPath)
        {
            var pair = sender as DownloadPageItemPair;
            pair.IsRefreshing = true;
            try
            {
                req = HttpWebRequest.Create(website);
                req.Method = "GET";
                using (WebResponse wr = req.GetResponse())
                {
                    var strm1 = wr.GetResponseStream();
                    var strm2 = new StreamReader(strm1);
                    ResponString = strm2.ReadToEnd();
                    strm1.Close();
                    strm1.Dispose();
                    strm2.Close();
                    strm2.Dispose();
                    jOb = JObject.Parse(ResponString);
                }
                req.Abort();
            }
            catch (Exception ex)
            {
                pair.IsRefreshing = false;
                pair.HasError = true;
                pair.ErrorDescription = ex.Message;
            }
        }

        private void Pair_ListRefreshEvent(object sender, string tmpFolderPath)
        {
            var pair = (sender as DownloadPageItemPair);
            pair.IsRefreshing = true;
            pair.Contents = Refresh(pair,tmpFolderPath);
            pair.IsRefreshing =false;
        }
        List<LauncherWebVersionInfoList> realret = new() { new("Other")};
        List<LauncherWebVersionInfoList> snapret = new() { new("Other") };
        string ResponString;
        JObject jOb;
        private List<LauncherWebVersionInfoList> Refresh(DownloadPageItemPair pair,string tmpFolderPath)
        {
            while (string.IsNullOrEmpty(ResponString))
            {
                if (pair.HasError)
                {
                    pair.IsRefreshing = false;
                    return new();
                }
            }
            if (pair.Tag == "realse")
            {
                if (realret.Count <= 1)
                {
                    foreach (var item in jOb["versions"])
                    {
                        if (item["type"].ToString() == "release")
                        {
                            if (Version.TryParse(item["id"].ToString(), out var version))
                            {
                                var Tag = $"{version.Major.ToString()}.{version.Minor.ToString()}";
                                var list = realret.Where(a => a.VersionMajor == Tag).ToList();
                                if (list.Count == 0)
                                {
                                    var nc = new LauncherWebVersionInfoList(Tag);
                                    nc.Add(new Generic() { Id = item["id"].ToString(), Type = item["type"].ToString(), Url = item["url"].ToString(), ReleaseTime = item["releaseTime"].ToString() });
                                    realret.Add(nc);
                                }
                                else
                                {
                                    list[0].Add(new Generic() { Id = item["id"].ToString(), Type = item["type"].ToString(), Url = item["url"].ToString(), ReleaseTime = item["releaseTime"].ToString() });
                                }
                            }
                            else
                            {
                                realret[0].Add(new Generic() { Id = item["id"].ToString(), Type = item["type"].ToString(), Url = item["url"].ToString(), ReleaseTime = item["releaseTime"].ToString() });
                            }
                        }
                    }
                }
                return realret;
            }
            else
            {
                if (snapret.Count <= 1)
                {
                    foreach (var item in jOb["versions"])
                    {
                        if (item["type"].ToString() == "snapshot")
                        {
                            if (Version.TryParse(item["id"].ToString(), out var version))
                            {
                                var Tag = $"{version.Major.ToString()}.{version.Minor.ToString()}";
                                var list = snapret.Where(a => a.VersionMajor == Tag).ToList();
                                if (list.Count == 0)
                                {
                                    var nc = new LauncherWebVersionInfoList(Tag);
                                    nc.Add(new Generic() { Id = item["id"].ToString(), Type = item["type"].ToString(), Url = item["url"].ToString(), ReleaseTime = item["releaseTime"].ToString() });
                                    snapret.Add(nc);
                                }
                                else
                                {
                                    list[0].Add(new Generic() { Id = item["id"].ToString(), Type = item["type"].ToString(), Url = item["url"].ToString(), ReleaseTime = item["releaseTime"].ToString() });
                                }
                            }
                            else
                            {
                                snapret[0].Add(new Generic() { Id = item["id"].ToString(), Type = item["type"].ToString(), Url = item["url"].ToString(), ReleaseTime = item["releaseTime"].ToString() });
                            }
                        }
                    }
                }
                return snapret;
            }

        }

    }

    [Export(typeof(ILuncherGameType))]

    public class Games : ILuncherGameType
    {
        public string[] SupportedType { get => new[] { "release" } ;set { } }

        public GameInfoBase Parse(string type ,string JsonPath)
        {
            if (type == "release")
            {
                return new GameTypes.MEFLRealseType(JsonPath);
            }
            else return null;
        }
    }


    [Export(typeof(IAccount))]
    public class Account : IAccount
    {
        AccountBase[] IAccount.GetSingUpAccounts(SettingArgs args)
        {
            var ret = new List<AccountBase>();
            try
            {
                var reg = RegManager.Read("LegacyAccounts");
                if (!string.IsNullOrEmpty(reg))
                {
                    var jOb = JToken.Parse(reg);
                    var List = new List<MEFLLegacyAccount>();
                    foreach (var item in jOb)
                    {
                        List.Add(new(item["UserName"].ToString(), item["Uuid"].ToString()));
                    }
                    foreach (var item in List)
                    {
                        ret.Add(item);
                    }
                }
                else
                {
                    RegManager.Write("LegacyAccounts", "[]");
                }
            }
            catch (Exception ex)
            {
                ret = new List<AccountBase>();
                //todo HandleException
            }
            return ret.ToArray();
        }

        AddAccountItem[] IAccount.GetSingUpPage(SettingArgs args)
        {
            var res = new List<AddAccountItem>();
            var Legacy = new AddAccountItem()
            {
                Width = 400,
                Height = 60,
                AddAccountContent = new AddALegacyAccountPage(),
                Content = new TextBlock() { Text = "离线账户", HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, FontSize = 30, FontWeight = FontWeight.FromOpenTypeWeight(999) },
                FinnalReturn = new MEFLLegacyAccount(String.Empty, Guid.NewGuid().ToString())
            };
            res.Add(Legacy);
            return res.ToArray();
        }

        //private AddAccountItem Legacy = new AddAccountItem() { Width=400,Height=60, AddAccountContent = new AddALegacyAccountPage(),FinnalReturn=new MEFLLegacyAccount(String.Empty,Guid.NewGuid().ToString()) };
        //todo i18n thx
        //Legacy.Content= new TextBlock() { Text="离线账户",HorizontalAlignment=HorizontalAlignment.Center,VerticalAlignment=VerticalAlignment.Center,FontSize=30,FontWeight=FontWeight.FromOpenTypeWeight(999)};
        //Legacy.MouseDown += Item_MouseDown;
        //MyStackPanel.Children.Add(Legacy);

    }
}
