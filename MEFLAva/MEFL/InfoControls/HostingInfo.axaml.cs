using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Media;
using MEFL.Contract;
using MEFL.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;

namespace MEFL.InfoControls
{
    public partial class HostingInfo : UserControl
    {
        public HostingInfo()
        {
            InitializeComponent();
        }
    }

    internal class HostingViewModel : PageModelViews.PageModelViewBase
    {
        Hosting h;
        bool _isOpenlnk = false;

        #region Props
        public string Name => h.FileName.Replace(Hosting.EndName, "");
        public string Description => h.Description;
        public string Publisher => h.Publisher; 
        public string Version => h.Version;
        public bool IsOpen
        {
            get { 
                _isOpenlnk = h.IsOpen; 
                return _isOpenlnk; }
            set
            {
                if (value&&h.Loaded == false)
                {
                    h.LoadImports();
                }
                h.IsOpen = value;
                _isOpenlnk= value;
                ReLoadThis();
                Invoke();
                Invoke(nameof(Icon));
            }
        }

        private object _iconlnk;

        public object Icon
        {
            get {
                if (_iconlnk == null)
                {
                    if (_isOpenlnk)
                    {
                        if (h.BaseInfo!=null)
                        {
                            _iconlnk= h.BaseInfo.Icon;
                        }
                    }
                }
                return _iconlnk; 
            }
        }

        #endregion

        #region Methods
        private void ReLoadThis()
        {
            if (h.IsOpen)
            {

            }
            else
            {

            }
        }
        #endregion

        #region ctor
        public HostingViewModel(Hosting hosting)
        {
            h = hosting;
            if (h.IsOpen)
            {
                hosting.LoadImports();
            }
            else
            {

            }
        }
        #endregion
    }

    internal static class HostingLoader
    {
        internal static void LoadImports(this Hosting h)
        {
            try
            {
                if (h.ac == null)
                {
                    h.ac = new AssemblyCatalog(h.FullPath);
                }
                h.cc = new CompositionContainer(h.ac);
                h.BaseInfo = h.cc.GetExport<IBaseInfo>().Value;
                h.Permissions = h.cc.GetExport<IPermissions>().Value;

                if (h.Permissions.UseSettingPageAPI)
                {
                    h.SettingPage = h.cc.GetExport<ISettingPage>().Value;
                }
                if (h.Permissions.UsePagesAPI)
                {
                    h.Pages = h.cc.GetExport<IPages>().Value;
                }
                if (h.Permissions.UseGameManageAPI)
                {
                    h.LuncherGameType = h.cc.GetExport<ILuncherGameType>().Value;
                }
                if (h.Permissions.UseDownloadPageAPI)
                {
                    h.Download = h.cc.GetExport<IDownload>().Value;
                }
                if (h.Permissions.UseAccountAPI)
                {
                    h.Account = h.cc.GetExport<IAccount>().Value;
                }
                h.Loaded= true;
                Debugger.Logger($"加载了一个插件，名称 {h.FileName}，版本：{h.Version} Guid {h.Guid}");
                h.ac.Dispose();
                h.cc.Dispose();
                h.ac = null;
                h.cc = null;
            }
            catch (Exception ex)
            {
                h.ExceptionInfo = $"{ex.Message} at {ex.Source}";
                h.IsOpen = false;
                h.Loaded= false;
            }
        }
    }
}
