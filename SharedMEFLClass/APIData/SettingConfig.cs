using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace MEFL.APIData
{
    public class SettingConfig
    {
        public string MEFLConfigForder;
        public int LogIndex { get; set; }
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
                Debug.Write($"无法加载配置： {ex.Message}");
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
}
