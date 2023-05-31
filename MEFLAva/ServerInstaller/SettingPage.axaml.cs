using Avalonia;
using Avalonia.Controls;
using MEFL.Contract;
using System.Diagnostics;

namespace ServerInstaller
{
    public partial class SettingPage : UserControl,IGameSettingPage
    {
        public SettingPage()
        {
            InitializeComponent();
            this.CancelBtn.Click += CancelBtn_Click;
            InstallBtn.Click += InstallBtn_Click;
            OnSelected += SettingPage_Quited;

            OpenUPW.Click += OpenUPW_Click;
            OpenAKA.PointerPressed += OpenAKA_PointerPressed;
        }
        private void OpenAKA_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            Process.Start("explorer.exe", "https://aka.ms/MinecraftEULA");
        }

        private void OpenUPW_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Process.Start("explorer.exe", "https://login.mc-user.com:233/server/intro");
        }

        private void SettingPage_Quited(object? sender, GameInfoBase e)
        {
            var d = (DataContext as ServerType);
            d.root.UpaOption.Server_Id = ServerID.Text;
            if (!string.IsNullOrEmpty(d.root.UpaOption.Server_Id))
            {
                OnlineOption.IsChecked = true;
            }
            var propp = Path.Combine(d.RootFolder, "server.properties");
            var outputStr = "";
            using (var reader = File.OpenRead(propp))
            {
                using (var str = new StreamReader(reader))
                {
                    var readed = false;
                    while (!str.EndOfStream)
                    {
                        var stri = str.ReadLine().Replace(" ", "");
                        if (stri.StartsWith("server-port="))
                        {
                            var rp1 = stri.Replace("server-port=", "");
                            stri = stri.Replace(rp1,PortTB.Text);
                        }
                        else if (stri.StartsWith("max-players="))
                        {
                            var rp1 = stri.Replace("max-players=", "");
                            stri = stri.Replace($"{rp1}", MaxPlayersCount.Text);
                        }
                        else if (stri.StartsWith("white-list="))
                        {
                            var rp1 = stri.Replace("white-list=", "");
                            stri = stri.Replace($"{rp1}",
                                WhitelistOption.IsChecked.ToString());
                        }
                        else if (stri.StartsWith("online-mode="))
                        {
                            var rp1 = stri.Replace("online-mode=", "");
                            stri = stri.Replace($"{rp1}",OnlineOption.IsChecked.ToString());
                        }
                        outputStr += stri;
                    }
                }
            }
            d.Save();
        }

        private void InstallBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            OnSelected?.Invoke(this,DataContext as GameInfoBase);
            Quited?.Invoke(this,e);
        }

        private void CancelBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Quited?.Invoke(this, e);
        }
        internal void ReadData()
        {

            var d = DataContext as ServerType;
            NameTP.Text = d.Name;
            VersionTB.Text = d.root.BaseVersion;
            if (!string.IsNullOrEmpty(d.root.UpaOption.Server_Id))
            {

                OnlineOption.IsChecked = true;
                OnlineOption.IsEnabled = false;
            }
            ServerID.Text = d.root.UpaOption.Server_Id;
            var propp = Path.Combine(d.RootFolder, "server.properties");
            using (var reader = File.OpenRead(propp))
            {
                using (var str = new StreamReader(reader))
                {
                    var readed = false;
                    while (!str.EndOfStream)
                    {
                        var stri = str.ReadLine().Replace(" ", "");
                        if (stri.StartsWith("server-port="))
                        {
                            PortTB.Text = stri.Replace("server-port=", "");
                        }
                        else if (stri.StartsWith("max-players="))
                        {
                            MaxPlayersCount.Text = stri.Replace("max-players=", "");
                        }
                        else if (stri.StartsWith("white-list="))
                        {
                            try
                            {
                                var BOOL = Convert.ToBoolean(stri.Replace("white-list=", ""));
                                WhitelistOption.IsChecked = BOOL;
                            }
                            catch
                            {
                                WhitelistOption.IsChecked = true;
                            }
                        }
                        else if (stri.StartsWith("online-mode="))
                        {
                            try
                            {
                                var BOOL = Convert.ToBoolean(stri.Replace("online-mode=", ""));
                                OnlineOption.IsChecked = BOOL;
                            }
                            catch
                            {
                                OnlineOption.IsChecked = false;
                            }
                        }
                    }
                }
            }
        }

        public event EventHandler<GameInfoBase> OnSelected;
        public event EventHandler<GameInfoBase> OnRemoved;
        public event EventHandler<GameInfoBase> OnPageBack;
        public event EventHandler<GameInfoBase> OnListUpdate;
        public event EventHandler<EventArgs> Quited;

        public void WindowSizeChanged(Size size)
        {

        }
    }
}
