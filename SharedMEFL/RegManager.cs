using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows;
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
            if (App.Current.Windows.Count != 0)
            {
                WinRegKey.SetValue(Key, Value);
                //todo i18N;
                Debugger.Logger($"写入了注册表，键：{Key}，值：{Value}");
            }
            else if(ForceWrite == true)
            {
                WinRegKey.SetValue(Key, Value);
                //todo i18N;
                Debugger.Logger($"写入了注册表，键：{Key}，值：{Value}");
            }
#endif
        }
        public static string Read(string Key)
        {
#if WINDOWS
            if (WinRegKey.GetValue(Key) == null||WinRegKey.GetValue(Key)==string.Empty)
            {
                WinRegKey.SetValue(Key, string.Empty);
            }
            var res = WinRegKey.GetValue(Key).ToString();
            //todo i18N;
            Debugger.Logger($"读取了注册表，键：{Key}，值：{res}");
            return res;
#endif
        }
    }
}
