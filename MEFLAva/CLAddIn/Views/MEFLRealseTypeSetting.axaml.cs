using Avalonia;
using Avalonia.Controls;
using MEFL.CLAddIn.GameTypes;
using MEFL.Contract;

namespace CLAddIn.Views
{
    public partial class MEFLRealseTypeSetting : UserControl,IGameSettingPage
    {
        public MEFLRealseTypeSetting()
        {
            InitializeComponent();
            this.DataContextChanged += MEFLRealseTypeSetting_DataContextChanged;
            UseMC.Checked += UseMC_Checked;
            Version.Checked += Version_Checked;
            Custom.Checked += Custom_Checked;
            ExcuteBtn.Click += ExcuteBtn_Click;
        }

        private void ExcuteBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Quited.Invoke(this,e);
        }

        private void Custom_Checked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (this.DataContext is CLGameType a)
            {
                a.GamePathType = GamePathType.Custom;
            }
        }

        private void Version_Checked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (this.DataContext is CLGameType a)
            {
                a.GamePathType = GamePathType.Versions;
            }
        }

        private void UseMC_Checked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (this.DataContext is CLGameType a)
            {
                a.GamePathType = GamePathType.DotMCPath;
            }
        }

        private void MEFLRealseTypeSetting_DataContextChanged(object? sender, EventArgs e)
        {
            if(this.DataContext is CLGameType a)
            {
                a.PropertyChanged -= MEFLRealseTypeSetting_PropertyChanged;
                a.PropertyChanged += MEFLRealseTypeSetting_PropertyChanged;
                if(a.GamePathType==GamePathType.DotMCPath)
                {
                    UseMC.IsChecked=true;
                }
                else if (a.GamePathType == GamePathType.Versions)
                {
                    Version.IsChecked = true;
                }
                else if(a.GamePathType == GamePathType.Custom)
                {
                    Custom.IsChecked = true;
                }
            }
        }

        private void MEFLRealseTypeSetting_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        public void WindowSizeChanged(Size size)
        {
            if (size.Height >= 700 && size.Width >= 800) {
                this.Width = 800;Height = 600;
            }
            else if (size.Height >= 700 && size.Width >= 900)
            {
                this.Width = 800; Height = 600;
            }
            else
            {
                this.Width = 500; Height = 300;
            }
        }

        public event EventHandler<GameInfoBase> OnSelected;
        public event EventHandler<GameInfoBase> OnRemoved;
        public event EventHandler<GameInfoBase> OnPageBack;
        public event EventHandler<GameInfoBase> OnListUpdate;
        public event EventHandler<EventArgs> Quited;
    }
}
