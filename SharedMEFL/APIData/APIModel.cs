using CoreLaunching.JsonTemplates;
using MEFL.Contract;
using MEFL.PageModelViews;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace MEFL.APIData
{
    internal static class APIModel
    {
        internal static ObservableCollection<Hosting> Hostings { get; set; }
        internal static bool SearchJavaThreadIsOK;
        internal static ObservableCollection<FileInfo> Javas { get; set; }
        internal static SettingConfig SettingConfig { get; set; }
        internal static ObservableCollection<AddInConfig> AddInConfigs { get; set; }

        internal static AccountCollection AccountConfigs;
        private static AccountBase _SelectedAccount { get; set; }
        private static string _SelectedAccountUUID;

        internal static string SelectedAccountUUID
        {
            get => _SelectedAccountUUID; set { 
                _SelectedAccountUUID = value;
                RegManager.Write("PlayerUuid",value);
            }
        }
        internal static AccountBase SelectedAccount
        {
            get {
                return _SelectedAccount;
            }
            set {
                if (value == null)
                {
                    _SelectedAccount = null;
                    App.Current.Resources["WelcomeWords"] = App.Current.Resources["WelcomeWords_NoAccounts"] as String;
                }
                else
                {
                    string uuid = value.Uuid.ToString();
                    for (int i = 0; i < AccountConfigs.Count; i++)
                    {
                        if (AccountConfigs[i].Selected)
                        {
                            AccountConfigs[i].Selected = false;
                        }
                    }
                    value.Selected = true;
                    _SelectedAccount = value;
                    SelectedAccountUUID = value.Uuid.ToString();
                    App.Current.Resources["WelcomeWords"] = value.WelcomeWords;
                    uuid = string.Empty;
                }
                UserManageModel.ModelView.Invoke("SelectedAccount");
            }
        }

        private static int _SelectedFolderIndex { get; set; }

        internal static int SelectedFolderIndex
        {
            get { return _SelectedFolderIndex; }
            set
            {       _SelectedFolderIndex = value;
                    RegManager.Write("SelectedFolderIndex", value.ToString(),true);
            }
        }
        internal static Arguments.SettingArgs SettingArgs { get; set; }
        internal static Contract.GameInfoBase CurretGame { get => SettingArgs.CurretGame; set { 
                if(value == null)
                {
                    SettingConfig.SelectedGame = string.Empty;
                    SettingArgs.CurretGame = null;
                }
                else
                {
                    SettingConfig.SelectedGame = value.RootFolder;
                    SettingArgs.CurretGame = value;
                }
                (App.Current.Resources["RMPMV"] as RealMainPageModelView).Invoke("CurretGame");
            } 
        }
        internal static GameInfoCollection GameInfoConfigs { get; set; }
        internal static ObservableCollection<MEFLFolderInfo> MyFolders 
        {
            get;set;
        }
        private static FileInfo[] tmp1;

        internal static bool AlwaysOpenNewAddIns { get => SettingConfig.AlwaysOpenNewExtensions; set { SettingConfig.AlwaysOpenNewExtensions = value; } }

        private static ObservableCollection<FileInfo> _SearchedJavas = new ObservableCollection<FileInfo>();
        internal static Contract.MEFLDownloader SelectedDownloader;
        public static DownloaderCollection Downloaders = new();
        public static DownloadSourceCollection DownloadSources = new();

        internal static ObservableCollection<FileInfo> SearchJavas()
        {
            _SearchedJavas = new ObservableCollection<FileInfo>();
            SearchJavaThreadIsOK = false;
            Thread t = new Thread(() => 
            {
                DirectoryInfo[] tmp2;
                var Folders = new ObservableCollection<DirectoryInfo>();
                try
                {
                    Folders.Add(new DirectoryInfo("C:\\Program Files"));
                    for (int i = 0; i < Folders.Count; i++)
                    {
                        try
                        {
                            tmp2 = Folders[i].GetDirectories();
                            foreach (var item in tmp2)
                            {
                                Folders.Add(item);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                catch (Exception ex)
                {

                }
                for (int i = 0; i < Folders.Count; i++)
                {
                    try
                    {
                        tmp1 = Folders[i].GetFiles();
                        foreach (var item in tmp1)
                        {
                            if (item.Name == "javaw.exe")
                            {
                                _SearchedJavas.Add(item);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                Folders.Clear();
                Folders = null;
                tmp1 = null;
                tmp2 = null;
                SearchJavaThreadIsOK = true;
            });
            t.Start();

            return _SearchedJavas;
        }
        static APIModel()
        {
            #region RegFolders
            try
            {
                var regKey = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<MEFLFolderInfo>>(RegManager.Read("Folders"));
                if (Directory.Exists(System.IO.Path.Combine(Environment.CurrentDirectory, ".minecraft")) == false)
                {
                    Directory.CreateDirectory(System.IO.Path.Combine(Environment.CurrentDirectory, ".minecraft"));
                }
                if (regKey.Count == 0)
                {
                    regKey.Add(new MEFLFolderInfo(System.IO.Path.Combine(Environment.CurrentDirectory, ".minecraft"), "本地文件夹"));
                }
                else if (regKey[0].Path != System.IO.Path.Combine(Environment.CurrentDirectory, ".minecraft"))
                {
                    regKey.Add(new MEFLFolderInfo(System.IO.Path.Combine(Environment.CurrentDirectory, ".minecraft"), "本地文件夹"));
                    SelectedFolderIndex = 0;
                }
                RegManager.Write("Folders", JsonConvert.SerializeObject(regKey), true);
                MyFolders = regKey;
                try
                {
                    SelectedFolderIndex = Convert.ToInt32(RegManager.Read("SelectedFolderIndex"));
                    GameInfoConfigs = MyFolders[SelectedFolderIndex].Games;
                }
                catch (Exception ex)
                {
                    MyFolders = new ObservableCollection<MEFLFolderInfo>();
                    MyFolders.Add(new MEFLFolderInfo(System.IO.Path.Combine(Environment.CurrentDirectory, ".minecraft"), "本地文件夹"));
                    SelectedFolderIndex = 0;
                    GameInfoConfigs = MyFolders[SelectedFolderIndex].Games;
                }
            }
            catch (Exception ex)
            {
                MyFolders = new ObservableCollection<MEFLFolderInfo>();
                MyFolders.Add(new MEFLFolderInfo(System.IO.Path.Combine(Environment.CurrentDirectory, ".minecraft"), "本地文件夹"));
                GameInfoConfigs = MyFolders[SelectedFolderIndex].Games;
                Debugger.Logger(ex.Message);
                RegManager.Write("Folders", JsonConvert.SerializeObject(new ObservableCollection<MEFLFolderInfo>() { new MEFLFolderInfo(System.IO.Path.Combine(Environment.CurrentDirectory, ".minecraft"), "本地文件夹") }), true);
            }
            #endregion
            AccountConfigs = new();
            SettingConfig = MEFL.APIData.SettingConfig.Load();
            AddInConfigs = MEFL.APIData.AddInConfig.GetAll();
            Hostings = Hosting.LoadAll();
            SettingArgs = new Arguments.SettingArgs();
            GameInfoConfigs = new GameInfoCollection();
            #region Reg
            #region RegSelectedFolderIndex
            try
            {
                SelectedFolderIndex = Convert.ToUInt16(RegManager.Read("SelectedFolderIndex"));
            }
            catch (Exception ex)
            {
                Debugger.Logger(ex.Message);
                RegManager.Write("SelectedFolderIndex", 0.ToString());
            }
            #endregion
            #region GameConfigs
            ObservableCollection<MEFLFolderInfo> res = new ObservableCollection<MEFLFolderInfo>() { { new MEFLFolderInfo(System.IO.Path.Combine(Environment.CurrentDirectory, ".minecraft"), "本地文件夹") } };
            foreach (var item in APIModel.MyFolders)
            {
                res.Add(item);
            }
            #endregion
            #region Singed Up Accounts
            #endregion
            #region Javas
            Javas = new ObservableCollection<FileInfo>();
            try
            {
                foreach (var item in JsonConvert.DeserializeObject<string[]>(RegManager.Read("RecordedJavas")))
                {
                    Javas.Add(new FileInfo(item));
                }
            }
            catch (Exception ex)
            {
                Javas = SearchJavas();
                var value = new List<String>();
                foreach (var item in Javas)
                {
                    value.Add(item.FullName);
                }
                var str = JsonConvert.SerializeObject(value);
                RegManager.Write("RecordedJavas", str,true);
            }
            try
            {
                SettingArgs.SelectedJava = new FileInfo(RegManager.Read("SelectedJava"));
            }
            catch (Exception ex)
            {
                if (Javas.Count > 0)
                {
                    SettingArgs.SelectedJava = Javas[0];
                    RegManager.Write("SelectedJava", SettingArgs.SelectedJava.FullName, true);
                }
            }
            #endregion
            #endregion
            _SelectedAccountUUID = RegManager.Read("PlayerUuid");
            if (Directory.Exists(System.IO.Path.Combine(Environment.CurrentDirectory, ".minecraft"))!=true)
            {
                Directory.CreateDirectory(System.IO.Path.Combine(Environment.CurrentDirectory, ".minecraft"));
            }
#if DEBUG

#else
            
#endif
        }
    }
}
