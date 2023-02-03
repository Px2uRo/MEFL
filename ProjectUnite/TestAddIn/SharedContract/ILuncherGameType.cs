namespace MEFL.Contract;

public interface ILuncherGameType
{
	string[] SupportedType { get; }

	GameInfoBase Parse(string type,string JsonPath);
}
