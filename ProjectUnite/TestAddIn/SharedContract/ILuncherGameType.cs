namespace MEFL.Contract;

public interface ILuncherGameType
{
	string[] SupportedType { get; set; }

	GameInfoBase Parse(string type,string JsonPath);
}
