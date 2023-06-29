using Avalonia;
using Avalonia.Controls;
using MEFL.APIData;
using MEFL.Arguments;
using MEFL.AvaControls;
using MEFL.Contract;
using MEFL.PageModelViews;
using MEFL.Views;
using MEFL.Views.DialogContents;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MEFL.InfoControls
{
    public partial class DownloadItemButton : UserControl
    {
        public DownloadItemButton()
        {
            InitializeComponent();
            Excutebtn.Click += Excutebtn_Click;
            MoreOptionGrid.PointerEnter += MoreOptionGrid_PointerEnter;
            MoreOptionGrid.PointerLeave += MoreOptionGrid_PointerLeave;
            MoreOptionBtn.Click += MoreOptionBtn_Click;
        }

        private void MoreOptionBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ContentDialog.Show(DownloaderContextMenu.GetUI(DataContext as LauncherWebVersionInfo));
        }

        private void MoreOptionGrid_PointerLeave(object? sender, Avalonia.Input.PointerEventArgs e)
        {
            Animations.MarginBack(new Thickness(30, 0, 0, 0)).RunAsync(MoreOptionBtn, null);
            Animations.WidthBack(288.0).RunAsync(Excutebtn, null);
        }

        private void MoreOptionGrid_PointerEnter(object? sender, Avalonia.Input.PointerEventArgs e)
        {
            Animations.MarginGo(new(0, 0, 0, 0)).RunAsync(MoreOptionBtn, null);
            Animations.WidthGo(258.0).RunAsync(Excutebtn, null);
        }

        private void Excutebtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if(APIModel.SelectedDownloader == null)
            {
                WaringDialog.Show("没有选中的下载器，请查看设置");
            }
            else
            {
                var dotMc = APIModel.MyFolders[APIModel.SelectedFolderIndex].Path;
                InstallProcess progress = null;
                var zainfo = DataContext as LauncherWebVersionInfo;
                if(zainfo.DirectDownload(APIModel.Javas.ToArray(),dotMc, out var page,out var args))
                {
                    var localF = DownloadingProgressPageModel.ModelView.DownloadingProgresses.GetUsingFiles();
                    InstallArguments fargs;
                    if (args.Count()==0)
                    {
                        progress = APIModel.SelectedDownloader.InstallMinecraft(zainfo.Url,
dotMc, APIModel.DownloadSources.Selected, new InstallArguments[] {
new(APIModel.Javas.ToArray(), zainfo.Id, dotMc, null,this.DataContext as LauncherWebVersionInfo)},
localF);
                    }
                    else
                    {
                        progress = APIModel.SelectedDownloader.InstallMinecraft(zainfo.Url,
                        dotMc, APIModel.DownloadSources.Selected,args,localF);
                    }
                    if (progress!=null)
                    {
                        DownloadingProgressPageModel.ModelView.DownloadingProgresses.Add(progress);
                    }
                    else
                    {
                        WaringDialog.Show("无效的下载进程，去找插件开发者去。");
                    }
                }
                else
                {
                    page.Solved += Page_Solved;
                    ContentDialog.Show(page);
                }
            }
        }

        private void Page_Solved(object? sender, IEnumerable<InstallArguments> e)
        {
            InstallArguments fargs;
            if((sender as IInstallPage).Info != null)
            {
                var dotMc = APIModel.MyFolders[APIModel.SelectedFolderIndex].Path;
                var localF = DownloadingProgressPageModel.ModelView.DownloadingProgresses.GetUsingFiles();
                if (e.Count()==0)
                {
                    var jRE = APIModel.SettingArgs.SelectedJava;
                    fargs = new(APIModel.Javas.ToArray(),(sender as IInstallPage).Info.Id, dotMc, null,DataContext as LauncherWebVersionInfo);
                    var progress = APIModel.SelectedDownloader.InstallMinecraft((sender as IInstallPage).Info.Url,
                                dotMc, APIModel.DownloadSources.Selected, new[] {fargs}, localF);
                    if (progress != null)
                    {
                        DownloadingProgressPageModel.ModelView.DownloadingProgresses.Add(progress);
                    }
                    else
                    {
                        WaringDialog.Show("无效的下载进程，去联系插件开发者。");
                    }
                }
                else
                {

                    var progress = APIModel.SelectedDownloader.InstallMinecraft((sender as IInstallPage).Info.Url,
                                dotMc, APIModel.DownloadSources.Selected, e, localF);
                    if (progress != null)
                    {
                        DownloadingProgressPageModel.ModelView.DownloadingProgresses.Add(progress);
                    }
                    else
                    {
                        WaringDialog.Show("无效的下载进程，去联系插件开发者。");
                    }
                }
            }
            else
            {
                WaringDialog.Show("IInstallPage.Info == null，请联系插件开发者");
            }
            ContentDialog.Quit();
        }

        public DownloadItemButton(LauncherWebVersionInfo info):this()
        {
            this.DataContext= info;
        }
    }
}
