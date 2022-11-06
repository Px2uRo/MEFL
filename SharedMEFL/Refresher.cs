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

namespace MEFL
{
    public static class Refresher
    {
        static List<String> Support = new List<string>();
        static Thread t;
        public static bool Refreshing { get; set; }
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
                            if (jOb["type"] == null)
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
                                                        foreach (var type in Hst.LuncherGameType.SupportedType)
                                                        {
                                                            MyFolders[SelectedFolderIndex].Games.Add(Hst.LuncherGameType.Parse(jOb["type"].ToString(), SubJson));
                                                        }
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
                            MyFolders[SelectedFolderIndex].Games.Add(new Contract.MEFLErrorType("不存在Json", SubJson));
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
                }
                catch (Exception ex)
                {
                    Debugger.Logger($"刷新时发现未知错误 {ex.Message} at {ex.Source}");
                    Refreshing = false;
                }
                return;
            });
            t.Start();
        }
    }

}