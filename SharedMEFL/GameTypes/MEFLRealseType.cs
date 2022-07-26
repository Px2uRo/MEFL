using CoreLaunching.JsonTemplates;
using MEFL.Arguments;
using MEFL.Contract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MEFL.GameTypes
{
    public class MEFLRealseType : MEFL.Contract.GameInfoBase
    {
        private MEFLStandardOtherArgumentTemplate _MSOAT { get; set; }
        private CoreLaunching.JsonTemplates.Root _Root { get; set; }
        public override string GameTypeFriendlyName { get => App.Current.Resources["I18N_String_MEFLGameInfos_Realse"].ToString(); set => throw new NotImplementedException(); }
        public override string Description 
        { 
            get { 
                if (_MSOAT.Description != null)
                {
                    return _MSOAT.Description;
                }
                else 
                {
                    return _Root.Id;
                }
            } 
            set => _MSOAT.Description=value; 
        }
        public override string Version { get => _Root.Id; set => _Root.Id=value; }
        public override string Name { get => _Root.Id; set => _Root.Id = value; }
        public override object IconSource { 
            get 
            {
                if (_MSOAT.CustomIconPath != null)
                {
                    return new Image() { Source=new BitmapImage(new Uri(_MSOAT.CustomIconPath))};
                }
                else
                {
                    return App.Current.Resources["I18N_String_MEFLGameInfos_Realse_Icon"];
                }
            } 
            set 
            {
                if(value is String)
                {
                    _MSOAT.CustomIconPath = value as String;
                }
            } 
        }
        public override string NativeLibrariesPath { get 
            { 
                if (_MSOAT.NativeLibrariesPath != null) 
                {
                    return _MSOAT.NativeLibrariesPath;
                }
                else
                {
                    return Path.Combine(Path.GetDirectoryName(GameJsonPath),"\\natives");
                }
            } 
            set 
            {
                _MSOAT.NativeLibrariesPath=value;
            } 
        }
        public override List<Root_Libraries> GameLibraries { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override Contract.JavaVersion JavaVerion { get => new Contract.JavaVersion() { Component= _Root.JavaVersion.Component,MajorVersion= _Root.JavaVersion.MajorVersion }; set => throw new NotImplementedException(); }
        public override string GameArgs { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string JVMArgs { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string OtherGameArgs { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string OtherJVMArgs { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override bool IsFavorate { get => _MSOAT.IsFavorite; set => _MSOAT.IsFavorite=value; }
        public override string GameJsonPath { get;set; }
        public override bool LaunchByLauncher => true;

        public override int JavaVersion {
            get
            {
                try
                {
                    return _Root.JavaVersion.MajorVersion;
                }
                catch (Exception ex)
                {
                    return 8;
                }
            }
            set => throw new NotImplementedException(); }

        public override string GameFolder { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override Process Launch(SettingArgs args)
        {
            throw new NotImplementedException();
        }
        public MEFLRealseType(string JsonPath)
        {
            string otherArgsPath = Path.Combine(Path.GetDirectoryName(JsonPath), "MEFLOtherArguments.json");
            _MSOAT =new MEFLStandardOtherArgumentTemplate(otherArgsPath);
            otherArgsPath = string.Empty;
            GameJsonPath=JsonPath;
            try
            {
                _Root = JsonConvert.DeserializeObject<Root>(System.IO.File.ReadAllText(JsonPath));
            }
            catch (Exception ex)
            {

            }
        }
    }
    public class MEFLStandardOtherArgumentTemplate : OtherArgumentTemplateBase
    {
        #region Privates
        [JsonIgnore]
        private string _JsonPath { get; set; }
        [JsonIgnore]
        private bool _IsFavorite { get; set; }
        [JsonIgnore]
        private string _Description { get; set; }
        [JsonIgnore]
        private string _OtherJVMArguments { get; set; }
        [JsonIgnore]
        private string _OtherGameArguments { get; set; }
        [JsonIgnore]
        private string _NativeLibrariesPath { get; set; }
        [JsonIgnore]
        private string _CustomIconPath { get; set; }
        #endregion
        #region Props
        public override bool IsFavorite { get => _IsFavorite; set { _IsFavorite = value; ChangeProperty(); } }
        public override string Description { get => _Description; set { _Description = value; ChangeProperty(); } }
        public override string OtherJVMArguments { get => _OtherJVMArguments; set { _OtherJVMArguments = value; ChangeProperty(); } }
        public override string OtherGameArguments { get => _OtherGameArguments; set { _OtherGameArguments = value; ChangeProperty(); } }
        public override string NativeLibrariesPath { get => _NativeLibrariesPath; set { _NativeLibrariesPath = value; ChangeProperty(); } }
        public override string CustomIconPath { get => _CustomIconPath; set { _CustomIconPath = value; ChangeProperty(); } }
        public override void ChangeProperty()
        {
            try
            {
                if (System.IO.File.Exists(_JsonPath) != true)
                {
                    System.IO.File.Create(_JsonPath).Close();
                }
                System.IO.File.WriteAllText(_JsonPath, JsonConvert.SerializeObject(this));
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        public MEFLStandardOtherArgumentTemplate(string JsonPath)
        {
            _JsonPath = JsonPath;
            try
            {
                if (System.IO.File.Exists(_JsonPath) != true)
                {
                    System.IO.File.Create(_JsonPath).Close();
                }
                var root = JsonConvert.DeserializeObject<MEFLStandardOtherArgumentTemplate>(System.IO.File.ReadAllText(JsonPath));
                if (root != null)
                {
                    Description = root.Description;
                    IsFavorite = root.IsFavorite;
                    OtherJVMArguments = root.OtherJVMArguments;
                    OtherGameArguments = root.OtherGameArguments;
                    NativeLibrariesPath = root.NativeLibrariesPath;
                    CustomIconPath = root.CustomIconPath;
                }
                root = null;
            }
            catch (Exception)
            {

            }
        }

    }

}
