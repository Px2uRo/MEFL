using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using DynamicData;
using MEFL.APIData;
using MEFL.AvaControls;
using MEFL.Contract;
using MEFL.InfoControls;
using MEFL.PageModelViews;
using System;
using System.Threading.Tasks;
using static MEFL.APIData.APIModel;

namespace MEFL.Views.DialogContents
{
    public partial class GamesManagerContent : UserControl, IDialogContent
    {
        public event EventHandler<EventArgs> Quited;
        public static GamesManagerContent UI = new GamesManagerContent();
        public GamesManagerContent()
        {
            InitializeComponent();
            this.DataContext = App.Current.Resources["RMPMV"];
            CancelBtn.Click += Contr_Enabled;
            AddFolderBtn.Click += AddFolderBtn_Click;
            DelFolderBtn.Click += DelFolderBtn_Click;
        }

        private async void DelFolderBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (SelectedFolderIndex == 0)
            {
                return;
            }
            else
            {
                var newInd = SelectedFolderIndex - 1;
                MyFolders.Remove(MyFolders[SelectedFolderIndex]);
                SelectedFolderIndex = newInd;
                await GameRefresher.Main.Refresh();
            }
        }

        private async void AddFolderBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var d = new OpenFolderDialog();
            d.Title = "Ñ¡ÔñÎÄ¼þ¼Ð";
            if (App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mv = desktop.MainWindow as MainWindow;
                var res = await d.ShowAsync(mv);
                if (string.IsNullOrEmpty(res))
                {
                    return;
                }
                else
                {
                    var ui = AddFolderDialogs.UI;
                    ui.FolderPath= res;
                    ContentDialog.Show(ui);
                }
            }
        }

        public void ReLoad()
        {
            GameRefresher.Main.PropertyChanged -= Main_PropertyChanged;
            GameRefresher.Main.PropertyChanged += Main_PropertyChanged;
            GameRefresher.Main.Refresh().Start();
        }

        private void Main_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var r = sender as GameRefresher;
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                if (r.RefreshOK == true)
                {
                    for (int i = 0; i < Games.Children.Count; i++)
                    {
                        (Games.Children[i].DataContext as GameInfoBase)
                        .Dispose();
                        GC.SuppressFinalize(Games.Children[i]);
                        Games.Children.RemoveAt(i);
                        i--;
                    }
                    foreach (var item in r.GameInfos)
                    {
                        var contr = new GameInfoControl() { DataContext = item };
                        contr.Enabled += Contr_Enabled;
                        Games.Children.Add(contr);
                    }
                }
                else
                {

                }
            });
        }

        private void Contr_Enabled(object? sender, EventArgs e)
        {
            Quited.Invoke(this,e);
        }
    }
}
