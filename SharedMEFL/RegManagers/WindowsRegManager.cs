using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows;

using Microsoft.Win32;
using MEFL.RegManagers;
using System.Windows.Input;

namespace MEFL
{
    public class WindowsRegManager : BaseRegManager
    {
        #region const

        private const string SOFTEWARE = "Software";

        #endregion

        #region ctors
        public WindowsRegManager()
        {
            using (var key = Registry.CurrentUser.CreateSubKey(SOFTEWARE))
            {
                using (var mefl = key.CreateSubKey("MEFL", true))
                {

                }
            }
        }
        #endregion

        #region methods

        public override void Write(string key, string value, bool forceWrite)
        {
            var encodeValue = _decryption.Encrypt(value);

            using (var software = Registry.CurrentUser.CreateSubKey(SOFTEWARE))
            {
                using (var mefl = software.CreateSubKey("MEFL", true))
                {
                    mefl.SetValue(key, encodeValue, RegistryValueKind.String);
                }
            }

            Debugger.Logger($"写入了注册表，键：{key}，值：{value}");

            //if (App.Current.Windows.Count != 0)
            //{
            //     _winRegKey.SetValue(key, value);
            //    //todo i18N;
            //    Debugger.Logger($"写入了注册表，键：{key}，值：{value}");
            //}
            //else if (forceWrite == true)
            //{
            //    _winRegKey.SetValue(key, value);
            //    //todo i18N;
            //    Debugger.Logger($"写入了注册表，键：{key}，值：{value}");
            //}
        }
        public override string Read(string key)
        {
            var result = string.Empty;
            using (var software = Registry.CurrentUser.CreateSubKey(SOFTEWARE))
            {
                using (var mefl = software.CreateSubKey("MEFL", true))
                {
                    result = mefl.GetValue(key) as string;
                }
            }

            if (string.IsNullOrWhiteSpace(result))
            {
                result = string.Empty;
                Debugger.Logger($"读取了注册表，键：{key}，值：null");
            }
            else
            {
                Debugger.Logger($"读取了注册表，键：{key}，值：{result}");
            }

            return result;

        }
        #endregion

    }
}
