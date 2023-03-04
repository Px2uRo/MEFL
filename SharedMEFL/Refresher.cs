using static MEFL.APIData.APIModel;
using MEFL;
using System.Collections.ObjectModel;
using System.Threading;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Collections.Generic;
using System.Windows.Shapes;
using MEFL.PageModelViews;
using MEFL.Controls;
using System.Windows.Controls;
using System.Windows;

namespace MEFL
{
    public static class GameRefresher
    {
        static List<String> Support = new List<string>();
        static Thread t;
        private static bool _Refreshing;
        public static bool Refreshing { get => _Refreshing; set { 
                _Refreshing = value;
            } }
        public static void RefreshCurrect()
        {
            if (APIData.APIModel.MyFolders[SelectedFolderIndex] == null)
            {
                return;
            }
            if (Refreshing)
            {
                return;
            }
            Refreshing = true;
            MyFolders[SelectedFolderIndex].Games.Clear();
            Support.Clear();
            MyFolders[SelectedFolderIndex].Favorites = new ObservableCollection<string>();
            MyFolders[SelectedFolderIndex].Games = new GameInfoCollection();
            t = new Thread(() =>
            {
                try
                {
                    Refreshing = true;
                    #region 加载游戏嘛
                    var VersionPath = System.IO.Path.Combine(MyFolders[SelectedFolderIndex].Path, "versions");
                    if (Directory.Exists(VersionPath) != true)
                    {
                        Directory.CreateDirectory(VersionPath);
                    }
                    string[] directories = Directory.GetDirectories(VersionPath);
                    foreach (var item in directories)
                    {
                        var PrtDir = System.IO.Path.GetDirectoryName(item);
                        var SubDirName = item.Replace(PrtDir + "\\", string.Empty);
                        PrtDir = null;
                        var SubJson = System.IO.Path.Combine(item, $"{SubDirName}.json");
                        if (File.Exists(SubJson))
                        {
                            var jOb = FastLoadJson.Load(SubJson);
                            if (jOb == null)
                            {
                                MyFolders[SelectedFolderIndex].Games.Add(new Contract.MEFLErrorType($"无法解析该版本，Json无效或损坏", SubJson));
                            }
                            else if (jOb["type"] == null)
                            {
                                MyFolders[SelectedFolderIndex].Games.Add(new Contract.MEFLErrorType("不合法 Json", SubJson));
                            }
                            else
                            {
                                foreach (var Hst in Hostings)
                                {
                                    if (Hst.IsOpen)
                                    {
                                        try
                                        {
                                            if (Hst.Permissions != null)
                                            {
                                                if (Hst.Permissions.UseGameManageAPI)
                                                {
                                                    try
                                                    {
                                                        foreach (var type in Hst.LuncherGameType.SupportedType)
                                                        {
                                                            Support.Add(type);
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Debugger.Logger($"未知错误 {ex.Message} at {Hst.FileName} at {ex.Message}");
                                        }
                                    }
                                }
                                if (Support.Contains(jOb["type"].ToString()))
                                {
                                    foreach (var Hst in Hostings)
                                    {
                                        if (Hst.IsOpen)
                                        {
                                            try
                                            {
                                                if (Hst.Permissions != null)
                                                {
                                                    if (Hst.Permissions.UseGameManageAPI)
                                                    {
                                                        MyFolders[SelectedFolderIndex].Games.Add(Hst.LuncherGameType.Parse(jOb["type"].ToString(), SubJson));
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                MyFolders[SelectedFolderIndex].Games.Add(new Contract.MEFLErrorType($"从{Hst.FileName}中加载失败：{ex.Message}", SubJson));
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    MyFolders[SelectedFolderIndex].Games.Add(new Contract.MEFLErrorType($"不支持 {jOb["type"].ToString()} 版本", SubJson));
                                }
                            }
                            jOb = null;
                        }
                        else
                        {
                            try
                            {
                                MyFolders[SelectedFolderIndex].Games.Add(new Contract.MEFLErrorType("不存在Json", SubJson));
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        SubJson = null;
                    }
                    directories = null;
                    #endregion
                    #region 设置收藏夹嘛
                    var mefljsonpath = System.IO.Path.Combine(MyFolders[SelectedFolderIndex].Path, ".mefl.json");
                    if (File.Exists(mefljsonpath) != true)
                    {
                        File.Create(mefljsonpath).Close();
                    }
                    JObject jOb2 = new JObject();
                    try
                    {
                        jOb2 = FastLoadJson.Load(mefljsonpath);
                        if (jOb2 == null)
                        {
                            jOb2 = new JObject();
                        }
                        if (jOb2["Favorites"] == null)
                        {
                            throw new Exception();
                        }
                    }
                    catch (Exception ex)
                    {
                        jOb2.Add(new JProperty("Favorites", "[]"));
                        File.WriteAllText(mefljsonpath, JsonConvert.SerializeObject(jOb2));
                    }
                    MyFolders[SelectedFolderIndex].Favorites = JsonConvert.DeserializeObject<ObservableCollection<String>>(jOb2["Favorites"].ToString());
                    Refreshing = false;
                    #endregion
                    Refreshing = false;
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (var item in HostingsToUI.res.Children)
                        {
                            ((item as MyExtensionCard).DataContext as HostingModelView).Invoke("IsRefreshing");
                        }
                    });
                }
                catch (Exception ex)
                {
                    Debugger.Logger($"刷新时发现未知错误 {ex.Message} at {ex.Source}");
                    Refreshing = false;
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (var item in HostingsToUI.res.Children)
                        {
                            ((item as MyExtensionCard).DataContext as HostingModelView).Invoke("IsRefreshing");
                        }
                    });
                }
                return;
            });
            t.Start();
        }
    }

    public static class WebListRefresher
    {
        private static bool _IsRefreshingList;

        public static bool IsRefreshing
        {
            get { return _IsRefreshingList; }
            set { _IsRefreshingList = value; }
        }

        public static MyPageBase SovlePage = new() { Tag="SovlePage",Visibility=System.Windows.Visibility.Hidden};
        public static void ShowSovlePage()
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                MyPageBase From = null;
                foreach (MyPageBase item in (App.Current.Resources["MainPage"] as Grid).Children)
                {
                    if (item.Visibility == Visibility.Visible)
                    {
                        From = item;
                        if (item.Tag as String != From.Tag as String)
                        {
                            item.Hide();
                        }
                    }
                }
                foreach (MyPageBase item in FindControl.FromTag("SovlePage", (App.Current.Resources["MainPage"] as Grid)))
                {
                    item.Show(From);
                }
                From = null;
            });
        }

        public static void CleanSovlePage()
        {
            if(SovlePage.DataContext != null)
            {
                GC.SuppressFinalize(SovlePage.Content);
                SovlePage.Content = null;
                (SovlePage.DataContext as Contract.InstallProgressInput).Dispose();
                SovlePage.DataContext = null;
            }
        }


        internal static void GoToDownloadProgressPage()
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                MyPageBase From = null;
                foreach (MyPageBase item in (App.Current.Resources["MainPage"] as Grid).Children)
                {
                    if (item.Visibility == Visibility.Visible)
                    {
                        From = item;
                        if (item.Tag as String != From.Tag as String)
                        {
                            item.Hide();
                        }
                    }
                }
                foreach (MyPageBase item in FindControl.FromTag("DownloadingProgressPage", (App.Current.Resources["MainPage"] as Grid)))
                {
                    item.Show(From);
                }
                From = null;
            });
        }
    }
}