﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using MEFL.Contract;
using Newtonsoft.Json;

namespace MEFL.APIData
{
    /// <summary>
    /// SettingConfig 一般会放在 Json 里面
    /// </summary>
    public class SettingConfig
    {
        [JsonIgnore]
        public string MEFLConfigForder;
        private int _LogIndex;
        public int LogIndex { get => _LogIndex; set { _LogIndex = value; Update(); } }
        private string _PicturePath;
        public string PicturePath { get => _PicturePath; set { _PicturePath = value;Update(); } }
        private string _SelectedGame;
        public string SelectedGame { get => _SelectedGame; set { _SelectedGame = value;Update(); } }
        private string _OtherJVMArgs;
        public string OtherJVMArgs { get => _OtherJVMArgs; set { _OtherJVMArgs = value; Update(); } }
        private bool _AlwaysOpenNewExtensions;
        public bool AlwaysOpenNewExtensions { get =>_AlwaysOpenNewExtensions; set { _AlwaysOpenNewExtensions = value;Update(); } }
        public void Update()
        {
            try
            {
                FileInfo fi = new FileInfo(Path.Combine(Environment.CurrentDirectory, "MEFL\\Config.json"));
                if (fi.Exists!=true)
                {
                    fi.Create().Close();
                }
                System.IO.File.WriteAllText(fi.FullName, JsonConvert.SerializeObject(this),Encoding.Unicode);
                fi = null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR]无法更新设置 {ex.Message}");
            }
        }
        public static SettingConfig Load()
        {
            FileInfo fi = new FileInfo(Path.Combine(Environment.CurrentDirectory, "MEFL\\Config.json"));
            try
            {
                if (fi.Exists != true)
                {
                    fi.Create().Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            SettingConfig sc;
            try
            {
                sc = JsonConvert.DeserializeObject<SettingConfig>(File.ReadAllText(fi.FullName));
                fi = null;
            }
            catch (Exception ex)
            {
                Debugger.Logger($"无法加载配置： {ex.Message}");
                sc = new SettingConfig();
            }
            if (sc == null)
            {
                sc = new SettingConfig();
            }
            if (String.IsNullOrEmpty(sc.OtherJVMArgs))
            {
                sc.OtherJVMArgs = "-XX:+UseG1GC -XX:-UseAdaptiveSizePolicy -XX:-OmitStackTraceInFastThrow -Dfml.ignoreInvalidMinecraftCertificates=True -Dfml.ignorePatchDiscrepancies=True -Dlog4j2.formatMsgNoLookups=true";
            }
            return sc;
        }
        public SettingConfig()
        {
            LogIndex = 0;
            MEFLConfigForder = Path.Combine(Environment.CurrentDirectory, "MEFL");
            if (Directory.Exists(MEFLConfigForder)!=true)
            {
                Directory.CreateDirectory(MEFLConfigForder);
            }
        }
    }

    public class AddInConfig
    {
        public override string ToString()
        {
            return $"{Guid}:{IsOpen}";
        }
        public string Guid { get; set; }
        public bool IsOpen { get; set; }

        public static ObservableCollection<AddInConfig> GetAll()
        {
            ObservableCollection<AddInConfig> ret;
            var Path = System.IO.Path.Combine(Environment.CurrentDirectory, "AddIns\\Config.json");
            try
            {
                if (File.Exists(Path) != true)
                {
                    Directory.CreateDirectory(System.IO.Path.GetDirectoryName(Path));
                    File.Create(Path).Close();
                }
                using (StreamReader sr = new StreamReader(Path))
                {
                    ret = JsonConvert.DeserializeObject<ObservableCollection<AddInConfig>>(sr.ReadToEnd());
                }
                if (ret == null)
                {
                    ret = new ObservableCollection<AddInConfig>();
                }
                Debugger.Logger($"加载了插件设置,当前文档：{File.ReadAllText(Path)}");
            }
            catch (Exception ex)
            {
                Debugger.Logger(ex.Message);
                ret = new ObservableCollection<AddInConfig>();
            }
            ret.CollectionChanged += Ret_CollectionChanged;
            return ret;
        }

        private static void Ret_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            List<AddInConfig> NewList = new List<AddInConfig>();
            foreach (var item in sender as ObservableCollection<AddInConfig>)
            {
                if (!NewList.Contains(item))
                {
                    NewList.Add(item);
                }
            }
            Update(NewList);
        }

        public static void Update(List<AddInConfig> addInConfigs)
        {
            var Path = System.IO.Path.Combine(Environment.CurrentDirectory, "AddIns\\Config.json");
            try
            {
                if (Directory.Exists(Path) != true)
                {
                    File.Create(Path).Close();
                }
                File.WriteAllText(Path, JsonConvert.SerializeObject(addInConfigs));
                Debugger.Logger($"重写了插件设置,当前文档：{File.ReadAllText(Path)}");
            }
            catch (Exception ex)
            {
                Debugger.Logger(ex.Message);
            }
        }
    }
}
