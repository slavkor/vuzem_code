   M S B A M L   `   `   `  ���� H  DIsm.Departure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:Ism.Departure.ViewsIsm.Departure.Views  M IIsm.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nulliGclr-namespace:Ism.Infrastructure.Converters;assembly=Ism.InfrastructureIsm.Infrastructure.Converters -clr-namespace:Ism.DepartureIsm.Departure  S<clr-namespace:Ism.Infrastructure;assembly=Ism.InfrastructureIsm.Infrastructure iGclr-namespace:Ism.Infrastructure.Converters;assembly=Ism.InfrastructureIsm.Infrastructure.Converters Y?clr-namespace:Ism.Infrastructure.Ui;assembly=Ism.InfrastructureIsm.Infrastructure.Ui O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem ,    &Ism.Departure.Views.EmployeeSelectList   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     3local!clr-namespace:Ism.Departure.Views     ]	converterGclr-namespace:Ism.Infrastructure.Converters;assembly=Ism.Infrastructure     0localrgnclr-namespace:Ism.Departure     Linf<clr-namespace:Ism.Infrastructure;assembly=Ism.Infrastructure     ^
convertersGclr-namespace:Ism.Infrastructure.Converters;assembly=Ism.Infrastructure     Nui?clr-namespace:Ism.Infrastructure.Ui;assembly=Ism.Infrastructure     c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  P LPrism.Wpf, Version=6.3.0.0, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59$prismhttp://prismlibrary.com/  3system&clr-namespace:System;assembly=mscorlib  a	 ]Telerik.Windows.Data, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n
 jTelerik.Windows.Controls.GridView, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7j fTelerik.Windows.Controls.Data, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Input, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7w sTelerik.Windows.Controls.DataVisualization, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Itelerik1http://schemas.telerik.com/2008/xaml/presentation 	 
     !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel  .  � 	Resources  �� MergedDictionaries �� Source:  4Ism.Infrastructure.Converters.GridItemIndexConverter   ItemInexconverter; 
 5Telerik.Windows.Controls.GridView.GridViewSearchPanel �� 
TargetType  GW �� BasedOn �� Property �� Value��  �� $X Rpack://application:,,,/Ism.Infrastructure;component/Resources/BaseButtonStyle.xaml?��� $M Gpack://application:,,,/Ism.Infrastructure;component/Resources/Grid.xaml?�%s   &	        '       7    �� "  8   .���  � ��$ 20(���  � ��$
 Bold"���  � ��$ 50��.���  � ColumnDefinitions �� $��* �	 � RowDefinitions	 �� $��* ��� $
��Auto �.+�* 
 $Telerik.Windows.Controls.RadGridView  -   $
��list��; 
 5Telerik.Windows.Controls.GridView.GridViewDataControl
   AutoGenerateColumns
 .     ShowSearchPanel .    ShowGroupPanel .     IsFilteringAllowed .  * 	 $Telerik.Windows.Controls.DataControl   ItemsSource �� *List+   Columns 5 
 /Telerik.Windows.Controls.GridViewCheckBoxColumn     AutoSelectOnEdit . - 
 'Telerik.Windows.Controls.GridViewColumn   EditTriggers$ 	CellClick=�6	 
 0Telerik.Windows.Controls.GridViewBoundColumnBase 	  Header$  �� 	  DataMemberBinding �� *
IsSelected+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�1
 
 +Telerik.Windows.Controls.GridViewDataColumn
  $ Priimek��   
IsReadOnly .  �� *
LastName+
  $	 Ime�� .  �� *Name+
  $ Trenutna lokacija�� .  �� *CurrentOccupancy.Name+
  $ Od�� 	  DataFormatString$ 
dd.MM.yyyy�� .  �� *CurrentOccupancy.Start.Date+
  $ Št. dni�� .  �� *CurrentOccupancy.TotalDays+5  /Telerik.Windows.Controls.EventToCommandBehavior   EventBindings +  %Telerik.Windows.Controls.EventBinding     	EventName$ MouseDoubleClick��   RaiseOnHandledEvents .    Command �� *DoubleClickCommand+   CommandParameter ��  �� ElementName$
 list�� �� Path$ SelectedItem�]� $��1����.  .+��� $
��Left=�$��	Prekliči������ *CancelCommand+�� $��Right=�$��Izberi������ *SelectCommand+