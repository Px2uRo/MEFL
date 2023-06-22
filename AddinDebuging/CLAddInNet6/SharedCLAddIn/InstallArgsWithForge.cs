using CoreLaunching.DownloadAPIs.Forge;
using CoreLaunching.Forge;
using MEFL.Arguments;

internal class InstallArgsWithForge : InstallArguments
{

    public InstallArgsWithForge(FileInfo[] jAVAPaths,InstallArguments baseArgs, WebForgeInfo info): base(jAVAPaths,baseArgs.VersionName,baseArgs.CustomGameFolder,baseArgs.GameIcon)
    {
        Forge = info;
    }

    private WebForgeInfo _forge;

    public WebForgeInfo Forge
    {
        get { return _forge; }
        set { _forge = value; }
    }

}