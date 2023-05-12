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

        public DownloadingProgressBlock(InstallProcess progress):this()
        {
            progress.PropertyChanged += Progress_PropertyChanged;
            this.DataContext= progress;
            
            progress.Start();
        }

        private void Progress_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var vm = sender as InstallProcess;
            if (e.PropertyName == nameof(vm.CurrentProgress))
            {
                ValuePB.Value = vm.CurrentProgress * 100d;
                double percent = Math.Round(vm.CurrentProgress* 100, 1);
                ProgressTB.Text = percent.ToString() + "%";
            }
            else if (e.PropertyName == nameof(vm.CurrectProgressIndex)|| e.PropertyName == nameof(vm.TotalProgresses))
            {
                StepTB.Text = $"{vm.CurrectProgressIndex} / {vm.TotalProgresses}";
            }
            else if(e.PropertyName == nameof(vm.Content))
            {
                ContentTB.Text = vm.Content;
            }
            else if(e.PropertyName == "TotalSize" || e.PropertyName== "DownloadedSize")
            {
                ValuePB.Value = ((((vm as SingleProcess).DownloadedSize / (double)(vm as SingleProcess).TotalSize)) * 100d);
                ProgressTB.Text = ValuePB.Value.ToString() + "%";
                StepTB.Text = $"{(vm as SingleProcess).DownloadedSize} / {(vm as SingleProcess).TotalSize}";
            }
        }
    }
}
