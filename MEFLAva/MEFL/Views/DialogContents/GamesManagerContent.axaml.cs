using Avalonia.Controls;
using Avalonia.Threading;
using DynamicData;
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
                    if (Games.Children.Count != r.GameInfos.Count)
                    {
                        if (Games.Children.Count == 0)
                        {
                            foreach (var item in r.GameInfos)
                            {
                                var contr = new GameInfoControl() { DataContext = item };
                                contr.Enabled += Contr_Enabled;
                                Games.Children.Add(contr);
                            }
                        }
                        else if (Games.Children.Count > r.GameInfos.Count)
                        {
                            var len = r.GameInfos.Count - Games.Children.Count;
                            for (int i = 1; i <= len; i++)
                            {
                                Games.Children.RemoveAt(Games.Children.Count - i);
                            }
                        }
                        else if (Games.Children.Count < r.GameInfos.Count)
                        {
                            var len = Games.Children.Count - r.GameInfos.Count;
                            for (int i = 0; i < len; i++)
                            {
                                var contr = new GameInfoControl() { DataContext = DataContext = r.GameInfos[i] };
                                contr.Enabled += Contr_Enabled;
                                Games.Children.Add(contr);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < GameInfoConfigs.Count; i++)
                        {
                            Games.Children[i].DataContext = GameInfoConfigs[i];
                        }
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
