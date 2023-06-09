﻿using CoreLaunching.JsonTemplates;
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
#if WPF
using MEFL.CLAddIn.Pages;
using MEFL.CLAddIn.Properties;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MEFL.Controls;
#elif AVALONIA
using FrameworkElement = MEFL.Contract.IProcessManagePage;
using Pages = CLAddIn.Views;
using BitmapImage = Avalonia.Media.Imaging.Bitmap;
#endif
using io = System.IO;
using CLAddIn.Views;
using Avalonia.Media;
using CLAddIn.Properties;
using SkiaSharp;

namespace MEFL.CLAddIn.GameTypes
{
    public class CLGameType : MEFL.Contract.GameInfoBase,IModLoaderOption
    {
        internal bool startWithDebug => _MSOAT.StartInDebugMode;
        public override FrameworkElement GetManageProcessPage(Process process, SettingArgs args)
        {
#if WPF
            var res = new Pages.MEFLRealseTypeManage(process,this);
            return res;
#elif AVALONIA
            var res = new Pages.MEFLRealseTypeManage(process);
            return res;
#endif
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                FileNeedsToDownload.Clear();
                GC.SuppressFinalize(_MSOAT);
                GC.SuppressFinalize(_Root);
                GC.SuppressFinalize(FileNeedsToDownload);

                GC.WaitForPendingFinalizers();
            }
            base.Dispose(disposing);
        }
        private static MEFLRealseTypeSetting _settingPage = new Pages.MEFLRealseTypeSetting();
        private MEFLOtherArgs _MSOAT { get; set; }
        private CoreLaunching.JsonTemplates.Root _Root { get; set; }
        public override string GameTypeFriendlyName
        {
            get
            {
                if (ModLoaderType==ModLoaderType.Forge)
                {
                    return "Forge";
                }
                else
                {
                    return _Root.Type;
                }
            }
            set { }
        }
        public override string Description
        {
            get {
                if (_MSOAT.Description != null)
                {
                    return _MSOAT.Description;
                }
                else
                {
                    if (ModLoaderType == ModLoaderType.Forge)
                    {
                        return $"Forge: {_Root.Id}";
                    }
                    else
                    {
                        return _Root.Id;
                    }
                }
            }
            set => _MSOAT.Description = value;
        }
        public override string Version { get => _Root.Id; set => _Root.Id = value; }
#if WPF
        public static Stream ForgeStream = new MemoryStream(Resources.MaybeForge);
        static Stream snapshotStream = new MemoryStream(Resources.Snapshot);
        static BitmapImage Icon = new BitmapImage(new Uri("pack://application:,,,/RealseTypeLogo.png", UriKind.Absolute));
        static ImageSource forge = null;
        static ImageSource snapshot = null;
        public override ImageSource IconSource {
            get
            {
                if (_maybeForge)
                {
                    if (forge == null)
                    {
                        var decoder = new PngBitmapDecoder(ForgeStream, BitmapCreateOptions.None, BitmapCacheOption.Default);
                        forge = decoder.Frames[0];
                    }
                    return forge;
                }
                else if (VersionType=="snapshot")
                {
                    if (snapshot == null)
                    {
                        var decoder = new PngBitmapDecoder(snapshotStream, BitmapCreateOptions.None, BitmapCacheOption.Default);
                        snapshot = decoder.Frames[0];
                    }
                    return snapshot;
                }
                else
                {
                    return Icon;
                }
            }
        }
#elif AVALONIA
        static MemoryStream forgeIconSource;
        static MemoryStream releaseIconSource;
        static BitmapImage _forgeIcon;
        static BitmapImage _releaseIcon;
        public override IImage IconSource {
            get
            {
                if (ModLoaderType == ModLoaderType.Forge)
                {
                    if (forgeIconSource == null)
                    {
                        forgeIconSource = new MemoryStream();
                        Resources.ForgeIcon.Save(forgeIconSource, Resources.ForgeIcon.RawFormat);
                        forgeIconSource.Position = 0;
                        _forgeIcon = new BitmapImage(forgeIconSource);
                        forgeIconSource.Position = 0;
                    }
                    return _forgeIcon;
                }
                else
                {
                    if (releaseIconSource == null)
                    {
                        releaseIconSource = new MemoryStream();
                        Resources.ReleaseIcon.Save(releaseIconSource, Resources.ReleaseIcon.RawFormat);
                        releaseIconSource.Position = 0;
                        _releaseIcon = new BitmapImage(releaseIconSource);
                        releaseIconSource.Position = 0;
                    }
                    return _releaseIcon;
                }
            }
        }
#endif

        public override string NativeLibrariesPath { get
            {
                var res = string.Empty;
                if (_MSOAT.NativeLibrariesPath != null)
                {
                    res = _MSOAT.NativeLibrariesPath;
                }
                else
                {
                    res = System.IO.Path.Combine(io.Path.GetDirectoryName(GameJarPath), "natives");
                    if (!Directory.Exists(res))
                    {
                        Directory.CreateDirectory(res);
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
                if (_Root.Arguments != null)
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
                else if (_Root.Arguments != null && string.IsNullOrEmpty(_Root.OldMinecraftArguments))
                {
                    throw new Exception();
                }
                else
                {
                    res = _Root.OldMinecraftArguments;
                }
                return res;
            }
        }
        public override string JVMArgs { get
            {
                String res = String.Empty;
                if (_Root.Arguments != null)
                {
                    for (int i = 0; i < _Root.Arguments.Jvm.Count; i++)
                    {
                        var item = _Root.Arguments.Jvm[i];
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
        public override string OtherGameArgs { get => ""; set => throw new NotImplementedException(); }
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
        public GamePathType GamePathType { get => _MSOAT.GamePathType; set { _MSOAT.GamePathType = value; PropChanged(); } }
        public override string GameFolder { get {
                if (GamePathType == GamePathType.Versions)
                {
                    var path = $"{dotMinecraftPath}\\versions\\{Version}";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    return path;
                }
                else if (GamePathType == GamePathType.Custom)
                {
                    return _MSOAT.CustomDotMCPath;
                }
                else
                {
                    return dotMinecraftPath;
                }
            } set { }
        }

        public string CustomDotMCPath
        {
            get { return _MSOAT.CustomDotMCPath; }
            set { _MSOAT.CustomDotMCPath = value; }
        }


#if WPF
        public override IGameSettingPage SettingsPage {get {
                _settingPage.DataContext = this;
                _settingPage.SetShowModCard(_maybeForge);
                return _settingPage;
            }
        }
#elif AVALONIA
        public override IGameSettingPage SettingsPage {get{ _settingPage.DataContext = this; return _settingPage; } }
#endif
        public override string GameJarPath => GameJsonPath.Replace(".json", ".jar");

        public override string HeapDumpPath => "${HeapDumpPath}";

        public override int GameMaxMem { get => 0; set => throw new NotImplementedException(); }
        public override int GameMinMem { get => 0; set => throw new NotImplementedException(); }

        public override List<string> ClassPaths { get 
            {
                var AssetsJson = io.Path.Combine(dotMinecraftPath, "AssetsJsons", $"{_Root.AssetIndex.Id}.json");
                if (!Directory.Exists(io.Path.Combine(dotMinecraftPath, "AssetsJsons")))
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
                    var tst = io.Path.Combine(dotMinecraftPath, "assets\\objects", $"{item.Hash.Substring(0, 2)}");
                    if (!Directory.Exists(io.Path.Combine(dotMinecraftPath, "assets\\objects", $"{item.Hash.Substring(0, 2)}")))
                    {
                        Directory.CreateDirectory(io.Path.Combine(dotMinecraftPath, "assets\\objects", $"{item.Hash.Substring(0, 2)}"));
                    }
                    if (!io.File.Exists(io.Path.Combine(dotMinecraftPath, "assets\\objects", $"{item.Hash.Substring(0, 2)}\\{item.Hash}")))
                    {
                        FileNeedsToDownload.Add(new JsonFileInfo() { localpath = io.Path.Combine(dotMinecraftPath, "assets\\objects", $"{item.Hash.Substring(0, 2)}\\{item.Hash}"), Url = $"http://resources.download.minecraft.net/{item.Hash.Substring(0, 2)}/{item.Hash}" });
                    }
                }
                var res = new List<string>();
                foreach (var item in _Root.Libraries)
                {
                        if (item.Downloads.Artifact != null)
                        {
                            if (item.Downloads.Artifact.Path != null)
                            {
                                res.Add(io.Path.Combine(dotMinecraftPath, "libraries", item.Downloads.Artifact.Path.Replace(@"/", "\\")));
                                if (io.File.Exists(io.Path.Combine(dotMinecraftPath, "libraries", item.Downloads.Artifact.Path.Replace(@"/", "\\"))) == false)
                                {
                                    FileNeedsToDownload.Add(new JsonFileInfo() { Url = item.Downloads.Artifact.Url, size = Convert.ToInt32(item.Downloads.Artifact.Size), localpath = io.Path.Combine(dotMinecraftPath, "libraries", item.Downloads.Artifact.Path.Replace(@"/", "\\")), sha1 = item.Downloads.Artifact.Sha1 });
                                }
                            }
                        }
                }
                foreach (var item in _Root.Libraries)
                {
                    if (item.Natives.Count != 0)
                    {
                        foreach (var file in item.Downloads.Classifiers)
                        {
                            var todo = new JsonFileInfo()
                            {
                                Url = file.Item.Url,
                                size = Convert.ToInt32(
                                file.Item.Size),
                                localpath = io.Path.Combine(dotMinecraftPath, "libraries", file.Item.Path.Replace(@"/", "\\")),
                                sha1 = file.Item.Sha1
                            };
                            NativeFilesNeedToDepackage.Add(todo);
                            if (!io.File.Exists(todo.localpath))
                            {
                                FileNeedsToDownload.Add(todo);
                            }
                        }
                    }
                }
                if (io.File.Exists(GameJarPath) != true)
                {
                    FileNeedsToDownload.Add(new JsonFileInfo() { Url=_Root.Downloads.Client.Url,sha1=_Root.Downloads.Client.Sha1,size=System.Convert.ToInt32(_Root.Downloads.Client.Size),localpath=GameJarPath });
                }
                return res;
            } 
        }

        public override string MainClassName => _Root.MainClass;

        

        public override string AssetsIndexName => _Root.AssetIndex.Id;

        public override string VersionType { get 
            {
                return _Root.Type;
            } 
        }

        public override List<JsonFileInfo> FileNeedsToDownload { get; set ; }
        public override List<JsonFileInfo> NativeFilesNeedToDepackage { get ; set ; }

        public ModLoaderType ModLoaderType { get; set; }
        public string BaseVersion { get {
                if (!string.IsNullOrEmpty(_Root.InheritsFrom))
                {
                    return _Root.InheritsFrom;
                }
                else
                {
                    return _Root.ClientVersion;
                }
            } set => throw new NotImplementedException(); }

        public override DeleteResult Delete()
        {
#if WPF
            var mb = MyMessageBox.Show("确定要删除吗？", "警告", MessageBoxButton.YesNo);
            //todo IO 操作
            if (mb.Result == MessageBoxResult.Yes)
            {
                return DeleteResult.OK;
            }
            else
            {
                return DeleteResult.Canceled;
            }
#elif AVALONIA
            return DeleteResult.OK;
            //TODO Avalonia 自己的搞法
#endif

        }

        public override void Refresh()
        {
            FileNeedsToDownload= new List<JsonFileInfo>();
            NativeFilesNeedToDepackage= new List<JsonFileInfo>();
        }

        public CLGameType(string JsonPath)
        {
            FileNeedsToDownload = new List<JsonFileInfo>();
            string otherArgsPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(JsonPath), "MEFLOtherArguments.json");
            _MSOAT =new MEFLOtherArgs(otherArgsPath);
            otherArgsPath = string.Empty;
            GameJsonPath=JsonPath;
            try
            {
                _Root = JsonConvert.DeserializeObject<Root>(System.IO.File.ReadAllText(JsonPath));
                if (_Root.MainClass == "cpw.mods.bootstraplauncher.BootstrapLauncher")
                {
                    ModLoaderType = ModLoaderType.Forge;
                }
                else
                {
                    ModLoaderType = ModLoaderType.AnyOrNone;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
    public class MEFLOtherArgs : GameotherArgs
    {
#region Privates
        private string _JsonPath { get; set; }
        private string _Description { get; set; }
        private string _OtherJVMArguments { get; set; }
        private string _OtherGameArguments { get; set; }
        private string _NativeLibrariesPath { get; set; }
        private string _CustomIconPath { get; set; }
        private string _customDotMCPath { get; set; }
        private GamePathType _gamePathType = GamePathType.DotMCPath;
#endregion
#region Props
        public override string Description { get => _Description; set { _Description = value; ChangeProperty(); } }
        public override string OtherJVMArguments { get => _OtherJVMArguments; set { _OtherJVMArguments = value; ChangeProperty(); } }
        public override string OtherGameArguments { get => _OtherGameArguments; set { _OtherGameArguments = value; ChangeProperty(); } }
        public override string NativeLibrariesPath { get => _NativeLibrariesPath; set { _NativeLibrariesPath = value; ChangeProperty(); } }
        public override string CustomIconPath { get => _CustomIconPath; set { _CustomIconPath = value; ChangeProperty(); } }
        public override bool StartInDebugMode { get ; set ; }
        public override string CustomDotMCPath { get => _customDotMCPath; set { _customDotMCPath = value;ChangeProperty(); } }

        public override GamePathType GamePathType { get => _gamePathType; set {
                _gamePathType=value;ChangeProperty(); } }

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
        public MEFLOtherArgs(string JsonPath)
        {
            _JsonPath = JsonPath;
            try
            {
                if (System.IO.File.Exists(_JsonPath) != true)
                {
                    System.IO.File.Create(_JsonPath).Close();
                }
                var args = JsonConvert.DeserializeObject<MEFLOtherArgs>(System.IO.File.ReadAllText(JsonPath));
                if (args != null)
                {
                    Description = args.Description;
                    OtherJVMArguments = args.OtherJVMArguments;
                    OtherGameArguments = args.OtherGameArguments;
                    NativeLibrariesPath = args.NativeLibrariesPath;
                    CustomIconPath = args.CustomIconPath;
                    GamePathType=args.GamePathType;
                }
                else
                {
                    StartInDebugMode = false;
                }
                GC.SuppressFinalize(args);
                args = null;
            }
            catch (Exception)
            {

            }
        }

    }

}
