using MEFL.Contract;
using MEFL.Controls;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MEFL.CLAddIn.Pages
{
    /// <summary>
    /// MEFLRealseTypeSetting.xaml 的交互逻辑
    /// </summary>
    public partial class MEFLRealseTypeSetting : UserControl,IGameSettingPage
    {
        public event EventHandler<GameInfoBase> OnSelected;
        public event EventHandler<GameInfoBase> OnRemoved;
        public event EventHandler<GameInfoBase> OnPageBack;
        public event EventHandler<GameInfoBase> OnListUpdate;
        GameInfoBase _game;
        public GameInfoBase Game { get => _game; set { value.PropertyChanged -= Value_PropertyChanged; value.PropertyChanged += Value_PropertyChanged; _game = value; } }

        private void Value_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        private void Selected_Click(object sender, RoutedEventArgs e)
        {
            OnSelected?.Invoke(this, this.DataContext as GameInfoBase);
        }
        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            OnListUpdate?.Invoke(this, this.DataContext as GameInfoBase);
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            OnRemoved?.Invoke(this, this.DataContext as GameInfoBase);
        }

        public void SetShowModCard(bool value)
        {
            if (value)
            {

            }
            else
            {

            }
        }
        public MEFLRealseTypeSetting()
        {
            InitializeComponent();
        }

        private void MyButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
