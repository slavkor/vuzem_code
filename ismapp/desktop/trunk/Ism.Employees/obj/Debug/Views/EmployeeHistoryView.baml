   M S B A M L   `   `   `  ����H  DIsm.Employees, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:Ism.Employees.ViewsIsm.Employees.Views  5      -clr-namespace:Ism.EmployeesIsm.Employees  5      M IIsm.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS<clr-namespace:Ism.Infrastructure;assembly=Ism.InfrastructureIsm.Infrastructure 5      iGclr-namespace:Ism.Infrastructure.Converters;assembly=Ism.InfrastructureIsm.Infrastructure.Converters 5	      Y?clr-namespace:Ism.Infrastructure.Ui;assembly=Ism.InfrastructureIsm.Infrastructure.Ui 5
      G CIsm.Controls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA0clr-namespace:Ism.Controls;assembly=Ism.ControlsIsm.Controls 5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5      1&clr-namespace:System;assembly=mscorlibSystem 5      -    'Ism.Employees.Views.EmployeeHistoryView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      3local!clr-namespace:Ism.Employees.Views     5      0localrgnclr-namespace:Ism.Employees     5      Linf<clr-namespace:Ism.Infrastructure;assembly=Ism.Infrastructure     5      ^
convertersGclr-namespace:Ism.Infrastructure.Converters;assembly=Ism.Infrastructure     5	      Nui?clr-namespace:Ism.Infrastructure.Ui;assembly=Ism.Infrastructure     5
      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5      P	 LPrism.Wpf, Version=6.3.0.0, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59$prismhttp://prismlibrary.com/ 	 5      ;ctrl0clr-namespace:Ism.Controls;assembly=Ism.Controls 	 5      3system&clr-namespace:System;assembly=mscorlib 	 5      5xcdg*http://schemas.xceed.com/wpf/xaml/datagrid 	 5      4xctk)http://schemas.xceed.com/wpf/xaml/toolkit 	 5      0sys&clr-namespace:System;assembly=mscorlib 	 5      a
 ]Telerik.Windows.Data, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n jTelerik.Windows.Controls.GridView, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Documents.Core, Version=2019.1.215.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Input, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.Navigation, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Itelerik1http://schemas.telerik.com/2008/xaml/presentation 
      5      ! 	 Prism.Mvvm.ViewModelLocator    AutoWireViewModel  . .��5      �  � ColumnDefinitions 5   
   �� 5      $��* �6   6+   �� 5      $��* �6   6+   .+�5   
   *  $Telerik.Windows.Controls.RadGridView  ;  5Telerik.Windows.Controls.GridView.GridViewDataControl   AutoGenerateColumns .     ShowGroupPanel .     ShowColumnFooters . 8  2Telerik.Windows.Controls.GridView.BaseItemsControl   GroupRenderMode$
 Flat=�5      $��0��5      * 
 $Telerik.Windows.Controls.DataControl   ItemsSource 5      �� *	History+ �� Mode$ OneWay=�   Columns 5       1  +Telerik.Windows.Controls.GridViewDataColumn  5!      6  0Telerik.Windows.Controls.GridViewBoundColumnBase	   Header$
	 Opis��6S   -  'Telerik.Windows.Controls.GridViewColumn
   
IsReadOnly
 .    DataMemberBinding 6-   �� *	Company+6a     5"      $	 Od��6}      DataFormatString$ 
dd.MM.yyyy��6�   
 .  6-   �� *From+
 �� Now �� TargetNullValue# Z 6�     5#      $	 Do��6{   $ 
dd.MM.yyyy��6�   
 .  6-   �� *To+# Z 6�   5&        5'   
    .   .   . $
 Flat=�5+      $��1��5,       5'      �� *WorkHistory+$ OneWay=� 5-        5.      $
	 Opis��6W   
 .  6-   �� *Destination+6e     5/      $	 Od��6}   $ 
dd.MM.yyyy��6�   
 .  6-   �� *From+# Z 6�     50      $	 Do��6{   $ 
dd.MM.yyyy��6�   
 .  6-   �� *To+# Z 6�   53      54      55      