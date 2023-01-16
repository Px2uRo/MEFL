namespace MEFL.Contract;

public interface IPermissions
{
	bool UseSettingPageAPI { get; }

	bool UsePagesAPI { get; }

	bool UseGameManageAPI { get; }

	bool UseDownloadPageAPI { get; }

	bool UseAccountAPI { get; }
}
