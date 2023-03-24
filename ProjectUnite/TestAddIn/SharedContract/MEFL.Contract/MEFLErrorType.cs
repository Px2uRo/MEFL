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

	public override string GameTypeFriendlyName { get => "错误"; set { } }

	public override string Description { get; set; }

	public override string Version { get; set; }
 	static BitmapImage _image = null;
	public override ImageSource IconSource { get 
		{
			if(stream==null)
			{
                stream = new MemoryStream(Resources.Error);
				_image = new();
                _image.BeginInit();
                _image.StreamSource = stream;
                _image.EndInit();
            }
			return _image;
		} 
	}

	public override string NativeLibrariesPath { get; set; }

	public override string GameArgs { get; }

	public override string JVMArgs { get; }

	public override string OtherGameArgs { get; set; }

	public override string OtherJVMArgs { get; set; }

	public override string GameFolder { get; set; }

	static TextBlock Text = new();
	public override IGameSettingPage SettingsPage
	{
		get
		{
			return null;
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

	public MEFLErrorType(string ErrorDescription, string JsonPath)
    {
        Description = ErrorDescription;
		GameJsonPath = JsonPath;
	}

    public MEFLErrorType(string ErrorDescription, object title)
    {
        Description = ErrorDescription;
		GameJsonPath = "A:\\" + title.ToString();
    }
}
