   M S B A M L   `   `   `  ���� H  DIsm.Employees, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:Ism.Employees.ViewsIsm.Employees.Views  -clr-namespace:Ism.EmployeesIsm.Employees  M IIsm.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS<clr-namespace:Ism.Infrastructure;assembly=Ism.InfrastructureIsm.Infrastructure iGclr-namespace:Ism.Infrastructure.Converters;assembly=Ism.InfrastructureIsm.Infrastructure.Converters Y?clr-namespace:Ism.Infrastructure.Ui;assembly=Ism.InfrastructureIsm.Infrastructure.Ui G CIsm.Controls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA0clr-namespace:Ism.Controls;assembly=Ism.ControlsIsm.Controls O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem &     Ism.Employees.Views.EmployeeEdit   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     3local!clr-namespace:Ism.Employees.Views     0localrgnclr-namespace:Ism.Employees     Linf<clr-namespace:Ism.Infrastructure;assembly=Ism.Infrastructure     ^
convertersGclr-namespace:Ism.Infrastructure.Converters;assembly=Ism.Infrastructure     Nui?clr-namespace:Ism.Infrastructure.Ui;assembly=Ism.Infrastructure     c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  P	 LPrism.Wpf, Version=6.3.0.0, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59$prismhttp://prismlibrary.com/ 	 ;ctrl0clr-namespace:Ism.Controls;assembly=Ism.Controls 	 3system&clr-namespace:System;assembly=mscorlib 	 5xcdg*http://schemas.xceed.com/wpf/xaml/datagrid 	 4xctk)http://schemas.xceed.com/wpf/xaml/toolkit 	 n
 jTelerik.Windows.Controls.GridView, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a ]Telerik.Windows.Data, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Input, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.Navigation, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Gtelerik1http://schemas.telerik.com/2008/xaml/presentation 
     ! 	 Prism.Mvvm.ViewModelLocator    AutoWireViewModel  .    UcStyle#��[   � 	Resources  �� MergedDictionaries �� Source  WS �� 
TargetType  CustomWindowStyle �� BasedOn �� Property �� Value  True 	 False8  2Ism.Infrastructure.Converters.EnumBooleanConverter  EnumBooleanConverterF  @Ism.Infrastructure.Converters.SecurityScopeToVisibilityConverter  SecurityToVisbility��  �� $X Rpack://application:,,,/Ism.Infrastructure;component/Resources/BaseButtonStyle.xaml?��� $Y Spack://application:,,,/Ism.Infrastructure;component/Resources/BaseTextBoxStyle.xaml?�%�   &	       7  &	 @     &	 N     &	 ]     &	 b     �� " �8   .���  � ��$	 400����  � ��$	 400����True����	False��    .��� .  (System.Windows.Interactivity.Interaction   Triggers F 	 @Prism.Interactivity.InteractionRequest.InteractionRequestTrigger  3  -System.Windows.Interactivity.EventTriggerBase	   SourceObject	 �� **(EmployeeChangeEmployerInteractionRequest+
 �� Mode$
 OneWay=�.  (System.Windows.Interactivity.TriggerBase   Actions. + 	 %Prism.Interactivity.PopupWindowAction     IsModal .    WindowStartupLocation$ CenterOwnerX�   WindowStyle# [    WindowContent (	   "Ism.Employees.Views.ChangeEmployer	   � RowDefinitions �� $��* ��� $
��Auto � � ColumnDefinitions �� $
��0.5* ��� $��* �.+�� $��0��$��0�� �� $
��Auto ��� $
��Auto ��� $��* �.+�
  Ism.Controls.LayoutGroup
  $��0��$��0��.+�"  Ism.Controls.LabelledTextBox     Label$ Šifra��{�.  ,  &Ism.Infrastructure.Ui.FocusAdvancement   AdvancesByKey .    Text �� *Employee.Iid+$
 TwoWay=�  $	 Ime�� .  �� *Employee.Name+ �� UpdateSourceTrigger$ PropertyChanged=�$
 TwoWay=� �� ValidatesOnDataErrors .   $ Priimek�� .  �� *Employee.LastName+$ PropertyChanged=�$
 TwoWay=� .   $ Nadimek�� .  �� *Employee.NickName+$ PropertyChanged=�$
 TwoWay=� .   $ Naziv�� .  �� *Employee.Position+$ PropertyChanged=�$
 TwoWay=� .   $ Državljanstvo�� .  �� *Employee.Nationality+$ PropertyChanged=�$
 TwoWay=� . $  Ism.Controls.LabeledDatePicker     Label$ Datum rojstva�� .    	DateValue �� *Employee.BirthDay+$
 TwoWay=�$ PropertyChanged=� . 
 �� Now �� TargetNullValue# Z   $ Kraj rojstva�� .  �� *Employee.BirthPlace+$ PropertyChanged=�$
 TwoWay=� .   $ Emšo�� .  �� *Employee.Emso+$ PropertyChanged=�$
 TwoWay=� .   $ Davčna številka�� .  �� *Employee.TaxNumber+$ PropertyChanged=�$
 TwoWay=� .   $" Številka osebnega dokumenta�� .  �� *Employee.PersonalIdNumber+$ PropertyChanged=�$
 TwoWay=� .   $ Številka zavarovanja�� .  �� *Employee.InsuranceNumber+$ PropertyChanged=�$
 TwoWay=� .   $
 Iban�� .  �� *Employee.Iban+$ PropertyChanged=�$
 TwoWay=� .   $ Email�� .  �� *Employee.Email+$ PropertyChanged=�$
 TwoWay=� . "  Ism.Controls.LabeledComboBox     LabelContent$ Zakonski stan��   DisplayMemberPath$
 Name��$   IsSynchronizedWithCurrentItem$
 TrueY�   ItemsSource �� *MartialStatuses+$ PropertyChanged=�$
 TwoWay=�   SelectedItem �� *SelectedMartialStatus+$ PropertyChanged=�$
 TwoWay=�  $ 	Uporabnik�� .  �� *Employee.UserName+$ PropertyChanged=�$
 TwoWay=� . y���   �� Source  Z� ! �� 	Converter#! [ " �� ConverterParameter$" admin��$
 OneWay=�� $��1�� �� �� �� .+�]� ��. $��0��.+��� $
��Left=�.��Jeziki]� ��. $��1��$��30��.+��� $
��Left=�.��Delovno mesto*  $Telerik.Windows.Controls.RadComboBox  # �� DisplayMemberPath$# WorkPlaceName������ *
WorkPlaces+$
 TwoWay=�$ PropertyChanged=�$ �� SelectedItem$ �� *SelectedWorkPlace+$
 TwoWay=�$ PropertyChanged=�]� ��. $��2��$��1q�.+���    Ism.Employees.RegionNames%   ExternalEmployerRegion! 	 Prism.Regions.RegionManager&   
RegionName#& Z% � $��2�� �� $
��Auto ��� $��* �.+�]� $��0����.  .+��� $
��Left=�$��Delodajalec������ *ChangeEmployerCommand+y���   Z� #! [ $" employee.employer��$
 OneWay=��� $��1��._��� $��3��$��Uniform=����� *EmployeeImagePath+� $��0��$��1�� �� $��3* ��� $��* ��� $��* �.+��� $��0��'   DocumentsRegion#& Z' �� $��1��(   AddressRegion#& Z( �� $��2��)   ContactsRegion#& Z) ]� $��2��$��0��$��2����.  .+��� $
��Left=�$��	Prekliči������ *CancelCommand+�� $��Right=�$��Shrani��#��[ ���� *SaveCommand+