using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL.Arguments
{
	/// <summary>
	/// 安装参数
	/// </summary>
    public class InstallArguments:EventArgs
    {
        bool _isEmpty = false;
        public bool IsEmpty => _isEmpty;
		public static new InstallArguments Empty => new InstallArguments(true);
		private string _versionName;
		/// <summary>
		/// 版本名称
		/// </summary>
		public string VersionName => _versionName;

		private string _customGameFolder;
		/// <summary>
		/// 游戏文件夹
		/// </summary>
		public string CustomGameFolder => _customGameFolder;

		private string _gameIcon;
		/// <summary>
		/// 游戏图标
		/// </summary>
		public string GameIcon => _gameIcon;
		/// <summary>
		/// 安装参数
		/// </summary>
		/// <param name="versionName">版本名称</param>
		/// <param name="customGameFolder">自定义游戏文件夹</param>
		/// <param name="gameIcon">游戏图标</param>
		public InstallArguments(string versionName,string customGameFolder,string gameIcon):this(false)
		{
			_versionName= versionName;
			_customGameFolder= customGameFolder;
			_gameIcon= gameIcon;
		}

		public InstallArguments(bool isEmpty) 
		{ 
			_isEmpty= isEmpty;
		}

	}
}
