using Avalonia.Controls;
using Avalonia.Media;
using MEFL.Arguments;
using MEFL.Contract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerInstaller
{
    internal class ServerType : GameInfoBase
    {
        Root root;
        public override bool IgnoreAccount => true;
        public override bool IgnoreLauncherArguments => true;
        public override List<JsonFileInfo> FileNeedsToDownload { get; set; }
        public override List<JsonFileInfo> NativeFilesNeedToDepackage { get; set; }

        public override string HeapDumpPath => "";

        public override string AssetsIndexName => "";

        public override string VersionType => "Server";

        public override int GameMaxMem { get; set; }
        public override int GameMinMem { get;set; }

        public override List<string> ClassPaths => new();

        public override string MainClassName => "";

        public override string GameJsonPath { get;set; }
        public override string GameTypeFriendlyName { get => "MCFile"; set => throw new NotImplementedException(); }
        public override string Description { get; set; }
        public override string Version { get; set; }

        public override string GameJarPath => GameJsonPath.Replace(".json", ".jar");

        public override IImage IconSource => throw new NotImplementedException();

        public override string NativeLibrariesPath { get => ""; set => throw new NotImplementedException(); }

        public override string GameArgs => "";

        public override string JVMArgs => GetJVMArgs();

        private string GetJVMArgs()
        {
            var res = string.Empty;
            foreach (var item in root.Arguments.Jvm)
            {
                if(item is string str)
                {
                    res += (str + " ");
                }
                else if (item is PropertyResultPair pair)
                {
                    if(pair.TargetProperty== "BaseVersion")
                    {
                        try
                        {
                            foreach (var result in pair.Results)
                            {
                                if (result.Condition == ">1.17" )
                                {
                                    if(System.Version.Parse(root.BaseVersion) > new System.Version(1, 17))
                                    {
                                        res += result.Value;
                                        res += " ";
                                    }
                                }
                                else if (result.Condition == "==1.17")
                                {
                                    if (System.Version.Parse(root.BaseVersion) == new System.Version(1, 17))
                                    {
                                        res += result.Value;
                                        res += " ";
                                    }
                                }
                                else if (result.Condition== "1.12<=x<=1.16.5")
                                {
                                    var v = System.Version.Parse(root.BaseVersion);
                                    if (v >= new System.Version(1, 12) && v <= new System.Version(1, 17))
                                    {
                                        res += result.Value;
                                        res += " ";
                                    }
                                }
                                else if (result.Condition == "1.7<=x<=1.11.2")
                                {
                                    var v = System.Version.Parse(root.BaseVersion);
                                    if (v >= new System.Version(1, 7) && v <= new System.Version(1, 11,2))
                                    {
                                        res += result.Value;
                                        res += " ";
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else if(pair.TargetProperty== "UseUpa")
                    {
                        if (!string.IsNullOrEmpty(root.UpaOption.Server_Id))
                        {
                            res += pair.Results[0].Value.Replace("${server_id}",root.UpaOption.Server_Id);
                            res += " ";
                        }
                    }
                }
            }
            res = res.Replace("${jar_name}", $"\"{GameJarPath}\"");
            res = res.Replace("${max_mem}", Memory);
            res = res.Replace("${min_mem}", Memory);
            return res;
        }
        public string Memory = SettingsGetter.GetMemory().ToString();
        public override string OtherGameArgs { get => ""; set => throw new NotImplementedException(); }
        public override string OtherJVMArgs { get => ""; set => throw new NotImplementedException(); }

        public override int JavaMajorVersion => root.JavaMajor;

        public override string GameFolder { get => ""; set => throw new NotImplementedException(); }

        public override IGameSettingPage SettingsPage => throw new NotImplementedException();

        public override DeleteResult Delete()
        {
            return DeleteResult.Canceled;
        }

        public override IProcessManagePage GetManageProcessPage(Process process, SettingArgs args)
        {
            var res = new ManagePanel(this,process);
            return res;
        }

        public override void Refresh()
        {
            if (!File.Exists(GameJarPath))
            {
                FileNeedsToDownload.Add(new() { localpath = GameJarPath, Url = root.Downloads.Server.url, size = root.Downloads.Server.size, sha1 = root.Downloads.Server.sha1 });
            }
            if (!string.IsNullOrEmpty(root.UpaOption.Server_Id))
            {
                var lp = Path.Combine(Path.GetDirectoryName(GameJarPath),"Nide8Auth.jar");
                if (!File.Exists(lp))
                {
                    FileNeedsToDownload.Add(new() { localpath = lp, Url = "https://static.mc-user.com:233/downloads/nide8auth.jar", size = 178566, sha1 = "2f0c0308ade4848188647c4a9f37bf8d4718c93f" });
                }
            }
        }
        public ServerType()
        {
            FileNeedsToDownload = new();
            NativeFilesNeedToDepackage = new();
        }
        public ServerType(string jsonPath):this()
        {
            GameJsonPath = jsonPath;
            root = JsonConvert.DeserializeObject<Root>(File.ReadAllText(jsonPath));
        }
    }
}
