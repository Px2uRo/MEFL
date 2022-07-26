using MEFL.Contract;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MEFL.APIData
{
    public static class APIModel
    {
        public static SettingConfig SettingConfig { get; set; }
        public static List<AddInConfig> AddInConfigs { get; set; }
        public static void RemoveAddInsTheSameAddIn()
        {
            for (int i = 0; i < AddInConfigs.Count; i++)
            {
                if(i != 0)
                {
                    if (AddInConfigs[i].Guid == AddInConfigs[(i - 1)].Guid)
                    {
                        AddInConfigs.RemoveAt(i);
                        i--;
                    }
                }
            }
            AddInConfig.Update(AddInConfigs);
        }

        public static List<AccountBase> AccountConfigs;
        private static int _SelectedAccountIndex { get; set; }

        public static int SelectedAccountIndex
        {
            get { return _SelectedAccountIndex; }
            set { 
                if(value < AccountConfigs.Count)
                {
                    _SelectedAccountIndex = value;
                    App.Current.Resources["WelcomeWords"] = AccountConfigs[value].WelcomeWords;
                }
            }
        }

        private static int _SelectedFolderIndex { get; set; }

        public static int SelectedFolderIndex
        {
            get { return _SelectedFolderIndex; }
            set
            {
                if (value < MyFolders.Count+1)
                {
                    _SelectedFolderIndex = value;
                    RegManager.Write("SelectedFolderIndex", value.ToString());
                }
            }
        }

        private static MEFLFolderInfo _SelectedForder;

        public static MEFLFolderInfo SelectedForder
        {
            get { return _SelectedForder; }
            set { _SelectedForder = value; }
        }


        public static Arguments.SettingArgs SettingArgs { get; set; }
        public static Contract.GameInfoBase CurretGame { get => SettingArgs.CurretGame; set => SettingArgs.CurretGame = value; }
        public static ObservableCollection<Contract.GameInfoBase> GameInfoConfigs { get; set; }
        public static ObservableCollection<MEFLFolderInfo> MyFolders 
        {
            get;set;
        }

        static APIModel()
        {
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
                RegManager.Write("Folders", "[]");
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
            #region RegCurretGame
            try
            {

                foreach (var item in GameInfoConfigs)
                {
                    if (item.RootFolder == RegManager.Read("CurretGame"))
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
            #endregion
            SettingConfig = MEFL.APIData.SettingConfig.Load();
            AddInConfigs = MEFL.APIData.AddInConfig.GetAll();
            RemoveAddInsTheSameAddIn();
            AccountConfigs = new List<AccountBase>();

            if (Directory.Exists(System.IO.Path.Combine(Environment.CurrentDirectory, ".minecraft"))!=true)
            {
                Directory.CreateDirectory(System.IO.Path.Combine(Environment.CurrentDirectory, ".minecraft"));
            }
#if DEBUG
            AccountConfigs.Add(new MEFLLegacyAccount() {GetSetUserName = "Hongyu"});
            SelectedAccountIndex = 0;
            RegManager.SecurityWrite("","");
#else
            SlectedAccountIndex = -1;
#endif
        }
    }
}
