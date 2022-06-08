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

namespace MEFL
{
#if HOSTING
    public class Hosting
    {
        [Import(AllowRecomposition = true)]
        public IBaseAddIn BaseAddIn;

        [Import(AllowRecomposition = true)]
        public ISettingPage SettingPage;

        public Exception ExceptionInfo;
        public string FileName;

        public static ObservableCollection<Hosting> LoadAll()
        {
            ObservableCollection<Hosting> hc = new ObservableCollection<Hosting>();
            string path = System.IO.Path.Combine( 
                Environment.CurrentDirectory,"AddIns");
            List<FileInfo> l = new List<FileInfo>();
            if (System.IO.Directory.Exists(path) != true)
            {
                System.IO.Directory.CreateDirectory(path);
            }
            foreach (var item in new DirectoryInfo(path).GetFiles())
            {
                if (item.Name.EndsWith(".dll") && item.Name!="Contract.dll")
                {
                    l.Add(item);
                }
            }
            foreach (var item in l)
            {
                hc.Add(LoadOne(item.FullName));
            }
            l = null;
            path = null;
            return hc;
        }

        public static Hosting LoadOne(string Path)
        {
            Hosting h = new Hosting();
            h.FileName = System.IO.Path.GetFileName(Path);
            try
            {
                var ac = new AssemblyCatalog(Path);
                var cc = new CompositionContainer(ac);
                h.BaseAddIn = cc.GetExport<IBaseAddIn>().Value;
                try
                {
                    Guid.Parse(h.BaseAddIn.BaseInfo().Guid);
                }
                catch (Exception ex)
                {
                    throw new Exception($"非法 Uuid，详细信息：{ex.Message}");
                }
                if (h.BaseAddIn.Permissions().UseSeetingPageAPI == true)
                {
                    h.SettingPage = cc.GetExport<ISettingPage>().Value;
                }
                return h;
                ac = null;
                cc = null;
            }
            catch (Exception ex)
            {
                h.ExceptionInfo = ex;
                return h;
                h = null;
            }
        }
    }
#endif
}
