   M S B A M L   `   `   `  ���� R  NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R8clr-namespace:System.ComponentModel;assembly=WindowsBaseSystem.ComponentModel  H DIsm.Employees, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:Ism.Employees.ViewsIsm.Employees.Views M IIsm.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS<clr-namespace:Ism.Infrastructure;assembly=Ism.InfrastructureIsm.Infrastructure iGclr-namespace:Ism.Infrastructure.Converters;assembly=Ism.InfrastructureIsm.Infrastructure.Converters O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem '   !Ism.Employees.Views.EmployeesList   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006      @d2http://schemas.microsoft.com/expression/blend/2008      Hscm8clr-namespace:System.ComponentModel;assembly=WindowsBase      3local!clr-namespace:Ism.Employees.Views      Linf<clr-namespace:Ism.Infrastructure;assembly=Ism.Infrastructure      ]	converterGclr-namespace:Ism.Infrastructure.Converters;assembly=Ism.Infrastructure      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  ?dxmvvm2http://schemas.devexpress.com/winfx/2008/xaml/mvvm  P LPrism.Wpf, Version=6.3.0.0, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59$prismhttp://prismlibrary.com/  3system&clr-namespace:System;assembly=mscorlib  n	 jTelerik.Windows.Controls.GridView, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a
 ]Telerik.Windows.Data, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Input, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.Navigation, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Gtelerik1http://schemas.telerik.com/2008/xaml/presentation 	 
    !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel  .    UcStyle#��[   � 	Resources  �� MergedDictionaries �� Source; 	 5Telerik.Windows.Controls.GridView.GridViewSearchPanel �� 
TargetType  GW �� BasedOn �� Property �� Value:  4Ism.Infrastructure.Converters.GridItemIndexConverter  ItemInexconverter��  �� $X Rpack://application:,,,/Ism.Infrastructure;component/Resources/BaseButtonStyle.xaml?��� $M Gpack://application:,,,/Ism.Infrastructure;component/Resources/Grid.xaml?�%s   '        7  &	 V     �� "  8   .���  � ��$ 20(���  � ��$
 Bold"���  � ��$ 50��  .���  � ColumnDefinitions �� $��* ��� $
��Auto �.+�,  &Telerik.Windows.Controls.RadTabControl  -   $	��tab��.@�)  #Telerik.Windows.Controls.RadTabItem  $	��Vsi��
	 � Tag$	 0��.��* 	 $Telerik.Windows.Controls.RadGridView  -   
  Name$

 list��#   CanUserSearchInHiddenColumns . ; 	 5Telerik.Windows.Controls.GridView.GridViewDataControl   AutoGenerateColumns .     ShowSearchPanel .    ShowGroupPanel .     ShowColumnFooters . * 
 $Telerik.Windows.Controls.DataControl   ItemsSource �� *	Employees+   SelectedItem �� *SelectedEmployee+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�   RowStyle �� 3	 	 -Telerik.Windows.Controls.GridView.GridViewRow" 	  �� Triggers ��  �� Binding ��  �� RelativeSource �� *self+ �� Path$# DataContext.WorkPeriod.Active� �� Value 0.a���  � ��
 �r���   Columns 1
 	 +Telerik.Windows.Controls.GridViewDataColumn
  6 	 0Telerik.Windows.Controls.GridViewBoundColumnBase   Header$
 Delo��- 	 'Telerik.Windows.Controls.GridViewColumn   
IsReadOnly .    DataMemberBinding �� *" Employee.WorkPlace.WorkPlaceName+
  $ Priimek�� .  �� *Employee.LastName+   AggregateFunctions ( 
 "Telerik.Windows.Data.CountFunction  , 
 &Telerik.Windows.Data.AggregateFunction   Caption$ Število zaposlenih: ��
  $	 Ime�� .  �� *Employee.Name+
  $ Rojen��    DataFormatString$  
dd.MM.yyyy�� .  �� *Employee.BirthDay+
  $ Rojen v�� .  �� *Employee.BirthPlace+
  $ Emšo�� .  �� *Employee.Emso+
  $ Davčna�� .  �� *Employee.TaxNumber+
  $ Št. zavarovanja�� .  �� *Employee.InsuranceNumber+
  $ Datum prijave��$  
dd.MM.yyyy�� .  �� *WorkPeriod.Start+
  $ Datum odjave��$  
dd.MM.yyyy�� .  �� *WorkPeriod.End+
  $ Trenutni projekt�� .  �� *Project.PointName+5  /Telerik.Windows.Controls.EventToCommandBehavior!   EventBindings! +  %Telerik.Windows.Controls.EventBinding  "   	EventName$" MouseDoubleClick��#   RaiseOnHandledEvents# . $   Command$ �� *DoubleClickCommand+%   CommandParameter% �� & �� ElementName$
& list��$ SelectedItem�  $��	Zaposleni��$	 1��.��  -   $
 emplist�� .  .   .  .   .  �� *	Employees+ �� *SelectedEmployee+$ TwoWay=�$ PropertyChanged=� �� " 	  ��  ��  �� *self+$# DataContext.WorkPeriod.Active� 0.a���  � ��
 �r��� 
  $
 Delo�� .  �� *" Employee.WorkPlace.WorkPlaceName+
  $ Priimek�� .  �� *Employee.LastName+   $ Število zaposlenih: ��
  $	 Ime�� .  �� *Employee.Name+
  $ Rojen��$  
dd.MM.yyyy�� .  �� *Employee.BirthDay+
  $ Rojen v�� .  �� *Employee.BirthPlace+
  $ Emšo�� .  �� *Employee.Emso+
  $ Davčna�� .  �� *Employee.TaxNumber+
  $ Št. zavarovanja�� .  �� *Employee.InsuranceNumber+
  $ Datum prijave��$  
dd.MM.yyyy�� .  �� *WorkPeriod.Start+
  $ Datum odjave��$  
dd.MM.yyyy�� .  �� *WorkPeriod.End+
  $ Trenutni projekt�� .  �� *Project.PointName+!   $" MouseDoubleClick��# . $ �� *DoubleClickCommand+% �� $& emplist��$ SelectedItem�  $��
Izposojeni��$	 2��.��  -   $
 loanlist�� .  .   .  .   .  �� *	Employees+ �� *SelectedEmployee+$ TwoWay=�$ PropertyChanged=� �� " 	  ��  ��  �� *self+$# DataContext.WorkPeriod.Active� 0.a���  � ��
 �r��� 
  $
 Delo�� .  �� *" Employee.WorkPlace.WorkPlaceName+
  $ Priimek�� .  �� *Employee.LastName+   $ Število zaposlenih: ��
  $	 Ime�� .  �� *Employee.Name+
  $ Rojen��$  
dd.MM.yyyy�� .  �� *Employee.BirthDay+
  $ Rojen v�� .  �� *Employee.BirthPlace+
  $ Emšo�� .  �� *Employee.Emso+
  $ Davčna�� .  �� *Employee.TaxNumber+
  $ Št. zavarovanja�� .  �� *Employee.InsuranceNumber+
  $ Datum prijave��$  
dd.MM.yyyy�� .  �� *WorkPeriod.Start+
  $ Datum odjave��$  
dd.MM.yyyy�� .  �� *WorkPeriod.End+
  $ Trenutni projekt�� .  �� *Project.PointName+!   $" MouseDoubleClick��# . $ �� *DoubleClickCommand+% �� $& loanlist��$ SelectedItem�  $��Po gradbiščih��$	 3��.��  -   $
 alist�� .  .   .  .   .  �� *EmployeesAway+ �� " 	  ��  ��  �� *self+$# DataContext.WorkPeriod.Active� 0.a���  � ��
 �r��� 
  $ Priimek�� .  �� *
LastName+   $ Število zaposlenih: ��
  $	 Ime�� .  �� *Name+
  $ Rojen��$  
dd.MM.yyyy�� .  �� *
BirthDay+
  $ Rojen v�� .  �� *
BirthPlace+
  $ Emšo�� .  �� *Emso+
  $ Davčna�� .  �� *	TaxNumber+
  $ Št. zavarovanja�� .  �� *InsuranceNumber+
  $ Trenutni projekt�� .  �� *CurrentOccupancy.Name+
  $ število dni�� .  �� *CurrentOccupancy.TotalDays+!   $" MouseDoubleClick��# . $ �� *DoubleClickCommand+% �� $
& list��$ SelectedItem�  $
��Doma��$	 4��.��  -   $
 hlist�� .  .   .  .   .  �� *EmployeesHome+ �� " 	  ��  ��  �� *self+$# DataContext.WorkPeriod.Active� 0.a���  � ��
 �r��� 
  $ Priimek�� .  �� *
LastName+   $ Število zaposlenih: ��
  $	 Ime�� .  �� *Name+
  $ Rojen��$  
dd.MM.yyyy�� .  �� *
BirthDay+
  $ Rojen v�� .  �� *
BirthPlace+
  $ Emšo�� .  �� *Emso+
  $ Davčna�� .  �� *	TaxNumber+
  $ Št. zavarovanja�� .  �� *InsuranceNumber+
  $ Trenutni projekt�� .  �� *CurrentOccupancy.Name+
  $ število dni�� .  �� *CurrentOccupancy.TotalDays+!   $" MouseDoubleClick��# . $ �� *DoubleClickCommand+% �� $
& list��$ SelectedItem�!   $" SelectionChanged��# . $ �� *TabSelectionChangedCommand+% �� $	& tab��$ SelectedItem�