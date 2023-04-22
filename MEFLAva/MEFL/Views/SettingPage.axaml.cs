using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;
using Avalonia.Threading;
using DynamicData;
using MEFL.APIData;
using MEFL.Configs;
using MEFL.InfoControls;
using MEFL.PageModelViews;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MEFL.Views
{
    public partial class SettingPage : UserControl
    {
        internal static IControl UI = new SettingPage();
        ObservableCollection<string> itms = new ObservableCollection<string>();
        int downloadersCount = 0;

        protected override Size MeasureOverride(Size availableSize)
        {
            DownloadersGrid.Columns = (int)Math.Ceiling(availableSize.Width / 290.0);
            if (downloadersCount == 0)
            {
                DownloadersGrid.Rows = 1;
            }
            else
            {
                DownloadersGrid.Rows = (int)Math.Ceiling((double)downloadersCount / (double)DownloadersGrid.Columns);
            }
            return base.MeasureOverride(availableSize);
        }
        public SettingPage()
        {
            InitializeComponent();
            var dc = SettingPageModel.ModelView;
            this.DataContext = dc;
            dc.PropertyChanged += Dc_PropertyChanged;
            JavaList.SelectionChanged += JavaList_SelectionChanged;
            RefreshJavas.Click += RefreshJavas_Click;
            foreach (var item in dc.Javas)
            {
                itms.Add(item.FullName);
            }
            JavaList.Items =itms;
            JavaList.SelectedIndex = dc.SelectedJavaIndex;
            LoadDownloaderUI(dc.Downloaders);
            ImageButton.Click += ImageButton_Click;
        }

        private async void ImageButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if(App.Current.ApplicationLifetime is ClassicDesktopStyleApplicationLifetime desktp)
            {
                var mw = desktp.MainWindow as MainWindow;
                App.OpenFileDialog.AllowMultiple = false;
                App.OpenFileDialog.Filters.Clear();
                App.OpenFileDialog.Filters.Add(new() { Name="ͼƬ�ļ�",Extensions=new() { "png","jpg"} });
                var res = await App.OpenFileDialog.ShowAsync(mw);
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    mw.BackGround.Children.Clear();
                    try
                    {
                        var image = new Avalonia.Media.Imaging.Bitmap(res[0]);
                        var imaCont = new Avalonia.Controls.Image() { Source=image};
                        imaCont.Stretch = Stretch.UniformToFill;
                        mw.BackGround.Children.Add(imaCont);
                    }
                    catch
                    {

                    }
                });
            }
        }

        private void LoadDownloaderUI(DownloaderCollection downloaders)
        {
            downloadersCount = downloaders.Count;
            DownloadersGrid.Children.Clear();
            foreach (var item in downloaders)
            {
                var btn = new SelecteDownloaderButton(item);
                DownloadersGrid.Children.Add(btn);
                btn.Enablebtn.IsChecked = APIModel.SelectedDownloader==item;
            }
        }

        private void JavaList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            var smv = (SettingPageModelView)DataContext;
            var inde = ((ComboBox)sender).SelectedIndex;
            if(inde!= -1)
            {
                APIModel.SettingArgs.SelectedJava = smv.Javas[inde];
                RegManager.Write("SelectedJava", smv.Javas[inde].FullName);
            }
        }

        private void Dc_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var dc = (SettingPageModelView)sender;
            if(e.PropertyName == "Javas")
            {
                var smv = (SettingPageModelView)sender;
                JavaList.SelectedIndex = -1;
                if (smv.EnableSearchJava)
                {
                    itms.Clear();
                    foreach (var item in smv.Javas)
                    {
                        itms.Add(item.FullName);
                    }
                }
            }
            else if (e.PropertyName == "Downloaders")
            {
                LoadDownloaderUI(dc.Downloaders);
            }
            else if(e.PropertyName==nameof(dc.DownSources))
            {

            }
        }

        private void RefreshJavas_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var smv = (SettingPageModelView)DataContext;
            smv.EnableSearchJava = false;
            APIModel.SearchJavas();
        }
    }
}
