﻿using MEFL.ControlModelViews;
using MEFL.Controls;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MEFL.Pages
{
    /// <summary>
    /// ExtensionPage.xaml 的交互逻辑
    /// </summary>
    public partial class ExtensionPage : MyPageBase
    {
        public ExtensionPage()
        {
            InitializeComponent();
            foreach (var item in (this.DataContext as ExtensionPageModelView).Hostings)
            {
                ContentPage.Children.Add(new MyExtensionCard() { Hosting = item,Margin=new Thickness(0,0,0,15)});
            }
        }

        private void ChangePageContentButton_Checked(object sender, RoutedEventArgs e)
        {
            foreach (Panel item in (this.Content as Panel).Children)
            {
                item.Visibility = Visibility.Hidden;
            }
            foreach (Panel item in FindControl.FromTag((sender as ChangePageContentButton).Tag, this.Content as Panel))
            {
                item.Visibility = Visibility.Visible;
            }
            
        }
    }
}
