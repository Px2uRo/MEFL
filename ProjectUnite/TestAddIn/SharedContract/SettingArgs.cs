namespace MEFL.Arguments
{
    public class SettingArgs
    {
        public LangID LangID { get; set; }
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
