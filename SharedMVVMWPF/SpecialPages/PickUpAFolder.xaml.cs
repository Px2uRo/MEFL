using MEFL.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Media;
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
using ModelView = MEFL.PageModelViews.PickUpAFolderPageModelView;

namespace MEFL.SpecialPages
{
    /// <summary>
    /// PickUpAFolder.xaml 的交互逻辑
    /// </summary>
    public partial class PickUpAFolder : MyPageBase
    {
        public PickUpAFolder()
        {
            InitializeComponent();
            //todo 戳啦！注册表嘛！
        }



        public string Currect
        {
            get { return (string)GetValue(CurrectProperty); }
            set 
            {
                (this.DataContext as ModelView).Curret = value;
                SetValue(CurrectProperty, value); 
            }
        }

        // Using a DependencyProperty as the backing store for Currect.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrectProperty =
            DependencyProperty.Register("Currect", typeof(string), typeof(PickUpAFolder), new PropertyMetadata(null));



        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            var baseinfo = base.ArrangeOverride(arrangeBounds);
            (this.DataContext as ModelView).Columns = Convert.ToInt16(baseinfo.Width / 250);
            return baseinfo;
        }

        private void Changed(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Items")
            {
                PART_UniformGrid.Children.Clear();
                PART_UniformGrid.Rows = (this.DataContext as ModelView).Items.Count / (this.DataContext as ModelView).Columns;
                if(((this.DataContext as ModelView).Items.Count % (this.DataContext as ModelView).Columns) != 0)
                {
                    PART_UniformGrid.Rows += 1;
                }
                foreach (var item in (this.DataContext as ModelView).Items)
                {
                    var Child = new FileOrDictoryItem() { DataContext = item };
                    Child.Checked += Child_Checked;
                    Child.MouseDoubleClick += Child_MouseDoubleClick;
                    PART_UniformGrid.Children.Add(Child);
                }
            }
        }

        private void Child_Checked(object sender, RoutedEventArgs e)
        {
            (this.DataContext as ModelView).Selected = (sender as FileOrDictoryItem).DataContext as DirectoryInfo;
        }

        private void Child_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            (this.DataContext as ModelView).Curret = ((sender as FileOrDictoryItem).DataContext as DirectoryInfo).FullName;
        }

        private void ErrorAppeard(object sender, ValidationErrorEventArgs e)
        {
            (sender as TextBox).Text = e.Error.ErrorContent as String;
        }

        private void DoubleClick(object sender, MouseButtonEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }

        private void BackToRoot(object sender, RoutedEventArgs e)
        {
            (this.DataContext as ModelView).Curret = "根目录";
            (this.DataContext as ModelView).Selected = null;
        }

        private void DeleteSelected(object sender, RoutedEventArgs e)
        {
            //todo 删除选中的文件夹。
        }

        private void CreateNew(object sender, RoutedEventArgs e)
        {
            //todo 在当前目录下新建文件夹。
        }

        private void BackToParent(object sender, RoutedEventArgs e)
        {
            var parent = Directory.GetParent((DataContext as ModelView).Curret);
            if (parent == null || (DataContext as ModelView).Curret == "根目录")
            {
                (this.DataContext as ModelView).Curret = "根目录";
            }
            else
            {
                (this.DataContext as ModelView).Curret = parent.FullName;
            }
        }
        private void SelectThis(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
