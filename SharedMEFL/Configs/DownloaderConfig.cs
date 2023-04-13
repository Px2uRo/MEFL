namespace MEFL.Configs
{
    internal class DownloaderConfig
    {
        public string FileName { get; set; }
        public string DownloaderName { get; set; }
        public DownloaderConfig(string fileName, string downloaderName)
        {
            FileName = fileName;
            DownloaderName = downloaderName;
        }
        public DownloaderConfig()
        {

        }
    }
}