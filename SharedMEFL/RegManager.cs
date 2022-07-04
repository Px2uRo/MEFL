using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace MEFL
{
    public static class RegManager
    {
        static RegManager()
        {
            rsa = new RSACryptoServiceProvider();
        }
        private static RSACryptoServiceProvider rsa {get;set;}
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

        }
        public static object Read(string Key)
        {
            return null;
        }
    }
}
