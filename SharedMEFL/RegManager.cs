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
            string PublicKey = string.Empty;
            string PrivateKey = string.Empty;

            PublicKey = rsa.ToXmlString(false); // 获取公匙，用于加密
            PrivateKey = rsa.ToXmlString(true); // 获取公匙和私匙，用于解密

            Debug.WriteLine(PublicKey);// 将公匙保存到运行目录下的PublicKey
            Debug.WriteLine(PrivateKey); // 将公匙&私匙保存到运行目录下的PrivateKey
        }
        public static object SecurityRead(string Key)
        {
            return null;
        }
        public static void Write(string Key, string Value)
        {
#if WINDOWS
            if (App.Current.Windows.Count!=0)
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
