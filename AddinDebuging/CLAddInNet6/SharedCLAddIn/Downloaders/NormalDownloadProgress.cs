using MEFL.Contract;
using System.Timers;

namespace MEFL.CLAddIn.Downloaders
{
    internal class NormalDownloadProgress : DownloadProgress
    {
        Timer t;
        public override void Pause()
        {
            t.Stop();
            base.Pause();
        }
        public override void Cancel()
        {
            base.Cancel();
        }
        public override void Close()
        {
            base.Close();
        }
        public override void Start()
        {
            base.Start();
            TotalSize = 2500;
            while (DownloadedSize < TotalSize)
            {
                System.Threading.Thread.Sleep(20);
                DownloadedSize++;
                if (DownloadedSize >= TotalSize)
                {
                    this.Statu = DownloaderStatu.Finished;
                }
            }

        }

        private void T_Elapsed(object? sender, ElapsedEventArgs e)
        {
            this.DownloadedSize++;
            if(DownloadedSize >= TotalSize)
            {
                this.Statu = DownloaderStatu.Finished;
                (sender as Timer).Stop();
                (sender as Timer).Dispose();
            }
        }
    }
}