using MEFL.Contract;
using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL.CLAddIn.Downloaders
{
    internal static class SourceReplacer
    {
        internal static string Replace(string Native, DownloadSource[] sources)
        {
            var res = string.Empty;
            res = Native.Replace("http://launchermeta.mojang.com/mc/game/version_manifest.json", "${version_manifest}");
            res = res.Replace("https://piston-meta.mojang.com", "${json_&_AssIndex}");
            res = res.Replace("https://launchermeta.mojang.com/", "${json_&_AssIndex}");
            res = res.Replace("https://launcher.mojang.com/", "${json_&_AssIndex}");
            res = res.Replace("http://resources.download.minecraft.net", "${assets}");
            res = res.Replace("https://libraries.minecraft.net/", "${libraries}");
            for (int i = 0; i < sources.Length; i++)
            {
                res = res.Replace(oldValue:sources[i].ELItem, sources[i].Uri);
            }
            return res;
        }
    }
}
