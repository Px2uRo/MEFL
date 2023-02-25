using CoreLaunching.JsonTemplates;
using MEFL.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace MEFL.CLAddIn.Downloaders
{
    internal static class SourceReplacer
    {
        internal static string Replace(string native, DownloadSource[] sources)
        {
            native = native.Replace("http://launchermeta.mojang.com/mc/game/version_manifest.json", "${version_manifest}");
            native = native.Replace("https://launchermeta.mojang.com/", "${AssIndex}");
            native = native.Replace("https://launcher.mojang.com/", "${AssIndex}");
            native = native.Replace("http://resources.download.minecraft.net", "${assets}");
            native = native.Replace("https://libraries.minecraft.net/", "${libraries}");
            return native;
        }

        internal static string[] ReplaceMany(string[] natives, DownloadSource[] sources) 
        {
            for (int i = 0; i < natives.Count(); i++)
            {
                natives[i] = natives[i].Replace("http://launchermeta.mojang.com/mc/game/version_manifest.json", "${version_manifest}");
                natives[i] = natives[i].Replace("https://piston-meta.mojang.com", "${json}");
                natives[i] = natives[i].Replace("https://launchermeta.mojang.com/", "${AssIndex}");
                natives[i] = natives[i].Replace("https://launcher.mojang.com/", "${AssIndex}");
                natives[i] = natives[i].Replace("http://resources.download.minecraft.net", "${assets}");
                natives[i] = natives[i].Replace("https://libraries.minecraft.net/", "${libraries}");
                for (int j = 0; j < sources.Length; j++)
                {
                    natives[j] = natives[j].Replace(oldValue: sources[j].ELItem, sources[j].Uri);
                }
            }
            return natives;
        }


    }

    internal class Version_Api_Source:DownloadSource
    {
        public override string GetUri(string parama)
        {
            return $"https://bmclapi2.bangbang93.com/version/{parama}/client";
        }

        public Version_Api_Source()
        {
            ELItem = "${versions_api}";
            RuleSourceName= "BMCLAPI";
            Uri = "https://bmclapi2.bangbang93.com/version/${version}/${category}";
        }
    }
}
