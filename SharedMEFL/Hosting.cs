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
        public bool IsOpen { get; set; }

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
                    assembly = null;
                    guid = null;
                }
                catch (Exception ex)
                {
                    throw new Exception("Guid 不合法或者无法获取其 Guid");
                }

                foreach (var item in APIData.APIModel.AddInConfigs)
                {
                    if (item.Guid == h.Guid)
                    {
                        h.IsOpen = item.IsOpen;
                    }
                    else
                    {
                        h.IsOpen = false;
                    }
                }
                if (h.IsOpen == null)
                {
                    h.IsOpen = false;
                }

                if (h.IsOpen == true)
                {
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
                    Debugger.Logger($"加载了一个插件，名称 {h.FileName}，版本：{h.Version} Guid {h.Guid}");
                    ac = null;
                    cc = null;
                }
                fvif = null;
                return h;
                h = null;
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
            var di = new DirectoryInfo(path).GetFiles();
            var lg = new List<String>();
            foreach (var item in di)
            {
                if (item.Name.EndsWith(".dll"))
                {
                    try
                    {
                        Assembly assembly = Assembly.LoadFile(item.FullName);
                        string guid = assembly.ManifestModule.ModuleVersionId.ToString();
                        System.Guid.Parse(guid);
                        assembly = null;
                        if (lg.Contains(guid)!=true)
                        {
                            l.Add(item);
                            lg.Add(guid);
                        }
                        guid = null;
                    }
                    catch (Exception ex)
                    {
                        
                    }

                }
            }
            lg = null;
            hc = new Hosting[l.Count];
            for (int i = 0; i < l.Count; i++)
            {
                var h = LoadOne(l[i].FullName);
                for (int j = 0; j < APIData.APIModel.AddInConfigs.Count|| APIData.APIModel.AddInConfigs.Count==0; j++)
                {
                    if(APIData.APIModel.AddInConfigs.Count == 0)
                    {
                        APIData.AddInConfig item = new APIData.AddInConfig() { Guid = h.Guid, IsOpen = h.IsOpen };
                        APIData.APIModel.AddInConfigs.Add(item);
                        item = null;
                    }
                    else if (APIData.APIModel.AddInConfigs[j].Guid != h.Guid)
                    {
                        APIData.AddInConfig item = new APIData.AddInConfig() { Guid = h.Guid, IsOpen = h.IsOpen };
                        APIData.APIModel.AddInConfigs.Add(item);
                        item = null;
                    }
                }
                hc.SetValue(h,i);
                h=null;
            }
            APIData.AddInConfig.Update(APIData.APIModel.AddInConfigs);
            l = null;
            path = null;
            di=null;
            return hc;
        }

    }
#endif
}
