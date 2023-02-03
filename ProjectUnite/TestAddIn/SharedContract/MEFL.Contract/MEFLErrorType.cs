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
using MEFL.Controls;
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

	public override string AssetsIndexName
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override string VersionType => "Error";

	public override List<JsonFileInfo> FileNeedsToDownload
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

	public override List<JsonFileInfo> NativeFilesNeedToDepackage
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

	public override DeleteResult Delete()
	{
		var mb = MyMessageBox.Show("确定要删除吗？","警告",MessageBoxButton.YesNo);
		//todo IO 操作
		if (mb.Result == MessageBoxResult.Yes)
		{
			return DeleteResult.OK;
        }
		else
		{
			return DeleteResult.Canceled;
		}
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
