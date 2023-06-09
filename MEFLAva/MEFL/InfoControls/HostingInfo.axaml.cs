using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Presenters;
using Avalonia.Media;
using Avalonia.Threading;
using DynamicData;
using MEFL.APIData;
using MEFL.Configs;
using MEFL.Contract;
using MEFL.PageModelViews;
using MEFL.Views;
using Newtonsoft.Json;
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
            MoreGrid.PointerEnter += MoreGrid_PointerEnter;
            MoreGrid.PointerLeave += MoreGrid_PointerLeave;
        }

        private void MoreGrid_PointerLeave(object? sender, Avalonia.Input.PointerEventArgs e)
        {
            Animations.MarginBack(new Thickness(30, 0, 0, 0)).RunAsync(MoreBtn, null);
            Animations.WidthBack(288.0).RunAsync(Enablebtn, null);
        }

        private void MoreGrid_PointerEnter(object? sender, Avalonia.Input.PointerEventArgs e)
        {
            Animations.MarginGo(new Thickness(0,0,0,0)).RunAsync(MoreBtn, null);
            Animations.WidthGo(258.0).RunAsync(Enablebtn, null);
        }
    }

    internal class HostingViewModel : PageModelViews.PageModelViewBase
    {
        DownloaderConfig downloaderConfig;
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
                if (value&&h.ImportsLoaded == false)
                {
                    h.LoadImports();
                }
                h.IsOpen = value;
                _isOpenlnk= value;
                ReLoadThis(value);
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
        private void ReLoadThis(bool value)
        {
            if (value)
            {
                if (!string.IsNullOrEmpty(h.ExceptionInfo))
                {
                    return;
                }
                if (h.Permissions.UsePagesAPI)
                {
                    foreach (var item in h.Pages.IconAndPage)
                    {
                        if(App.Current.ApplicationLifetime is ClassicDesktopStyleApplicationLifetime app)
                        {
                            Dispatcher.UIThread.InvokeAsync(() =>
                            {
                                var mw = app.MainWindow as MainWindow;
                                var btn = new Button() { Width = 30, Height = 30, Margin = new(3) };
                                btn.Click += new((x, e) => {
                                    mw.ClearPage();
                                    mw.Page.Children.Add(item.Value);
                                });
                                mw.ButtonForAddIns.Children.Add(btn);
                            });
                        }
                    }
                }
                if (h.Permissions.UseAccountAPI)
                {
                    foreach (var item in h.Account.GetSingUpAccounts(APIModel.SettingArgs))
                    {
                        APIModel.AccountConfigs.Add(item);
                        if (APIModel.SelectedAccountUUID.ToString() == item.Uuid.ToString())
                        {
                            APIModel.SelectedAccount = item;
                        }
                    }
                }
                if (h.Permissions.UseDownloadPageAPI)
                {
                    DownloadPage.AddPairs(h.Download.GetPairs(APIModel.SettingArgs));
                    try
                    {
                        downloaderConfig = JsonConvert.DeserializeObject<DownloaderConfig>(RegManager.Read("Downloader"));
                    }
                    catch
                    {
                        downloaderConfig = null;
                    }
                    foreach (var item in h.Download.GetDownloaders(APIModel.SettingArgs))
                    {
                        if (downloaderConfig != null)
                        {
                            if (downloaderConfig.DownloaderName == item.Name && item.FileName == downloaderConfig.FileName)
                            {
                                APIModel.SelectedDownloader= item;
                            }
                        }
                        APIModel.Downloaders.Add(item);
                    }
                    var arr = h.Download.GetDownloadSources(APIModel.SettingArgs).ToArray();
                    foreach (var item in arr)
                    {
                        APIModel.DownloadSources.AddItem(item);
                    }
                }
            }
            else
            {
                var lnq = APIModel.AccountConfigs.Where((x)=>x.AddInGuid==h.Guid).ToArray();
                if (lnq.Contains(APIModel.SelectedAccount))
                {
                    APIModel.SelectedAccount = null;
                }
                APIModel.AccountConfigs.RemoveMany(lnq);

                var dPairLq = DownloadPage.TabS.Where((x) => x.Tag.ToString() == h.Guid.ToString()).ToArray();
                DownloadPage.RemoveTabItems(dPairLq);

                var downerLq = APIModel.Downloaders.Where((x) => x.AddInGuid == h.Guid).ToArray();
                foreach (var item in downerLq)
                {
                    APIModel.Downloaders.Remove(item);
                }
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
                ReLoadThis(true);
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
                    h.LuncherGameType = h.cc.GetExport<IGameTypeManage>().Value;
                }
                if (h.Permissions.UseDownloadPageAPI)
                {
                    h.Download = h.cc.GetExport<IDownload>().Value;
                }
                if (h.Permissions.UseAccountAPI)
                {
                    h.Account = h.cc.GetExport<IAccount>().Value;
                }
                h.ImportsLoaded= true;
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
                h.ImportsLoaded= false;
            }
        }
    }
}
