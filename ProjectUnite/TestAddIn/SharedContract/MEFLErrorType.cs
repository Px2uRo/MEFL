using MEFL.Arguments;
using MEFL.Contract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MEFL.Contract
{
    public class MEFLErrorType : Contract.GameInfoBase
    {
        public override string GameJsonPath { get ; set; }
        public override string GameTypeFriendlyName { get; set; }
        public override string Description { get; set; }
        public override string Version { get; set; }
        public override string Name { get; set; }
        public override object IconSource { get; set; }
        public override string NativeLibrariesPath { get; set; }
        public override List<Root_Libraries> GameLibraries { get; set; }
        public override JavaVersion JavaVerion { get; set; }
        public override string GameArgs { get; set; }
        public override string JVMArgs { get; set; }
        public override string OtherGameArgs { get; set; }
        public override string OtherJVMArgs { get; set; }
        public override bool IsFavorate { get; set; }

        public override bool LaunchByLauncher => true;

        public override int JavaVersion { get; set; }
        public override string GameFolder { get; set; }

        public override FrameworkElement ManagePage => new TextBlock() { Text="谢谢但是你是怎么启动的?"};
        //todo 设置页面
        public override FrameworkElement SettingsPage => throw new NotImplementedException();

        public override Process Launch(SettingArgs args)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {

        }

        public MEFLErrorType(string ErrorDescription,string JsonPath)
        {
            Description=ErrorDescription;
            GameJsonPath = JsonPath;
        }
    }
}
