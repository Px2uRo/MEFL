﻿using MEFL.Contract;
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

        public static Arguments.SettingArgs SettingArgs { get; set; }
        public static Contract.GameInfoBase CurretGame { get; set; }
        public static ObservableCollection<Contract.GameInfoBase> GameInfoConfigs { get; set; }
        public static ObservableCollection<MEFLFolderInfo> MyFolders { get; set; }

        static APIModel()
        {
            SettingArgs = new Arguments.SettingArgs();
            SettingConfig = MEFL.APIData.SettingConfig.Load();
            AddInConfigs = MEFL.APIData.AddInConfig.GetAll();
            RemoveAddInsTheSameAddIn();
            AccountConfigs = new List<AccountBase>();
            GameInfoConfigs = new ObservableCollection<Contract.GameInfoBase>();
            if (Directory.Exists(System.IO.Path.Combine(Environment.CurrentDirectory, ".minecraft"))!=true)
            {
                Directory.CreateDirectory(System.IO.Path.Combine(Environment.CurrentDirectory, ".minecraft"));
            }
            MyFolders = new ObservableCollection<MEFLFolderInfo>() { new MEFLFolderInfo() { Path= System.IO.Path.Combine(Environment.CurrentDirectory, ".minecraft") } };
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
