using MEFL.Configs;
using MEFL.Contract;
using MEFL.PageModelViews;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
            get => _SelectedAccount;

            set
            {
                if (value == null)
                {
                    _SelectedAccount = null;
#if WPF
                    App.Current.Resources["WelcomeWords"] = App.Current.Resources["WelcomeWords_NoAccounts"] as String;
#endif
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
                    SelectedAccountUUID = value.Uuid.ToString();
                    _SelectedAccount = value;
#if WPF
                    App.Current.Resources["WelcomeWords"] = value.WelcomeWords;
#endif
                    uuid = string.Empty;
                }
                UserManageModel.ModelView.Invoke("SelectedAccount");
#if AVALONIA
                if(App.Current.Resources.ContainsKey("RMPMV"))
                {
                    (App.Current.Resources["RMPMV"] as RealMainPageModelView).Invoke("AcoountName");
                }
#endif
            }
        }

        internal static int SelectedFolderIndex
        {
            get => SettingConfig.FolderIndex;
            set { SettingConfig.FolderIndex = value; }
        }
        internal static Arguments.SettingArgs SettingArgs { get; set; }
        internal static Contract.GameInfoBase CurretGame { 
            get => SettingArgs.CurretGame; set { 
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
        internal static MEFLFolderColletion MyFolders 
        {
            get;
            set;
        }
        private static FileInfo[] tmp1;

        internal static bool AlwaysOpenNewAddIns { get => SettingConfig.AlwaysOpenNewExtensions; set { SettingConfig.AlwaysOpenNewExtensions = value; } }

        private static ObservableCollection<FileInfo> _SearchedJavas = new ObservableCollection<FileInfo>();
        private static Contract.MEFLDownloader _selectedDownloader;
        internal static Contract.MEFLDownloader SelectedDownloader { get => _selectedDownloader; set {
                if (value != null)
                {
                    RegManager.Write("Downloader", JsonConvert.SerializeObject(new DownloaderConfig(value.FileName, value.Name)));
                }
                _selectedDownloader = value;
            } 
        }
        private static int _maxMemory = 512;
        public static int MaxMemory { get
            { 
                return _maxMemory;
            } 
            set
            {
                _maxMemory = value;
                RegManager.Write("MaxMemory",_maxMemory.ToString());
            } 
        }

        public static IndexToUI IndexToUI { get; internal set; }

        public static DownloaderCollection Downloaders = new();
        public static DownloadSourceCollection DownloadSources = new();

#if WPF
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
                    Folders.Add(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)));
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
                var value = new List<String>();
                foreach (var item in Javas)
                {
                    value.Add(item.FullName);
                }
                var str = JsonConvert.SerializeObject(value);
                RegManager.Write("RecordedJavas", str,true);
                SearchJavaThreadIsOK = true;
            });
            t.Start();

            return _SearchedJavas;
        }
#elif AVALONIA
        internal static void SearchJavas()
        {
            _SearchedJavas = new ObservableCollection<FileInfo>();
            SearchJavaThreadIsOK = false;
                DirectoryInfo[] tmp2;
                var Folders = new ObservableCollection<DirectoryInfo>();
                try
                {
                    Folders.Add(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)));
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
                var value = new List<String>();
            Javas = _SearchedJavas;
            foreach (var item in Javas)
                {
                    value.Add(item.FullName);
                }
                var str = JsonConvert.SerializeObject(value);
                RegManager.Write("RecordedJavas", str, true);
            SettingPageModel.ModelView.EnableSearchJava= true;
                SearchJavaThreadIsOK = true;
        }
#endif
        static APIModel()
        {
            SettingArgs = new Arguments.SettingArgs();
            SettingConfig = MEFL.APIData.SettingConfig.Load();
            _SelectedAccountUUID = RegManager.Read("PlayerUuid");
#region RegFolders
            MyFolders = MEFLFolderColletion.GetReg();
#endregion
            AccountConfigs = new();
            SelectedFolderIndex = MEFLFolderColletion.GetRegIndex();
            GameInfoConfigs = MyFolders[SelectedFolderIndex].Games;
            AddInConfigs = MEFL.APIData.AddInConfig.GetAll();
            Hostings = Hosting.LoadAll();
#region Reg
#region Singed Up Accounts
#endregion
#region Javas
            Javas = new ObservableCollection<FileInfo>();
            try
            {
                var collection = JsonConvert.DeserializeObject<string[]>(RegManager.Read("RecordedJavas"));
                if (collection == null)
                {
                    collection = new string[0];
                    RegManager.Write("RecordedJavas",JsonConvert.SerializeObject(collection),true);
                }
                foreach (var item in collection)
                {
                    Javas.Add(new FileInfo(item));
                }
            }
            catch (Exception ex)
            {

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
            if (Directory.Exists(System.IO.Path.Combine(Environment.CurrentDirectory, ".minecraft"))!=true)
            {
                Directory.CreateDirectory(System.IO.Path.Combine(Environment.CurrentDirectory, ".minecraft"));
            }
            try
            {
                _maxMemory = Convert.ToInt32(RegManager.Read("MaxMemory"));
            }
            catch (Exception ex)
            {

            }
#if DEBUG

#else
            
#endif
        }
    }
}
