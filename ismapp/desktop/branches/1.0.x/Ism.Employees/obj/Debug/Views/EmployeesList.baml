   M S B A M L   `   `   `  ����R  NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R8clr-namespace:System.ComponentModel;assembly=WindowsBaseSystem.ComponentModel  5      H DIsm.Employees, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:Ism.Employees.ViewsIsm.Employees.Views 5      M IIsm.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS<clr-namespace:Ism.Infrastructure;assembly=Ism.InfrastructureIsm.Infrastructure 5      iGclr-namespace:Ism.Infrastructure.Converters;assembly=Ism.InfrastructureIsm.Infrastructure.Converters 5	      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5      '   !Ism.Employees.Views.EmployeesList   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml      5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006      5      @d2http://schemas.microsoft.com/expression/blend/2008      5      Hscm8clr-namespace:System.ComponentModel;assembly=WindowsBase      5      3local!clr-namespace:Ism.Employees.Views      5      Linf<clr-namespace:Ism.Infrastructure;assembly=Ism.Infrastructure      5      ]	converterGclr-namespace:Ism.Infrastructure.Converters;assembly=Ism.Infrastructure      5	      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5
      ?dxmvvm2http://schemas.devexpress.com/winfx/2008/xaml/mvvm  5      P LPrism.Wpf, Version=6.3.0.0, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59$prismhttp://prismlibrary.com/  5      3system&clr-namespace:System;assembly=mscorlib  5      n	 jTelerik.Windows.Controls.GridView, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a
 ]Telerik.Windows.Data, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Input, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.Navigation, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Gtelerik1http://schemas.telerik.com/2008/xaml/presentation 	 
    5      !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel  .  � 	Resources 5       �� MergedDictionaries �� Source; 	 5Telerik.Windows.Controls.GridView.GridViewSearchPanel �� 
TargetType   GW �� BasedOn �� Property �� Value:  4Ism.Infrastructure.Converters.GridItemIndexConverter  ItemInexconverter�� 5   
    5      �� 5      $X Rpack://application:,,,/Ism.Infrastructure;component/Resources/BaseButtonStyle.xaml?�6%   6�   �� 5      $M Gpack://application:,,,/Ism.Infrastructure;component/Resources/Grid.xaml?�6%   6x   %�   '        7   &	 �     �� 5      "  6   8   .�5      ��  � ��$ 20(�6-   6:   �� 5       � ��$
 Bold"�6/   6>   �� 5       � ��$ 50��6+   68   5        5      6K   5"      .��5$      �  � ColumnDefinitions 5&   
   �� 5'      $��* �6   6+   �� 5(      $
��Auto �6   6.   .+�5*   
   ,  &Telerik.Windows.Controls.RadTabControl  -   5        $	��tab��5*       .@�5+      )  #Telerik.Windows.Controls.RadTabItem  $	��Vsi��6!   
	 � Tag$	 0��6/   .��5,      * 	 $Telerik.Windows.Controls.RadGridView  -   5        
  Name$

 list��50      #   CanUserSearchInHiddenColumns . ; 	 5Telerik.Windows.Controls.GridView.GridViewDataControl   AutoGenerateColumns .     ShowSearchPanel .    ShowGroupPanel .     ShowColumnFooters . * 
 $Telerik.Windows.Controls.DataControl   ItemsSource 5-      �� *	Employees+   SelectedItem 51      �� *SelectedEmployee+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�   RowStyle 55      �� 56      3	 	 -Telerik.Windows.Controls.GridView.GridViewRow" 	 6     �� Triggers 57      �� 58   "    �� Binding 6.   ��  �� RelativeSource �� *self+ �� Path$# DataContext.WorkPeriod.Active� �� Value 06�   .a�59   &   ��  � ��
 �r���6T   5:   #   5<         Columns 5?      1
 	 +Telerik.Windows.Controls.GridViewDataColumn
  5@      6 	 0Telerik.Windows.Controls.GridViewBoundColumnBase   Header$
 Delo��65   - 	 'Telerik.Windows.Controls.GridViewColumn   
IsReadOnly .    DataMemberBinding 6D   �� *" Employee.WorkPlace.WorkPlaceName+6�   
  5A      $ Priimek��65    .  6G   �� *Employee.LastName+   AggregateFunctions 5B      ( 
 "Telerik.Windows.Data.CountFunction  5C   "   , 
 &Telerik.Windows.Data.AggregateFunction   Caption$ Število zaposlenih: ��68   5E      
  5F      $	 Ime��65    .  6C   �� *Employee.Name+6�   
  5G      $ Rojen��65       DataFormatString$  
dd.MM.yyyy��6u    .  6E   �� *Employee.BirthDay+6�   
  5H      $ Rojen v��65    .  6G   �� *Employee.BirthPlace+6�   
  5I      $ Emšo��65    .  6D   �� *Employee.Emso+6�   
  5J      $ Davčna��65    .  6F   �� *Employee.TaxNumber+6�   
  5K      $ Št. zavarovanja��65    .  6N   �� *Employee.InsuranceNumber+6�   
  5M      $ Datum prijave��65   $  
dd.MM.yyyy��6|    .  6M   �� *WorkPeriod.Start+6�   
  5N      $ Datum odjave��65   $  
dd.MM.yyyy��6y    .  6L   �� *WorkPeriod.End+6�   
  5O      $ Trenutni projekt��65    .  6P   �� *Project.PointName+6�   
  5P      $ Varilski certifikati��65    .  6T   �� *WedlerCerts+6�   5  /Telerik.Windows.Controls.EventToCommandBehavior!   EventBindings! 5T      +  %Telerik.Windows.Controls.EventBinding  5U      "   	EventName$" MouseDoubleClick��6W   #   RaiseOnHandledEvents# . $   Command$ 60   �� *DoubleClickCommand+%   CommandParameter% 6t   �� & �� ElementName$
& list��$ SelectedItem�6�   5W      5X        5Y      $��	Zaposleni��6!   $	 1��65   .��5Z        -   5        $
 emplist��5^       .  .   .  .   .  5[      �� *	Employees+ 5_      �� *SelectedEmployee+$ TwoWay=�$ PropertyChanged=� 5c      �� 5d      " 	 6     5e      �� 5f   "    6.   ��  �� *self+$# DataContext.WorkPeriod.Active� 06�   .a�5g   &   ��  � ��
 �r���6T   5h   #   5j       5l      
  5m      $
 Delo��65    .  6D   �� *" Employee.WorkPlace.WorkPlaceName+6�   
  5n      $ Priimek��65    .  6G   �� *Employee.LastName+ 5o        5p   "   $ Število zaposlenih: ��68   5r      
  5s      $	 Ime��65    .  6C   �� *Employee.Name+6�   
  5t      $ Rojen��65   $  
dd.MM.yyyy��6u    .  6E   �� *Employee.BirthDay+6�   
  5u      $ Rojen v��65    .  6G   �� *Employee.BirthPlace+6�   
  5v      $ Emšo��65    .  6D   �� *Employee.Emso+6�   
  5w      $ Davčna��65    .  6F   �� *Employee.TaxNumber+6�   
  5x      $ Št. zavarovanja��65    .  6N   �� *Employee.InsuranceNumber+6�   
  5z      $ Datum prijave��65   $  
dd.MM.yyyy��6|    .  6M   �� *WorkPeriod.Start+6�   
  5{      $ Datum odjave��65   $  
dd.MM.yyyy��6y    .  6L   �� *WorkPeriod.End+6�   
  5|      $ Trenutni projekt��65    .  6P   �� *Project.PointName+6�   ! 5~        5      $" MouseDoubleClick��6W   # . $ 60   �� *DoubleClickCommand+% 6t   �� $& emplist��$ SelectedItem�6�   5�      5�        5�      $��
Izposojeni��6!   $	 2��66   .��5�        -   5        $
 loanlist��5�       .  .   .  .   .  5�      �� *	Employees+ 5�      �� *SelectedEmployee+$ TwoWay=�$ PropertyChanged=� 5�      �� 5�      " 	 6     5�      �� 5�   "    6.   ��  �� *self+$# DataContext.WorkPeriod.Active� 06�   .a�5�   &   ��  � ��
 �r���6T   5�   #   5�       5�      
  5�      $
 Delo��65    .  6D   �� *" Employee.WorkPlace.WorkPlaceName+6�   
  5�      $ Priimek��65    .  6G   �� *Employee.LastName+ 5�        5�   "   $ Število zaposlenih: ��68   5�      
  5�      $	 Ime��65    .  6C   �� *Employee.Name+6�   
  5�      $ Rojen��65   $  
dd.MM.yyyy��6u    .  6E   �� *Employee.BirthDay+6�   
  5�      $ Rojen v��65    .  6G   �� *Employee.BirthPlace+6�   
  5�      $ Emšo��65    .  6D   �� *Employee.Emso+6�   
  5�      $ Davčna��65    .  6F   �� *Employee.TaxNumber+6�   
  5�      $ Št. zavarovanja��65    .  6N   �� *Employee.InsuranceNumber+6�   
  5�      $ Datum prijave��65   $  
dd.MM.yyyy��6|    .  6M   �� *WorkPeriod.Start+6�   
  5�      $ Datum odjave��65   $  
dd.MM.yyyy��6y    .  6L   �� *WorkPeriod.End+6�   
  5�      $ Trenutni projekt��65    .  6P   �� *Project.PointName+6�   ! 5�        5�      $" MouseDoubleClick��6W   # . $ 60   �� *DoubleClickCommand+% 6t   �� $& loanlist��$ SelectedItem�6�   5�      5�        5�      $��Po gradbiščih��6!   $	 3��68   .��5�        -   5        $
 alist��5�       .  .   .  .   .  5�      �� *EmployeesAway+ 5�      �� 5�      " 	 6     5�      �� 5�   "    6.   ��  �� *self+$# DataContext.WorkPeriod.Active� 06�   .a�5�   &   ��  � ��
 �r���6T   5�   #   5�       5�      
  5�      $ Priimek��65    .  6G   �� *
LastName+ 5�        5�   "   $ Število zaposlenih: ��68   5�      
  5�      $	 Ime��65    .  6C   �� *Name+6{   
  5�      $ Rojen��65   $  
dd.MM.yyyy��6l    .  6E   �� *
BirthDay+6�   
  5�      $ Rojen v��65    .  6G   �� *
BirthPlace+6�   
  5�      $ Emšo��65    .  6D   �� *Emso+6|   
  5�      $ Davčna��65    .  6F   �� *	TaxNumber+6�   
  5�      $ Št. zavarovanja��65    .  6N   �� *InsuranceNumber+6�   
  5�      $ Trenutni projekt��65    .  6P   �� *CurrentOccupancy.Name+6�   
  5�      $ število dni��65    .  6K   �� *CurrentOccupancy.TotalDays+6�   ! 5�        5�      $" MouseDoubleClick��6W   # . $ 60   �� *DoubleClickCommand+% 6t   �� $
& list��$ SelectedItem�6�   5�      5�        5�      $
��Doma��6!   $	 4��6/   .��5�        -   5        $
 hlist��5�       .  .   .  .   .  5�      �� *EmployeesHome+ 5�      �� 5�      " 	 6     5�      �� 5�   "    6.   ��  �� *self+$# DataContext.WorkPeriod.Active� 06�   .a�5�   &   ��  � ��
 �r���6T   5�   #   5�       5�      
  5�      $ Priimek��65    .  6G   �� *
LastName+ 5�        5�   "   $ Število zaposlenih: ��68   5�      
  5�      $	 Ime��65    .  6C   �� *Name+6{   
  5�      $ Rojen��65   $  
dd.MM.yyyy��6l    .  6E   �� *
BirthDay+6�   
  5�      $ Rojen v��65    .  6G   �� *
BirthPlace+6�   
  5�      $ Emšo��65    .  6D   �� *Emso+6|   
  5�      $ Davčna��65    .  6F   �� *	TaxNumber+6�   
  5�      $ Št. zavarovanja��65    .  6N   �� *InsuranceNumber+6�   
  5�      $ Trenutni projekt��65    .  6P   �� *CurrentOccupancy.Name+6�   
  5�      $ število dni��65    .  6K   �� *CurrentOccupancy.TotalDays+6�   ! 5�        5�      $" MouseDoubleClick��6W   # . $ 60   �� *DoubleClickCommand+% 6t   �� $
& list��$ SelectedItem�6�   5�      5�      ! 5�        5�      $" SelectionChanged��6W   # . $ 6(   �� *TabSelectionChangedCommand+% 6t   �� $	& tab��$ SelectedItem�6�   5     5     5     