using System;
using System.Collections.Generic;
using System.Text;
#if HOSTING
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
#endif
using MEFL.Contract;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using MEFL.APIData;

namespace MEFL
{
#if HOSTING
    public class Hosting
    {
        public override string ToString()
        {
            return FileName + Description;
        }

        [Import(AllowRecomposition = true)]
        public ISettingPage SettingPage;

        [Import(AllowRecomposition = true)]
        public IPermissions Permissions;

        [Import(AllowRecomposition=true)]
        public IBaseInfo BaseInfo;

        [Import(AllowRecomposition = true)]
        public IPages Pages;

        [Import(AllowRecomposition = true)]
        public ILuncherGameType LuncherGameType;

        [Import(AllowRecomposition = true)]
        public IDownload Download; 
        
        [Import(AllowRecomposition = true)]
        public IAccount Account;
        public string ExceptionInfo { get; set; }
        public string FileName { get; private set; }
        public string FullPath { get; private set; }
        public string Version { get; private set; }
        public string Publisher { get; private set; }
        public string Description { get; private set; }
        public string Guid { get; private set; }
        private bool _isOpen;
        AssemblyCatalog ac;
        CompositionContainer cc;
        public bool IsOpen { 
            get 
            {
                foreach (var item in APIData.APIModel.AddInConfigs)
                {
                    if (item.Guid==this.Guid)
                    {
                        _isOpen = item.IsOpen;
                        break;
                    }
                }
                return _isOpen;
            } set 
            {
                _isOpen = value;
                bool Contians = false;
                foreach (var item in APIModel.AddInConfigs)
                {
                    if (item.Guid == Guid)
                    {
                        Contians = true;
                    }
                }
                if (Contians)
                {
                    foreach (var item in APIModel.AddInConfigs)
                    {
                        if (item.Guid == Guid)
                        {
                            item.IsOpen = _isOpen;
                        }
                    }
                }
                else
                {
                    APIModel.AddInConfigs.Add(new AddInConfig() { Guid = Guid, IsOpen = _isOpen });
                }
                if (IsOpen)
                {
                    try
                    {
                        if (ac == null)
                        {
                            ac = new AssemblyCatalog(FullPath);
                        }
                        cc = new CompositionContainer(ac);
                        BaseInfo = cc.GetExport<IBaseInfo>().Value;
                        Permissions = cc.GetExport<IPermissions>().Value;

                        if (Permissions.UseSettingPageAPI)
                        {
                            SettingPage = cc.GetExport<ISettingPage>().Value;
                        }
                        if (Permissions.UsePagesAPI)
                        {
                            Pages = cc.GetExport<IPages>().Value;
                        }
                        if (Permissions.UseGameManageAPI)
                        {
                            LuncherGameType = cc.GetExport<ILuncherGameType>().Value;
                        }
                        if (Permissions.UseDownloadPageAPI)
                        {
                            Download = cc.GetExport<IDownload>().Value;
                        }
                        if (Permissions.UseAccountAPI)
                        {
                            Account = cc.GetExport<IAccount>().Value;
                        }
                        Debugger.Logger($"加载了一个插件，名称 {FileName}，版本：{Version} Guid {Guid}");
                        ac = null;
                        cc = null;
                    }
                    catch (Exception ex)
                    {
                        ExceptionInfo = $"{ex.Message} at {ex.Source}";
                        IsOpen = false;
                    }
                }
                else
                {
                    if (cc != null)
                    {
                        if (Permissions != null)
                        {
                            if (Permissions.UseSettingPageAPI)
                            {
                                cc.ReleaseExport(cc.GetExport<ISettingPage>());
                            }
                            if (Permissions.UsePagesAPI)
                            {
                                cc.ReleaseExport(cc.GetExport<IPages>());
                            }
                            if (Permissions.UseGameManageAPI)
                            {
                                cc.ReleaseExport(cc.GetExport<ILuncherGameType>());
                            }
                            if (Permissions.UseDownloadPageAPI)
                            {
                                cc.ReleaseExport(cc.GetExport<IDownload>());
                            }
                            if (Permissions.UseAccountAPI)
                            {
                                cc.ReleaseExport(cc.GetExport<IAccount>());
                            }
                        }

                        cc.Dispose();
                    }

                   Debugger.Logger($"卸载了一个插件，名称 {FileName}，版本：{Version} Guid {Guid}");
                }
                AddInConfig.Update(APIModel.AddInConfigs);
            } 
        }

        public static ObservableCollection<Hosting> LoadAll()
        {
            var res = new ObservableCollection<Hosting>();
            string path = System.IO.Path.Combine( 
                Environment.CurrentDirectory,"MEFL\\AddIns");
            List<FileInfo> l = new List<FileInfo>();
            if (System.IO.Directory.Exists(path) != true)
            {
                System.IO.Directory.CreateDirectory(path);
            }
            var di = new DirectoryInfo(path).GetFiles();
            var lg = new List<String>();
            foreach (var item in di)
            {
                if (item.Name.EndsWith(".mefl.dll"))
                {
                    res.Add(LoadOne(item.FullName));
                }
            }

            lg = null;
            return res;
        }

        private static Hosting LoadOne(string Path)
        {
            Hosting h = new Hosting();
            h.FullPath = Path;
            h.FileName = System.IO.Path.GetFileName(Path);
            try
            {
                try
                {
                    Assembly assembly = Assembly.LoadFile(Path);
                    h.Guid = assembly.ManifestModule.ModuleVersionId.ToString();
                    foreach (var item in assembly.CustomAttributes)
                    {
                        if (item.AttributeType == typeof(AssemblyCompanyAttribute))
                        {
                            foreach (var arg in item.ConstructorArguments)
                            {
                                h.Publisher += arg.ToString();
                            }
                        }
                        else if (item.AttributeType == typeof(AssemblyDescriptionAttribute))
                        {
                            foreach (var arg in item.ConstructorArguments)
                            {
                                h.Description += arg.ToString();
                            }
                        }
                        else if (item.AttributeType == typeof(AssemblyFileVersionAttribute))
                        {
                            foreach (var arg in item.ConstructorArguments)
                            {
                                h.Version += arg.ToString();
                            }
                        }
                    }
                    assembly = null;
                }
                catch (Exception ex)
                {
                    throw new Exception("Guid 不合法或者无法获取其 Guid");
                }
            }
            catch(Exception ex)
            {
                h.ExceptionInfo = $"{ex.Message} at {ex.Source}";
            }

            bool Contians = false;
            foreach (var item in APIModel.AddInConfigs)
            {
                if (item.Guid == h.Guid)
                {
                    Contians = true;
                }
            }
            if (Contians)
            {
                foreach (var item in APIModel.AddInConfigs)
                {
                    if (item.Guid == h.Guid)
                    {
                        h.IsOpen = item.IsOpen;
                    }
                }
            }
            else
            {
                h.IsOpen = APIModel.AlwaysOpenNewAddIns;
            }
            return h;
        }
    }
#endif
}
