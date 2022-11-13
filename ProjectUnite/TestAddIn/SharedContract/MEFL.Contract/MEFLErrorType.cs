using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MEFL.Arguments;
#if NET6_0
using MEFL.Contract.Properties;
#elif NET4_0
using ContractNetFw4.Properties;
#endif

namespace MEFL.Contract;

public class MEFLErrorType : GameInfoBase
{
	static MemoryStream stream;
	public override string GameJarPath
	{
		get
		{
			return string.Empty;
		}
	}

	public override string GameJsonPath { get; set; }

	public override string GameTypeFriendlyName { get; set; }

	public override string Description { get; set; }

	public override string Version { get; set; }

	public override ImageSource IconSource { get 
		{
			BitmapImage Image = new();
			Image.BeginInit();
			Image.StreamSource = stream;
            Image.EndInit();
			return Image;
		} 
	}

	public override string NativeLibrariesPath { get; set; }

	public override string GameArgs { get; }

	public override string JVMArgs { get; }

	public override string OtherGameArgs { get; set; }

	public override string OtherJVMArgs { get; set; }

	public override string GameFolder { get; set; }

	static TextBlock Text = new();
	public override FrameworkElement SettingsPage
	{
		get
		{
			Text.Text = Description;
			return Text;
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

	public override string VersionType => "Error";

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

	protected override void Dispose(bool disposing)
	{
		
	}

	public override void Delete()
	{
	}

	public override void Refresh()
	{
		throw new NotImplementedException();
	}

	static MEFLErrorType()
	{
        stream = new MemoryStream(Resources.Error);
    }
	public MEFLErrorType(string ErrorDescription, string JsonPath)
	{
		Description = ErrorDescription;
		GameJsonPath = JsonPath;
	}
}
