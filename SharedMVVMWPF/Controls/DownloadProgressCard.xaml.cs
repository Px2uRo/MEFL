using MEFL.Contract;
using System;
using System.Collections.Generic;
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

namespace MEFL.Controls
{
    /// <summary>
    /// DownloadProgressCard.xaml 的交互逻辑
    /// </summary>
    public partial class DownloadProgressCard : MyCard
    {
        public DownloadProgressCard()
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            (this.DataContext as DownloadProgress).PropertyChanged += DownloadProgressCard_PropertyChanged;
            base.OnApplyTemplate();
        }

        private void DownloadProgressCard_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var source = sender as DownloadProgress;
            if (e.PropertyName == "DownloadedSize")
            {
                Dispatcher.Invoke(() =>
                {
                    PB.Value=((double)source.DownloadedSize/(double)source.TotalSize)*100;
                });
            }
        }
    }
}
