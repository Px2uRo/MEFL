using MEFL.PageModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ModelView = MEFL.PageModelViews.DownloadPageModelView;
using MEFL.Controls;
using System.Collections;
using CoreLaunching.JsonTemplates;
using MEFL.Contract;
using MEFL.APIData;
using MEFL.SpecialPages;

namespace MEFL.Pages
{
    /// <summary>
    /// DownloadPage.xaml 的交互逻辑
    /// </summary>
    public partial class DownloadPage : MyPageBase
    {
        protected override Size MeasureOverride(Size constraint)
        {
            PART_Boxes.Width = constraint.Width - 200;
            PART_Boxes.Height = this.ActualHeight;
            return base.MeasureOverride(constraint);
        }
        public bool Inied;
        public DownloadPage()
        {
            this.DataContext = ModelView.ModelView;
            ModelView.ModelView.PropertyChanged += ModelView_PropertyChanged;
            ModelView.UI=this;
            InitializeComponent();
            ModelView.ModelView.HasErrors = true;
            ModelView.ModelView.ErrorDescription = "没有要显示的项目";
        }
        int ChildCount = 0;
        private void ModelView_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var mv = sender as ModelView;
            if(e.PropertyName == "ItemSource")
            {
                Dispatcher.Invoke(() => {
                    for (int i = 0; i < MySP.Children.Count; i++)
                    {
                        GC.SuppressFinalize(MySP.Children[i]);
                        MySP.Children.RemoveAt(i);
                        i--;
                    }
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    ChildCount = MySP.Children.Count;
                });
                if (mv.ItemSource.Count >= ChildCount)
                {
                    for (int i = 0; i < mv.ItemSource.Count; i++)
                    {
                        if (ChildCount > i)
                        {
                            Dispatcher.Invoke(() => {
                                (MySP.Children[i] as MyItemsCard).ItemsSource = mv.ItemSource[i];
                                (MySP.Children[i] as MyItemsCard).Title = mv.ItemSource[i].VersionMajor;
                            });
                        }
                        else
                        {
                            Dispatcher.Invoke(() => {
                                MySP.Children.Add(new MyItemsCard() { ItemsSource = mv.ItemSource[i], Title = mv.ItemSource[i].VersionMajor });
                            });
                            ChildCount++;
                        }
                    }
                }
                Dispatcher.Invoke(() => {
                    for (int i = 0; i < MySP.Children.Count; i++)
                    {
                        if (((MySP.Children[i] as MyItemsCard).ItemsSource as IList).Count == 0)
                        {
                            MySP.Children.RemoveAt(i);
                            i--;
                        }
                    }
                    for (int i = 0; i < MySP.Children.Count; i++)
                    {
                        if(i > 2)
                        {
                            (MySP.Children[i] as MyItemsCard).IsSwaped = true;
                        }
                    }
                    if (MySP.Children.Count == 0)
                    {
                        mv.HasErrors = true;
                    }
                });
                mv.IsRefreshing = false;
            }
            else if (e.PropertyName == "IsRefreshing")
            {
                Dispatcher.Invoke(() => {
                    if (mv.IsRefreshing == true)
                    {
                        RefreshingBox.Visibility = Visibility.Visible;
                        MySP.Visibility = Visibility.Hidden;
                        ErrorBox.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        RefreshingBox.Visibility = Visibility.Hidden;
                        if (mv.HasErrors == true)
                        {
                            ErrorBox.Visibility = Visibility.Visible;
                            MySP.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            ErrorBox.Visibility = Visibility.Hidden;
                            MySP.Visibility = Visibility.Visible;
                        }
                    }
                });
            }
        }

        private void DownloadItem(object sender, MouseButtonEventArgs e)
        {
            var file = (sender as MyItemsCardItem).DataContext as LauncherWebVersionInfo;
            if(APIModel.SelectedDownloader == null)
            {
                MessageBox.Show("没有选中的下载器，去整个插件先？");
            }
            else
            {
                var result = file.Download(APIModel.SelectedDownloader, APIModel.MyFolders[APIModel.SelectedFolderIndex].Path, APIModel.SettingArgs, APIModel.DownloadSources.Selected);
                if (result.HasError != true) {
                    DownloadingProgressPageModel.ModelView.DownloadingProgresses.Add(result.Progress);
                }
                else
                {
                    result.DownloadButtonClickEvent += Result_DownloadButtonClickEvent;
                    WebListRefresher.SovlePage.Content=result.Page;
                    WebListRefresher.SovlePage.DataContext = result;
                    WebListRefresher.ShowSovlePage();
                }
            }
        }

        private void Result_DownloadButtonClickEvent(LauncherProgressResult result)
        {
            DownloadingProgressPageModel.ModelView.DownloadingProgresses.Add(result.Progress);
            WebListRefresher.CleanSovlePage();
            WebListRefresher.GoToDownloadProgressPage();
        }
    }
}
