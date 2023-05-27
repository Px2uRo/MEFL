using Avalonia.Controls.ApplicationLifetimes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MEFL.Contract
{
    public static class SettingsGetter
    {
        public static int GetMemory()
        {
            var res = RegManager.Read("MaxMemory");
            return Convert.ToInt32(res);
        }
    }

    public static class RegManager
    {
        static RegManager()
        {
            rsa = new RSACryptoServiceProvider();
#if WINDOWS
            WinRegKey = Registry.CurrentUser.CreateSubKey("Software").CreateSubKey("MEFL");
#elif AVALONIA
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                var appdata = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                FileP = Path.Combine(appdata, "MEFL", "Config.json");
                Directory.CreateDirectory(Path.GetDirectoryName(FileP));
                if (!File.Exists(FileP))
                {
                    File.Create(FileP).Close();
                }
                try
                {
                    Reg = JObject.Parse(File.ReadAllText(FileP));
                }
                catch
                {
                    Reg = new();
                }
            }
#endif
        }
        private static RSACryptoServiceProvider rsa { get; set; }
#if WINDOWS
        private static RegistryKey WinRegKey { get; set; }
        public static void Close()
        {
            WinRegKey.Close();
            WinRegKey.Dispose();
            WinRegKey = null;
        }

#elif AVALONIA
        public static JObject Reg;
        public static string FileP;
#endif
        public static string Read(string Key)
        {
#if WINDOWS
            var res = WinRegKey.GetValue(Key);
            if (res == null)
            {
                WinRegKey.SetValue(Key, string.Empty);
                res = WinRegKey.GetValue(Key).ToString();
            }
            //todo i18N;
            Debugger.Logger($"读取了注册表，键：{Key}，值：{res}");
            return res.ToString();
#elif AVALONIA
            if (Reg[Key] != null)
            {
                return Reg[Key].ToString();
            }
            else
            {
                Reg[Key] = string.Empty;
                return Reg[Key].ToString();
            }
#endif
        }
    }

}
