using MEFL.Contract;
using MEFL.PageModelViews;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MEFL
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Contract.Advanced.AddProcessFromList += Advanced_OnDownloadProgressCreated;
            ThreadPool.SetMinThreads(64, 64);
            string str = String.Empty;
            foreach (var item in e.Args)
            {
                str += item;
            }
            Debugger.Logger(string.Format((this.Resources["I18N_String_App_OnStartUp"] as String),str));

            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            (App.Current.Resources["Background"] as Grid).Children.Add(SettingPageModel.img);

            base.OnStartup(e);
        }

        private void Advanced_OnDownloadProgressCreated(object? sender, NativeLocalPairsList e)
        {
            //DownloadingProgressPageModel.ModelView.DownloadingProgresses.Add(e);
            //WebListRefresher.GoToDownloadProgressPage();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            RegManager.Close();
            Debugger.Logger(this.Resources["I18N_String_App_OnExit"] as String);
            base.OnExit(e);
            Process.GetCurrentProcess().Kill();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString());
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Debug.WriteLine($"{e.Exception.Message}");
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if(e.Exception.Source.ToString()!= "MEFL.Controls")
            {
                MessageBox.Show(e.Exception.Message);
            }
            Debugger.Logger(e.Exception.Message,"ERROR");
            e.Handled = true;
        }
    }
}
