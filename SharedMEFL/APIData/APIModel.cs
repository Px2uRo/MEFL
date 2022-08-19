using MEFL.Contract;
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
    public static class APIModel
    {
        public static ObservableCollection<Hosting> Hostings { get; set; }
        public static bool SearchJavaThreadIsOK;
        public static ObservableCollection<FileInfo> Javas { get; set; }
        public static SettingConfig SettingConfig { get; set; }
        public static ObservableCollection<AddInConfig> AddInConfigs { get; set; }

        public static ObservableCollection<AccountBase> AccountConfigs;
        private static AccountBase _SelectedAccount { get; set; }

        public static AccountBase SelectedAccount
        {
            get {
                //todo return null
                return null;
            }
            set {
                if (value == null)
                {
                    _SelectedAccount = null;
                    App.Current.Resources["WelcomeWords"] = App.Current.Resources["WelcomeWords_NoAccounts"] as String;
                }
                else
                {
                    _SelectedAccount = value;
                    RegManager.Write("PlayerUuid", value.Uuid);
                    App.Current.Resources["WelcomeWords"] = value.WelcomeWords;
                }
            }
        }

        private static int _SelectedFolderIndex { get; set; }

        public static int SelectedFolderIndex
        {
            get { return _SelectedFolderIndex; }
            set
            {
                if (0<=value&&value< MyFolders.Count+1)
                {
                    _SelectedFolderIndex = value;
                    RegManager.Write("SelectedFolderIndex", value.ToString());
                }
            }
        }
        public static Arguments.SettingArgs SettingArgs { get; set; }
        public static Contract.GameInfoBase CurretGame { get => SettingArgs.CurretGame; set { SettingConfig.SelectedGame = value.RootFolder; SettingArgs.CurretGame = value; } }
        public static ObservableCollection<Contract.GameInfoBase> GameInfoConfigs { get; set; }
        public static ObservableCollection<MEFLFolderInfo> MyFolders 
        {
            get;set;
        }
        private static ObservableCollection<AccountBase> _LegacyAccounts;
        private static FileInfo[] tmp1;

        public static ObservableCollection<AccountBase> LegacyAccounts { get => _LegacyAccounts; set { _LegacyAccounts = value;}
        }

        public static bool AlwaysOpenNewAddIns { get => SettingConfig.AlwaysOpenNewExtensions; set { SettingConfig.AlwaysOpenNewExtensions = value; } }

        private static ObservableCollection<FileInfo> _SearchedJavas = new ObservableCollection<FileInfo>();

        public static ObservableCollection<FileInfo> SearchJavas()
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
            AccountConfigs = new ObservableCollection<AccountBase>();
            SettingConfig = MEFL.APIData.SettingConfig.Load();
            AddInConfigs = MEFL.APIData.AddInConfig.GetAll();
            Hostings = Hosting.LoadAll();
            SettingArgs = new Arguments.SettingArgs();
            #region Reg
            #region RegFolders
            MyFolders = new ObservableCollection<MEFLFolderInfo>();
            try
            {
                foreach (var item in Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<MEFLFolderInfo>>(RegManager.Read("Folders")))
                {
                    MyFolders.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debugger.Logger(ex.Message);
                RegManager.Write("Folders", "[]",true);
            }
            #endregion
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
            GameInfoConfigs = res[SelectedFolderIndex].Games;
            #endregion
            #region CurretGame
            try
            {
                foreach (var item in GameInfoConfigs)
                {
                    if (item.RootFolder == SettingConfig.SelectedGame)
                    {
                        CurretGame = item;
                    }
                }
            }
            catch (Exception ex)
            {
                Debugger.Logger(ex.Message);
                RegManager.Write("CurretGame", String.Empty);
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
