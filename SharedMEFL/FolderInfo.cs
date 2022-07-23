﻿using MEFL.Contract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace MEFL
{
    public class MEFLFolderInfo
    {
        private string _path;
        public string Path { get => _path; set 
            {
                _path=value;
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
                            //TODO 对多个插件进行处理
                            if (jOb["type"].ToString() == "release")
                            {
                                Games.Add(new GameTypes.MEFLRealseType(SubJson));
                            }
                            else
                            {
                                Games.Add(new Contract.MEFLErrorType(string.Format("不支持此版本：{0}", jOb["type"].ToString()),SubJson));
                            }
                        }
                        jOb = null;
                    }
                    else
                    {
                        Games.Add(new Contract.MEFLErrorType("不存在Json",SubJson));
                    }
                    SubJson = null;
                }
                directories = null;
            } 
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
    }
}
