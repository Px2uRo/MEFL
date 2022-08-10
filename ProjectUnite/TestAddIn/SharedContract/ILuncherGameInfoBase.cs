namespace MEFL.Contract
{
    public interface ILuncherGameType
    {
        public GameInfoBase Parse(string JsonPath);
        public string[] SupportedType { get; set; }
    }
}
