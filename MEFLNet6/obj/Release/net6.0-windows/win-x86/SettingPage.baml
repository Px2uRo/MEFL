   M S B A M L   `   `   `  ���� H  DMEFL.Controls, Version=0.0.7.6, Culture=neutral, PublicKeyToken=nullD2clr-namespace:MEFL.Controls;assembly=MEFL.ControlsMEFL.Controls  ? ;MEFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:MEFL.Pages
MEFL.Pages    MEFL.Pages.SettingPage   8c2clr-namespace:MEFL.Controls;assembly=MEFL.Controls  W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     *localclr-namespace:MEFL.Pages        SYTLE_Standard_ColorBrush   MEFL.Controls.MyPageBase    	LineBrush#  �    � 	Resources 9!clr-namespace:MEFL.PageModelViewsMEFL.PageModelViews -  'MEFL.PageModelViews.IsJavaNullConverter 
 isNull9!clr-namespace:MEFL.PageModelViewsMEFL.PageModelViews .  (MEFL.PageModelViews.SettingPageModelView.  (MEFL.PageModelViews.SettingPageModelView   PropertyChanged  SPMV��%�   &	       &	 4       . !clr-namespace:MEFL.PageModelViews       & !clr-namespace:MEFL.PageModelViews  $ propChanged��   SideBar �� .+�+   %MEFL.Controls.ChangePageContentButton  -     Name$ DefalutChangeButton��  I18N_String_Setting_Game#���  
 � Tag$
 Game��  -      I18N_String_Setting_Launcher#���  $ Launcher��  -     I18N_String_Setting_Custom#���  $ Custom��  -   $��	下载器��$ 
Downloader��  -    ! I18N_String_Setting_Info_MEFL#���  $
 Info��  �� 
TargetType �� Property �� Value��%8   '        �� "  .���  �@ SideBarBrush# �   .��� .+��� $ Launcher��.+�   MEFL.Controls.MyCard    I18N_String_Language_Opts	   Title#	 �  .��� 
 � ColumnDefinitions
 �� $
��auto ��� $��* �.+���  / +I18N_String_Setting_Launcher_SelcetLanguage#���  $��0,0,20,0q�   MEFL.Controls.MyComboBox  $��1��$��20�� � DataContext# �   �� SelectedIndex �� *	LangIndex+.@���  #	 I18N_String_Language_Opts_zh_CN#��� 	 ��  '
 #I18N_String_Language_Opts_zh_yue_CN#��� 
 ��  ' #I18N_String_Language_Opts_zh_yue_HK#���  ��  # I18N_String_Language_Opts_zh_MO#���  ��  # I18N_String_Language_Opts_zh_HK#���  ��  # I18N_String_Language_Opts_zh_TW#���  ��  # I18N_String_Language_Opts_zh_SG#���  ��  # I18N_String_Language_Opts_en_US#���  ��  # I18N_String_Language_Opts_en_UK#���    $	 系统��   IsAbleToSwap$
 True��.��� 
 �� $
��auto ��� $��* �.+��� $��临时文件夹路径��$��0,0,20,0q��� $��1��# �  ���� *TempFolderPath+�� $ Custom��.+�   . *I18N_String_Setting_Custom_BackgroundImage#	 �  .��   MEFL.Controls.MyButton  $
��Left=�# �   3 /I18N_String_Setting_Custom_BackgroundImage_Open#���  ���� *ChangeBackgroundCommand+  $	 主题颜色��.��A� $	��500�� A� Columns$ 5��.+�	� -   $��淡蓝��$ 0��	� -   $��浅绿��$ 1��	� -   $��亮黄��$ 2��	� -	   $��橙色��$ 3��	� -
   $��粉红��$ 4���� $
 Game��# �  .+�  $	 通用设置��.��� 
 �� $
��auto ���  � RowDefinitions �� $��30 ��� $��30 �.+��� $��默认 Java��$��Center=�� $��1��.+�� 
 �� $��* ��� $��30 ��� $��30 �.+�  -   $��JavaCB������ *Javas+ �� ConverterParameter$ ItemsSource�� �� 	Converter# [ {��� *Javas+$ 	IsEnabled��# [  �� *SelectedJavaIndex+  -   $��R��$��1��$��27��$��27��{��� *EnableSearchJava+  -   $��+��$��2��$
��Left=�$��27��$��27��{��� *EnableSearchJava+�� $��其他 JVM 参数��$��0��$��1��$��0,10,0,0q�� $��1��$��1��$��0,10,0,0q�
 �� �� $��30 �.+��� ���� *OtherJVMArgs+  -   $��R��$��1��$��27��$��27��  $	 内存分配��.���� .+��� $0��*这个地方做的比较潦草，请见谅����  �� Orientation$ 
Horizontal=�$��3,0,0,0q�.+��� $��最大：(MegaBytes)���� $��36������ *	MaxMemory+�� $
 Info��.+�  $	 关于 MEFL��.��� 
 �� $��80 ��� .+��� $��80��$��80��$��0��$*��$pack://application:,,,/MEFL_Logo.png��� $��1�� �� $
��auto ��� $
��auto ��� .+��� $
��MEFL��$��40(�$
��Bold"��� $��Version 0.0.0��$��1��  -      
Background$ Transparent��$��18(�   BorderThickness$ 1q�$��25��$��微软粉作品��$
��auto��$��2��$��Bottom=�$* $https://space.bilibili.com/283605961��  $	 特别鸣谢��.���� .+�� $��0,0,0,5q�$��50��
 �� $��50 ��� �� $	��150 �.+��� $��0��$��0,0,0,5q�$+��%pack://application:,,,/fanbal_束.png���� $��1��.+��� $��1��$��
fanbal 束��$
��Bold"�$��16(��� $��1�� J� Opacity$	 0.8Q�$
��Wrap=�$����对 MEFL 提供了很多技术支持，尽管项目里面没有他的手写的一行代码，但是每一行代码都是他教给我的。��  -   $��40��$	��150��$��Center=�$��2��$��访问主页��$+ %https://space.bilibili.com/337176361/��  $	 鸣谢��$
 True��   IsSwaped$
 True��.���� .+��� $ 
Horizontal=�.+�� $��0,0,15,5q�$��50��$	��250��
 �� $��50 ��� �� $��60 �.+��� $��0��$��0,0,0,5q�$*��$pack://application:,,,/RCKejijun.png���� $��1��.+��� $��1��$��科技菌Byte��$
��Bold"�$��15(��� $��1��$	 0.8Q�$
��Wrap=�$B��<很早认识的一个朋友。感谢对微软粉的支持。��  -   $��30��$��60��$��Center=�$��2��$��访问主页��$* $https://space.bilibili.com/454258954��� $��0,0,15,5q�$��50��$	��250��
 �� $��50 ��� �� $��60 �.+��� 
�������$��0��$��0,0,0,5q��� $��1��.+��� $��1��$��清水ユウヒ��$
��Bold"�$��15(��� $��1��$	 0.8Q�$
��Wrap=�$B��<认识两年的朋友，谢谢大会员，孩子很喜欢。��  -   $��30��$��60��$��Center=�$��2��$��访问主页��$* $https://space.bilibili.com/487121153���� $ 
Horizontal=�.+�� $��0,0,0,5q�$��50��$	��250��
 �� $��50 ��� �� $��60 �.+��� $��0��$��0,0,0,5q�$+��%pack://application:,,,/TunaFish2K.png���� $��1��.+��� $��1��$��
TunaFish2K��$
��Bold"�$��15(��� $��1��$	 0.8Q�$
��Wrap=�$0��*在 2022 年之前为 MEFL 发电的朋友��  -   $��30��$��60��$��Center=�$��2��$��访问主页��$$ https://afdian.net/@TunaFish2K��  $	 开源许可��$
 True��$
 True��.���� .+�� $��0,0,0,5q�$��40��
 �� �� $	��150 �.+��� $��0��.+��� $��1��$��NewtonSoft.Json��$
��Bold"�$��16(��� $��1��$	 0.8Q�$
��Wrap=�$C��=[MIT LICENSE]Popular high-performance JSON framework for .NET��  -   $��30��$��80��$��Center=�$��2��$��访问项目��$% https://www.newtonsoft.com/json���� $ 
Downloader��# �   	   MEFL.Controls.MyItemsCard  MyS �� 
TargetType � Triggers :� RoutedEvent W� From	 W� To g� Duration �� 
TargetName �� TargetProperty  SYTLE_Standard_Background  MyDS��%�  &	       &	 2    �� " 	 .���  � �� �� " ��.���  � �� �� " ��.R�� -   $��60��$��0,0,0,5q�
 �� $��60 ��� .+��� $
��Rect��$ 0Q�#���   $��2���� ���� *Icon+�� $��1��.+��� ���� *Name+�� ���� *Description+�� ���� *	Version+ :� $ 
MouseEnter��.[��� .���� .��W�  0 0.3$ 	0:0:0:0.2D�$
 Rect��$ Opacity�:� $ 
MouseLeave��.[��� .���� .��W�  0.3 0$ 	0:0:0:0.2D�$
 Rect��$ Opacity���  � ��# �   ��  � ��# �  ��  � ��$ 0,0,0,0q��� " 	 .���  � �� �� " ��.���  � �� �� " ��.R�� $��20��$��0,0,0,5q�
 �� $	��140 ��� $
��auto ��� $
��auto �.+��� $
��Rect��$ 0Q�#���   $��3���� ���� *Key+�� -   $��1������ *Value+�� $��2������ *Value.Selected.Uri+ :� $ 
MouseEnter��.[��� .���� .��W�  0 0.3$ 	0:0:0:0.2D�$
 Rect��$ Opacity�:� $ 
MouseLeave��.[��� .���� .��W�  0.3 0$ 	0:0:0:0.2D�$
 Rect��$ Opacity���  � ��# �   ��  � ��# �  ��  � ��$ 0,0,0,0q�.+�	  -   $	��MyC��#���  ���� *Downloaders+ 	  Title �� �� $�当前选中下载器：�� �� ��� *SelectedDownloaderString+  �� Mode$  OneWay=�	  $ 管理下载源��#���  ���� *DownSources+