   M S B A M L   `   `   `  ����H  DIsm.Departure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:Ism.Departure.ViewsIsm.Departure.Views  5      M IIsm.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nulliGclr-namespace:Ism.Infrastructure.Converters;assembly=Ism.InfrastructureIsm.Infrastructure.Converters 5      -clr-namespace:Ism.DepartureIsm.Departure  5      S<clr-namespace:Ism.Infrastructure;assembly=Ism.InfrastructureIsm.Infrastructure 5	      iGclr-namespace:Ism.Infrastructure.Converters;assembly=Ism.InfrastructureIsm.Infrastructure.Converters 5
      Y?clr-namespace:Ism.Infrastructure.Ui;assembly=Ism.InfrastructureIsm.Infrastructure.Ui 5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5      ,    &Ism.Departure.Views.EmployeeSelectList   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      3local!clr-namespace:Ism.Departure.Views     5      ]	converterGclr-namespace:Ism.Infrastructure.Converters;assembly=Ism.Infrastructure     5      0localrgnclr-namespace:Ism.Departure     5      Linf<clr-namespace:Ism.Infrastructure;assembly=Ism.Infrastructure     5	      ^
convertersGclr-namespace:Ism.Infrastructure.Converters;assembly=Ism.Infrastructure     5
      Nui?clr-namespace:Ism.Infrastructure.Ui;assembly=Ism.Infrastructure     5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5      P LPrism.Wpf, Version=6.3.0.0, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59$prismhttp://prismlibrary.com/  5      3system&clr-namespace:System;assembly=mscorlib  5      a	 ]Telerik.Windows.Data, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n
 jTelerik.Windows.Controls.GridView, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Documents.Core, Version=2018.2.619.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Input, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l hTelerik.Windows.Scheduling.Core, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7w sTelerik.Windows.Controls.DataVisualization, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.Navigation, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7o kTelerik.Windows.Controls.GanttView, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7j fTelerik.Windows.Controls.Data, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Qtelerik1http://schemas.telerik.com/2008/xaml/presentation
 	 
         5      !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel  .  � 	Resources 5       �� MergedDictionaries �� Source:  4Ism.Infrastructure.Converters.GridItemIndexConverter   ItemInexconverter; 
 5Telerik.Windows.Controls.GridView.GridViewSearchPanel �� 
TargetType  GW �� BasedOn �� Property �� Value�� 5   
    5      �� 5      $X Rpack://application:,,,/Ism.Infrastructure;component/Resources/BaseButtonStyle.xaml?�6%   6�   �� 5      $M Gpack://application:,,,/Ism.Infrastructure;component/Resources/Grid.xaml?�6%   6x   %�   &	        '       7    5      6K   �� 5      "  6   8   .�5      ��  � ��$ 20(�6-   6:   �� 5       � ��$
 Bold"�6/   6>   �� 5       � ��$ 50��6+   68   5      5      .��5!      �  � ColumnDefinitions 5#   
   �� 5$      $��* �6   6+   	 � RowDefinitions	 5'   
   �� 5(      $��* �6   6)   �� 5)      $
��Auto �6   6,   .+�5,   
   * 
 $Telerik.Windows.Controls.RadGridView  -   5        $
��list��5,      ; 
 5Telerik.Windows.Controls.GridView.GridViewDataControl
   AutoGenerateColumns
 .     ShowSearchPanel .    ShowGroupPanel .     IsFilteringAllowed .  * 	 $Telerik.Windows.Controls.DataControl   ItemsSource 6*   �� *List+   Columns 5-      5 
 /Telerik.Windows.Controls.GridViewCheckBoxColumn  5/         AutoSelectOnEdit . - 
 'Telerik.Windows.Controls.GridViewColumn   EditTriggers$ 	CellClick=�6I   6	 
 0Telerik.Windows.Controls.GridViewBoundColumnBase 	  Header$  ��6�    	  DataMemberBinding 6b   �� *
IsSelected+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6�   1
 
 +Telerik.Windows.Controls.GridViewDataColumn
  50      $ Priimek��6-      
IsReadOnly .  6?   �� *
LastName+51      
  52      $	 Ime��6-    .  6:   �� *Name+6r   
  53      $ Trenutna lokacija��6-    .  6H   �� *CurrentOccupancy.Name+6�   
  54      $ Od��6-    	  DataFormatString$ 
dd.MM.yyyy��6t    .  69   �� *CurrentOccupancy.Start.Date+6�   
  55      $ Št. dni��6-    .  6>   �� *CurrentOccupancy.TotalDays+6�   5  /Telerik.Windows.Controls.EventToCommandBehavior   EventBindings 5U      +  %Telerik.Windows.Controls.EventBinding  5V         	EventName$ MouseDoubleClick��6O      RaiseOnHandledEvents .    Command 6(   �� *DoubleClickCommand+   CommandParameter 6l   ��  �� ElementName$
 list�� �� Path$ SelectedItem�6�   5X      ]� 5Z   
   $��1��6   ��.  .+�5[      �� $
��Left=�6   $��	Prekliči��6+   ��6>   �� *CancelCommand+6c   �� 5\      $��Right=�6   $��Izberi��6,   ��6=   �� *SelectCommand+6b   5]      5^      5_      