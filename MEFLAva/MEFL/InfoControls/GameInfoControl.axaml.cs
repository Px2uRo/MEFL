using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Styling;
using MEFL.APIData;
using MEFL.Contract;
using System;

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
        }

        private void MoreInfoGrid_PointerLeave(object? sender, Avalonia.Input.PointerEventArgs e)
        {
            Animations.AniBack.RunAsync(MoreInfoBtn, null);
            Animations.WidthBack.RunAsync(Enablebtn, null);
        }

        private void MoreInfoGrid_PointerEnter(object? sender, Avalonia.Input.PointerEventArgs e)
        {
            Animations.AniGo.RunAsync(MoreInfoBtn,null);
            Animations.WidthGo.RunAsync(Enablebtn,null);
        }

        private void Enablebtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            APIModel.CurretGame = this.DataContext as GameInfoBase;
            Enabled?.Invoke(this,null);
        }
    }
}
