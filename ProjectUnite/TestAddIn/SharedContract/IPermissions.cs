namespace MEFL.Contract
{
#if CONTRACT
    public interface IPermissions
    {
        public bool UseSeetingPageAPI { get; }
        public bool UsePagesAPI { get; }
        public bool UseGameManageAPI { get; }
    }
#endif
}