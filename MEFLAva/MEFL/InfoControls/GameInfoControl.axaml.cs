using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Styling;
using Avalonia.Threading;
using MEFL.APIData;
using MEFL.AvaControls;
using MEFL.Contract;
using MEFL.Views.DialogContents;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MEFL.InfoControls
{
    public partial class GameInfoControl : UserControl
    {

        public event EventHandler Enabled;
        public GameInfoControl()
        {
            InitializeComponent();
            Enablebtn.Click += Enablebtn_Click;
            MoreInfoGrid.PointerEnter += MoreInfoGrid_PointerEnter;
            MoreInfoGrid.PointerLeave += MoreInfoGrid_PointerLeave;
            MoreInfoBtn.Click += MoreInfoBtn_Click;
            FavorBtn.Click += FavorBtn_Click;
            DeleteBtn.Click += DeleteBtn_Click;
        }

        private async void DeleteBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if(DataContext is GameInfoBase g)
            {
                var res = await g.Delete();
                if (res == DeleteResult.Finished)
                {
                    if(Parent is Panel p)
                    {
                        p.Children.Remove(this);
                    }
                }
            }
        }

        private void FavorBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var f = APIModel.MyFolders[APIModel.SelectedFolderIndex];
            if(DataContext is GameInfoBase g)
            {
                var name = Path.GetFileName(g.GameJsonPath);
                if (f.Favorites.Contains(name))
                {
                    f.Favorites.Remove(name);
                }
                else
                {
                    f.Favorites.Add(name);
                }
            }
            GamesManagerContent.UI.ClearSP();
            GamesManagerContent.UI.AddSP(GameRefresher.Main.GameInfos);
        }

        private void MoreInfoBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var g = DataContext as GameInfoBase; 
            if (g != null)
            {
                ContentDialog.Show(g.SettingsPage);
            }
        }

        private void MoreInfoGrid_PointerLeave(object? sender, Avalonia.Input.PointerEventArgs e)
        {
            Animations.MarginBack(new Thickness(30, 0, 0, 0)).RunAsync(MoreInfoBtn, null);
            Animations.MarginBack(new Thickness(30, 0, 0, 0)).RunAsync(DeleteBtn, null);
            Animations.MarginBack(new Thickness(30, 0, 0, 0)).RunAsync(FavorBtn, null);
            Animations.WidthBack(288.0).RunAsync(Enablebtn, null);
        }

        private void MoreInfoGrid_PointerEnter(object? sender, Avalonia.Input.PointerEventArgs e)
        {
            Animations.MarginGo(new(0, 0, 0, 0)).RunAsync(MoreInfoBtn, null);
            Animations.MarginGo(new(0, 0, 0, 0)).RunAsync(DeleteBtn, null);
            Animations.MarginGo(new(0, 0, 0, 0)).RunAsync(FavorBtn, null);
            Animations.WidthGo(258.0).RunAsync(Enablebtn,null);
        }

        private void Enablebtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            APIModel.CurretGame = this.DataContext as GameInfoBase;
            Enabled?.Invoke(this,null);
        }
    }
}
