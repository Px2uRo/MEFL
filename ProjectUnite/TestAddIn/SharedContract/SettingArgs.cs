using System.IO;

namespace MEFL.Arguments
{
    public class SettingArgs
    {
        public LangID LangID { get; set; }
        public Contract.GameInfoBase CurretGame { get; set; }
        public Contract.AccountBase CurretAccount { get; set; }
        public string CurretJavaPath { get; set; }
        public string DefaultJavaArgs { get; set; }
        public string DefaultOtherGameArgs { get; set; }
        public FileInfo SelectedJava { get; set; }
    }

    /// <summary>
    /// 语言 ID ENUM
    /// </summary>
    public enum LangID
    {
        zh_CN,
        zh_yue_CN,
        zh_yue_HK,
        zh_HK,
        zh_MO,
        zh_TW,
        zh_SG,
        en_US,
        en_UK,
        ja
    }
}
