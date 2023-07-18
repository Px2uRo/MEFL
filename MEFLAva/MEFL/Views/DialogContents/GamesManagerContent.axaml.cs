using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Avalonia.Threading;
using DynamicData;
using MEFL.APIData;
using MEFL.AvaControls;
using MEFL.Contract;
using MEFL.InfoControls;
using MEFL.PageModelViews;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static MEFL.APIData.APIModel;

namespace MEFL.Views.DialogContents
{
    public partial class GamesManagerContent : UserControl, IDialogContent
    {
        private UniformGrid GetUniformGrid(string gameTypeFriendlyName)
        {
            var res = new UniformGrid();
            var column = (int)Width / 285;
            if(column<1) column= 1;
            res.Columns = column;
            res.Tag= gameTypeFriendlyName;
            return res;
        }
        public event EventHandler<EventArgs> Quited;
        public static GamesManagerContent UI = new GamesManagerContent();
        public GamesManagerContent()
        {
            InitializeComponent();
            this.DataContext = App.Current.Resources["RMPMV"];
            CancelBtn.Click += Contr_Enabled;
            AddFolderBtn.Click += AddFolderBtn_Click;
            DelFolderBtn.Click += DelFolderBtn_Click;
            ExploreFolderBtn.Click += ExploreFolderBtn_Click;
            SearchBtn.Click += SearchBtn_Click;
            SearchBox.PropertyChanged += SearchBox_PropertyChanged;
        }

        private void SearchBox_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == TextBox.TextProperty)
            {
                SearchBtn_Click(e, new Avalonia.Interactivity.RoutedEventArgs());
            }
        }

        private void SearchBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                var r = GameRefresher.Main;
                if (r.RefreshOK == true)
                {
                    if (r.RefreshOK == true)
                    {
                        ClearSP();
                        AddSP(r.GameInfos.Where(x => x.Name.Contains(SearchBox.Text)));
                    }
                    else
                    {
                    }
                }
                else
                {

                }
            });
        }

        private void ExploreFolderBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var path = MyFolders[SelectedFolderIndex].Path;
            Process.Start("Explorer.exe", path);
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
            d.Title = "选择文件夹";
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

        private void ReColumn()
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                for (int i = 0; i < Games.Children.Count; i++)
                {
                    if (Games.Children[i] is UniformGrid ug)
                    {
                        var column = (int)Width / 285;
                        if (column < 1) column = 1;
                        ug.Columns = column;
                    }
                }
            });

        }

        public void ReLoad()
        {
            SearchBox.Text = string.Empty;
            SearchBox.IsEnabled = false;
            GameRefresher.Main.PropertyChanged -= Main_PropertyChanged;
            GameRefresher.Main.PropertyChanged += Main_PropertyChanged;
            GameRefresher.Main.Refresh().Start();
        }

        internal void ClearSP()
        {
            for (int i = 0; i < Games.Children.Count; i++)
            {
                if (Games.Children[i] is UniformGrid ug)
                {
                    for (int j = 0; j < ug.Children.Count; j++)
                    {
                        if((ug.Children[j] is GameInfoControl c)&&c.DataContext is GameInfoBase g)
                        {
                            g.Dispose();
                            GC.SuppressFinalize(c);
                            ug.Children.Remove(c);
                            j--;
                        }
                    }
                    Games.Children.Remove(ug);
                    GC.SuppressFinalize(ug);
                    i--;
                }
                else if (Games.Children[i] is TextBlock tb)
                {
                    Games.Children.Remove(tb);
                    GC.SuppressFinalize(tb);
                    i--;
                }
            }
        }

        internal void AddSP(IEnumerable<GameInfoBase> items)
        {
            List<string> strings = new List<string>();
            List<UniformGrid> ugs = new();
            var f = APIModel.MyFolders[APIModel.SelectedFolderIndex].Favorites;
            if (f.Count > 0)
            {
                strings.Add("收藏夹");
                ugs.Add(GetUniformGrid("收藏夹"));
            }
            foreach (var item in items)
            {
                var name = Path.GetFileName(item.GameJsonPath);
                if (f.Contains(name))
                {
                    var contr = new GameInfoControl() { DataContext = item };
                    contr.Margin = new Thickness(3);
                    contr.Enabled += Contr_Enabled;
                    var targ = ugs.Where(ug => ug.Tag.ToString() == "收藏夹").First();
                    targ.Children.Add(contr);
                }
                else
                {
                    if (!strings.Contains(item.GameTypeFriendlyName))
                    {
                        strings.Add(item.GameTypeFriendlyName);
                        ugs.Add(GetUniformGrid(item.GameTypeFriendlyName));
                    }
                    var contr = new GameInfoControl() { DataContext = item };
                    contr.Margin = new Thickness(3);
                    contr.Enabled += Contr_Enabled;
                    var targ = ugs.Where(ug => ug.Tag.ToString() == item.GameTypeFriendlyName).First();
                    targ.Children.Add(contr);
                }
            }
            strings = strings.OrderBy(x => x).ToList();
            ugs = ugs.OrderBy(x => x.Tag.ToString()).ToList();
            for (int i = 0; i < strings.Count; i++)
            {
                Games.Children.Add(new TextBlock() { Text = strings[i],FontWeight=Avalonia.Media.FontWeight.Bold, FontSize = 20 });
                Games.Children.Add(ugs[i]);
            }
        }
        private void Main_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var r = sender as GameRefresher;
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                SearchBox.IsEnabled = true;
                if (r.RefreshOK == true)
                {
                    ClearSP();
                    AddSP(r.GameInfos);
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

        public void WindowSizeChanged(Size size)
        {
            if (size.Width > 650&& size.Width < 1300)
            {
                this.Width= 600;
            }
            else if (size.Width>=1300)
            {
                Width = 900;
            }
            else
            {
                Width = 330;
            }

            if (size.Height > 550 && size.Height < 800)
            {
                Height= 400;
            }
            else if (size.Height >= 800)
            {
                Height= 600;
            }
            else
            {
                Height = 300;
            }

            ReColumn();
        }
    }
}
