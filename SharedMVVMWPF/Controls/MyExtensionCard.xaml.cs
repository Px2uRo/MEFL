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

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (Hosting == null)
            {
                throw new Exception("未设置 Hosting 属性");
            }
            if(Hosting.ExceptionInfo != null)
            {
                base.Title = $"加载文件 {Hosting.FileName} 错误";
                PART_THE_Content.Children.Clear();
                PART_THE_Content.Children.Add
                    (new TextBlock() { Text = Hosting.ExceptionInfo.Message });
            }
            else if(Hosting.IsOpen==true)
            {
                if (Hosting.BaseInfo.Title != null)
                {
                    this.Title = Hosting.BaseInfo.Title;
                }
                else
                {
                    this.Title = Hosting.FileName;
                }
            }
            else
            {
                Hosting.Icon = "已关闭";
                this.Title = Hosting.FileName;
            }
            PART_THE_Content.DataContext = Hosting;
        }



        public Hosting Hosting
        {
            get { return (Hosting)GetValue(HostingProperty); }
            set { SetValue(HostingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Hosting.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HostingProperty =
            DependencyProperty.Register("Hosting", typeof(Hosting), typeof(MyExtensionCard), new PropertyMetadata(null));

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if (Hosting.IsOpen==false)
            {
                MessageBox.Show("请启用插件后重试");
            }
            else 
            {
                try
                {
                    Process.Start("explorer.exe", (sender as Hyperlink).NavigateUri.AbsoluteUri.ToString());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        private void MyCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Hosting.IsOpen = true;
            try
            {
                ReloadThis();
            }
            catch (Exception ex)
            {
                PART_THE_Content.Children.Clear();
                PART_THE_Content.Children.Add(new TextBlock() { Text=ex.Message});
            }
            PART_THE_Content.DataContext = Hosting;
        }
        private void MyCheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
            Hosting.IsOpen = false;
            RemoveThis();
            Hosting.Icon = "已关闭";
            PART_THE_Content.DataContext = Hosting;
        }

        private void ReloadThis()
        {
            foreach (var item in APIData.APIModel.AddInConfigs)
            {
                if (item.Guid.ToString() == Hosting.Guid.ToString())
                {
                    item.IsOpen = true;
                    break;
                }
            }
            Hosting = Hosting.LoadOne(Hosting.FullPath,true);
            Hosting.IsOpen = true;
            int i = 0;
            if (Hosting.Permissions == null)
            {
                Assembly ass;
                string AddInContract = "Unknown";
                try
                {
                    Assembly[] assblies = AppDomain.CurrentDomain.GetAssemblies();
                    ass = assblies[0];
                    AssemblyName assnm = new AssemblyName();
                    foreach (var item in assblies)
                    {
                        if (item.FullName.Contains(System.IO.Path.GetFileNameWithoutExtension(Hosting.FileName)))
                        {
                            ass = item;
                            var asses = ass.GetReferencedAssemblies();
                            foreach (var assem in asses)
                            {
                                if (assem.FullName.Contains("Contract"))
                                {
                                    assnm = assem;
                                    break;
                                }
                            }
                            asses = null;
                            break;
                        }
                    }
                    assblies = null;
                    if (assnm.ToString().Contains("Contract"))
                    {
                        AddInContract = assnm.Version.ToString();
                    }
                    else
                    {
                        AddInContract = "Unknown";
                    }
                }
                catch (Exception ex)
                {
                    AddInContract = "Unknown";
                }
                ass = null;
                throw new Exception($"无法启用插件，可能是因为协议不符合，当前协议版本{PageModelViews.SettingPageModel.ContractVersion}，插件协议版本{AddInContract}");
            }
            if (Hosting.Permissions.UsePagesAPI)
            {
                foreach (var Dir in Hosting.Pages.IconAndPage)
                {
                    ChangePageButton button = new ChangePageButton()
                    {
                        Width = 45,
                        Content = Dir.Key,
                        Tag = $"{Hosting.Guid}-Pages-{i.ToString()}"
                    };
                    (App.Current.Resources["AddInChangePageButtons"] as StackPanel).Children.Add(button);
                    MyPageBase Page = Dir.Value;
                    Page.Tag = $"{Hosting.Guid}-Pages-{i.ToString()}";
                    Page.Visibility = System.Windows.Visibility.Hidden;
                    (App.Current.Resources["MainPage"] as Grid).Children.Add(Page);
                    Page = null;
                    i++;
                }
            }
            this.Title = Hosting.BaseInfo.Title;
        }

        private void RemoveThis()
        {
            foreach (var item in APIData.APIModel.AddInConfigs)
            {
                if (item.Guid.ToString() == Hosting.Guid.ToString())
                {
                    item.IsOpen = false;
                    break;
                }
            }
            for (int i = 0; i < (App.Current.Resources["AddInChangePageButtons"] as StackPanel).Children.Count; i++)
            {
                if(((App.Current.Resources["AddInChangePageButtons"] as StackPanel).Children[i] as FrameworkElement).Tag.ToString().Contains(Hosting.Guid))
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
            //(App.Current.Resources["AddInChangePageButtons"] as StackPanel).Children
            //(App.Current.Resources["MainPage"] as Grid).Children.Remove();

            Hosting = Hosting.LoadOne(Hosting.FullPath);
            this.Title = Hosting.FileName;
        }
    }
}
