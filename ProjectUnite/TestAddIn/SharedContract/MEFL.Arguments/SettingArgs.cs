using System.IO;
using MEFL.Contract;

namespace MEFL.Arguments;

public class SettingArgs
{
	public LangID LangID { get; set; }

	public GameInfoBase CurretGame { get; set; }

	public AccountBase CurretAccount { get; set; }

	public string CurretJavaPath { get; set; }

	public string DefaultJavaArgs { get; set; }

	public string DefaultOtherGameArgs { get; set; }

	public FileInfo SelectedJava { get; set; }
	public string Location { get; set; }
}
