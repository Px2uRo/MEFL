using MEFL.Arguments;
using MEFL.Contract;
using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL.CLAddIn.DownloadSources
{
    public class BAVMSource : DownloadSource
    {
        public override string ELItem => "${version_manifest}";

        public override string RuleSourceName => "BMCLAPI";

        public override string GetUri(SettingArgs args)
        {
            return "https://bmclapi2.bangbang93.com/mc/game/version_manifest.json";
        }
    }

    public class BAJSONAssIndSource : DownloadSource
    {
        public override string ELItem => "${json_&_AssIndex}";

        public override string RuleSourceName => "BMCLAPI";

        public override string GetUri(SettingArgs args)
        {
            return "https://bmclapi2.bangbang93.com";
        }
    }
    public class BAAsetSource : DownloadSource
    {
        public override string ELItem => "${assets}";

        public override string RuleSourceName => "BMCLAPI";

        public override string GetUri(SettingArgs args)
        {
            return "https://bmclapi2.bangbang93.com/assets";
        }
    }

    public class BALibSource : DownloadSource
    {
        public override string ELItem => "${libraries}";

        public override string RuleSourceName => "BMCLAPI";

        public override string GetUri(SettingArgs args)
        {
            return "https://bmclapi2.bangbang93.com/maven";
        }
    }
}
