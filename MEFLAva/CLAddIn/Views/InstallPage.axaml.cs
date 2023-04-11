using Avalonia.Controls;
using MEFL.Contract;

namespace CLAddIn.Views
{
    public partial class InstallPage : UserControl,IInstallPage
    {
        private string url;
        private string dotMCFolder;
        private string subFolderString;
        private MEFLDownloader downloader;
        private DownloadSource[] sources;
        private string[] usingLocalFiles;

        public InstallPage()
        {
            InitializeComponent();
        }

        public InstallPage(string url, string dotMCFolder, string subFolderString, MEFLDownloader downloader, DownloadSource[] sources, string[] usingLocalFiles)
        {
            this.url = url;
            this.dotMCFolder = dotMCFolder;
            this.subFolderString = subFolderString;
            this.downloader = downloader;
            this.sources = sources;
            this.usingLocalFiles = usingLocalFiles;
        }

        public event EventHandler<DownloadProgress> Solved;
    }
}
