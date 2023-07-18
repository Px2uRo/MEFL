using Avalonia.Controls.ApplicationLifetimes;
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

            try
            {
                ObservableCollection<string> f = null;
                if (File.Exists(_configPath))
                {
                    var t = File.ReadAllText(_configPath);
                    f = JsonConvert.DeserializeObject<ObservableCollection<string>>(t);
                }
                else
                {
                    File.CreateText(_configPath);
                }
                if (f != null)
                {
                    Favorites = f;
                }
                else
                {
                    Favorites = new();
                }   
            }
            catch (Exception ex)
            {
                Favorites = new();
            }
            Favorites.CollectionChanged += Favorites_CollectionChanged;
        }

        private void Favorites_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(App.Current.ApplicationLifetime is ClassicDesktopStyleApplicationLifetime app)
            {
                if (app.Windows.Count > 0)
                {
                    File.WriteAllText(_configPath, Favorites.ToJson());
                }
            }
        }
        [JsonIgnore]
        public ObservableCollection<string> Favorites { get; set; }

#if AVALONIA
        public override string ToString()
        {
            return this.FriendlyName + $"({Path})";
        }
#endif
    }

    public class MEFLFolderColletion : ObservableCollection<MEFLFolderInfo>
    {
#if AVALONIA
        protected override void InsertItem(int index, MEFLFolderInfo item)
        {
            base.InsertItem(index, item);
            WriteToReg();
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
            WriteToReg();
        }
#endif
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
                var regKey = Newtonsoft.Json.JsonConvert.DeserializeObject<MEFLFolderColletion>(regstr);
                foreach (var item in regKey)
                {
                    res.Add(new(item.Path,item.FriendlyName));
                }
                GC.SuppressFinalize(regKey);
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
