using CoreLaunching.JsonTemplates;
using MEFL.APIData;
using MEFL.Contract;
using MEFL.PageModelViews;
using System;
using System.Collections.Generic;
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
    /// DownloadProgressCard.xaml 的交互逻辑
    /// </summary>
    public partial class DownloadProgressCard : MyCard
    {
        bool Paused = false;
        public DownloadProgressCard()
        {
            InitializeComponent();
            LogLB.ItemsSource = new List<string>();
            //var source = DownloadProgress;
            //if (source != null)
            //{
                //source.OnLogWriteLine += Source_OnLogWriteLine;
                //source.OnLogClear += Source_OnLogClear;
            //}

        }

        private void Source_OnLogClear(object? sender, EventArgs e)
        {
            //var lb = new ListBox();
            //lb.ItemsSource = new List<string>();
            //lb.Items.Clear();
            LogLB.Items.Clear();
        }

        private void Source_OnLogWriteLine(object? sender, string e)
        {
            LogLB.Items.Add(e);
        }

        private void PauseBtn(object sender, RoutedEventArgs e)
        {
            if (Paused)
            {
                Paused = false;
                PauseButn.Content = "暂停";
                    (this.DataContext as DownloadProgress).Continue();
            }
            else
            {
                Paused = true;
                PauseButn.Content = "继续";
                    (this.DataContext as DownloadProgress).Pause();
            }
        }

        private void CancelBtn(object sender, RoutedEventArgs e)
        {
            (this.DataContext as DownloadProgress).Cancel();
        }

        public override void OnApplyTemplate()
        {
            if (this.DataContext != null)
            {
                var item = this.DataContext as DownloadProgress;
                item.PropertyChanged += DownloadProgressCard_PropertyChanged;
                    item.Start();
            }
            base.OnApplyTemplate();
        }

        private void DownloadProgressCard_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var source = sender as DownloadProgress;
            if(e.PropertyName == nameof(source.State))
            {
                if (source.State == DownloadProgressState.Finished)
                {
                    DownloadingProgressPageModel.ModelView.DownloadingProgresses.Remove(source);
                }
                else if (source.State == DownloadProgressState.Failed)
                {
                    Dispatcher.Invoke(() => {
                        this.Content = source.ErrorInfo;
                    });
                }
                else if (source.State == DownloadProgressState.Canceled)
                {
                    DownloadingProgressPageModel.ModelView.DownloadingProgresses.Remove(source);
                }
                else if(source.State == DownloadProgressState.Pauseing||source.State == DownloadProgressState.RetryingOrContiuning)
                {
                    Dispatcher.Invoke(() => {
                        ControlingBtn.Visibility = Visibility.Visible;
                    });
                }
                else if (source.State == DownloadProgressState.Paused|| source.State == DownloadProgressState.Downloading)
                {
                    Dispatcher.Invoke(() => {
                        ControlingBtn.Visibility = Visibility.Hidden;
                    });
                }
            }
            else if (e.PropertyName == nameof(source.TotalCount))
            {
                Dispatcher.Invoke(() => {
                    FileNums.Text = $"{source.DownloadedItems}/{source.TotalCount}";
                });
            }
            else if(e.PropertyName == nameof(source.DownloadedItems))
            {
                Dispatcher.Invoke(() => {
                    FileNums.Text = $"{source.DownloadedItems}/{source.TotalCount}";
                });
            }
            else if (e.PropertyName == nameof(source.DownloadedSize))
            {
                Dispatcher.Invoke(() =>
                {
                    MegaBytes.Text = $"{source.DownloadedSize} / {source.TotalSize}";
                    var str = (((double)source.DownloadedSize / (double)source.TotalSize) * 100).ToString();
                    if (str.Length >= 5)
                    {
                        str = str.Substring(0, 5);
                    }
                    else if (str.Length == 4)
                    {
                        str = str.Substring(0, 4);
                    }
                    else if (str.Length == 3)
                    {
                        str = str.Substring(0, 3);
                    }
                    else if (str.Length == 2)
                    {
                        str = str.Substring(0, 2);
                    }
                    else if (str.Length == 1)
                    {
                        str = str.Substring(0, 1);
                    }
                    Percents.Text = str + "%";
                    PB.Value=((double)source.DownloadedSize/(double)source.TotalSize)*100;
                });
            }
        }
    }
}
