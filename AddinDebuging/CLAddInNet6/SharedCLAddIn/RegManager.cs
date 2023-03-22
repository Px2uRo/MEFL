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

namespace MEFL.CLAddIn
{
    public static class RegManager
    {
        static RegManager()
        {
            rsa = new RSACryptoServiceProvider();
#if WINDOWS
            WinRegKey = Registry.CurrentUser.CreateSubKey("Software").CreateSubKey("MEFL").CreateSubKey("CLAddIn");
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
            if (Application.Current.Windows.Count != 0)
            {
                WinRegKey.SetValue(Key, Value);
                //todo i18N;
            }
            else if(ForceWrite == true)
            {
                WinRegKey.SetValue(Key, Value);
                //todo i18N;
            }
#endif
        }
        public static string Read(string Key)
        {
#if WINDOWS
            if (WinRegKey.GetValue(Key) == null)
            {
                WinRegKey.SetValue(Key, string.Empty);
            }
            var res = WinRegKey.GetValue(Key).ToString();
            //todo i18N;
            return res;
#endif
        }
    }
}
