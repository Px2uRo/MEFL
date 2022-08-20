using MEFL.PageModelViews;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
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
                            Page = null;
                            i++;
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
                    for (int i = 0; i < (App.Current.Resources["MainPage"] as Grid).Children.Count; i++)
                    {
                        if (((App.Current.Resources["MainPage"] as Grid).Children[i] as FrameworkElement).Tag.ToString().Contains(Hosting.Guid))
                        {
                            (App.Current.Resources["MainPage"] as Grid).Children.RemoveAt(i);
                            i--;
                        }
                    }
                    #endregion
                }
                #region Games
                (App.Current.Resources["RMPMV"] as RealMainPageModelView).RefreshFolderInfoCommand.Execute(null);
                #endregion
            }
            catch (Exception ex)
            {
                ErrorInfo = $"{ex.Message} at {ex.Source}";
            }
            Invoke(nameof(PublisherUri));
        }
        public bool IsOpen
        {
            get
            {
                UIReload(Hosting.IsOpen);
                return Hosting.IsOpen; 
            }
            set {
                Hosting.IsOpen = value;
                Invoke(nameof(IsOpen));
                Invoke(nameof(Icon));
                Invoke(nameof(ErrorInfo));
            }
        }

        public HostingModelView(Hosting hosting)
        {
            Hosting = hosting;
        }
    }
}
