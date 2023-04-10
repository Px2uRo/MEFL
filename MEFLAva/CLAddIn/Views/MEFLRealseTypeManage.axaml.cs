using Avalonia.Controls;
using MEFL.Contract;
using System.Diagnostics;

namespace CLAddIn.Views
{
    public partial class MEFLRealseTypeManage : UserControl,IProcessManagePage
    {
        public MEFLRealseTypeManage()
        {
            InitializeComponent();
        }

        public MEFLRealseTypeManage(Process process)
        {
            InitializeComponent();
            //process.StartInfo.RedirectStandardError = true;
            //process.StartInfo.RedirectStandardOutput = true;
            process.EnableRaisingEvents = true;
            process.Exited += Process_Exited;
            process.Start();
        }

        private void Process_Exited(object? sender, EventArgs e)
        {
            Exited?.Invoke(this, e);
        }

        public event EventHandler Exited;
    }
}
