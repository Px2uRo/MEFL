namespace MEFL.Contract
{
    public interface ILuncherGameType
    {
        public GameInfoBase Parse(string JsonPath,string VersionType);
        public string[] SupportedType { get; set; }
    }
}
