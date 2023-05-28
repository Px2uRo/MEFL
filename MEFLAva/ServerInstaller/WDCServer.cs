using MEFL.Arguments;
using MEFL.Callers;
using MEFL.Contract;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServerInstaller.Properties;
using System.Net;

namespace ServerInstaller
{
    public class WDCServer : LauncherWebVersionContext
    {
        public override string Name => "Server";

        public override bool Install(LauncherWebVersionInfo info, FileInfo[] javas, string dotMCPath, out IInstallContextMenuPage page, out InstallProcess process)
        {
            page = new InstallServerPage(info,javas,dotMCPath);
            process = null;
            return false;
        }
    }

    public class InstServer : InstallProcess
    {
        private LauncherWebVersionInfo _info;
        private FileInfo[] _javas;
        private string _dotMCPath;
        private bool _installupa;
        private string _serverId;
        InstServerBaseArgs _args;
        #region Overrides
        public override void Cancel()
        {

        }

        public override void Continue()
        {

        }

        public override bool GetUsingLocalFiles(out string[] paths)
        {
            paths = new string[0];
            return false;
        }

        public override void Pause()
        {

        }

        public override void Retry()
        {

        }

        public override void Start()
        {
            new Thread(() => 
            {
            try
            {
                string Text = "";
                using (var clt = new WebClient())
                {
                    Text = clt.DownloadString(_info.Url);
                }
                MCFile serverdowns = JsonConvert.DeserializeObject<MCFile>(JObject.Parse(Text)["downloads"]["server"].ToString());
                var vP = Path.Combine(_dotMCPath, $"versions\\{_args.CustomName}");
                Directory.CreateDirectory(vP);
                using (var jsonP = File.CreateText(Path.Combine(vP, $"{_args.CustomName}.json")))
                {
                    jsonP.Write(CreateJsonText(serverdowns, Convert.ToInt32(JObject.Parse(Text)["javaVersion"]["majorVersion"].ToString())));
                }
                var pro = DownloaderCaller.CallSingleProcess(serverdowns.url, Path.Combine(vP, $"{_args.CustomName}.jar"));
                    pro.PropertyChanged += ((s, e) => 
                    {
                        var p = s as SingleProcess;
                        if (e.PropertyName == nameof(p.DownloadedSize)|| e.PropertyName == nameof(p.TotalSize))
                        {
                            this.CurrentProgress = ((double)p.DownloadedSize / (double)p.TotalSize);
                        }
                    });
                    Content = "正在下载 原版 端";
                    pro.Start();
                    pro.Finished += ((s,e) =>
                    {
                        using (var fs = File.CreateText(Path.Combine(vP,"eula.txt")))
                        {
                            fs.WriteLine("eula=TRUE");
                        }
                        using (var fs = File.CreateText(Path.Combine(vP, "server.properties")))
                        {
                            fs.WriteLine($"server-port={_args.Port}");
                            fs.WriteLine($"white-list={_args.WL}");
                            fs.WriteLine($"max-players={_args.MaxPlayers}");
                        }
                        if(!string.IsNullOrEmpty(_args.UpaServerID))
                        {
                            Content = "正在下载 统一通行证 运行库";
                            using (var clt = new WebClient())
                            {
                                clt.DownloadFile("https://static.mc-user.com:233/downloads/nide8auth.jar", Path.Combine(vP, "nide8auth.jar"));
                            }
                            using (var bs = File.OpenWrite(Path.Combine(vP, "server.properties")))
                            {
                                using (var fs = new StreamWriter(bs))
                                {
                                    fs.WriteLine($"online-mode={true}");
                                }
                            }
                        }
                        else
                        {
                            using (var bs = File.OpenWrite(Path.Combine(vP, "server.properties")))
                            {
                                using (var fs = new StreamWriter(bs))
                                {
                                    fs.WriteLine($"online-mode={_args.OnlineMode}");
                                }
                            }
                        }
                        Finish();
                    });
                }
                catch (Exception ex)
                {
                    Fail();
                }
            }).Start();
        }

        private string CreateJsonText(MCFile serverdowns,int majorJava)
        {
            var res = JsonConvert.DeserializeObject<Root>(Resources.VersionInfoTemplate);
            res.BaseVersion = _info.Id;
            res.Type= "server";
            res.ServerType = _args.Type;
            res.JavaMajor= majorJava;
            if (!string.IsNullOrEmpty(_args.UpaServerID))
            {
                res.UpaOption = new();
                res.UpaOption.Server_Id= _args.UpaServerID;
            }
            else
            {
                res.UpaOption = new();
            }
            res.Downloads = new();
            res.Downloads.Server = serverdowns;
            var rtxt= JsonConvert.SerializeObject(res, Formatting.Indented);
            return rtxt;
        }

        #endregion


        public InstServer(InstServerBaseArgs args)
        {
            _args= args;
        }
        public InstServer(InstServerBaseArgs args,LauncherWebVersionInfo info, FileInfo[] javas, string dotMCPath):this(args)
        {
            _info = info;
            _javas = javas;
            _dotMCPath = dotMCPath;
        }

        public InstServer(InstServerBaseArgs args,LauncherWebVersionInfo info, FileInfo[] javas, string dotMCPath,bool upa,string serverId):this(args,info,javas,dotMCPath)
        {
            _installupa = upa;
            _serverId = serverId;
        }
    }

    public class InstServerBaseArgs
    {
        private int _maxPlayers;

        public int MaxPlayers => _maxPlayers;

        private bool _onlineMode;

        public bool OnlineMode => _onlineMode;

        private bool _wl;

        public bool WL => _wl;
        private string _port;
        public string Port => _port;
        private string _upaServerID;

        public string UpaServerID => _upaServerID;
        private string _customName;

        public string CustomName => _customName;
        string _type = "server";
        public string Type => _type;

        public InstServerBaseArgs(int max_players,string customName,bool online,bool wl,string port, string upaServerID)
        {
            _maxPlayers = max_players;
            _onlineMode= online;
            _wl= wl;
            _port= port;
            _upaServerID= upaServerID;
            _customName= customName;
        }
    }
}