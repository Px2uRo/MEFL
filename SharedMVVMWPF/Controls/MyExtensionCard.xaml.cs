using MEFL.APIData;
using MEFL.Contract;
using MEFL.PageModelViews;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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

namespace MEFL.Controls
{
    /// <summary>
    /// MyExtensionCard.xaml 的交互逻辑
    /// </summary>
    public partial class MyExtensionCard : MEFL.Controls.MyCard
    {
        public MyExtensionCard()
        {
            InitializeComponent();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            //Hosting.IsOpen==
            if ((DataContext as HostingModelView).IsOpen!=true)
            {
                MessageBox.Show("请启用插件后重试");
            }
            else 
            {
                try
                {
                    Process.Start("explorer.exe", (sender as Hyperlink).NavigateUri.ToString());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }
    }

    public class HostingModelView : PageModelViews.PageModelViewBase
    {
        public Visibility IsRefreshing
        {
            get
            {
                Invoke(nameof(IsNotRefreshing));
                if (GameRefresher.Refreshing)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Hidden;
                }
            }
        }

        public Visibility IsNotRefreshing
        {
            get
            {
                if (!GameRefresher.Refreshing)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Hidden;
                }
            }
        }

        Hosting Hosting;

        public Visibility NoError
        {
            get
            {
                if(HasError == Visibility.Hidden)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Hidden;
                }
            }
        }
        private Visibility _hasError = Visibility.Hidden;
        public Visibility HasError { get {
                return _hasError;
            } private set { _hasError = value; } }
        public string FileName => Hosting.FileName;
        public string Publisher => Hosting.Publisher;
        public string Description => Hosting.Description;
        public string Version => Hosting.Version;
        public Uri PublisherUri {

            get {
                if (Hosting.BaseInfo == null)
                {
                    return null;
                }
                else
                {
                    return Hosting.BaseInfo.PulisherUri;
                }
            }
        }
        public string ErrorInfo { get {
                if (!String.IsNullOrEmpty(Hosting.ExceptionInfo))
                {
                    HasError = Visibility.Visible;
                    Invoke(nameof(HasError));
                    Invoke(nameof(NoError));
                }
                else
                {
                    HasError = Visibility.Hidden;
                }
                return Hosting.ExceptionInfo;
            }
            set {
                Hosting.ExceptionInfo = value;
                Invoke(nameof(HasError));
            }
        }
        public object Icon { get
            {
                if (IsOpen == true)
                {
                    try
                    {
                        return Hosting.BaseInfo.Icon;
                    }
                    catch (Exception ex)
                    {
                        HasError = Visibility.Visible;
                        Invoke(nameof(HasError));
                        return "获取图标失败";
                    }
                }
                else
                {
                    return "已关闭";
                }
            } 
        }
        void UIReload(bool IsOpen)
        {
            try
            {
                if (IsOpen)
                {
                    UIReload(false);
                    #region Pages
                    if (Hosting.Permissions.UsePagesAPI)
                    {
                        int i = 0;
                        foreach (var Dir in Hosting.Pages.IconAndPage)
                        {
                            ChangePageButton button = new ChangePageButton()
                            {
                                Width = 45,
                                Content = Dir.Key,
                                Tag = $"{Hosting.Guid}-Pages-{i}"
                            };
                            (App.Current.Resources["AddInChangePageButtons"] as StackPanel).Children.Add(button);
                            MyPageBase Page = Dir.Value;
                            Page.Tag = $"{Hosting.Guid}-Pages-{i}";
                            Page.Visibility = System.Windows.Visibility.Hidden;
                            (App.Current.Resources["MainPage"] as Grid).Children.Add(Page);
                            Hosting.Pages.Added(i,APIData.APIModel.SettingArgs);
                            Page = null;
                            i++;
                        }
                    }
                    #endregion
                    #region Accounts
                    if (Hosting.Permissions.UseAccountAPI)
                    {
                        foreach (var item in Hosting.Account.GetSingUpAccounts(APIData.APIModel.SettingArgs))
                        {
                            APIData.APIModel.AccountConfigs.Add(item);
                            if(item.Uuid != null)
                            {
                                if (APIModel.SelectedAccountUUID == item.Uuid)
                                {
                                    APIModel.SelectedAccount=item;
                                }
                            }
                        }
                    }
                    #endregion
                    #region Downloads
                    if (Hosting.Permissions.UseDownloadPageAPI)
                    {
                        foreach (var pair in Hosting.Download.GetPairs(APIData.APIModel.SettingArgs))
                        {
                            var btn = new ChangePageContentButton() { Content = pair.Title ,Tag=$"{Hosting.Guid}:{pair.Tag}",DataContext=pair};
                            btn.Checked += Btn_Checked;
                            if (DownloadPageModelView.UI != null)
                            {
                                App.Current.Dispatcher.Invoke(() =>
                                {
                                    (DownloadPageModelView.UI.SideBar as StackPanel).Children.Add(btn);
                                });
                                if (!DownloadPageModelView.UI.Inied && (DownloadPageModelView.UI.SideBar as StackPanel).Children.Count > 0)
                                {
                                    ((DownloadPageModelView.UI.SideBar as StackPanel).Children[0] as ChangePageContentButton).IsChecked = true;
                                    DownloadPageModelView.UI.Inied = true;
                                }
                            }
                            else
                            {
                                new Thread(() =>
                                {
                                    Thread.Sleep(2000);
                                    App.Current.Dispatcher.Invoke(() =>
                                    {
                                        if (DownloadPageModelView.UI != null)
                                        {
                                            App.Current.Dispatcher.Invoke(() =>
                                            {
                                                (DownloadPageModelView.UI.SideBar as StackPanel).Children.Add(btn);
                                            });
                                            if (!DownloadPageModelView.UI.Inied && (DownloadPageModelView.UI.SideBar as StackPanel).Children.Count > 0)
                                            {
                                                ((DownloadPageModelView.UI.SideBar as StackPanel).Children[0] as ChangePageContentButton).IsChecked = true;
                                                DownloadPageModelView.UI.Inied = true;
                                            }
                                        }
                                    });
                                }).Start();
                            }
                        }
                        foreach (var item in Hosting.Download.GetDownloaders(APIData.APIModel.SettingArgs))
                        {
                            APIModel.Downloaders.Add(item);
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Pages
                    for (int i = 0; i < (App.Current.Resources["AddInChangePageButtons"] as StackPanel).Children.Count; i++)
                    {
                        if (((App.Current.Resources["AddInChangePageButtons"] as StackPanel).Children[i] as FrameworkElement).Tag.ToString().Contains(Hosting.Guid))
                        {
                            (App.Current.Resources["AddInChangePageButtons"] as StackPanel).Children.RemoveAt(i);
                            i--;
                        }
                    }
                    int AddInPageIndex = 0;
                    for (int i = 0; i < (App.Current.Resources["MainPage"] as Grid).Children.Count; i++)
                    {
                        if (((App.Current.Resources["MainPage"] as Grid).Children[i] as FrameworkElement).Tag.ToString().Contains(Hosting.Guid))
                        {
                            (App.Current.Resources["MainPage"] as Grid).Children.RemoveAt(i);
                            Hosting.Pages.Delected(AddInPageIndex, APIData.APIModel.SettingArgs);
                            AddInPageIndex++;
                            i--;
                        }
                    }
                    #endregion
                    #region Accounts
                        for (int i = 0; i < APIModel.AccountConfigs.Count; i++)
                        {
                            var item = APIModel.AccountConfigs[i];
                            if (item.AddInGuid == Hosting.Guid)
                            {
                                item.Dispose();
                                APIModel.AccountConfigs.Remove(item);
                                i--;
                            }
                        }
                    
                    if (APIData.APIModel.SelectedAccount != null)
                    {
                        if (APIData.APIModel.SelectedAccount.AddInGuid == Hosting.Guid)
                        {
                            APIData.APIModel.SelectedAccount = null;
                        }
                    }
                    #endregion
                    #region Downloads
                        if (DownloadPageModelView.UI != null)
                        {
                        for (int i = 0; i < (DownloadPageModelView.UI.SideBar as StackPanel).Children.Count; i++)
                        {
                            var btn = (FrameworkElement)(DownloadPageModelView.UI.SideBar as StackPanel).Children[i];
                            if (btn.Tag.ToString().Contains(Hosting.Guid))
                            {
                                i--;
                                (DownloadPageModelView.UI.SideBar as StackPanel).Children.Remove(btn);
                                DownloadPageItemPair pair = btn.DataContext as DownloadPageItemPair;
                                if(pair.Contents != null)
                                {
                                    pair.Dispose();
                                }
                                GC.SuppressFinalize(btn);
                            }
                        }
                            DownloadPageModelView.ModelView.Invoke("ItemSource");
                            if ((DownloadPageModelView.UI.SideBar as StackPanel).Children.Count == 0)
                            {
                            DownloadPageModelView.UI.Inied = false;
                            }
                        }

                    for (int i = 0; i < APIModel.Downloaders.Count; i++)
                    {
                        if (APIModel.Downloaders[i].AddInGuid == Hosting.Guid)
                        {
                            APIModel.Downloaders.RemoveAt(i);
                            i--;
                        }
                    }

                    #endregion
                }
                #region Games
                (App.Current.Resources["RMPMV"] as RealMainPageModelView).RefreshFolderInfoCommand.Execute("Force");
                #endregion
                GameRefresher.Refreshing = true;
                App.Current.Dispatcher.Invoke(() =>
                {
                    foreach (var item in HostingsToUI.res.Children)
                    {
                        ((item as MyExtensionCard).DataContext as HostingModelView).Invoke("IsRefreshing");
                    }
                });
                GameRefresher.RefreshCurrect();
            }
            catch (Exception ex)
            {
                if (Hosting.ExceptionInfo != null)
                {
                    ErrorInfo = Hosting.ExceptionInfo;
                }
                else
                {
                    ErrorInfo = $"{ex.Message} at {ex.Source}";
                }
            }
            Invoke(nameof(IsOpen));
            Invoke(nameof(PublisherUri));
            Invoke(nameof(Icon));
            Invoke(nameof(ErrorInfo));
        }

        private void Btn_Checked(object sender, RoutedEventArgs e)
        {
            DownloadPageItemPair pair = null ;
            WebListRefresher.IsRefreshing = true;
            DownloadPageModelView.ModelView.Invoke("IsRefreshing");
            new Thread(() =>
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    pair = (sender as FrameworkElement).DataContext as DownloadPageItemPair;
                });
                new Thread(() =>
                {
                    pair.WebRefresh(APIModel.SettingConfig.TempFolderPath);
                    pair.RefreshList(APIModel.SettingConfig.TempFolderPath);
                }).Start();
                if (pair != null)
                {
                    Thread.Sleep(200);
                    while (pair.IsRefreshing)
                    {

                    }
                    if (pair.HasError)
                    {
                        DownloadPageModelView.ModelView.ErrorDescription = pair.ErrorDescription;
                        DownloadPageModelView.ModelView.Invoke("ErrorDescription");
                    }
                    else
                    {
                        DownloadPageModelView.ModelView.ItemSource = pair.Contents;
                    }
                    DownloadPageModelView.ModelView.HasErrors = pair.HasError;
                    DownloadPageModelView.ModelView.Invoke("HasErrors");
                    DownloadPageModelView.ModelView.Invoke("IsRefreshing");
                    DownloadPageModelView.ModelView.Invoke("ItemSource");
                }
            }).Start();
        }

        public bool IsOpen
        {
            get
            {
                return Hosting.IsOpen; 
            }
            set
            {
                if (value)
                {
                    Hosting.IsOpen = value;
                    UIReload(value);
                }
                else
                {
                    UIReload(value);
                    Hosting.IsOpen = value;
                }
            }
        }

        public HostingModelView(Hosting hosting)
        {
            Hosting = hosting;
            UIReload(Hosting.IsOpen);
        }
    }
}
