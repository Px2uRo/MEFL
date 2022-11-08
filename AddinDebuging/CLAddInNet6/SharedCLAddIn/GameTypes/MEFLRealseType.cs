using CoreLaunching.JsonTemplates;
using MEFL.Arguments;
using MEFL.Contract;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using io = System.IO;

namespace MEFL.CLAddIn.GameTypes
{
    public class MEFLRealseType : MEFL.Contract.GameInfoBase
    {
        private List<String> _ItemsNeedsToExtract = new List<string>();
        public override List<string> ItemsNeedsToExtract => _ItemsNeedsToExtract;
        public override FrameworkElement GetManageProcessPage(Process process, SettingArgs args)
        {
            var res = new Pages.MEFLRealseTypeManage(process);
            return res;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _ItemsNeedsToExtract.Clear();
                FileNeedsToDownload.Clear();
                GC.SuppressFinalize(Icon);
                GC.SuppressFinalize(_MSOAT);
                GC.SuppressFinalize(_Root);
                GC.SuppressFinalize(FileNeedsToDownload);

                GC.WaitForPendingFinalizers();
            }
            base.Dispose(disposing);
        }
        private static FrameworkElement _settingPage = new Pages.MEFLRealseTypeSetting();
        private MEFLStandardOtherArgumentTemplate _MSOAT { get; set; }
        private CoreLaunching.JsonTemplates.Root _Root { get; set; }
        public override string GameTypeFriendlyName { get => "发布"; set => throw new NotImplementedException(); }
        public override string Description
        {
            get {
                if (_MSOAT.Description != null)
                {
                    return _MSOAT.Description;
                }
                else
                {
                    return _Root.Id;
                }
            }
            set => _MSOAT.Description = value;
        }
        public override string Version { get => _Root.Id; set => _Root.Id = value; }
        static BitmapImage Icon = new BitmapImage(new Uri("pack://application:,,,/RealseTypeLogo.png", UriKind.Absolute));
        public override ImageSource IconSource {
            get
            {
                return Icon;
            }
        }

        public override string NativeLibrariesPath { get
            {
                var res = string.Empty;
                if (_MSOAT.NativeLibrariesPath != null)
                {
                    res = _MSOAT.NativeLibrariesPath;
                }
                else
                {
                    if (!Directory.Exists(System.IO.Path.Combine(dotMinecraftPath, "natives")))
                    {
                        Directory.CreateDirectory(System.IO.Path.Combine(dotMinecraftPath, "natives"));
                    }
                    res = System.IO.Path.Combine(dotMinecraftPath, "natives");
                }
                foreach (var item in _Root.Libraries)
                {
                    if (item.Downloads.Classifiers != null)
                    {
                        if (item.Downloads.Artifact.Path != null)
                        {
                            _ItemsNeedsToExtract.Add(io.Path.Combine(dotMinecraftPath, "libraries", item.Downloads.Artifact.Path.Replace(@"/", "\\")));
                        }
                    }
                }
                return res;
            }
            set
            {
                _MSOAT.NativeLibrariesPath = value;
            }
        }
        public override string GameArgs { get 
            {
                var res = string.Empty;
                if (_Root.Arguments!=null)
                {
                    foreach (var item in _Root.Arguments.Game)
                        {
                            if (!item.HasValues)
                            {
                                res += $" {item.ToString()}";
                            }
                        }
                    return res;
                }
                else
                {
                    res = _Root.MinecraftArguments;
                }
                return res;
            } 
        }
        public override string JVMArgs { get
            {
                String res = String.Empty;
                if (_Root.Arguments != null)
                {
                    foreach (var item in _Root.Arguments.Jvm)
                    {
                        if (item.Contains(' '))
                        {
                            res += $" \"{item}\"";
                        }
                        else
                        {
                            res += $" {item}";
                        }
                    }
                }
                else
                {
                    res += " \"-Dos.name=${Dos.name}\" -Dos.version=${Dos.version} -XX:HeapDumpPath=${HeapDumpPath} \"-Djava.library.path=${natives_directory}\" -Dminecraft.launcher.brand=${launcher_name} -Dminecraft.launcher.version=${launcher_version} -Dminecraft.client.jar=${Dminecraft.client.jar}";
                    string cps = string.Empty;
                    foreach (var item in ClassPaths)
                    {
                        cps += item;
                        cps += ";";
                    }
                    cps += GameJarPath;
                    res += $" -cp \"{cps}\"";
                }
                return res;
            }
        }
        public override string OtherGameArgs { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string OtherJVMArgs { get => _MSOAT.OtherJVMArguments; set => _MSOAT.OtherJVMArguments = value; }
        public override string GameJsonPath { get; set; }

        public override int JavaMajorVersion {
            get
            {
                try
                {
                    return _Root.JavaVersion.MajorVersion;
                }
                catch (Exception ex)
                {
                    return 8;
                }
            }
        }

        public override string GameFolder { get => dotMinecraftPath; set => throw new NotImplementedException(); }

        public override FrameworkElement SettingsPage => _settingPage;

        public override string GameJarPath => GameJsonPath.Replace(".json", ".jar");

        public override string HeapDumpPath => "${HeapDumpPath}";

        public override int GameMaxMem { get => 0; set => throw new NotImplementedException(); }
        public override int GameMinMem { get => 0; set => throw new NotImplementedException(); }

        public override List<string> ClassPaths { get 
            {
                var res = new List<string>();
                foreach (var item in _Root.Libraries)
                {
                    if (item.Downloads.Artifact.Path != null)
                    {
                        res.Add(io.Path.Combine(dotMinecraftPath, "libraries", item.Downloads.Artifact.Path.Replace(@"/", "\\")));
                        if (io.File.Exists(io.Path.Combine(dotMinecraftPath, "libraries", item.Downloads.Artifact.Path.Replace(@"/", "\\"))) == false)
                        {
                            FileNeedsToDownload.Add(new LauncherWebFileInfo() { Url=item.Downloads.Artifact.Url,size=Convert.ToInt32(item.Downloads.Artifact.Size),localpath= io.Path.Combine(dotMinecraftPath, "libraries", item.Downloads.Artifact.Path.Replace(@"/", "\\")) ,sha1=item.Downloads.Artifact.Sha1});
                        }
                    }
                }
                if (io.File.Exists(GameJarPath) != true)
                {
                    FileNeedsToDownload.Add(new LauncherWebFileInfo() { Url=_Root.Downloads.Client.Url,sha1=_Root.Downloads.Client.Sha1,size=System.Convert.ToInt32(_Root.Downloads.Client.Size),localpath=GameJarPath });
                }
                return res;
            } 
        }

        public override string MainClassName => _Root.MainClass;

        public override string AssetsRoot { get 
            {
                var AssetsJson = io.Path.Combine(dotMinecraftPath, "AssetsJsons", $"{_Root.AssetIndex.Id}.json");
                if (!Directory.Exists(io.Path.Combine(dotMinecraftPath,"AssetsJsons")))
                {
                    Directory.CreateDirectory(io.Path.Combine(dotMinecraftPath, "AssetsJsons"));
                }
                Stream strm;
                if (!io.File.Exists(AssetsJson))
                {
                    var websrm = HttpWebRequest.Create(_Root.AssetIndex.Url).GetResponse().GetResponseStream();
                    StreamReader sr = new StreamReader(websrm);
                    FileStream fs = new FileStream(AssetsJson, FileMode.CreateNew);
                    byte[] bArr = new byte[1024];
                    int size = websrm.Read(bArr, 0, (int)bArr.Length);
                    while (size > 0)
                    {
                        fs.Write(bArr, 0, size);
                        size = websrm.Read(bArr, 0, (int)bArr.Length);
                    }
                    fs.Close();
                    sr.Close();
                    websrm.Close();
                }
                var AssetsJOb = JsonConvert.DeserializeObject<CoreLaunching.JsonTemplates.AssetsObject>(io.File.ReadAllText(AssetsJson));
                foreach (var item in AssetsJOb.Objects)
                {
                    var tst = io.Path.Combine(dotMinecraftPath, "assets\\objects", $"{item.Hash.Substring(0,2)}");
                    if (!Directory.Exists(io.Path.Combine(dotMinecraftPath, "assets\\objects", $"{item.Hash.Substring(0,2)}")))
                    {
                        Directory.CreateDirectory(io.Path.Combine(dotMinecraftPath, "assets\\objects", $"{item.Hash.Substring(0, 2)}"));
                    }
                    if (!io.File.Exists(io.Path.Combine(dotMinecraftPath, "assets\\objects",$"{item.Hash.Substring(0,2)}\\{item.Hash}")))
                    {
                        FileNeedsToDownload.Add(new LauncherWebFileInfo() { localpath= io.Path.Combine(dotMinecraftPath, "assets\\objects", $"{item.Hash.Substring(0, 2)}\\{item.Hash}"),Url=$"http://resources.download.minecraft.net/{item.Hash.Substring(0, 2)}/{item.Hash}"});
                    }
                }
                return io.Path.Combine(dotMinecraftPath, "assets");
            } 
        }

        public override string AssetsIndexName => _Root.AssetIndex.Id;

        public override string VersionType => _Root.Type;

        public override List<LauncherWebFileInfo> FileNeedsToDownload { get; set ; }
        public override List<LauncherWebFileInfo> NativeFilesNeedToDepackage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override void Delete()
        {
            Dispose();
        }

        public override void Refresh()
        {
            FileNeedsToDownload= new List<LauncherWebFileInfo>();
        }

        public MEFLRealseType(string JsonPath)
        {
            FileNeedsToDownload = new List<LauncherWebFileInfo>();
            string otherArgsPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(JsonPath), "MEFLOtherArguments.json");
            _MSOAT =new MEFLStandardOtherArgumentTemplate(otherArgsPath);
            otherArgsPath = string.Empty;
            GameJsonPath=JsonPath;
            try
            {
                _Root = JsonConvert.DeserializeObject<Root>(System.IO.File.ReadAllText(JsonPath));
            }
            catch (Exception ex)
            {

            }
        }
    }
    public class MEFLStandardOtherArgumentTemplate : OtherArgumentTemplateBase
    {
        #region Privates
        [JsonIgnore]
        private string _JsonPath { get; set; }
        [JsonIgnore]
        private bool _IsFavorite { get; set; }
        [JsonIgnore]
        private string _Description { get; set; }
        [JsonIgnore]
        private string _OtherJVMArguments { get; set; }
        [JsonIgnore]
        private string _OtherGameArguments { get; set; }
        [JsonIgnore]
        private string _NativeLibrariesPath { get; set; }
        [JsonIgnore]
        private string _CustomIconPath { get; set; }
        #endregion
        #region Props
        public override bool IsFavorite { get => _IsFavorite; set { _IsFavorite = value; ChangeProperty(); } }
        public override string Description { get => _Description; set { _Description = value; ChangeProperty(); } }
        public override string OtherJVMArguments { get => _OtherJVMArguments; set { _OtherJVMArguments = value; ChangeProperty(); } }
        public override string OtherGameArguments { get => _OtherGameArguments; set { _OtherGameArguments = value; ChangeProperty(); } }
        public override string NativeLibrariesPath { get => _NativeLibrariesPath; set { _NativeLibrariesPath = value; ChangeProperty(); } }
        public override string CustomIconPath { get => _CustomIconPath; set { _CustomIconPath = value; ChangeProperty(); } }
        public override void ChangeProperty()
        {
            try
            {
                if (System.IO.File.Exists(_JsonPath) != true)
                {
                    System.IO.File.Create(_JsonPath).Close();
                }
                System.IO.File.WriteAllText(_JsonPath, JsonConvert.SerializeObject(this));
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        public MEFLStandardOtherArgumentTemplate(string JsonPath)
        {
            _JsonPath = JsonPath;
            try
            {
                if (System.IO.File.Exists(_JsonPath) != true)
                {
                    System.IO.File.Create(_JsonPath).Close();
                }
                var root = JsonConvert.DeserializeObject<MEFLStandardOtherArgumentTemplate>(System.IO.File.ReadAllText(JsonPath));
                if (root != null)
                {
                    Description = root.Description;
                    IsFavorite = root.IsFavorite;
                    OtherJVMArguments = root.OtherJVMArguments;
                    OtherGameArguments = root.OtherGameArguments;
                    NativeLibrariesPath = root.NativeLibrariesPath;
                    CustomIconPath = root.CustomIconPath;
                }
                root = null;
            }
            catch (Exception)
            {

            }
        }

    }

}
