using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows;
using Newtonsoft.Json.Linq;
#if WINDOWS
using Microsoft.Win32;
#endif

namespace MEFL
{
    public static class RegManager
    {
        static RegManager()
        {
            rsa = new RSACryptoServiceProvider();
#if WINDOWS
            WinRegKey = Registry.CurrentUser.CreateSubKey("Software").CreateSubKey("MEFL");
#elif AVALONIA
            if(Environment.OSVersion.Platform == PlatformID.Win32NT )
            {
                var appdata = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                FileP = Path.Combine(appdata,"MEFL", "Config.json");
                Directory.CreateDirectory(Path.GetDirectoryName(FileP));
                if(!File.Exists(FileP) )
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
        public static void SecurityWrite(string Key, string Value)
        {
            //todo Security
            Write(Key, Value);
        }
        public static object SecurityRead(string Key)
        {
            //todo Security
            return Read(Key);
        }
        public static void Write(string Key, string Value)
        {
#if WINDOWS
            Write(Key, Value, false);
#endif
        }
        public static void Write(string Key, string Value,bool ForceWrite)
        {
#if WINDOWS
            if(ForceWrite == true)
            {
                WinRegKey.SetValue(Key, Value);
                //todo i18N;
                Debugger.Logger($"写入了注册表，键：{Key}，值：{Value}");
            }
            else if (App.Current.Windows.Count != 0)
            {
                WinRegKey.SetValue(Key, Value);
                //todo i18N;
                Debugger.Logger($"写入了注册表，键：{Key}，值：{Value}");
            }
#elif AVALONIA
            Reg[Key] = Value;
            File.WriteAllText(FileP,Reg.ToString());
#endif
        }
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
