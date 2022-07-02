using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using MEFL.Contract;
using Newtonsoft.Json;

namespace MEFL.APIData
{
    public class SettingConfig
    {
        public string MEFLConfigForder;
        public int LogIndex { get; set; }
        public string PicturePath { get; set; }
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
                Debug.WriteLine($"无法加载配置： {ex.Message}");
                sc = new SettingConfig();
            }
            if (sc == null)
            {
                sc = new SettingConfig();
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
        public string Guid { get; set; }
        public bool IsOpen { get; set; }

        public static List<AddInConfig> GetAll()
        {
            List<AddInConfig> ret;
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
                    ret = JsonConvert.DeserializeObject<List<AddInConfig>>(sr.ReadToEnd());
                }
                if (ret == null)
                {
                    ret = new List<AddInConfig>();
                }
                Debugger.Logger($"加载了插件设置,当前文档：{File.ReadAllText(Path)}");
            }
            catch (Exception ex)
            {
                Debugger.Logger(ex.Message);
                ret = new List<AddInConfig>();
            }
            return ret;
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
