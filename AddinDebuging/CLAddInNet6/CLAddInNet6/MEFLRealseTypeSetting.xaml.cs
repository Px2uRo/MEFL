using CoreLaunching.Parser;
using MEFL.CLAddIn.GameInfos;
using MEFL.CLAddIn.GameTypes;
using MEFL.Contract;
using MEFL.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
        private static ObservableCollection<CLMapInfo> _mapInfos;
        private static FileSystemWatcher _mapWacther;

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
            Modi_Save(Modi_Save_Card, new EventArgs());
            Modi_Mod(Modi_Mod_Card, new());
        }
        private void Value_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName== "GamePathType")
            {
                Modi_Save(Modi_Save_Card, new EventArgs());
                Modi_Mod(Modi_Mod_Card, new());
            }
        }

        private void Modi_Mod(object? sender, EventArgs e)
        {
            var con = (DataContext as CLGameType);
            if (con == null)
            {
                return;
            }
            else
            {

            }
        }

        private void Modi_Save(object? sender, EventArgs e)
        {
            var sen = (sender as MyItemsCard);
            var con = (DataContext as CLGameType);
            if (con == null)
            {
                return;
            }
            else
            {
                if (con.GameFolder == null)
                {
                    if (_mapInfos != null)
                    {
                        _mapInfos.Clear();
                    }
                    return;
                }
                var path = System.IO.Path.Combine(con.GameFolder, "saves");
                if (_mapInfos == null)
                {
                    _mapInfos = new ObservableCollection<CLMapInfo>();
                }
                else
                {
                    _mapInfos.Clear();
                }
                foreach (var item in GameInfos.GetAllMaps.FromFolder(path))
                {
                    _mapInfos.Add(item);
                }
                if (_mapWacther != null)
                {
                    _mapWacther.Dispose();
                    _mapWacther = null;
                }
                _mapWacther = new FileSystemWatcher(path);
                _mapWacther.IncludeSubdirectories = true;
                _mapWacther.NotifyFilter = NotifyFilters.DirectoryName;
                _mapWacther.Changed += _mapWacther_Changed;
                _mapWacther.Created += _mapWacther_Created;
                _mapWacther.Deleted += _mapWacther_Deleted;
                _mapWacther.Renamed += _mapWacther_Renamed;
                _mapWacther.EnableRaisingEvents= true;
                sen.ItemsSource = _mapInfos;
                sen.Title = $"地图个数：{_mapInfos.Count}";
            }
        }

        private void _mapWacther_Renamed(object sender, RenamedEventArgs e)
        {
            CLMapInfo target = _mapInfos.Where(x=> x.Name == e.OldName).ToArray()[0];
            target.Name = e.Name;
        }

        private void _mapWacther_Deleted(object sender, FileSystemEventArgs e)
        {
            
                Dispatcher.Invoke(() =>
                {
                    if (Path.GetDirectoryName(e.FullPath) == System.IO.Path.Combine((DataContext as CLGameType).GameFolder, "saves"))
                    {
                        CLMapInfo target = _mapInfos.Where(x => x.Name == e.Name).ToArray()[0];
                        _mapInfos.Remove(target);
                        Modi_Save_Card.Title = $"地图个数：{_mapInfos.Count}";

                    }
                });
            
        }

        private void _mapWacther_Created(object sender, FileSystemEventArgs e)
        {
                Dispatcher.Invoke(() =>
                {
                    if (Path.GetDirectoryName(e.FullPath) == System.IO.Path.Combine((DataContext as CLGameType).GameFolder, "saves"))
                    {
                        CLMapInfo target = new CLMapInfo(e.FullPath);
                        _mapInfos.Add(target);
                        Modi_Save_Card.Title = $"地图个数：{_mapInfos.Count}";

                    }
                });
        }

        private void _mapWacther_Changed(object sender, FileSystemEventArgs e)
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

        private void OpenSavesFolder(object sender, RoutedEventArgs e)
        {
            var data = this.DataContext as CLGameType;
            Process.Start("explorer.exe",Path.Combine(data.GameFolder,"saves"));
        }

        private void RepairFiles(object sender, RoutedEventArgs e)
        {

        }
    }
}
