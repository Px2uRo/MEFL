using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Themes.Default;
using Avalonia.Themes.Fluent;
using MEFL.PageModelViews;
using MEFL.Views;
using System.Diagnostics;
using System.Linq;

namespace MEFL
{
    public partial class App : Application
    {
        internal static OpenFolderDialog OpenFolderDialog = new();
        internal static OpenFileDialog OpenFileDialog = new();
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
                desktop.Exit += Desktop_Exit;
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void Desktop_Exit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
        {
            for (int i = 0; i < ManageProcessesPageModel.ModelView.RunningGames.Count; i++)
            {
                ManageProcessesPageModel.ModelView.RunningGames.Values.ToArray()[i].LauncherQuited();
            }
            Process.GetCurrentProcess().Kill();
        }
    }
}