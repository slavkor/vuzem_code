   M S B A M L   `   `   `  ����G  CIsm.Security, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:Ism.Security.ViewsIsm.Security.Views  5      G CIsm.Controls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA0clr-namespace:Ism.Controls;assembly=Ism.ControlsIsm.Controls 5           Ism.Security.Views.UserAdd   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      2local clr-namespace:Ism.Security.Views     5      Econtrols0clr-namespace:Ism.Controls;assembly=Ism.Controls     5        � 	Resources  5
       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $X Rpack://application:,,,/Ism.Infrastructure;component/Resources/BaseButtonStyle.xaml?�6%   6�   %    5      .��5      �  � ColumnDefinitions 5   
   �� 5      6!   �� 5      6!    � RowDefinitions 5   
   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $��* �6   6)   �� 5      $
��Auto �6   6,   .+�5   
     Ism.Controls.LayoutGroup  $��1��6   $��0��6,   $��2��6<   $  Ism.Controls.EnterKeyTraversal   	IsEnabled . .+�5      "  Ism.Controls.LabelledTextBox     Label$ Uporabniško ime��6'      Text 6?   �� *
UserName+6[     5       $ Geslo��6'    65   �� *
Password+6Q   5!      �� 5+   
   $��0��6   $��5��6!   ��6.   �� *CancelCommand+.��6P   Cancel6X   �� 5,   
   $��1��6   $��5��6!   ��6.   �� *LoginCommand+.��6O   Login6V   5-      5.      