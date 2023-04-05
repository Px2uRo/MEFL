using Avalonia.Controls;
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
        }

        private void Enablebtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            APIModel.CurretGame = this.DataContext as GameInfoBase;
            Enabled?.Invoke(this,null);
        }
    }
}
