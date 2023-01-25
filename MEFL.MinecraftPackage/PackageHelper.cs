using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEFL.MinecraftPackage;

public static class PackageHelper
{
    public static string GetDownloadsLocalPath(string minecraftRootDir, string id, string name)
    {
        return Path.Combine(minecraftRootDir, "versions", id, name);
    }

}
