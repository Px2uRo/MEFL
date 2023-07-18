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
using System.Threading;
using System.Diagnostics;
using Avalonia.Interactivity;
using Timer = System.Timers.Timer;

namespace MEFL.Views
{
    public partial class SettingPage : UserControl
    {
        internal static SettingPage UI = new SettingPage();
        ObservableCollection<string> itms = new ObservableCollection<string>();
        int downloadersCount = 0;
        Timer _timer = new(2000);
        Size _mess;
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
            _mess = new(availableSize.Width - Margin.Left - Margin.Right - 30, availableSize.Height);

            if(DataContext is SettingPageModelView dc)
            {
                var usdmem = dc.UsedMem;

                UsedInfo.Text = "使用了：" + Math.Round((double) usdmem/ 1024d, 1).ToString() + " G";
                SettedInfo.Text = "分配了：" + Math.Round((double)dc.Memory / 1024d, 1).ToString() + " G";
                UsedBlock.Width = ((double)usdmem / (double)dc.Totalmemory) * _mess.Width;
                var res = ((double)usdmem + (double)dc.Memory) / (double)dc.Totalmemory;
                SetBlock.Width = res * _mess.Width;
                SettedInfo.SetValue(Canvas.LeftProperty, UsedBlock.Width);
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
            JavaList.Items = itms;
            JavaList.SelectedIndex = dc.SelectedJavaIndex;
            LoadDownloaderUI(dc.Downloaders);
            ImageButton.Click += ImageButton_Click;
            if (dc.AutoMemory)
            {
                MemOPEN.IsChecked = true;
                MemSlider.IsVisible = false;
            }
            else
            {
                MemCLOSE.IsChecked = true;
                MemSlider.IsVisible = true;
            }
            UsedInfo.Text = "使用了：" + Math.Round((double)dc.UsedMem / 1024d, 1).ToString() + " G";
            SettedInfo.Text = "分配了：" + Math.Round((double)dc.Memory / 1024d, 1).ToString() + " G";
            UsedBlock.Width = ((double)dc.UsedMem / (double)dc.Totalmemory) * _mess.Width;
            var res = ((double)dc.UsedMem + (double)dc.Memory) / (double)dc.Totalmemory;
            SetBlock.Width = res * _mess.Width;
            SettedInfo.SetValue(Canvas.LeftProperty, UsedBlock.Width);
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();

            MemSlider.PropertyChanged += MemSlider_PropertyChanged;
        }

        private void MemSlider_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == Slider.ValueProperty)
            {
                var value = (double)e.NewValue;
                var dc = DataContext as SettingPageModelView;

                var usdmem = dc.UsedMem;
                ulong newmemo = (ulong)(dc.Totalmemory * value - usdmem);
                if((long)newmemo < 512)
                {
                    dc.Memory = 512;
                    MemSlider.Value = (double)(usdmem + 512) / (double)dc.Totalmemory;
                    return;
                }
                dc.Memory = newmemo;
                UsedInfo.Text = "使用了：" + Math.Round((double)usdmem / 1024d, 1).ToString() + " G";
                SettedInfo.Text = "分配了：" + Math.Round((double)dc.Memory / 1024d, 1).ToString() + " G";
                UsedBlock.Width = ((double)usdmem / (double)dc.Totalmemory) * _mess.Width;
                var res = ((double)usdmem + (double)dc.Memory) / (double)dc.Totalmemory;
                SetBlock.Width = res * _mess.Width;
                SettedInfo.SetValue(Canvas.LeftProperty, UsedBlock.Width);
            }
        }

        private void _timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                var dc = DataContext as SettingPageModelView;
                if (this.IsVisible&&Opacity==1d)
                {
                    dc.AutoRefresh = true;
                }
                else
                {
                    dc.AutoRefresh = false;
                }
            });
        }

        private async void ImageButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            (DataContext as SettingPageModelView).ChangeBackgroundCommand.Execute("");
        }

        private void LoadDownloaderUI(DownloaderCollection downloaders)
        {
            downloadersCount = downloaders.Count;
            DownloadersGrid.Children.Clear();
            foreach (var item in downloaders)
            {
                var btn = new SelecteDownloaderButton(item);
                DownloadersGrid.Children.Add(btn);
                btn.Enablebtn.IsChecked = APIModel.SelectedDownloader == item;
            }
        }

        private void JavaList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            var smv = (SettingPageModelView)DataContext;
            var inde = ((ComboBox)sender).SelectedIndex;
            if (inde != -1)
            {
                APIModel.SettingArgs.SelectedJava = smv.Javas[inde];
                RegManager.Write("SelectedJava", smv.Javas[inde].FullName);
            }
        }

        private void Dc_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var dc = (SettingPageModelView)sender;
            if (e.PropertyName == "Javas")
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
                    if (smv.Javas.Count > 0)
                    {
                        JavaList.SelectedIndex = 0;
                    }
                }
            }
            else if (e.PropertyName == "Downloaders")
            {
                LoadDownloaderUI(dc.Downloaders);
            }
            else if (e.PropertyName == nameof(dc.DownSources))
            {
                LoadSourceLB();
            }
            else if (e.PropertyName == nameof(dc.UsedMem))
            {
                UsedInfo.Text = "使用了：" + Math.Round((double)dc.UsedMem / 1024d, 1).ToString() + " G";
                SettedInfo.Text = "分配了：" + Math.Round((double)dc.Memory / 1024d, 1).ToString() + " G";
                UsedBlock.Width = ((double)dc.UsedMem / (double)dc.Totalmemory) * _mess.Width;
                var res = ((double)dc.UsedMem + (double)dc.Memory) / (double)dc.Totalmemory;
                SetBlock.Width = res * _mess.Width;
                SettedInfo.SetValue(Canvas.LeftProperty,UsedBlock.Width);
            }
        }

        private void AutoMemChecked(object? sender, RoutedEventArgs e)
        {
            var dc = DataContext as SettingPageModelView;
            if ((sender as Control).Name.Contains("OPEN"))
            {
                MemSlider.IsEnabled= false;
                dc.AutoMemory= true;
                MemSlider.IsVisible = false;
            }
            else
            {
                MemSlider.IsEnabled = true;
                dc.AutoMemory = false;
                MemSlider.IsVisible = true;
            }
        }

        private void RefreshJavas_Click(object? sender, RoutedEventArgs e)
        {
            var smv = (SettingPageModelView)DataContext;
            smv.EnableSearchJava = false;
            APIModel.SearchJavas();
        }


        private void LDD(object? sender, EventArgs e)
        {
            var cmb = (sender as ComboBox);
            DSList data = (sender as ComboBox).Items as DSList;
            cmb.SelectedIndex = data.IndexOf(data.Selected);
        }
        private void CGD(object? sender, SelectionChangedEventArgs e)
        {
            DSList data = (sender as ComboBox).Items as DSList;
            if(data == null)
            {
                return;
            }
            if (data.Count == 0)
            {

            }
            else if ((sender as ComboBox).SelectedIndex > data.Count - 1 || (sender as ComboBox).SelectedIndex == -1)
            {
                data.Selected = data[0];
                (sender as ComboBox).SelectedIndex = 0;
            }
            else
            {
                data.Selected = data[(sender as ComboBox).SelectedIndex];
            }
        }

        internal void LoadSourceLB()
        {
            SourcesLB.Items = APIModel.DownloadSources;
        }
    }
}
