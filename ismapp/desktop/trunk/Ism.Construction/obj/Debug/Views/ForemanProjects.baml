   M S B A M L   `   `   `  ����K  GIsm.Construction, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:Ism.Construction.ViewsIsm.Construction.Views  5      M IIsm.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS<clr-namespace:Ism.Infrastructure;assembly=Ism.InfrastructureIsm.Infrastructure 5      ,    &Ism.Construction.Views.ForemanProjects   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6local$clr-namespace:Ism.Construction.Views     5      Linf<clr-namespace:Ism.Infrastructure;assembly=Ism.Infrastructure     5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5      P LPrism.Wpf, Version=6.3.0.0, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59$prismhttp://prismlibrary.com/  5	      n jTelerik.Windows.Controls.GridView, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k	 gTelerik.Windows.Controls.Chart, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a
 ]Telerik.Windows.Data, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Documents.Core, Version=2019.1.215.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Input, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l hTelerik.Windows.Scheduling.Core, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7| xTelerik.Windows.Controls.FixedDocumentViewersUI, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7z vTelerik.Windows.Controls.FixedDocumentViewers, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.Navigation, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7o kTelerik.Windows.Controls.GanttView, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7j fTelerik.Windows.Controls.Data, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7w sTelerik.Windows.Controls.DataVisualization, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Wtelerik1http://schemas.telerik.com/2008/xaml/presentation  	 
           5
      !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel  .    UcStyle#��[  5       � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $X Rpack://application:,,,/Ism.Infrastructure;component/Resources/BaseButtonStyle.xaml?�6%   6�   %    5      .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   6)   �� 5      $
��Auto �6   6,   .+�5   
   )  #Telerik.Windows.Controls.RadListBox  -   5          Name$
 list��5   *   $��0��6   
���   �$��1q�6m   $��10q�6�   $
��Auto=�5      ��5   8   �� *
Projects+>  8Telerik.Windows.Controls.Primitives.ItemsControlSelector   SelectedItem 5      �� *SelectedProject+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=���5      �� 5      .R�5      +  %Telerik.Windows.Controls.RadWrapPanel  	   Orientation$	 
Horizontal=�6+   5       ��5"      �� 5#      .R�5$      �� $��1q�6   
���  ��._�5%      � 
 � ColumnDefinitions
 5&      �� 5'   "    z� SharedSizeGroup$ ll��63   6J   �� 5(   "   65    5*      �� 5+   "   $
��Auto �60   �� 5,   "   $
��Auto �60   �� 5-   "   $
��Auto �60   �� 5.   "   $
��Auto �60   �� 5/   "   $
��Auto �60   �� 50   "   $
��Auto �60   .+�52      �� $��0��6(   $��0��68   $��Gradbišče��6E   6X   �� 53      $��0��6(   $��1��68   $��Stranka��6E   6V   �� 54      $��0��6(   $��2��68   $��Projekt��6E   6V   �� 55      $��0��6(   $��3��68   $��Številka projekta��6E   6`   �� 56      $��0��6(   $��4��68   $��Začetek��6E   6V   �� 57      $��0��6(   $��5��68   $��Konec��6E   6T   �� 58      $��1��6(   $��0��68   ��6F   ��  �� ElementName$ csite�� �� Path$
 Text��� 59      $��1��6(   $��1��68   ��6F   �� $
 cust��$
 Text��� 5:      $��1��6(   $��2��68   ��6F   �� *Name+�� 5;      $��1��6(   $��3��68   ��6F   �� *Externalnumber+�� 5<      $��1��6(   $��4��68   ��6F   �� *
Start.Date+ �� StringFormat$ 
dd.MM.yyyy���� 5=      $��1��6(   $��5��68   ��6F   �� *
End.Date+$ 
dd.MM.yyyy��5>      5?      5@      ��5B      �� 5C       �� 
TargetType-  'Telerik.Windows.Controls.RadListBoxItem"  6   .�5D      �� 2  ,Telerik.Windows.Controls.Primitives.ListItem �� Property �@ 
IsSelected �� Value 63   �� *
IsSelected+$ TwoWay=�$ PropertyChanged=�6�   5E      5G      �� 5I   
   $��1��6   $y�	Collapsed=�6"   .+�5J      �� -   5        $��csite��5J      ��6%   �� * SiteData.ConstructionSite.Name+6W   �� 5K      -   5        $
��cust��5K      ��6$   �� *)'SiteData.ConstructionSite.Customer.Name+6_   5L      5M      5N      