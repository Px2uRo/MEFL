using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using MEFL.Arguments;

namespace MEFL.Contract;

public class MEFLErrorType : GameInfoBase
{
	public override string GameJarPath
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override string GameJsonPath { get; set; }

	public override string GameTypeFriendlyName { get; set; }

	public override string Description { get; set; }

	public override string Version { get; set; }

	public override ImageSource IconSource { get; }

	public override string NativeLibrariesPath { get; set; }

	public override string GameArgs { get; }

	public override string JVMArgs { get; }

	public override string OtherGameArgs { get; set; }

	public override string OtherJVMArgs { get; set; }

	public override string GameFolder { get; set; }

	public override FrameworkElement SettingsPage
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override string HeapDumpPath
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override int GameMaxMem
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public override int GameMinMem
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public override List<string> ClassPaths
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override string MainClassName
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override int JavaMajorVersion
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override string AssetsRoot
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override string AssetsIndexName
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override string VersionType
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override List<LauncherWebFileInfo> FileNeedsToDownload
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public override List<LauncherWebFileInfo> NativeFilesNeedToDepackage
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public override List<string> ItemsNeedsToExtract
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override FrameworkElement GetManageProcessPage(Process process, SettingArgs args)
	{
		throw new NotImplementedException();
	}

	public override void Dispose()
	{
	}

	public override void Delete()
	{
	}

	public override void Refresh()
	{
		throw new NotImplementedException();
	}

	public MEFLErrorType(string ErrorDescription, string JsonPath)
	{
		Description = ErrorDescription;
		GameJsonPath = JsonPath;
	}
}
