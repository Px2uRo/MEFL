using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace MEFL.InfoControls
{
    public partial class AccountsControl : UserControl
    {
        public AccountsControl()
        {
            InitializeComponent();
            Enablebtn.Click += Enablebtn_Click;
            MoreInfoGrid.PointerEnter += MoreInfoGrid_PointerEnter;
            MoreInfoGrid.PointerLeave += MoreInfoGrid_PointerLeave;
        }

        private void Enablebtn_Click(object? sender, RoutedEventArgs e)
        {

        }

        private void MoreInfoGrid_PointerLeave(object? sender, Avalonia.Input.PointerEventArgs e)
        {
            Animations.AniBack.RunAsync(MoreInfoBtn, null);
            Animations.WidthBack.RunAsync(Enablebtn, null);
        }

        private void MoreInfoGrid_PointerEnter(object? sender, Avalonia.Input.PointerEventArgs e)
        {
            Animations.AniGo.RunAsync(MoreInfoBtn, null);
            Animations.WidthGo.RunAsync(Enablebtn, null);
        }

    }
}
