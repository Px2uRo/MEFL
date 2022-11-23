using MEFL.Contract;
using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL.CLAddIn.Downloaders
{
    public class NormalProgress:DownloadProgress
    {
        public override void Close()
        {
            base.Close();
        }
        public override void Pause()
        {
            base.Pause();
        }
        public override void Start()
        {
            base.Start();
        }
        public override void Stop()
        {
            base.Stop();
        }
    }
}
