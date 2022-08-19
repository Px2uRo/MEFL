using MEFL.APIData;
using MEFL.Contract;
using MEFL.PageModelViews;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace MEFL
{
    public class MEFLFolderInfo
    {
        private string _configPath { get => System.IO.Path.Combine(_path, ".mefl.json"); }
        private string _path;
        public string Path { get => _path; set 
            {
                Refresh(value);
            } 
        }
        public void Refresh(string path)
        {
            _path = path;
            #region 加载游戏嘛
            Games.Clear();
            Games = new ObservableCollection<GameInfoBase>();
            _VersionPath = System.IO.Path.Combine(_path, "versions");
            if (Directory.Exists(_VersionPath) != true)
            {
                Directory.CreateDirectory(_VersionPath);
            }
            string[] directories = Directory.GetDirectories(_VersionPath);
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
                        Games.Add(new Contract.MEFLErrorType("不合法 Json", SubJson));
                    }
                    else
                    {
                        List<String> Support = new List<string>();
                            foreach (var Hst in APIModel.Hostings)
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
                            foreach (var Hst in APIModel.Hostings)
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
                                                    Games.Add(Hst.LuncherGameType.Parse(SubJson));
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Games.Add(new Contract.MEFLErrorType($"从{Hst.FileName}中加载失败：{ex.Message}", SubJson));
                                    }
                                }
                            }
                        }
                        else
                        {
                            Games.Add(new Contract.MEFLErrorType($"不支持 {jOb["type"].ToString()} 版本", SubJson));
                        }
                    }
                    jOb = null;
                }
                else
                {
                    Games.Add(new Contract.MEFLErrorType("不存在Json", SubJson));
                }
                SubJson = null;
            }
            directories = null;
            #endregion
            #region 设置收藏夹嘛
            if (File.Exists(_configPath) != true)
            {
                File.Create(_configPath).Close();
            }
            JObject jOb2 = new JObject();
            try
            {
                jOb2 = FastLoadJson.Load(_configPath);
                if(jOb2["Favorites"] == null)
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                jOb2.Add(new JProperty("Favorites", "[]"));
                File.WriteAllText(_configPath, JsonConvert.SerializeObject(jOb2));
            }
            Favorites = JsonConvert.DeserializeObject<ObservableCollection<String>>(jOb2["Favorites"].ToString());
            #endregion
        }
        JObject jOb2 = new JObject();
        int symbol;
        public void SetToFavorite(GameInfoBase Target)
        {
            symbol = Favorites.Count;
            jOb2 = new JObject();
            if (File.Exists(_configPath) != true)
            {
                File.Create(_configPath).Close();
            }
            try
            {
                jOb2 = FastLoadJson.Load(_configPath);
                if (jOb2["Favorites"] == null)
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                jOb2.Add(new JProperty("Favorites", "[]"));
            }
            for (int i = 0; i < Favorites.Count; i++)
            {
                if (Target.GameJsonPath.ToString() == Favorites[i].ToString())
                {
                    Favorites.Remove(Target.GameJsonPath);
                    if(Favorites.Count == 0)
                    {
                        symbol = -1;
                    }
                }
                else
                {
                    Favorites.Add(Target.GameJsonPath);
                }
            }
            if (symbol == 0)
            {
                Favorites.Add(Target.GameJsonPath);
            }
            File.WriteAllText(_configPath, JsonConvert.SerializeObject(new JObject() { new JProperty("Favorites", JsonConvert.SerializeObject(Favorites)) }));
            jOb2 = null;
        }
        private string _VersionPath;
        public string FriendlyName { get; set; }
        [JsonIgnore]
        public ObservableCollection<String> VersionJsons { get; set; }
        [JsonIgnore]
        public ObservableCollection<GameInfoBase> Games { get; set; }
        public MEFLFolderInfo(string Path,string FriendlyName)
        {
            VersionJsons=new ObservableCollection<String>();
            Games=new ObservableCollection<GameInfoBase>();
            this.Path = Path;
            this.FriendlyName = FriendlyName;
        }
        [JsonIgnore]
        public ObservableCollection<String> Favorites { get; set; }
    }
}
