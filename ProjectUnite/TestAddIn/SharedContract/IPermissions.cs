namespace MEFL.Contract
{
#if CONTRACT
    public interface IPermissions
    {
        public bool UseSettingPageAPI { get; }
        public bool UsePagesAPI { get; }
        public bool UseGameManageAPI { get; }
        public bool UseDownloadPageAPI { get; }
    }
#endif
}