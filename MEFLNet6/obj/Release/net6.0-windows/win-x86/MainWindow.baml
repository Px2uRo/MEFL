   M S B A M L   `   `   `  ���� ?  ;MEFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:MEFLMEFL  -clr-namespace:MEFL.ControlsMEFL.Controls  'clr-namespace:MEFL.Pages
MEFL.Pages  H DMEFL.Controls, Version=0.0.7.6, Culture=neutral, PublicKeyToken=nullD2clr-namespace:MEFL.Controls;assembly=MEFL.ControlsMEFL.Controls     MEFL.MainWindow   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     $localclr-namespace:MEFL     )cclr-namespace:MEFL.Controls     &pclr-namespace:MEFL.Pages     Bcon2clr-namespace:MEFL.Controls;assembly=MEFL.Controls     -     � Title$  
MainWindow��$	��450��$	��800�� � WindowStyle$
 None=�$	��320��$	��600��'  !System.Windows.Shell.WindowChrome   WindowChrome      CaptionHeight$ 0Q� � 	Resources &   MEFL.Controls.MyLoadingAnimation �� 
TargetType �� Property   SYTLE_Standard_ColorBrush �� Value  MEFL.Controls.MyCard  SYTLE_Standard_Background  MEFL.Controls.MyPageBase+  %MEFL.Controls.ChangePageContentButton  MEFL.Controls.MyButton"  MEFL.Controls.MyComboBoxItem#  MEFL.Controls.MyItemsCardItem	  MEFL.Controls.MyItemsCard  SYTLE_Standard_BorderBrush��%!  '        '  '     '  x     '  �     '  �     '      '  K    '	  w    �� "  .���  �@ Color# �   �� "  .���  � ��# �   ��  � ��# �  ��  � ��$ 0,0,0,15q��� "  .���  �@ 	LineBrush# �   �� "  .���  �@ SideBarBrush# �   �� "  .���  � ��# �   ��  � ��$	 140����  � ��$ 35���� "  .���  �@ 
Decoration# �   �� "  .���  �@ 
Decoration# �   �� " 	 .���  � ��# �  ��  � ��# �  ��  �@	 BorderThickness$ 5q�.��� .+���   I18N_String_Insider#���  $	��100(�
�������$��Bottom=�$��Right=�$��	0,0,40,40q�$��Microsoft Yahei)�$
��Bold"���   
Background#���  �  � RowDefinitions �� $��40 ��� .+�� -   #���   	 J� Opacity$		 0.9Q�.+��� 
 �� Orientation$
 
Horizontal=�.+�$
   MEFL.Controls.ChangePageButton
  $   MEFL.Controls.ChangePageButton   	MouseDown$ ReturnToRealPage��$��45��$��0,5,0,0q�  HomePageButtonStatu
 � Tag# �  .��F� $��Center=�$��Center=������48N5�48��j��>��j��>N5�<n� N5�<n� ��j���j�N5p48��a��0@�48L�)��>L�)Q�>��a��0@p<n� ��a��0@�<n� L�)�L�)Q��a��0@p��� ��(&o �?�\��� �h�Z�� �h�ZD���x� D���x� L�)�N�` L�)�N�` D��рWA D��рWA �� ���(&o �?�� 	
���   �$	��0.8��$��24�� �� Stretch$
 Fill=�$��26����  ! I18N_String_Setting_Title_Now#���  $��20(�$��Center=�$��10,0,0,0q��� $
 
Horizontal=�$��Right=�.+��� -     AddInChangePageButtons#���  ��   ChangePageButtons#���  �� $
 
Horizontal=�.+�� $��45��$��0,5,0,0q�.+��� $��Center=�$��10��$��1��
���   ��� -   $��MinWindowButton��
�������$	 0Q�� $��45��$��0,5,0,0q�.+��� -   $��MaxWindowIcon��$��15��$��12�� �� CornerRadius$ 2��
���   �$��1q��� -   $��WinWindowIcon��$y�Hidden=�._�� .+��� $��12��$��10��$ 2��
���   �$��1q��� $��12��$��10��$ 2��
���   �$��1q�$��5,0,0,5q��� -   $��ChangeSizeButton��
�������$	 0Q�� $��45��$��0,5,0,0q�.+��� -   $��CloseWindowsButton��
���  ��$	 0Q�� .+��� $��Center=�$��15��$��1��
���   � J� RenderTransformOrigin$ 0.5,0.5&�z�V� .~��� �� ��  �� Angle$	 -45Q�T� �� $��Center=�$��15��$��1��
���   �$ 0.5,0.5&�z�V� .~��� �� �� $ 45Q�T� �� $��1�� 	 MainPage#��� 	 