namespace MEFL.Contract;

public interface IGameTypeManage
{
	string[] SupportedType { get; }

	GameInfoBase Parse(string type,string JsonPath);
}
