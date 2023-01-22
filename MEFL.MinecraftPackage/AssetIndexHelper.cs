using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEFL.MinecraftPackage;
public static class AssetIndexHelper
{
    public static string CreateRemoteURI(string hash)
    {
        return $"http://resource.download.minecraft.net/{hash.Substring(0,2)}/{hash}";
    }

    ///<summary>minecraftRootDir一般是指 .minecraft 文件夹。该方法用于在 资源索引文件中创建指定文件。</summary>
    public static string CreateLocalPath(string minecraftRootDir,string name)
    {
        return System.IO.Path.Combine(minecraftRootDir,"assets",name);
    }
}
