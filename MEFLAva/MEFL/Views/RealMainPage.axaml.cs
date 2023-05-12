using Avalonia;
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
        protected override Size ArrangeOverride(Size finalSize)
        {
            var b = base.ArrangeOverride(finalSize);
            ToolCanvas.Width = b.Width - 10;
            ToolCanvas.Height = b.Height - 50;
            return b;
        }
        public static RealMainPage UI = new RealMainPage();
        public RealMainPage()
        {
            InitializeComponent();
            this.DataContext = new RealMainPageModelView();
            LaunchBtn.Click += LaunchBtn_Click;
            GameBtn.Click += GameBtn_Click;
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
            AccountBtn.Click += AccountBtn_Click;
            GameSettings.Click += GameSettings_Click;
        }

        private void GameSettings_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ContentDialog.Show((DataContext as RealMainPageModelView).CurretGame.SettingsPage);
        }

        private void AccountBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ChooseAccountDialog.UI.ReLoad();
            ContentDialog.Show(ChooseAccountDialog.UI);
        }

        private void GameBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            GamesManagerContent.UI.ReLoad();
            ContentDialog.Show(GamesManagerContent.UI);
        }

        private void LaunchBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            LaunchGameDialog.UI.ReLoad();
            ContentDialog.Show(LaunchGameDialog.UI);
        }
    }
}
