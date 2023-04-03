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
using System.Threading;
#if WPF
using System.Windows.Shapes;
#elif AVALONIA

#endif

namespace MEFL
{
    public class MEFLFolderInfo
    {
        private string _configPath { get => System.IO.Path.Combine(Path, ".mefl.json"); }
        public string Path { get; set; }
        JObject jOb2 = new JObject();
        int symbol;
        public void SetToFavorite(GameInfoBase target)
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
            if (Favorites.Contains(target.GameJsonPath))
            {
                Favorites.Remove(target.GameJsonPath);
            }
            else
            {
                Favorites.Add(target.GameJsonPath);
            }
            File.WriteAllText(_configPath, JsonConvert.SerializeObject(new JObject() { new JProperty("Favorites", JsonConvert.SerializeObject(Favorites)) }));
            jOb2 = null;
        }
        private string _VersionPath;
        public string FriendlyName { get; set; }
        [JsonIgnore]
        public ObservableCollection<String> VersionJsons { get; set; }
        [JsonIgnore]
        public GameInfoCollection Games { get; set; }
        public MEFLFolderInfo(string Path,string FriendlyName)
        {
            VersionJsons=new ObservableCollection<String>();
            Games=new GameInfoCollection();
            this.Path = Path;
            this.FriendlyName = FriendlyName;
        }
        [JsonIgnore]
        public ObservableCollection<String> Favorites { get; set; }
    }

    public class MEFLFolderColletion : ObservableCollection<MEFLFolderInfo>
    {
        public void WriteToReg()
        {
            var regObj = new MEFLFolderColletion();
            for (int i = 1; i < this.Count; i++)
            {
                regObj.Add(this[i]);
            }
            RegManager.Write("Folders",JsonConvert.SerializeObject(regObj));
            GC.SuppressFinalize(regObj);
            regObj = null;
        }
        public static MEFLFolderColletion GetReg()
        {
            Directory.CreateDirectory(System.IO.Path.Combine(Environment.CurrentDirectory, ".minecraft"));
            MEFLFolderColletion res = new() { new MEFLFolderInfo(System.IO.Path.Combine(Environment.CurrentDirectory, ".minecraft"), "本地文件夹")};
            var regstr = RegManager.Read("Folders");
            try
            {
#if !AVALONIA
                var regKey = Newtonsoft.Json.JsonConvert.DeserializeObject<MEFLFolderColletion>(regstr);
                foreach (var item in regKey)
                {
                    res.Add(new(item.Path,item.FriendlyName));
                }
                GC.SuppressFinalize(regKey);
#endif
            }
            catch (Exception ex)
            {
                RegManager.Write("Folders", "[]",true);
            }
            return res;
        }
        internal static int GetRegIndex()
        {
            var res = APIModel.SettingConfig.FolderIndex;
            if (res+1> APIModel.MyFolders.Count)
            {
                return 0;
            }
            else
            {
                return res;
            }
        }
    }
}
