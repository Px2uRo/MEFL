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

namespace MEFL
{
#if HOSTING
    public class Hosting
    {

        [Import(AllowRecomposition = true)]
        public ISettingPage SettingPage;

        [Import(AllowRecomposition = true)]
        public IPermissions Permissions;

        [Import(AllowRecomposition=true)]
        public IBaseInfo BaseInfo;

        public Exception ExceptionInfo { get; set; }
        public string FileName { get; set; }
        public string Version { get; set; }
        public string Publisher { get; set; }
        public string Description { get; set; }
        public object Icon { get; set; }
        public Uri PulisherUri { get; set; }
        public Uri ExtensionUri { get; set; }
        public string Guid { get; set; }

        public static Hosting LoadOne(string Path)
        {
            Hosting h = new Hosting();
            h.FileName = System.IO.Path.GetFileName(Path);
            try
            {
                var fvif = FileVersionInfo.GetVersionInfo(Path);
                h.Version = fvif.FileVersion;
                h.Publisher = fvif.CompanyName;
                h.Description = fvif.FileDescription;

                try
                {
                    Assembly assembly = Assembly.LoadFile(Path);
                    string guid = assembly.ManifestModule.ModuleVersionId.ToString(); 
                    System.Guid.Parse(guid);
                    h.Guid = guid;
                    guid = null;
                    guid = null;
                }
                catch (Exception ex)
                {
                    throw new Exception("Guid 不合法或者无法获取其 Guid");
                }


                var ac = new AssemblyCatalog(Path);
                var cc = new CompositionContainer(ac);
                h.BaseInfo = cc.GetExport<IBaseInfo>().Value;
                h.Permissions = cc.GetExport<IPermissions>().Value;
                h.Icon = h.BaseInfo.Icon;
                h.PulisherUri = h.BaseInfo.PulisherUri;
                h.ExtensionUri = h.BaseInfo.ExtensionUri;
                
                if (h.Permissions.UseSeetingPageAPI == true)
                {
                    h.SettingPage = cc.GetExport<ISettingPage>().Value;
                }
                fvif = null;
                ac = null;
                cc = null;
                Debugger.Logger($"加载了一个插件，名称 {h.FileName}，版本：{h.Version} Guid {h.Guid}");
                return h;
            }
            catch (Exception ex)
            {
                h.ExceptionInfo = ex;
                return h;
                h = null;
            }
        }


        public static Hosting[] LoadAll()
        {
            Hosting[] hc;
            string path = System.IO.Path.Combine( 
                Environment.CurrentDirectory,"AddIns");
            List<FileInfo> l = new List<FileInfo>();
            if (System.IO.Directory.Exists(path) != true)
            {
                System.IO.Directory.CreateDirectory(path);
            }
            var fls = new DirectoryInfo(path).GetFiles();
            hc = new Hosting[fls.Length];
            for (int i = 0; i < fls.Length; i++)
            {
                hc.SetValue(LoadOne(fls[i].FullName),i);
            }
            fls = null;
            l = null;
            path = null;
            return hc;
        }

    }
#endif
}
