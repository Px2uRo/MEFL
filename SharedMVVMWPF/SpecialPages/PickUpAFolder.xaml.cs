using MEFL.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
            (this.DataContext as ModelView).Curret= "根目录";
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            var baseinfo = base.ArrangeOverride(arrangeBounds);
            (this.DataContext as ModelView).Columns = Convert.ToInt16(baseinfo.Width / 250);
            return baseinfo;
        }

        private void Changed(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Curret")
            {
                if(Directory.Exists((this.DataContext as ModelView).Curret))
                {
                    ErrorBorder.Visibility = Visibility.Hidden;
                }
                else
                {
                    ErrorBorder.Visibility = Visibility.Hidden;
                    //TODO i18N
                    ErrorBOX.Text = "不存在此文件夹";
                }
            }
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
                    Child.MouseDown += Child_MouseDown;
                    PART_UniformGrid.Children.Add(Child);
                }
            }
        }

        private void Child_MouseDown(object sender, MouseButtonEventArgs e)
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

    }
}
