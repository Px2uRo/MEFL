﻿#pragma checksum "..\..\..\..\..\SharedMVVMWPF\SpecialPages\PickUpAFolder.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8D1FB4B61EF234E7BA0141D563B9751643E6E3F5"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using MEFL.Controls;
using MEFL.PageModelViews;
using MEFL.SpecialPages;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace MEFL.SpecialPages {
    
    
    /// <summary>
    /// PickUpAFolder
    /// </summary>
    public partial class PickUpAFolder : MEFL.Controls.MyPageBase, System.Windows.Markup.IComponentConnector {
        
        
        #line 48 "..\..\..\..\..\SharedMVVMWPF\SpecialPages\PickUpAFolder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.UniformGrid PART_UniformGrid;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\..\..\SharedMVVMWPF\SpecialPages\PickUpAFolder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border ErrorBorder;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\..\..\..\SharedMVVMWPF\SpecialPages\PickUpAFolder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock ErrorBOX;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.7.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/MEFL;component/specialpages/pickupafolder.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\SharedMVVMWPF\SpecialPages\PickUpAFolder.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.7.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.7.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 31 "..\..\..\..\..\SharedMVVMWPF\SpecialPages\PickUpAFolder.xaml"
            ((MEFL.Controls.MyButton)(target)).Click += new System.Windows.RoutedEventHandler(this.BackToRoot);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 32 "..\..\..\..\..\SharedMVVMWPF\SpecialPages\PickUpAFolder.xaml"
            ((MEFL.Controls.MyButton)(target)).Click += new System.Windows.RoutedEventHandler(this.BackToParent);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 33 "..\..\..\..\..\SharedMVVMWPF\SpecialPages\PickUpAFolder.xaml"
            ((System.Windows.Controls.TextBox)(target)).AddHandler(System.Windows.Controls.Validation.ErrorEvent, new System.EventHandler<System.Windows.Controls.ValidationErrorEventArgs>(this.ErrorAppeard));
            
            #line default
            #line hidden
            
            #line 34 "..\..\..\..\..\SharedMVVMWPF\SpecialPages\PickUpAFolder.xaml"
            ((System.Windows.Controls.TextBox)(target)).MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.DoubleClick);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 44 "..\..\..\..\..\SharedMVVMWPF\SpecialPages\PickUpAFolder.xaml"
            ((MEFL.Controls.MyButton)(target)).Click += new System.Windows.RoutedEventHandler(this.DeleteSelected);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 45 "..\..\..\..\..\SharedMVVMWPF\SpecialPages\PickUpAFolder.xaml"
            ((MEFL.Controls.MyButton)(target)).Click += new System.Windows.RoutedEventHandler(this.CreateNew);
            
            #line default
            #line hidden
            return;
            case 6:
            this.PART_UniformGrid = ((System.Windows.Controls.Primitives.UniformGrid)(target));
            return;
            case 7:
            this.ErrorBorder = ((System.Windows.Controls.Border)(target));
            return;
            case 8:
            this.ErrorBOX = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

