using MEFL.Arguments;
using MEFL.Callers;
using MEFL.Contract;
using Newtonsoft.Json.Linq;
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
                    var serverurl = JObject.Parse(Text)["downloads"]["server"]["url"].ToString();
                    var pro = DownloaderCaller.CallSingleProcess(serverurl,"I:\\Test.jar");
                    pro.Start();
                    pro.PropertyChanged += ((s, e) => 
                    {
                        var p = s as SingleProcess;
                        if (e.PropertyName == nameof(p.DownloadedSize)|| e.PropertyName == nameof(p.TotalSize))
                        {
                            this.CurrentProgress = ((double)p.DownloadedSize / (double)p.TotalSize);
                        }
                    });
                    pro.Finished += ((s,e) =>
                    {

                    });
                }
                catch (Exception ex)
                {
                    Fail();
                }
            }).Start();
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
        private bool _onlineMode;

        public bool OnlineMode => _onlineMode;

        private bool _wl;

        public bool WL => _wl;
        private string _port;
        public string Port => _port;
        private string _upaServerID;

        public string UpaServerID => _upaServerID;

        public InstServerBaseArgs(bool online,bool wl,string port, string upaServerID)
        {
            _onlineMode= online;
            _wl= wl;
            _port= port;
            _upaServerID= upaServerID;
        }
    }
}