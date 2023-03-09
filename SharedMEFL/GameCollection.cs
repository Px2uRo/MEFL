﻿using MEFL.APIData;
using MEFL.Contract;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MEFL
{
    #region GameCollection
    public class GameInfoCollection : ObservableCollection<GameInfoBase>
    {

        protected override void RemoveItem(int index)
        {
            if (this[index] == APIModel.CurretGame)
            {
                APIModel.CurretGame = null;
            }
            this[index].Dispose();
            base.RemoveItem(index);
        }

        protected override void ClearItems()
        {
            foreach (var item in this)
            {
                if (item == APIModel.CurretGame)
                {
                    if (!GameRefresher.Refreshing)
                    {
                        APIModel.CurretGame = null;
                    }
                }
                item.Dispose();
            }
            base.ClearItems();
        }

        public override string ToString()
        {
            var res = "";
            foreach (var item in this.Items)
            {
                if (item == null)
                {
                    res += $"空值\n";
                }
                else
                {
                    res += $"{item.Name}: {item.GameTypeFriendlyName}\n";
                }
            }
            return res;
        }
    }

    public static class GameLoader
    {
        private static void RefreshSupported()
        {
            foreach (var Hst in APIData.APIModel.Hostings)
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
                                        Supported.Add(type);
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
        }
        static List<string> Supported = new();
        public static Task LoadAll(MEFLFolderInfo folder)
        {
            var task = Task.Factory.StartNew(() => {
                #region 加载游戏嘛
                if(folder.Games == null||folder.Favorites==null)
                {
                    folder.Games = new();
                    folder.Favorites= new();
                }
                folder.Games.Clear();
                folder.Favorites.Clear();

                var VersionPath = System.IO.Path.Combine(folder.Path, "versions");
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
                    var subJson = System.IO.Path.Combine(item, $"{SubDirName}.json");
                    if (File.Exists(subJson))
                    {
                        var jOb = FastLoadJson.Load(subJson);
                        if (jOb == null)
                        {
                            folder.Games.Add(new Contract.MEFLErrorType($"无法解析该版本，Json无效或损坏", subJson));
                        }
                        else if (jOb["type"] == null)
                        {
                            folder.Games.Add(new Contract.MEFLErrorType("不合法 Json", subJson));
                        }
                        else
                        {
                            RefreshSupported();
                            folder.Games.Add(LoadOne(jOb,subJson));
                        }
                        jOb = null;
                    }
                    else
                    {
                        folder.Games.Add(new Contract.MEFLErrorType("不存在Json", subJson));
                    }
                    subJson = null;
                }
                directories = null;
                #endregion
                #region 设置收藏夹嘛
                var mefljsonpath = System.IO.Path.Combine(folder.Path, ".mefl.json");
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
                folder.Favorites = JsonConvert.DeserializeObject<ObservableCollection<String>>(jOb2["Favorites"].ToString());
                #endregion
            });
            return task;
        }

        private static GameInfoBase LoadOne(JObject jOb,string subJson)
        {
            GameInfoBase res = null;
            if (Supported.Contains(jOb["type"].ToString()))
            {
                foreach (var Hst in APIData.APIModel.Hostings)
                {
                    if (Hst.IsOpen)
                    {
                        try
                        {
                            if (Hst.Permissions != null)
                            {
                                if (Hst.Permissions.UseGameManageAPI)
                                {
                                    res = (Hst.LuncherGameType.Parse(jOb["type"].ToString(), subJson));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            res = (new Contract.MEFLErrorType($"从{Hst.FileName}中加载失败：{ex.Message}", subJson));
                        }
                    }
                }
            }
            else
            {
                res = (new Contract.MEFLErrorType($"不支持 {jOb["type"].ToString()} 版本", subJson));
            }
            return res;
        }
    }
    #endregion
    public class AccountCollection : ObservableCollection<AccountBase>
    {

        protected override void RemoveItem(int index)
        {
            if (this[index] == APIModel.SelectedAccount)
            {
                APIModel.SelectedAccount = null;
            }
            this[index].Dispose();
            base.RemoveItem(index);
        }

        protected override void ClearItems()
        {
            foreach (var item in this)
            {
                if (item == APIModel.SelectedAccount)
                {
                    if (!GameRefresher.Refreshing)
                    {
                        APIModel.SelectedAccount = null;
                    }
                }
                item.Dispose();
            }
            base.ClearItems();
        }

        public override string ToString()
        {
            var res = "";
            foreach (var item in this.Items)
            {
                if (item == null)
                {
                    res += $"空值\n";
                }
                else
                {
                    res += $"{item.UserName}:{item.Uuid}\n";
                }
            }
            return res;
        }
    }
}
