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
using System.Windows.Shapes;

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
}
