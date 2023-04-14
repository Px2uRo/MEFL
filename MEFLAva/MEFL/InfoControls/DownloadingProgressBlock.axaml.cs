using Avalonia.Controls;
using MEFL.Contract;
using System;

namespace MEFL.InfoControls
{
    public partial class DownloadingProgressBlock : UserControl
    {
        public DownloadingProgressBlock()
        {
            InitializeComponent();
        }

        public DownloadingProgressBlock(DownloadProgress progress):this()
        {
            progress.PropertyChanged += Progress_PropertyChanged;
            this.DataContext= progress;
            
            progress.Start();
        }

        private void Progress_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var vm = sender as DownloadProgress;
            if (e.PropertyName == nameof(vm.DownloadedItems))
            {
                CountTB.Text = $"{vm.TotalCount} 里下载好了 {vm.DownloadedItems}";
            }
            else if (e.PropertyName == nameof(vm.TotalCount))
            {
                CountTB.Text = $"{vm.TotalCount} 里下载好了 {vm.DownloadedItems}";
            }
            else if (e.PropertyName == nameof(vm.DownloadedSize))
            {
                var valu = (((double)vm.DownloadedSize) / (double)(vm.TotalSize));
                ValuePB.Value = valu * 100d;
                double percent = Math.Round(valu *100,1);
                ProgressTB.Text = percent.ToString() + "%";
            }
            else if (e.PropertyName == nameof(vm.ErrorInfo))
            {

            }
        }
    }
}
