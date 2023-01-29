using MEFL.APIData;
using MEFL.Contract;
using MEFL.Controls;
using MEFL.PageModelViews;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace MEFL.Pages
{
    /// <summary>
    /// RealMainPage.xaml 的交互逻辑
    /// </summary>
    public partial class RealMainPage : MEFL.Controls.MyPageBase
    {

        public RealMainPage()
        {
            InitializeComponent();
            (this.DataContext as RealMainPageModelView).PropertyChanged += RealMainPageModelView_PropertyChanged;
        }

        private void MyButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeGameBorder.Visibility = Visibility.Visible;
        }
        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            ((sender as FrameworkElement).DataContext as GameInfoBase).Delete();
            (this.DataContext as RealMainPageModelView).GameInfoConfigs.Remove((sender as FrameworkElement).DataContext as GameInfoBase);
            (this.DataContext as RealMainPageModelView).Invoke("GameInfoConfigs");
        }
        private void SetItemToFavorite(object sender, RoutedEventArgs e)
        {
            (App.Current.Resources["RMPMV"] as RealMainPageModelView).MyFolders[APIModel.SelectedFolderIndex].SetToFavorite((sender as FrameworkElement).DataContext as GameInfoBase);
            (App.Current.Resources["RMPMV"] as RealMainPageModelView).Invoke("GameInfoConfigs");
        }
        private void ItemSetting(object sender, RoutedEventArgs e)
        {
            bool yes =false;
            GenerlSettingGameModel.ModelView.Game = (sender as FrameworkElement).DataContext as GameInfoBase;
            if(FindControl.FromTag("SettingGamePage", (App.Current.Resources["MainPage"] as Grid)).Length == 0)
            {
                yes = true;
            }
            if (yes)
            {
                GenerlSettingGameModel.UI = new SpecialPages.GameSettingPage()
                {
                    Tag = "SettingGamePage",
                    Visibility = Visibility.Hidden,
                    Content = ((sender as FrameworkElement).DataContext as GameInfoBase).SettingsPage,
                    DataContext = GenerlSettingGameModel.ModelView
                };
                (App.Current.Resources["MainPage"] as Grid).Children.Add(GenerlSettingGameModel.UI);
            }
            MyPageBase From = null;
            foreach (MyPageBase item in (App.Current.Resources["MainPage"] as Grid).Children)
            {
                if (item.Visibility == Visibility.Visible)
                {
                    From = item;
                }
            }
            foreach (MyPageBase item in FindControl.FromTag("SettingGamePage", (App.Current.Resources["MainPage"] as Grid)))
            {
                item.Show(From);
            }
        }

        private void RealMainPageModelView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ProcessModelView")
            {
                (sender as RealMainPageModelView).ProcessModelView.PropertyChanged += ProcessModelViewAkaStatuStackPanel_PropertyChanged;
            }
        }
        static DoubleAnimation _dnani = new DoubleAnimation() { EasingFunction=new PowerEase(),Duration=new Duration(TimeSpan.FromSeconds(0.2))};
        private void ProcessModelViewAkaStatuStackPanel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName== "IsStarting")
            {
                if((sender as ProcessModelView).IsStarting == true)
                {
                    Dispatcher.Invoke(() =>
                    {
                        LaunchGameBox.Visibility = Visibility.Hidden;
                        CancelButton.Visibility = Visibility.Visible;
                        _dnani.From = 0;
                        _dnani.To = 80;
                        StatuStackPanel.BeginAnimation(HeightProperty, _dnani);
                    });
                }
            }
            else if(e.PropertyName== "Failed")
            {
                Dispatcher.Invoke(() =>
                {
                    CancelButton.Visibility = Visibility.Hidden;
                LaunchGameBox.Visibility = Visibility.Visible;
                _dnani.From = 80;
                _dnani.To = 0;
                StatuStackPanel.BeginAnimation(HeightProperty, _dnani);
                MessageBox.Show((sender as ProcessModelView).ErrorInfo, e.PropertyName);
                });
            }
            else if (e.PropertyName == "Succeed")
            {
                if ((sender as ProcessModelView).Succeed==true)
                {
                    Dispatcher.Invoke(() =>
                    {
                        ManageProcessesPageModel.ModelView.RunningGames.Add((DataContext as RealMainPageModelView).ProcessModelView.GetProcess);
                        ManageProcessesPageModel.ModelView.RunningGames[ManageProcessesPageModel.ModelView.RunningGames.Count - 1].Start();
                        ManageProcessesPageModel.ModelView.ContentGrid.Children.Add((DataContext as RealMainPageModelView).ProcessModelView.Game.GetManageProcessPage(ManageProcessesPageModel.ModelView.RunningGames[ManageProcessesPageModel.ModelView.RunningGames.Count - 1], APIModel.SettingArgs));
                        MyPageBase From = null;
                        foreach (MyPageBase item in (App.Current.Resources["MainPage"] as Grid).Children)
                        {
                            if (item.Visibility == Visibility.Visible)
                            {
                                From = item;
                            }
                        }
                        foreach (MyPageBase item in FindControl.FromTag("ProcessesManagePage", (App.Current.Resources["MainPage"] as Grid)))
                        {
                            item.Show(From);
                        }
                        CancelButton_Click(sender,new RoutedEventArgs());
                    });
                }
            }
        }

        private void MyComboBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            (this.DataContext as RealMainPageModelView).CurretGame=
                ((sender as Controls.MyItemsCardItem).DataContext 
                as Contract.GameInfoBase);
        }
        static ProcessModelView _pmv;
        private void LaunchGame(object sender, RoutedEventArgs e)
        {
            if (APIModel.CurretGame != null)
            {
            try
            {
                    _pmv= new ProcessModelView(APIModel.CurretGame);
                    (DataContext as RealMainPageModelView).ProcessModelView = _pmv;
                    (DataContext as RealMainPageModelView).ProcessModelView.BuildProcess();
            }
                catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            }
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if(CancelButton.Visibility == Visibility.Hidden)
                {
                    LaunchGame(sender, e);
                }
            }
            else if (e.Key == Key.Escape)
            {
                if (CancelButton.Visibility == Visibility.Visible)
                {
                    CancelButton_Click(sender, e);
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                CancelButton.Visibility = Visibility.Hidden;
                LaunchGameBox.Visibility = Visibility.Visible;
                _dnani.From = 80;
                _dnani.To = 0;
                StatuStackPanel.BeginAnimation(HeightProperty, _dnani);
            });
            _pmv.Cancel();
            _pmv = null;
        }
    }

    public class ProcessModelView : PageModelViews.PageModelViewBase
    {
        public GameInfoBase Game;
        private bool _isStarting;

        public bool IsStarting
        {
            get { return _isStarting; }
            set { _isStarting = value; Invoke(nameof(IsStarting)); }
        }

        private bool _Succeed;
        public bool Succeed
        {
            get { return _Succeed; }
            set { _Succeed = value; Invoke(nameof(Succeed)); }
        }
        private double _Progress;

        public double Progress
        {
            get { return _Progress; }
            set { _Progress = value; Invoke(nameof(Progress)); }
        }
        private string _Statu;

        public string Statu
        {
            get { return _Statu; }
            set { _Statu = value;Invoke(nameof(Statu)); }
        }
        private bool _Failed;
        private string _ErrorInfo;

        public string ErrorInfo
        {
            get { return _ErrorInfo; }
            set { _ErrorInfo = value; }
        }


        public bool Failed
        {
            get { return _Failed; }
            set { _Failed = value; Invoke(nameof(Failed)); }
        }

        private Process _Content;

        public Process GetProcess
        {
            get { return _Content; }
            set { _Content = value; }
        }
        Thread t;
        public void BuildProcess()
        {
                t = new Thread(() => {
                    try
                    {
                        IsStarting = true;
                        Process p = new Process();
                        ProcessStartInfo i = new ProcessStartInfo();
                        Game.Refresh();
                        #region 处理Java
                        Statu = "处理Java";
                        if (APIModel.SettingArgs.SelectedJava == null)
                        {
                            ErrorInfo = $"未选中 JAVA 设置页面去看一下先。";
                            Failed = true;
                            return;
                        }
                        if (Game.JavaMajorVersion == FileVersionInfo.GetVersionInfo(APIModel.SettingArgs.SelectedJava.FullName).FileMajorPart)
                        {
                            i.FileName = APIModel.SettingArgs.SelectedJava.FullName;
                        }
                        else
                        {
                            ErrorInfo = $"不合适的 JAVA\n需要的Java版本\n{Game.JavaMajorVersion}\n当前选择的Java\n{APIModel.SettingArgs.SelectedJava.FullName}\n版本为{FileVersionInfo.GetVersionInfo(APIModel.SettingArgs.SelectedJava.FullName).FileMajorPart}";
                            Failed = true;
                            return;
                        }
                        Progress = 1;
                        #endregion
                        #region 登录用户
                        Statu = "登录用户";
                        try
                        {
                            if(APIModel.SelectedAccount == null)
                            {
                                throw new Exception("未登陆账户");
                            }
                            APIModel.SelectedAccount.LaunchGameAction(APIModel.SettingArgs);
                            Progress = 10;
                        }
                        catch (Exception ex)
                        {
                            ErrorInfo = $"无法登录用户 {ex.Message}，错误发生在 {ex.Source}";
                            Failed = true;
                            return;
                        }
                        #endregion
                        #region 拼接参数
                        try
                        {
                            Statu = "拼接参数";
                            string Args = string.Empty;
                            if (String.IsNullOrEmpty(Game.OtherJVMArgs))
                            {
                                Args += APIModel.SettingConfig.OtherJVMArgs;
                            }
                            else
                            {
                                Args += Game.OtherJVMArgs;
                            }
                            Args += $" {Game.JVMArgs}";

                            var mems = string.Empty;
                            if (Game.GameMaxMem == null || Game.GameMinMem == null || Game.GameMaxMem == 0)
                            {
                                mems = " -Xmn256m -Xmx1256m";
                            }
                            else
                            {
                                mems = $" -Xmn{Game.GameMinMem.ToString()}m -Xmx{Game.GameMaxMem.ToString()}m";
                            }
                            Args += mems;
                            Args += $" {Game.MainClassName}";
                            Args += $" {Game.GameArgs}";
                            var cps = String.Empty;
                            cps += "\"";
                            foreach (var item in Game.ClassPaths)
                            {
                                cps += item;
                                cps += ";";
                            }
                            cps += Game.GameJarPath;
                            cps += "\"";
                            Dictionary<string, string> dic = new Dictionary<string, string>()
            {
                {"${cp}",$"{cps}"},
                {"${classpath}",$"{cps}"},
                {"${HeapDumpPath}","MojangTricksIntelDriversForPerformance_javaw.exe_minecraft.exe.heapdump"},
                {"${auth_player_name}",$"\"{APIData.APIModel.SelectedAccount.UserName}\""},
                {"${version_name}",$"\"{Game.Version}\""},
                {"${game_directory}",$"\"{Game.GameFolder}\""},
                {"${assets_root}",$"\"{Game.dotMinecraftPath}\\assets\""},
                {"${game_assets}",$"\"{Game.dotMinecraftPath}\\assets\\virtual\\legacy\""},
                {"${assets_index_name}",$"{Game.AssetsIndexName}"},
                {"${auth_uuid}",$"{APIData.APIModel.SelectedAccount.Uuid}" },
                {"${auth_access_token}",$"{APIModel.SelectedAccount.AccessToken}"},
                {"${user_type}",$"{APIModel.SelectedAccount.UserType}"},
                {"${version_type}",$"{Game.VersionType}"},
                {"${launcher_name}","MEFL"},
                {"${launcher_version}","0.1" },
                {"${Dos.name}","Windows 10"},
                {"${Dos.version}","10.0"},
                {"${natives_directory}",$"\"{Game.NativeLibrariesPath}\""},
                {"${Dminecraft.client.jar}",Game.GameJarPath }
            };
                            foreach (var item in dic)
                            {
                                Args = Args.Replace(item.Key.ToString(), item.Value.ToString());
                            }
                            i.Arguments = Args;
                            p.StartInfo = i;
                            Progress = 20;
                        }
                        catch (Exception ex)
                        {
                            ErrorInfo = $"无法拼接启动参数{ex.Message}，错误发生在{ex.Source}";
                            Failed = true;
                            return;
                        }
                        #endregion
                        #region 补全文件
                        Statu = "补全文件";
                        #region GetTotalSize
                        double TotalSize = 0.0;
                        double DownloadedSize = 0.0;
                        int DownloadedItems = 0;
                        int TotalItems = 0;
                        foreach (var item in Game.FileNeedsToDownload)
                        {
                            TotalSize += item.size;
                            TotalItems++;
                        }
                        #endregion
                        #region Download
                        if (TotalSize != 0.0)
                        {
                            Debug.WriteLine(TotalItems);
                            foreach (var item in Game.FileNeedsToDownload)
                            {
                                try
                                {
                                    Directory.CreateDirectory(Path.GetDirectoryName(item.localpath));
                                    File.Create(item.localpath).Close();

                                    //todo 补全文件
                                    //APIModel.SelectedDownloader.CreateProgress(item.Url, item.localpath);
                                }
                                catch (Exception ex)
                                {
                                    ErrorInfo = $"无法下载{item.Url}，原因是{ex.Message}";
                                    Failed = true;
                                    return;
                                }
                            }
                        }
                        #endregion
                        #region 解压Native
                        foreach (var item in Game.NativeFilesNeedToDepackage)
                        {
                            try
                            {
                                CoreLaunching.ZipFile.Export(item.localpath, Game.NativeLibrariesPath, true);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.InnerException.Message);
                            }
                        }
                        if (TotalSize-DownloadedSize == 0.0)
                        {
                            Progress = 100;
                            p.StartInfo.RedirectStandardError = true;
                            p.StartInfo.RedirectStandardOutput = true;
                            p.EnableRaisingEvents = true;
                            GetProcess = p;
                            Succeed = true;
                            return;
                        }
                        #endregion
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        ErrorInfo = ex.Message;
                        Failed = true;
                        return;
                    }
                });
                t.Start();
        }

        public void Cancel()
        {
            t = null;
        }

        public ProcessModelView(GameInfoBase game)
        {
            Game = game;
        }
    }

}
