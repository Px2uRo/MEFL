using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MEFL.APIData;
using MEFL.AvaControls;
using MEFL.Contract;
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
            APIModel.SelectedAccount = this.DataContext as AccountBase;
            this.Icon.Child= null;
            ContentDialog.Quit();
        }

        private void MoreInfoGrid_PointerLeave(object? sender, Avalonia.Input.PointerEventArgs e)
        {
            Animations.MarginBack(new Thickness(30, 0, 0, 0)).RunAsync(MoreInfoBtn, null);
            Animations.WidthBack(288.0).RunAsync(Enablebtn, null);
        }

        private void MoreInfoGrid_PointerEnter(object? sender, Avalonia.Input.PointerEventArgs e)
        {
            Animations.MarginGo(new Thickness(0,0,0,0)).RunAsync(MoreInfoBtn, null);
            Animations.WidthGo(258.0).RunAsync(Enablebtn, null);
        }

    }
}
