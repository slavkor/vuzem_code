   M S B A M L   `   `   `  ����H  DIsm.Employees, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:Ism.Employees.ViewsIsm.Employees.Views  5      -clr-namespace:Ism.EmployeesIsm.Employees  5      M IIsm.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS<clr-namespace:Ism.Infrastructure;assembly=Ism.InfrastructureIsm.Infrastructure 5      iGclr-namespace:Ism.Infrastructure.Converters;assembly=Ism.InfrastructureIsm.Infrastructure.Converters 5	      Y?clr-namespace:Ism.Infrastructure.Ui;assembly=Ism.InfrastructureIsm.Infrastructure.Ui 5
      G CIsm.Controls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA0clr-namespace:Ism.Controls;assembly=Ism.ControlsIsm.Controls 5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5      &     Ism.Employees.Views.EmployeeEdit   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      3local!clr-namespace:Ism.Employees.Views     5      0localrgnclr-namespace:Ism.Employees     5      Linf<clr-namespace:Ism.Infrastructure;assembly=Ism.Infrastructure     5      ^
convertersGclr-namespace:Ism.Infrastructure.Converters;assembly=Ism.Infrastructure     5	      Nui?clr-namespace:Ism.Infrastructure.Ui;assembly=Ism.Infrastructure     5
      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5      P	 LPrism.Wpf, Version=6.3.0.0, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59$prismhttp://prismlibrary.com/ 	 5      ;ctrl0clr-namespace:Ism.Controls;assembly=Ism.Controls 	 5      3system&clr-namespace:System;assembly=mscorlib 	 5      5xcdg*http://schemas.xceed.com/wpf/xaml/datagrid 	 5      4xctk)http://schemas.xceed.com/wpf/xaml/toolkit 	 5      n
 jTelerik.Windows.Controls.GridView, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a ]Telerik.Windows.Data, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Input, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.Navigation, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Gtelerik1http://schemas.telerik.com/2008/xaml/presentation 
     5      ! 	 Prism.Mvvm.ViewModelLocator    AutoWireViewModel  .    UcStyle#��[  5       � 	Resources 5       �� MergedDictionaries �� Source  WS �� 
TargetType  CustomWindowStyle �� BasedOn �� Property �� Value  True 	 False8  2Ism.Infrastructure.Converters.EnumBooleanConverter  EnumBooleanConverterF  @Ism.Infrastructure.Converters.SecurityScopeToVisibilityConverter  SecurityToVisbility�� 5   
    5      �� 5      $X Rpack://application:,,,/Ism.Infrastructure;component/Resources/BaseButtonStyle.xaml?�6%   6�   �� 5      $Y Spack://application:,,,/Ism.Infrastructure;component/Resources/BaseTextBoxStyle.xaml?�6%   6�   %  &	       7  &	 }     &	 �     &	 �     &	 �     �� 5      " �6   8   .�5      ��  � ��$	 400��6-   6;   �� 5       � ��$	 400��6.   6<   5      ��5       True��60   ��5!      	False��62     5"      6M     5#      6Z   5$      .��5&      � .  (System.Windows.Interactivity.Interaction   Triggers 5'   
   F 	 @Prism.Interactivity.InteractionRequest.InteractionRequestTrigger  5(      3  -System.Windows.Interactivity.EventTriggerBase	   SourceObject	 6.   �� **(EmployeeChangeEmployerInteractionRequest+
 �� Mode$
 OneWay=�.  (System.Windows.Interactivity.TriggerBase   Actions. 5)      + 	 %Prism.Interactivity.PopupWindowAction     IsModal .    WindowStartupLocation$ CenterOwnerX�69      WindowStyle# [ 6]      WindowContent 5*      (	   "Ism.Employees.Views.ChangeEmployer	  5+      5-      5.       � RowDefinitions 51   
   �� 52      $��* �6   6)   �� 53      $
��Auto �6   6,    � ColumnDefinitions 55   
   �� 56      $
��0.5* �6   6.   �� 57      $��* �6   6+   .+�5:   
   � $��0��6   $��0��6    5;      �� 5<      $
��Auto �6    60   �� 5=      $
��Auto �6    60   �� 5>      $��* �6    6-   .+�5A      
  Ism.Controls.LayoutGroup
  $��0��6   $��0��6,   .+�5C      "  Ism.Controls.LabelledTextBox     Label$ Šifra��6'   {�.  ,  &Ism.Infrastructure.Ui.FocusAdvancement   AdvancesByKey .    Text 65   �� *Employee.Iid+$
 TwoWay=�6�     5D      $	 Ime��6'    .  63   �� *Employee.Name+ �� UpdateSourceTrigger$ PropertyChanged=�$
 TwoWay=� �� ValidatesOnDataErrors . 6�     5E      $ Priimek��6'    .  67   �� *Employee.LastName+$ PropertyChanged=�$
 TwoWay=� . 6�     5F      $ Nadimek��6'    .  67   �� *Employee.NickName+$ PropertyChanged=�$
 TwoWay=� . 6�     5G      $ Naziv��6'    .  65   �� *Employee.Position+$ PropertyChanged=�$
 TwoWay=� . 6�     5H      $ Državljanstvo��6'    .  6=   �� *Employee.Nationality+$ PropertyChanged=�$
 TwoWay=� . 6�   $  Ism.Controls.LabeledDatePicker  5I         Label$ Datum rojstva��6)    .    	DateValue 6?   �� *Employee.BirthDay+$
 TwoWay=�$ PropertyChanged=� . 
 �� Now �� TargetNullValue# Z 6    5J      $ Kraj rojstva��6'    .  6<   �� *Employee.BirthPlace+$ PropertyChanged=�$
 TwoWay=� . 6�     5K      $ Emšo��6'    .  64   �� *Employee.Emso+$ PropertyChanged=�$
 TwoWay=� . 6�     5L      $ Davčna številka��6'    .  6?   �� *Employee.TaxNumber+$ PropertyChanged=�$
 TwoWay=� . 6�     5M      $" Številka osebnega dokumenta��6'    .  6K   �� *Employee.PersonalIdNumber+$ PropertyChanged=�$
 TwoWay=� . 6�     5N      $ Številka zavarovanja��6'    .  6D   �� *Employee.InsuranceNumber+$ PropertyChanged=�$
 TwoWay=� . 6�     5O      $
 Iban��6'    .  64   �� *Employee.Iban+$ PropertyChanged=�$
 TwoWay=� . 6�     5P      $ Email��6'    .  65   �� *Employee.Email+$ PropertyChanged=�$
 TwoWay=� . 6�   "  Ism.Controls.LabeledComboBox  5Q         LabelContent$ Zakonski stan��6'      DisplayMemberPath$
 Name��6�   $   IsSynchronizedWithCurrentItem$
 TrueY�6     ItemsSource 6D   �� *MartialStatuses+$ PropertyChanged=�$
 TwoWay=�   SelectedItem 6�   �� *SelectedMartialStatus+$ PropertyChanged=�$
 TwoWay=�6?    5R      $ 	Uporabnik��6'    .  69   �� *Employee.UserName+$ PropertyChanged=�$
 TwoWay=� . y�6�   ��   �� Source  Z� ! �� 	Converter#! [ " �� ConverterParameter$" admin��$
 OneWay=�6_  5S      � 5U      $��1��6    5V      �� 5W      6)   �� 5X      6)   �� 5Y      6)   .+�5[      ]� ��. $��0��61   .+�5\      �� $
��Left=�6   .��62   Jeziki6:   5o      ]� 5q      ��. $��1��61   $��30��6A   .+�5r      �� $
��Left=�6   .��62   Delovno mesto6A   *  $Telerik.Windows.Controls.RadComboBox  5s      # �� DisplayMemberPath$# WorkPlaceName��6   ��6*   �� *
WorkPlaces+$
 TwoWay=�$ PropertyChanged=�$ �� SelectedItem$ 6�   �� *SelectedWorkPlace+$
 TwoWay=�$ PropertyChanged=�6   5t      ]� 5v      ��. $��2��61   $��1q�6A   .+�5w      ��    Ism.Employees.RegionNames%   ExternalEmployerRegion! 	 Prism.Regions.RegionManager&   
RegionName#& Z% 6%   6   5x      5y      � 5}      $��2��6    5~      �� 5      $
��Auto �6$   64   �� 5�      $��* �6$   61   .+�5�      ]� $��0��6   ��.  .+�5�      �� $
��Left=�6   $��Delodajalec��63   ��6I   �� *ChangeEmployerCommand+y�6s   ��   Z� #! [ $" employee.employer��$
 OneWay=�6  5�      �� 5�      $��1��6   ._�5�      �� $��3��6   $��Uniform=�6O   ��6*   �� *EmployeeImagePath+6O   5�      5�      5�      � 5�   
   $��0��6   $��1��6    5�      �� 5�      $��3* �6    6.   �� 5�      $��* �6    6-   �� 5�      $��* �6    6-   .+�5�      �� $��0��6   '   DocumentsRegion#& Z' 6*   6}   �� 5�      $��1��6   (   AddressRegion#& Z( 6*   6{   �� 5�      $��2��6   )   ContactsRegion#& Z) 6*   6|   5�      ]� 5�   
   $��2��6   $��0��6"   $��2��62   ��.  .+�5�      �� $
��Left=�6   $��	Prekliči��6+   ��6>   �� *CancelCommand+�� 5�      $��Right=�6   $��Shrani��6,   #��[ 6]   ��6=   �� *SaveCommand+6�   5�      5�      5�      