using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MEFL.APIData;
using MEFL.Contract;
using System;

namespace MEFL.InfoControls
{
    public partial class SelecteDownloaderButton : UserControl
    {
        public SelecteDownloaderButton()
        {
            InitializeComponent();
            Enablebtn.Checked += Enablebtn_Checked;
            Enablebtn.Unchecked += Enablebtn_Unchecked;
            MoreInfoGrid.PointerEnter += MoreInfoGrid_PointerEnter;
            MoreInfoGrid.PointerLeave += MoreInfoGrid_PointerLeave;
        }

        private void Enablebtn_Unchecked(object? sender, RoutedEventArgs e)
        {
            if (APIModel.Downloaders.Count == 1)
            {
                Enablebtn.IsChecked = true;
            }
        }

        public SelecteDownloaderButton(MEFLDownloader downloader):this()
        {
            this.DataContext= downloader;
        }

        private void Enablebtn_Checked(object? sender, RoutedEventArgs e)
        {
            if(Parent is Panel parent)
            {
                foreach (var item in parent.Children)
                {
                    if(item is SelecteDownloaderButton btn)
                    {
                        if (btn != this)
                        {
                            btn.UnCheck();
                        }
                    }
                }
            }
            APIModel.SelectedDownloader = DataContext as MEFLDownloader;
        }

        private void UnCheck()
        {
            Enablebtn.IsChecked = false;
        }

        private void MoreInfoGrid_PointerLeave(object? sender, Avalonia.Input.PointerEventArgs e)
        {
            Animations.MarginBack(new Thickness(30, 0, 0, 0)).RunAsync(MoreInfoBtn, null);
            Animations.WidthBack(288.0).RunAsync(Enablebtn, null);
        }

        private void MoreInfoGrid_PointerEnter(object? sender, Avalonia.Input.PointerEventArgs e)
        {
            Animations.MarginGo(new Thickness(0, 0, 0, 0)).RunAsync(MoreInfoBtn, null);
            Animations.WidthGo(258.0).RunAsync(Enablebtn, null);
        }

    }
}
