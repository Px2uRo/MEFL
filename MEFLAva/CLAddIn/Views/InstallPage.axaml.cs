using Avalonia.Controls;
using CLAddIn.Properties;
using CoreLaunching.JsonTemplates;
using CoreLaunching;
using MEFL.Arguments;
using MEFL.Contract;

namespace CLAddIn.Views
{
    public partial class InstallPage : UserControl,IInstallPage
    {

        public LauncherWebVersionInfo Info { get; set; }

        string _dotMCPath;

        public InstallPage()
        {
            InitializeComponent();
            CancelBtn.Click += CancelBtn_Click;
            InstallBtn.Click += InstallBtn_Click;
            NameTP.PropertyChanged += NameTP_PropertyChanged;
        }

        private void NameTP_PropertyChanged(object? sender, Avalonia.AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == TextBlock.TextProperty)
            {
                var vp = System.IO.Path.Combine(_dotMCPath, "versions", NameTP.Text);
                if (vp.EndsWith("."))
                {
                    InstallBtn.IsEnabled = false;
                    DesTB.Text = $"�ļ��в���С�����β";
                }
                else if (Directory.Exists(vp))
                {
                    InstallBtn.IsEnabled = false;
                    DesTB.Text = $"�ļ��� {NameTP.Text} �Ѵ���";
                }
                else
                {
                    InstallBtn.IsEnabled = true;
                    DesTB.Text = $"������װ{NameTP.Text}";
                }
            }
        }

        private void InstallBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var vp = System.IO.Path.Combine(_dotMCPath, "versions", NameTP.Text);
            if (Directory.Exists(vp))
            {
                DesTB.Text=("�뿨BUG�𣿵����ļ����Ѵ���");
                InstallBtn.IsEnabled = false;
                return;
            }
            else
            {
                Directory.CreateDirectory(vp);
            }
            Solved?.Invoke(this, new(NameTP.Text,null,null));
        }

        private void CancelBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Quited?.Invoke(this, e);
        }

        public InstallPage(LauncherWebVersionInfo info,string dotMCPath) : this()
        {
            Info = info;
            _dotMCPath = dotMCPath;
            DesTB.Text= $"������װ{info.Id}";
            NameTP.Text = info.Id;
        }

        public event EventHandler<InstallArguments> Solved;
        public event EventHandler<EventArgs> Quited;
    }
}
