using CoreLaunching.Forge;
using MEFL.Arguments;

internal class InstallArgsWithForge : InstallArguments
{
    public InstallArgsWithForge(string versionName, string customGameFolder, string gameIcon) : base(versionName, customGameFolder, gameIcon)
    {

    }

    public InstallArgsWithForge(InstallArguments baseArgs, WebForgeInfo info):this(baseArgs.VersionName,baseArgs.CustomGameFolder,baseArgs.GameIcon)
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