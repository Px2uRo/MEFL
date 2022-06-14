namespace MEFL.Contract
{
#if CONTRACT
    public interface IPermissions
    {
#if NET4_0
        bool UseSeetingPageAPI { get; }

#else
        public bool UseSeetingPageAPI { get; }
        public bool UsePagesAPI { get; }

#endif
    }
#endif
}
