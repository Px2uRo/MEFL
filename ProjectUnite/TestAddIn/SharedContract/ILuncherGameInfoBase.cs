namespace MEFL.Contract
{
    public interface ILuncherGameType
    {
        public GameInfoBase Parse(string JsonPath,string VersionType);
        public string[] SupportedType { get; set; }
    }

    public abstract class GameInfoBase
    {
        public abstract string GameTypeFriendlyName { get; set; }

        public abstract string Description { get; set; }

        public abstract string Version { get; set; }

        public abstract string Name { get; set; }
        public abstract void Refresh(Arguments.SettingArgs args);
        public abstract object IconSource { get; set; }
    }
}
