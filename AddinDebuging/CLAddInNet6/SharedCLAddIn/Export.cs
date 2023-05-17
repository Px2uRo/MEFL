using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using CLAddIn.Views;
using CoreLaunching.JsonTemplates;
using MEFL.Arguments;
using MEFL.CLAddIn;
using MEFL.CLAddIn.Downloaders;
using MEFL.CLAddIn.GameTypes;
using MEFL.CLAddIn.WebVersion;
using MEFL.Contract;
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
#if WPF
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using MEFL.Contract.Controls;
using MEFL.Controls;
using MEFL.CLAddIn.Pages;
#elif AVALONIA

#endif


namespace MEFL.CLAddIn.Export
{
    [Export(typeof(IBaseInfo))]
    public class BaseInfo : IBaseInfo
    {
        public object Title => "CoreLaunching fo MEFL";
#if WPF
        public object Icon { get {
                    var path = new System.Windows.Shapes.Path()
                    {
                        Width = 80,
                        Height = 80,
                        Fill = new SolidColorBrush(Colors.Aqua),
                        Data = new PathGeometry()
                        {
                            FillRule = FillRule.EvenOdd,
                            Figures = PathFigureCollection.Parse(
"M34.0609 32.2539 29.202 34.7632 29.2603 34.7951 34.0609 32.3328ZM19.7771 17.5252 22.5147 18.6349 22.5147 21.9929 19.734 23.4289 17.0683 21.9639 17.0683 18.5188ZM25.7469 13.6493 15.2005 17.5177 15.2005 30.9303 25.579 36.6342 29.202 34.7632 24.7112 32.312 24.7112 26.4383 29.3614 24.7442 34.0609 26.6361 34.0609 32.2539 36.4051 31.0433 36.4051 17.9695ZM10.7255 8.68032 8.52369 9.48796 8.52369 12.2882 10.6905 13.4791 12.9508 12.3118 12.9508 9.5823ZM0 0 14.5205 0 14.6624 1.40584C15.3138 4.58457 18.1306 6.97573 21.5068 6.97573 24.883 6.97573 27.6998 4.58457 28.3512 1.40584L28.4932 0 43.0136 0 43.0136 13.5243C46.8721 13.5243 50 16.6474 50 20.5 50 24.3526 46.8721 27.4757 43.0136 27.4757L43.0136 41 12.1257 41C5.42887 41 0 35.5794 0 28.8927Z"
                    )
                        }
                    };
                return path;
            } 
        }
#elif AVALONIA
        public object Icon
        {
            get
            {
                var path = new Avalonia.Controls.Shapes.Path()
                {
                    Width = 200,
                    Height = 200,
                    Fill = new SolidColorBrush(Colors.Aqua),
                    Data = new PathGeometry()
                    {
                        FillRule = FillRule.EvenOdd,
                        Figures = PathFigures.Parse(
"M34.0609 32.2539 29.202 34.7632 29.2603 34.7951 34.0609 32.3328ZM19.7771 17.5252 22.5147 18.6349 22.5147 21.9929 19.734 23.4289 17.0683 21.9639 17.0683 18.5188ZM25.7469 13.6493 15.2005 17.5177 15.2005 30.9303 25.579 36.6342 29.202 34.7632 24.7112 32.312 24.7112 26.4383 29.3614 24.7442 34.0609 26.6361 34.0609 32.2539 36.4051 31.0433 36.4051 17.9695ZM10.7255 8.68032 8.52369 9.48796 8.52369 12.2882 10.6905 13.4791 12.9508 12.3118 12.9508 9.5823ZM0 0 14.5205 0 14.6624 1.40584C15.3138 4.58457 18.1306 6.97573 21.5068 6.97573 24.883 6.97573 27.6998 4.58457 28.3512 1.40584L28.4932 0 43.0136 0 43.0136 13.5243C46.8721 13.5243 50 16.6474 50 20.5 50 24.3526 46.8721 27.4757 43.0136 27.4757L43.0136 41 12.1257 41C5.42887 41 0 35.5794 0 28.8927Z"
                )
                    }
                };
                return path;
            }
        }
#endif


        public Uri PulisherUri => new Uri("https://space.bilibili.com/283605961");

        public Uri ExtensionUri => new("https://space.bilibili.com/283605961");

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
            return new MEFLDownloader[] {new PinKcatDownloader()};
        }

        public DownloadSource[] GetDownloadSources(SettingArgs args)
        {
            var lst = new List<CLDownloadSource>() {
                new BMCLAPI_Version_Api_Source(),
                new MCBBS_Version_Api_Source(),
                new() { ELItem="${version_manifest}",RuleSourceName = "BMCLAPI",Uri= "https://bmclapi2.bangbang93.com/mc/game/version_manifest.json" },
                new() { ELItem = "${AssIndex}", RuleSourceName = "BMCLAPI", Uri = "https://bmclapi2.bangbang93.com" },
                new() { ELItem = "${assets}", RuleSourceName = "BMCLAPI", Uri = "https://bmclapi2.bangbang93.com/assets" },
                new() { ELItem = "${libraries}", RuleSourceName = "BMCLAPI", Uri = "https://bmclapi2.bangbang93.com/maven/" },
                new() { ELItem="${version_manifest}",RuleSourceName = "MCBBS",Uri= "https://download.mcbbs.net/" },
                new() { ELItem = "${AssIndex}", RuleSourceName = "MCBBS", Uri = "https://download.mcbbs.net/" },
                new() { ELItem = "${assets}", RuleSourceName = "MCBBS", Uri = "https://download.mcbbs.net/assets" },
                new() { ELItem = "${libraries}", RuleSourceName = "MCBBS", Uri = "https://download.mcbbs.net/maven/" },
                new() { ELItem="${version_manifest}",RuleSourceName = "Mojang",Uri= "http://launchermeta.mojang.com/mc/game/version_manifest.json" },
                new() { ELItem = "${AssIndex}", RuleSourceName = "Mojang", Uri = "https://launcher.mojang.com/" },
                new() { ELItem = "${assets}", RuleSourceName = "Mojang", Uri = "http://resources.download.minecraft.net" },
                new() { ELItem = "${libraries}", RuleSourceName = "Mojang", Uri = "https://libraries.minecraft.net/" }};
            return lst.ToArray();
        }

        public DownloadPageItemPair[] GetPairs(SettingArgs args)
        {
            DownloadPageItemPair RealsePair = new RealsePair("Release", realret, "realse");
            DownloadPageItemPair[] ret = new DownloadPageItemPair[] { RealsePair};
            foreach (var pair in ret)
            {
                pair.WebRefreshEvent += Pair_WebRefreshEvent;
                pair.ListRefreshEvent += Pair_ListRefreshEvent;
            }
            return ret;
        }

        private void Pair_WebRefreshEvent(object sender, string tmpFolderPath)
        {
            var pair = sender as DownloadPageItemPair;
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
                pair.HasError = true;
                pair.ErrorDescription = ex.Message;
            }
        }

        private void Pair_ListRefreshEvent(object sender, string tmpFolderPath)
        {
            var pair = (sender as DownloadPageItemPair);
            pair.Contents = Refresh(pair,tmpFolderPath);
            pair.RefreshCompete();
        }
        List<LauncherWebVersionInfoList> realret = new() { new("Other")};
        string ResponString;
        JObject jOb;
        private List<LauncherWebVersionInfoList> Refresh(DownloadPageItemPair pair,string tmpFolderPath)
        {
            while (string.IsNullOrEmpty(ResponString))
            {
                if (pair.HasError)
                {
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
                                var list = realret.Where(a => a.Title == Tag).ToList();
                                if (list.Count == 0)
                                {
                                    var nc = new LauncherWebVersionInfoList(Tag);
                                    nc.Add(new GenericWebVersion() { Id = item["id"].ToString(), Type = item["type"].ToString(), Url = item["url"].ToString(), ReleaseTime = item["releaseTime"].ToString() });
                                    realret.Add(nc);
                                }
                                else
                                {
                                    list[0].Add(new GenericWebVersion() { Id = item["id"].ToString(), Type = item["type"].ToString(), Url = item["url"].ToString(), ReleaseTime = item["releaseTime"].ToString() });
                                }
                            }
                            else
                            {
                                realret[0].Add(new GenericWebVersion() { Id = item["id"].ToString(), Type = item["type"].ToString(), Url = item["url"].ToString(), ReleaseTime = item["releaseTime"].ToString() });
                            }
                        }
                    }
                }

                var first = realret[0];
                if (first.Title == "Other")
                {
                    realret.RemoveAt(0);
                    if (first.Count != 0)
                    {
                        realret.Add(first);
                    }
                }
            }
            return realret;
        }

    }

    [Export(typeof(ILuncherGameType))]

    public class Games : ILuncherGameType
    {
        public string[] SupportedType => new[] { "release" , "snapshot" };

        public GameInfoBase Parse(string type ,string JsonPath)
        {
            try
            {
                if (type == "release")
                {
                    var _Root = JsonConvert.DeserializeObject<Root>(System.IO.File.ReadAllText(JsonPath));
                    if (_Root.MainClass== "cpw.mods.bootstraplauncher.BootstrapLauncher")
                    {
                        return new CLGameType(JsonPath, true);
                    }
                    else
                    {
                        return new CLGameType(JsonPath, false);
                    }
                }
                else
                {
                    return new CLGameType(JsonPath, false);
                }
            }
            catch (Exception ex)
            {
                return new MEFLErrorType(ex.Message, JsonPath);
            }
        }
    }


    [Export(typeof(IAccount))]
    public class Account : IAccount
    {
        AccountBase[] IAccount.GetSingUpAccounts(SettingArgs args)
        {
            return AccountsManagement.Model.List.ToArray();
        }

#if WPF
        static AddAccountItem Legacy = new AddAccountItem()
        {
            Width = 400,
            Height = 60,
            AddAccountContent = new AddALegacyAccountPage(),
            Content = new TextBlock() { Text = "离线账户", HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, FontSize = 30, FontWeight = FontWeight.FromOpenTypeWeight(999) },
        };

        static AddAccountItem msa = new AddAccountItem()
        {
            Width = 400,
            Height = 60,
            AddAccountContent = new AddNewMSAccount(),
            Content = new TextBlock() { Text = "微软登录", HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, FontSize = 30, FontWeight = FontWeight.FromOpenTypeWeight(999) },
        };
        AddAccountItem[] IAccount.GetSingUpPage(SettingArgs args)
        {
            msa.MouseDown -= Msa_Ini;
            msa.MouseDown += Msa_Ini;
            var res = new List<AddAccountItem>{Legacy,msa};
            return res.ToArray();
        }

        private void Msa_Ini(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ((sender as AddAccountItem).AddAccountContent as AddNewMSAccount).ResetWebb();
        }
#elif AVALONIA

        static AddLegacyAccountPage legaPage = new();

        static AddMSAccountPage mspage = new();

        static AddUnitedPassportPage auppage = new();

        static AddAccountItem Legacy = new AddAccountItem(
            new TextBlock() { Margin = new(5), Text = "离线登录", HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, FontSize = 20, FontWeight = (FontWeight)999 },
            legaPage);

        static AddAccountItem Msa = new AddAccountItem(
            new TextBlock() { Margin=new(5), Text = "微软账号", HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, FontSize = 20, FontWeight = (FontWeight)999 },
            mspage); 
        static AddAccountItem Aup = new AddAccountItem(
            new TextBlock() { Margin = new(5), Text = "统一通行证", HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, FontSize = 20, FontWeight = (FontWeight)999 },
            auppage);
        public AddAccountItem[] GetSingUpPage(SettingArgs args)
        {
            legaPage.Refresh();
            mspage.Refresh();
            auppage.Refresh();
            return new AddAccountItem[3] { Legacy, Msa,Aup};
        }
#endif
        //private AddAccountItem Legacy = new AddAccountItem() { Width=400,Height=60, AddAccountContent = new AddALegacyAccountPage(),FinnalReturn=new MEFLLegacyAccount(String.Empty,Guid.NewGuid().ToString()) };
        //todo i18n thx
        //Legacy.Content= new TextBlock() { Text="离线账户",HorizontalAlignment=HorizontalAlignment.Center,VerticalAlignment=VerticalAlignment.Center,FontSize=30,FontWeight=FontWeight.FromOpenTypeWeight(999)};
        //Legacy.MouseDown += Item_MouseDown;
        //MyStackPanel.Children.Add(Legacy);

    }
}
