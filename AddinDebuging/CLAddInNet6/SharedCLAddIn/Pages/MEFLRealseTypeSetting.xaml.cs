using MEFL.CLAddIn.GameTypes;
using MEFL.Contract;
using MEFL.Controls;
using System;
using System.ComponentModel;
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

        private void Radio_Checked(object sender, RoutedEventArgs e)
        {
            CustomTB.IsEnabled = false;
            var rdo = (RadioButton)sender;
            var con = (DataContext as CLGameType);
            if (rdo.Name == "Radio1")
            {
                con.GamePathType = GamePathType.DotMCPath;
            }
            if (rdo.Name == "Radio2")
            {
                con.GamePathType = GamePathType.Versions;
            }
            if (rdo.Name == "Radio3")
            {
                con.GamePathType = GamePathType.Custom;
                CustomTB.IsEnabled= true;
            }
        }
        private void Data_Changed(object sender, DependencyPropertyChangedEventArgs e)
        {
            var con = (DataContext as CLGameType);
            con.PropertyChanged -= Value_PropertyChanged;
            con.PropertyChanged += Value_PropertyChanged;
            if (con.GamePathType == GamePathType.DotMCPath)
            {
                Radio1.IsChecked = true;
            }
            else if (con.GamePathType==GamePathType.Versions)
            {
                Radio2.IsChecked = true;
            }
            else
            {
                Radio3.IsChecked = true;
            }
        }
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
