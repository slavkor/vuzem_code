   M S B A M L   `   `   `  ����M  IIsm.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullY?clr-namespace:Ism.Infrastructure.Ui;assembly=Ism.InfrastructureIsm.Infrastructure.Ui  5      E AIsm.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Ism.Common.ViewsIsm.Common.Views 5      G CIsm.Controls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA0clr-namespace:Ism.Controls;assembly=Ism.ControlsIsm.Controls 5      &    Ism.Common.Views.EditContactView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      Nui?clr-namespace:Ism.Infrastructure.Ui;assembly=Ism.Infrastructure     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:Ism.Common.Views     5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5      P LPrism.Wpf, Version=6.3.0.0, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59$prismhttp://prismlibrary.com/  5	      4xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5
      ;ctrl0clr-namespace:Ism.Controls;assembly=Ism.Controls  5      !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel  .  � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $X Rpack://application:,,,/Ism.Infrastructure;component/Resources/BaseButtonStyle.xaml?�6%   6�   %    5      .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   6)   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   .+�5   
     Ism.Controls.LayoutGroup  $	��500��6   $��0��6*   $  Ism.Controls.EnterKeyTraversal   	IsEnabled .  � DataContext 67   �� *	Contact+ �� UpdateSourceTrigger$ PropertyChanged=�.+�5      "  Ism.Controls.LabelledTextBox     Label$ Šifra��6#   {�.  ,   &Ism.Infrastructure.Ui.FocusAdvancement	   AdvancesByKey	 . 
   Text
 61   �� *Iid+ �� Mode$ OneTime=�6�     5      $	 Ime��6#   	 . 
 6/   �� *Name+$ PropertyChanged=�6�     5      $ Priimek��6#   	 . 
 63   �� *
LastName+$ PropertyChanged=�6�     5       $ Telefon��6#   	 . 
 63   �� *Phone+$ PropertyChanged=�6�     5!      $ Mobilni telefon��6#   	 . 
 6;   �� *MobilePhone+$ PropertyChanged=�6�     5"      $ Službeni telefon��6#   	 . 
 6<   �� *OrganizationPhone+$ PropertyChanged=�6�     5#      $ Službeni mobilni telefon��6#   	 . 
 6D   �� *OrganizationMobilePhone+$ PropertyChanged=�6�     5$      $ Email��6#   	 . 
 61   �� *Email+$ PropertyChanged=�6�     5%      $
 Opis��6#      	MultiLine . 	 . 
 6A   �� *Description+$ PropertyChanged=�6�   5&      ]� 5*   
   $��2��6   $��0��6"   ��.  .+�5+      �� $
��Left=�6   $��	Prekliči��6+   ��6>   �� *CancelCommand+6c   �� 5,      $��Right=�6   $��Shrani��6,   ��6=   �� *SaveCommand+6_   5-      5.      5/      