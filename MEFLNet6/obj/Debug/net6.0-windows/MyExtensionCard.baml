   M S B A M L   `   `   `  ����H  DMEFL.Controls, Version=0.0.7.6, Culture=neutral, PublicKeyToken=nullD2clr-namespace:MEFL.Controls;assembly=MEFL.ControlsMEFL.Controls  5       MEFL#   MEFL.Controls.MyExtensionCard   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Dlocal2clr-namespace:MEFL.Controls;assembly=MEFL.Controls     5      
�������$	��150��5         SYTLE_Standard_ColorBrush#���   6     � 	Resources  5	       �� 
TargetType �� Property �� Value��%T   '��       �� 5
   
   " ��6   .�5      ��  � ��$
 Wrap=�6-   5      5         MEFL.Controls.MyCard   Title 5      � 5   
   $
��auto��6   .+�5      �� $
��Bold"�6   5      .��5      � .+�5   
   � y�6   �� *	NoError+.+�5      �� .+�5      � $��50��6    � ColumnDefinitions 5      �� 5      $��50 �6+   �� 5      .+�5      �� $��50��6'   $��50��62   $
��Left=�6W   ��6>   �� *Icon+6W   � 5      $��1��6   $��5,0,0,0q�6+    � RowDefinitions 5      �� 5      �� 5      �� 5       .+�5"      �� $��0��6%    �� Orientation$ 
Horizontal=�62   .+�5#      ��   I18N_String_AddIns_Author#���  6(   �� 5$      �� 5%      -   5         � Cursor$
 Hand��5%   e   ��6(   �� *PublisherUri+.�6s   .�5&   "   �� ��6,   �� *	Publisher+5'      5(      5)      �� 5*      $��1��6%   $ 
Horizontal=�62   .+�5+      ��  # I18N_String_AddIns_Descripition#���  6(   �� 5,      ��6(   �� *Description+5-      �� 5.      $��2��6%   $ 
Horizontal=�62   .+�5/      ��   I18N_String_AddIns_Version#���  6(   �� 50      ��6(   �� *	Version+51      52      53      54         MEFL.Controls.MyCheckBox  55      -   5        	   
Background#	 �   56      $��Right=�57      $	��Top=�58      
 d� 	IsChecked
 55      �� *IsOpen+y�6<   �� *IsNotRefreshing+58      &    MEFL.Controls.MyLoadingAnimation  59      $��Right=�6K   $	��Top=�6g   $��30��6   $��30��6�   y�6'   �� *IsRefreshing+6�   5:      � 5;   
   y�6   �� *
HasError+.+�5<      �� $��Center=�63   $��Center=�6N   ��6   �� *	ErrorInfo+6N   5=      5>      5?      