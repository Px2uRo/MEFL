using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using MEFL.APIData;
using MEFL.Contract;
using MEFL.Controls;
using Newtonsoft.Json;

namespace MEFL.SpecialPages
{
    /// <summary>
    /// RenameFolderPage.xaml 的交互逻辑
    /// </summary>
    public partial class RenameFolderPage : MyPageBase
    {
        public RenameFolderPage()
        {
            InitializeComponent();
        }

        private void Change(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                for (int i = 0; i < (App.Current.Resources["MainPage"] as Grid).Children.Count; i++)
                {
                    if (((App.Current.Resources["MainPage"] as Grid).Children[i] as MyPageBase).Tag == "PickUP" || ((App.Current.Resources["MainPage"] as Grid).Children[i] as MyPageBase).Tag == "RenameFolder"
                    )
                    {
                        (App.Current.Resources["MainPage"] as Grid).Children.RemoveAt(i);
                    }
                }
            (App.Current.Resources["MainPage"] as Grid).Children.Add(new SpecialPages.PickUpAFolder() { Tag = "PickUP", Visibility = Visibility.Hidden, Currect = this.SelectedPath });
                MyPageBase From = new MyPageBase();
                foreach (MyPageBase item in (App.Current.Resources["MainPage"] as Grid).Children)
                {
                    if (item.Visibility == Visibility.Visible)
                    {
                        From = item;
                    }
                }
                foreach (MyPageBase item in FindControl.FromTag("PickUP", (App.Current.Resources["MainPage"] as Grid)))
                {
                    item.Show(From);
                }
            }
        }

        public string SelectedPath
        {
            get { return (string)GetValue(SelectedPathProperty); }
            set { 
                SetValue(SelectedPathProperty, value);
                (this.DataContext as PageModelViews.RenameAFolderModelView).SelectedPath = value;
            }
        }

        // Using a DependencyProperty as the backing store for SelectedPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedPathProperty =
            DependencyProperty.Register("SelectedPath", typeof(string), typeof(RenameFolderPage), new PropertyMetadata(string.Empty));
    }
}
