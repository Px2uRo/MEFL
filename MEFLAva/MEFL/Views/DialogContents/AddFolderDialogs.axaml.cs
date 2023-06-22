using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using MEFL.AvaControls;
using MEFL.Contract;
using MEFL.PageModelViews;
using System;
using System.IO;
using static MEFL.APIData.APIModel;

namespace MEFL.Views.DialogContents
{
    public partial class AddFolderDialogs : UserControl, IDialogContent
    {
        public static AddFolderDialogs UI = new AddFolderDialogs();
        private string _folderPath;

        public string FolderPath
        {
            get { return _folderPath; }
            set { _folderPath = value;
                //Dispatcher.UIThread.InvokeAsync(
                    PathTB.Text = value;
                //);
            }
        }

        public AddFolderDialogs()
        {
            InitializeComponent();
            PathTB.PropertyChanged += PathTB_PropertyChanged;
            CancelBtn.Click += CancelBtn_Click;
            EnableBtn.Click += EnableBtn_Click;
            ChangeBtn.Click += ChangeBtn_Click;
        }

        private async void ChangeBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
           App.OpenFolderDialog.Title = "Ñ¡ÔñÎÄ¼þ¼Ð";
            if (App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mv = desktop.MainWindow as MainWindow;
                var res = await App.OpenFolderDialog.ShowAsync(mv);
                if (string.IsNullOrEmpty(res))
                {
                    return;
                }
                else
                {
                    FolderPath= res;
                }
            }
        }

        private void EnableBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var vm = RealMainPage.UI.DataContext as RealMainPageModelView;
            vm.MyFolders.Add(new MEFLFolderInfo(PathTB.Text, NameTB.Text));
            vm.SelectedFolderIndex = MyFolders.Count - 1;
            ContentDialog.Show(GamesManagerContent.UI);
        }

        private void CancelBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {

        }

        private void PathTB_PropertyChanged(object? sender, Avalonia.AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == TextBlock.TextProperty)
            {
                var folderName = PathTB.Text.Replace(Path.GetDirectoryName(PathTB.Text) + "\\", "");
                if (folderName == ".minecraft")
                {
                    folderName = Path.GetDirectoryName(PathTB.Text);
                    folderName = folderName.Replace(Path.GetDirectoryName(folderName)+"\\","");
                }
                NameTB.Text = folderName;
            }
        }

        public void WindowSizeChanged(Size size)
        {
            //throw new NotImplementedException();
        }

        public event EventHandler<EventArgs> Quited;
    }
}
