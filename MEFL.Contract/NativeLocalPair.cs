using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL.Contract
{
    public class NativeLocalPair
    {
        public string NativeUrl { get; set; }
        public string LocalPath { get; set; }

        public NativeLocalPair(string nativeUrl, string localPath)
        {
            NativeUrl = nativeUrl;
            LocalPath = localPath;
        }

        public override string ToString()
        {
            return $"uri:{NativeUrl} local:{LocalPath}";
        }

    }
}
