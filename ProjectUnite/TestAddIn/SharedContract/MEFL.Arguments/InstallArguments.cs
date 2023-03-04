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
		public InstallArguments(string versionName,string customGameFolder,string gameIcon)
		{
			_versionName= versionName;
			_customGameFolder= customGameFolder;
			_gameIcon= gameIcon;
		}
	}
}
