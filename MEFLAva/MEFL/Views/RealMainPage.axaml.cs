using Avalonia.Controls;
using Avalonia.Threading;
using MEFL.APIData;
using MEFL.AvaControls;
using MEFL.PageModelViews;
using MEFL.Views.DialogContents;
using System.Linq;
using static MEFL.APIData.APIModel;

namespace MEFL.Views
{
    public partial class RealMainPage : UserControl
    {
        public static RealMainPage UI = new RealMainPage();
        public RealMainPage()
        {
            InitializeComponent();
            this.DataContext = new RealMainPageModelView();
            LaunchBtn.Click += LaunchBtn_Click;
            GameBtn.Click += GameBtn_Click;
            PMV = new();
            PMV.PropertyChanged += PMV_PropertyChanged;
            GameLoader.AwaitLoadAll(MyFolders[SelectedFolderIndex]);
            if (!string.IsNullOrEmpty(APIModel.SettingConfig.SelectedGame))
            {
                var f = MyFolders[SelectedFolderIndex];
                var linq = f.Games.Where((x) => x.RootFolder.EndsWith(APIModel.SettingConfig.SelectedGame)).ToArray();
                if (linq.Count() > 0)
                {
                    CurretGame = linq[0];
                }
            }
        }

        private void GameBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            GamesManagerContent.UI.ReLoad();
            ContentDialog.Show(GamesManagerContent.UI);
        }

        private void PMV_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var vm = (ProcessModelView)sender;
            if (e.PropertyName =="Progress")
            {
                var stn = vm.Progress.ToString();
                if (stn.Length >= 2)
                {
                    LaunchBtnText.Text =  stn[..2] + "%";
                }
                else if (stn.Length >= 1)
                {
                    LaunchBtnText.Text = stn[..1] + "%";
                }
            }
            else if (e.PropertyName==nameof(vm.Failed))
            {
                Debugger.Logger(vm.ErrorInfo);
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    LaunchBtnText.Text = "Launch!";
                });
            }
        }

        static ProcessModelView PMV;
        private void LaunchBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            PMV.Game = APIModel.CurretGame;
            PMV.BuildProcess();
        }
    }
}
