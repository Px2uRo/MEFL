using MEFL.Contract;
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
        public event EventHandler<EventArgs> OnSelected;
        public event EventHandler<EventArgs> OnRemoved;
        public event EventHandler<EventArgs> OnPageBack;
        public event EventHandler<EventArgs> OnListUpdate;
        GameInfoBase _game;
        public GameInfoBase Game { get => _game; set { value.PropertyChanged -= Value_PropertyChanged; value.PropertyChanged += Value_PropertyChanged; _game = value; } }

        private void Value_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            
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
