using CoreLaunching.JsonTemplates;
using MEFL.Arguments;
using MEFL.Contract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using io = System.IO;

namespace MEFL.GameTypes
{
    public class MEFLRealseType : MEFL.Contract.GameInfoBase
    {
        public override FrameworkElement GetManageProcessPage(Process process, SettingArgs args)
        {
            return _managePage;
        }
        public override void Dispose()
        {

        }
        private static FrameworkElement _settingPage = new SpecialPages.MEFLRealseTypeSetting();
        public static FrameworkElement _managePage = new SpecialPages.MEFLRealseTypeManage();
        private MEFLStandardOtherArgumentTemplate _MSOAT { get; set; }
        private CoreLaunching.JsonTemplates.Root _Root { get; set; }
        public override string GameTypeFriendlyName { get => App.Current.Resources["I18N_String_MEFLGameInfos_Realse"].ToString(); set => throw new NotImplementedException(); }
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
                if (_MSOAT.NativeLibrariesPath != null)
                {
                    return _MSOAT.NativeLibrariesPath;
                }
                else
                {
                    return System.IO.Path.Combine(dotMinecraftPath, "natives");
                }
            }
            set
            {
                _MSOAT.NativeLibrariesPath = value;
            }
        }
        public override List<Root_Libraries> GameLibraries { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string GameArgs { get 
            {
                var res = string.Empty;
                if (_Root.Arguments!=null)
                {
                    foreach (var item in _Root.Arguments.Game.Array)
                    {
                        res += $" {item}";
                    }
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
        public override bool LaunchByLauncher => true;

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
                        res.Add(io.Path.Combine(dotMinecraftPath,"libraries", item.Downloads.Artifact.Path.Replace(@"/","\\")));
                    }
                }
                return res;
            } 
        }

        public override string MainClassName => _Root.MainClass;

        public override string AssetsRoot => io.Path.Combine(dotMinecraftPath,"assets");

        public override string AssetsIndexName => _Root.AssetIndex.Id;

        public override string VersionType => _Root.Type;

        public override Process Launch(SettingArgs args)
        {
            throw new NotImplementedException();
        }

        public override void Delete()
        {
            Dispose();

        }

        public MEFLRealseType(string JsonPath)
        {
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
