using MEFL.APIData;
using MEFL.Contract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MEFL
{
    public static class ProcessCreater
    {
        public static Process NewGame(GameInfoBase Game)
        {
            Process p = new Process();
            ProcessStartInfo i = new ProcessStartInfo();
            if (Game.JavaMajorVersion == FileVersionInfo.GetVersionInfo(APIModel.SettingArgs.SelectedJava.FullName).FileMajorPart)
            {
                i.FileName = APIModel.SettingArgs.SelectedJava.FullName;
            }
            else
            {
                throw new Exception($"不合适的 JAVA\n需要的Java版本\n{Game.JavaMajorVersion}\n当前选择的Java\n{APIModel.SettingArgs.SelectedJava.FullName}\n版本为{FileVersionInfo.GetVersionInfo(APIModel.SettingArgs.SelectedJava.FullName).FileMajorPart}");
            }
            string Args = string.Empty;
            if (String.IsNullOrEmpty(Game.OtherJVMArgs))
            {
                Args += APIModel.SettingConfig.OtherJVMArgs;
            }
            else
            {
                Args += Game.OtherJVMArgs;
            }
            Args += $" {Game.JVMArgs}";

            var mems = string.Empty;
            if (Game.GameMaxMem == null || Game.GameMinMem == null || Game.GameMaxMem == 0)
            {
                mems = " -Xmn256m -Xmx1256m";
            }
            else
            {
                mems = $" -Xmn{Game.GameMinMem.ToString()}m -Xmx{Game.GameMaxMem.ToString()}m";
            }
            Args += mems;
            Args += $" {Game.MainClassName}";
            Args += $" {Game.GameArgs}";
            var cps = String.Empty;
            cps += "\"";
            foreach (var item in Game.ClassPaths)
            {
                cps += item;
                cps +=";";
            }
            cps +=Game.GameJarPath;
            cps += "\"";
            Dictionary<string, string> dic = new Dictionary<string, string>()
            {
                {"${cp}",$"{cps}"},
                {"${classpath}",$"{cps}"},
                {"${HeapDumpPath}","MojangTricksIntelDriversForPerformance_javaw.exe_minecraft.exe.heapdump"},
                {"${auth_player_name}",$"\"{APIData.APIModel.SelectedAccount.UserName}\""},
                {"${version_name}",$"\"{Game.Version}\""},
                {"${game_directory}",$"\"{Game.GameFolder}\""},
                {"${assets_root}",$"\"{Game.AssetsRoot}\""},
                {"${assets_index_name}",$"{Game.AssetsIndexName}"},
                {"${auth_uuid}",$"{APIData.APIModel.SelectedAccount.Uuid}" },
                {"${auth_access_token}",$"{APIModel.SelectedAccount.AccessToken}"},
                {"${user_type}",$"{APIModel.SelectedAccount.UserType}"},
                {"${version_type}",$"{Game.VersionType}"},
                {"${launcher_name}","MEFL"},
                {"${launcher_version}","0.1" },
                {"${Dos.name}","Windows 10"},
                {"${Dos.version}","10.0"},
                {"${natives_directory}",Game.NativeLibrariesPath},
                {"${Dminecraft.client.jar}",Game.GameJarPath }
            };
            foreach (var item in dic)
            {
                Args = Args.Replace(item.Key.ToString(),item.Value.ToString());
            }
            i.Arguments = Args;
            p.StartInfo = i;
            return p;
        }
    }
}
