using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using MEFL.APIData;
using MEFL.Contract;
using MEFL.Controls;
using Newtonsoft.Json;
using System.Linq;
using MEFL.Pages;
using System.Reflection.Metadata;
using MEFL.EventsMethod;
using System.IO;
using CoreLaunching.JsonTemplates;
using System.Windows.Threading;

namespace MEFL.PageModelViews
{
    /// <summary>
    /// 也叫做RMPMV
    /// </summary>
    public class RealMainPageModelView:PageModelViewBase
    {
        private Visibility _changeGameVisblity = Visibility.Hidden;

        public Visibility ChangeGameVisblity
        {
            get { return _changeGameVisblity; }
            set { _changeGameVisblity = value; Invoke(nameof(ChangeGameVisblity)); }
        }

        private Pages.ProcessModelView _ProcessModelView;
        public Pages.ProcessModelView ProcessModelView { get => _ProcessModelView; set { _ProcessModelView = value; Invoke(nameof(ProcessModelView)); } }
        public Contract.GameInfoBase CurretGame
        {
            get 
            {
                 return APIModel.CurretGame;
            }
            set 
            {
                APIModel.CurretGame = value;
                Invoke("CurretGame");
            }
        }

        public Visibility IsRefreshing { get; set; }
        public Visibility IsNotRefreshing { get {
                if (IsRefreshing == Visibility.Visible)
                {
                    return Visibility.Hidden;
                }
                else
                {
                    return Visibility.Visible;
                }
            } }

        public GameInfoCollection GameInfoConfigs
        {
            get
            {
                Invoke(nameof(IsRefreshing));
                Invoke(nameof(IsNotRefreshing));
                return APIModel.GameInfoConfigs;
            }
            set
            {
                APIModel.GameInfoConfigs = value;
                Invoke("GameInfoConfigs");
            }
        }

        public MEFLFolderColletion MyFolders 
        {
            get
            {
                return APIModel.MyFolders;
            }
            set
            {
                APIModel.MyFolders = value;
                Invoke("MyFolders");
            }
        }

        public int SelectedFolderIndex
        {
            get
            {
                return APIData.APIModel.SelectedFolderIndex; }
            set {
                if (value < 0)
                {
                    value = 0;
                }
                APIData.APIModel.SelectedFolderIndex = value;
                APIData.APIModel.GameInfoConfigs =MyFolders[value].Games;
                Invoke("GameInfoConfigs");
                Invoke("SelectedFolderIndex");
            }
        }

        public ICommand ChangeAccountCommand { get => RealMainPageModel.ChangeAccountCommand; }
        public ICommand AddFolderInfoCommand { get=> RealMainPageModel.AddFolderInfoCommand; 
            set { RealMainPageModel.AddFolderInfoCommand = value; } }
        public ICommand GameSettingCommand { get; set; }
        public ICommand RefreshFolderInfoCommand { get; set; }
        public ICommand OpenInExplorer =>new OpenInExplorer();

        public RealMainPageModelView()
        {
            RefreshFolderInfoCommand = new RefreshFolderInfoCommand();
            GameInfoConfigs = MyFolders[SelectedFolderIndex].Games;
            RefreshFolderInfoCommand_ClickBeihavior(null);
            GameSettingCommand = new GameSettingCommand();
        }

        public void RefreshFolderInfoCommand_ClickBeihavior(string parameter)
        {
            new Thread(() =>
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    if (GameRefresher.Refreshing != true){
                        GameRefresher.RefreshCurrect();
                        IsRefreshing = Visibility.Visible;
                        Invoke(nameof(IsRefreshing));
                        Invoke(nameof(IsNotRefreshing));
                        while (GameRefresher.Refreshing)
                        {
                        }
                        SelectedFolderIndex = SelectedFolderIndex;
                        Invoke(nameof(IsRefreshing));
                        Invoke(nameof(IsNotRefreshing));
                        IsRefreshing = Visibility.Hidden;
                        try
                        {
                            foreach (var item in GameInfoConfigs)
                            {
                                if (item.RootFolder == APIModel.SettingConfig.SelectedGame)
                                {
                                    CurretGame = item;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Debugger.Logger(ex.Message);
                            RegManager.Write("CurretGame", String.Empty);
                        }
                    }
                    else if(parameter == "Force")
                    {
                        MyFolders[SelectedFolderIndex].Games = new GameInfoCollection();
                        GameRefresher.Refreshing = false;
                        GameRefresher.RefreshCurrect();
                        IsRefreshing = Visibility.Visible;
                        Invoke(nameof(IsRefreshing));
                        Invoke(nameof(IsNotRefreshing));
                        while (GameRefresher.Refreshing)
                        {
                        }
                        SelectedFolderIndex = SelectedFolderIndex;
                        Invoke(nameof(IsRefreshing));
                        Invoke(nameof(IsNotRefreshing));
                        IsRefreshing = Visibility.Hidden;
                        try
                        {
                            foreach (var item in GameInfoConfigs)
                            {
                                if (item.RootFolder == APIModel.SettingConfig.SelectedGame)
                                {
                                    CurretGame = item;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Debugger.Logger(ex.Message);
                            RegManager.Write("CurretGame", String.Empty);
                        }
                    }
                    Invoke(nameof(GameInfoConfigs));
                });
            }).Start();
        }
    }

    public class OpenInExplorer : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            System.Diagnostics.Process.Start("explorer.exe", APIModel.MyFolders[APIModel.SelectedFolderIndex].Path);
        }
    }

    public class GameSettingCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }


        public void Execute(object? parameter)
        {
            if (APIModel.CurretGame == null)
            {
                MessageBox.Show("没有选中的游戏");
                return;
            }
            bool yes = false;
            //GenerlSettingGameModel.ModelView._gameWatcher = APIModel.CurretGame;
            if (FindControl.FromTag("SettingGamePage", (App.Current.Resources["MainPage"] as Grid)).Length == 0)
            {
                yes = true;
            }
            if (APIModel.CurretGame.SettingsPage is not FrameworkElement)
            {
                MyMessageBox.Show("添加用户页面不是 FrameworkElement，请联系开发者");
                return;
            }
            if (yes)
            {
                var newPage = new SpecialPages.GameSettingPage()
                {
                    Tag = "SettingGamePage",
                    Visibility = Visibility.Hidden,
                    Content = APIModel.CurretGame.SettingsPage,
                };
                (App.Current.Resources["MainPage"] as Grid).Children.Add(newPage);
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
                item.Content = APIModel.CurretGame.SettingsPage;
                #region Events
                APIModel.CurretGame.SettingsPage.OnPageBack -= EventToolkit.SettingsPage_OnPageBack;
                APIModel.CurretGame.SettingsPage.OnRemoved -= EventToolkit.SettingsPage_OnRemoved;
                APIModel.CurretGame.SettingsPage.OnSelected -= EventToolkit.SettingsPage_OnSelected;
                APIModel.CurretGame.SettingsPage.OnListUpdate -= EventToolkit.SettingsPage_OnListUpdate;
                APIModel.CurretGame.SettingsPage.OnPageBack += EventToolkit.SettingsPage_OnPageBack;
                APIModel.CurretGame.SettingsPage.OnRemoved += EventToolkit.SettingsPage_OnRemoved;
                APIModel.CurretGame.SettingsPage.OnSelected += EventToolkit.SettingsPage_OnSelected;
                APIModel.CurretGame.SettingsPage.OnListUpdate += EventToolkit.SettingsPage_OnListUpdate;
                #endregion
                item.Show(From);
            }

        }
    }
    public class RefreshFolderInfoCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            (App.Current.Resources["RMPMV"] as RealMainPageModelView).RefreshFolderInfoCommand_ClickBeihavior(parameter as String);
        }
    }

    public static class RealMainPageModel
    {
        public static ICommand AddFolderInfoCommand;
        public static ICommand ChangeAccountCommand;
        static RealMainPageModel()
        {
            AddFolderInfoCommand = new AddFolderInfoCommand();
            ChangeAccountCommand = new ChangeAccountCommand();
        }
    }

    public class ChangeAccountCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            MyPageBase From = null;
            foreach (MyPageBase item in (App.Current.Resources["MainPage"] as Grid).Children)
            {
                if (item.Visibility == Visibility.Visible)
                {
                    From = item;
                }
            }
            foreach (MyPageBase item in FindControl.FromTag("UserManagePage", (App.Current.Resources["MainPage"] as Grid)))
            {
                item.Show(From);
            }
        }
    }


    public class AddFolderInfoCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            
            //Todo 注册表嘛！i18N嘛！
            string RegKey = "根目录";

            for (int i = 0; i < (App.Current.Resources["MainPage"] as Grid).Children.Count; i++)
            {
                if (((App.Current.Resources["MainPage"] as Grid).Children[i] as MyPageBase).Tag == "PickUP"|| ((App.Current.Resources["MainPage"] as Grid).Children[i] as MyPageBase).Tag == "RenameFolder"
                )
                {
                    (App.Current.Resources["MainPage"] as Grid).Children.RemoveAt(i);
                }
            }
            (App.Current.Resources["MainPage"] as Grid).Children.Add(new SpecialPages.PickUpAFolder() { Tag="PickUP",Visibility=Visibility.Hidden,Currect=RegKey});
            MyPageBase From = null;
            foreach (MyPageBase item in (App.Current.Resources["MainPage"] as Grid).Children)
            {
                if (item.Visibility == Visibility.Visible)
                {
                    From = item;
                }
            }
            foreach (MyPageBase item in FindControl.FromTag("PickUP", (App.Current.Resources["MainPage"] as Grid)))
            {
                item.Show(From);
            }

            RegKey = null;
        }
    }
    public class IndexToUI : IValueConverter
    {
        #region cards
        Controls.MyItemsCard favorcard = new Controls.MyItemsCard()
        {
            Height = 0,
            IsAbleToSwap = true,
            Title = "Favorite",
            Margin = VarMargin,
            BorderThickness = VarBorderThickness,
            BorderBrush = VarBorderBrush,
            CornerRadius = VarConrnerRadius
        };
        Controls.MyItemsCard newCard = new Controls.MyItemsCard()
        {
            Height = 0,
            IsAbleToSwap = true,
            Title = "NEW",
            Margin = VarMargin,
            BorderThickness = VarBorderThickness,
            BorderBrush = VarBorderBrush,
            CornerRadius = VarConrnerRadius,
            ItemsSource=new ObservableCollection<GameInfoBase>()
        };
        Controls.MyCard Note = new Controls.MyCard()
        {
            Height = 0,
            Title = "提示",
            Margin = VarMargin,
            BorderThickness = VarBorderThickness,
            BorderBrush = VarBorderBrush,
            CornerRadius = VarConrnerRadius
        };
        #endregion
        #region 一堆字段而已
        internal bool UpdateUI = false;

        FileSystemWatcher _gameWatcher;
        static Thickness VarMargin = new Thickness(0, 0, 0, 2);
        static Thickness VarBorderThickness = new Thickness(5);
        static Brush VarBorderBrush = App.Current.Resources["SYTLE_Standard_BorderBrush"] as SolidColorBrush;
        static CornerRadius VarConrnerRadius = new CornerRadius(5);
        public StackPanel MyGamesSP = new StackPanel();
        List<Controls.MyItemsCard> cards;
        ObservableCollection<Contract.GameInfoBase> favoritem;
        ObservableCollection<Contract.GameInfoBase> newItems;

        Queue<GameInfoBase> ReadyToBeRemoved = new();
        Queue<string> _readyToBeAdded = new();
        #endregion
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            #region Watcher而已
            if (_gameWatcher != null)
            {
                _gameWatcher.Dispose();
                _gameWatcher = null;
            }
            var path = Path.Combine(APIModel.MyFolders[APIModel.SelectedFolderIndex].Path,"versions");
            if (Directory.Exists(path))
            {
                _gameWatcher = new FileSystemWatcher(path);
                _gameWatcher.IncludeSubdirectories = true;
                //_gameWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName;
                //_gameWatcher.Created += _mapWacther_Created;
                //_gameWatcher.Deleted += _mapWacther_Deleted;
                //_gameWatcher.EnableRaisingEvents = true;
            }
            #endregion

            foreach (var item in value as ObservableCollection<Contract.GameInfoBase>)
            {
                item.Dispose();
            }
            MyGamesSP.Children.Clear();

            #region 收藏夹而已
            cards = new List<Controls.MyItemsCard>();
             favoritem = new ObservableCollection<Contract.GameInfoBase>();
            ObservableCollection<String> favorites = (App.Current.Resources["RMPMV"] as RealMainPageModelView).MyFolders[APIModel.SelectedFolderIndex].Favorites;
            foreach (var item in APIModel.GameInfoConfigs)
            {
                foreach (var item2 in favorites)
                {
                    if (item.GameJsonPath == item2)
                    {
                        favoritem.Add(item);
                    }
                }
            }
            favorcard.ItemsSource = favoritem;
            if (favoritem.Count != 0)
            {
                favorcard.Height = double.NaN;
            }
            newCard.Height = 0;
            (newCard.ItemsSource as ObservableCollection<GameInfoBase>).Clear();
            MyGamesSP.Children.Add(newCard);
            MyGamesSP.Children.Add(favorcard);
            #endregion

            #region 确定有多少卡片而已
            List<string> recorded = new List<string>();
            foreach (var item in APIModel.GameInfoConfigs)
            {
                if (recorded.Contains(item.GameTypeFriendlyName) != true)
                {
                    cards.Add(new Controls.MyItemsCard() { IsAbleToSwap = true, Title = item.GameTypeFriendlyName, Margin = VarMargin, BorderThickness = VarBorderThickness, BorderBrush = VarBorderBrush, CornerRadius = VarConrnerRadius });
                    recorded.Add(item.GameTypeFriendlyName);
                }
            }
            #endregion

            #region 给卡片加 ItemSources 而已

            for (int i = 0; i < cards.Count; i++)
            {
                var cardItemSources = new ObservableCollection<Contract.GameInfoBase>();
                foreach (var item in APIModel.GameInfoConfigs)
                {
                    if (item.GameTypeFriendlyName == cards[i].Title.ToString())
                    {
                        cardItemSources.Add(item);
                    }
                }
                cards[i].ItemsSource = cardItemSources;
            }
            #endregion
            if(favorcard.Items.Count== 0)
            {
                favorcard.Height = 0;
            }
            if(newCard.Items.Count== 0)
            {
                newCard.Height = 0;
            }
            if (cards.Count == 0)
            {
                Note.Height = double.NaN;
                Note.Content = "没有下载的版本\n请自行下载或导入";
                MyGamesSP.Children.Add(Note);
            }
            else
            {
                foreach (var item in cards)
                {
                    MyGamesSP.Children.Add(item);
                }
            }
            return MyGamesSP;
        }
        #region WatcherMethods
        internal void DealWithNew()
        {
            while (_readyToBeAdded.Count > 0)
            {
                var tar = _readyToBeAdded.Dequeue();
                AddNew(GameLoader.LoadOne(tar));
            }
            while (ReadyToBeRemoved.Count > 0) 
            {   
                var tar = ReadyToBeRemoved.Dequeue();
                DeleteNew(tar);
            };
        }
        private void AddNew(GameInfoBase game)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                var linq = cards.Where((x) => x.Title.ToString() == game.GameTypeFriendlyName).ToArray();
                APIModel.GameInfoConfigs.Add(game);
                if (linq.Count() == 0)
                {
                    newCard.ItemsSource = newItems;
                    var tar = newCard.ItemsSource as ObservableCollection<GameInfoBase>;
                    tar.Add(game);
                    newCard.Height = double.NaN;
                    newCard.PART_MY_CARD.OriginalHeight = (tar.Count) * 30 + 70;
                    if (!newCard.PART_MY_CARD.IsSwaped)
                    {
                        newCard.PART_MY_CARD.Height = newCard.PART_MY_CARD.OriginalHeight;
                    }
                }
                else
                {
                    (linq[0].ItemsSource as ObservableCollection<GameInfoBase>).Add(game);
                    linq[0].PART_MY_CARD.OriginalHeight += 30;
                    if (!linq[0].IsSwaped)
                    {
                        linq[0].PART_MY_CARD.Height = linq[0].PART_MY_CARD.OriginalHeight;
                    }
                }
            });
        }
        private void DeleteNew(GameInfoBase game)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                var linq = cards.Where((x) => x.Title.ToString() == game.GameTypeFriendlyName).ToArray();
                var items = (linq[0].ItemsSource as ObservableCollection<GameInfoBase>);
                items.Remove(game);
                linq[0].PART_MY_CARD.OriginalHeight -= 30;
                if (!linq[0].IsSwaped)
                {
                    linq[0].PART_MY_CARD.Height = linq[0].PART_MY_CARD.OriginalHeight;
                }

                APIModel.GameInfoConfigs.Remove(game);
                game.Dispose();
            });
        }
        #endregion

        private void _mapWacther_Deleted(object sender, FileSystemEventArgs e)
        {
            var versionpath = Path.Combine(APIModel.MyFolders[APIModel.SelectedFolderIndex].Path, "versions");
            if (Path.GetDirectoryName(e.FullPath) == versionpath)
            {
                var linq = APIModel.GameInfoConfigs.Where((x) =>x.GameJsonPath==e.FullPath).ToArray()[0];
                if (UpdateUI)
                {
                    DeleteNew(linq);
                }
                else
                {
                    ReadyToBeRemoved.Enqueue(linq);
                }
            }
        }


        private void _mapWacther_Created(object sender, FileSystemEventArgs e)
        {
            var versionpath = Path.Combine(APIModel.MyFolders[APIModel.SelectedFolderIndex].Path, "versions",Path.GetFileNameWithoutExtension(e.FullPath));
            var jsonpath = Path.GetDirectoryName(e.FullPath);
            if (e.Name.EndsWith(".json")&&versionpath==jsonpath)
            {
                if (UpdateUI == false)
                {
                    _readyToBeAdded.Enqueue(e.FullPath);
                }
                else
                {
                    AddNew(GameLoader.LoadOne(e.FullPath));
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public IndexToUI()
        {
            APIModel.IndexToUI = this;
        }
    }
}
