   M S B A M L   `   `   `  ����H  DIsm.Departure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:Ism.Departure.ViewsIsm.Departure.Views  5      -clr-namespace:Ism.DepartureIsm.Departure  5      M IIsm.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS<clr-namespace:Ism.Infrastructure;assembly=Ism.InfrastructureIsm.Infrastructure 5      iGclr-namespace:Ism.Infrastructure.Converters;assembly=Ism.InfrastructureIsm.Infrastructure.Converters 5	      Y?clr-namespace:Ism.Infrastructure.Ui;assembly=Ism.InfrastructureIsm.Infrastructure.Ui 5
      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5      5    /Ism.Departure.Views.OriginDestinationSelectList   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      3local!clr-namespace:Ism.Departure.Views     5      0localrgnclr-namespace:Ism.Departure     5      Linf<clr-namespace:Ism.Infrastructure;assembly=Ism.Infrastructure     5      ^
convertersGclr-namespace:Ism.Infrastructure.Converters;assembly=Ism.Infrastructure     5	      Nui?clr-namespace:Ism.Infrastructure.Ui;assembly=Ism.Infrastructure     5
      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5      P LPrism.Wpf, Version=6.3.0.0, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59$prismhttp://prismlibrary.com/  5      3system&clr-namespace:System;assembly=mscorlib  5      a	 ]Telerik.Windows.Data, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n
 jTelerik.Windows.Controls.GridView, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7j fTelerik.Windows.Controls.Data, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Input, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7w sTelerik.Windows.Controls.DataVisualization, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Itelerik1http://schemas.telerik.com/2008/xaml/presentation 	 
     5      !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel  .  � 	Resources 5       �� MergedDictionaries �� Source   VisibilityConverter; 
 5Telerik.Windows.Controls.GridView.GridViewSearchPanel �� 
TargetType  GW �� BasedOn �� Property �� Value�� 5   
    5      �� 5      $X Rpack://application:,,,/Ism.Infrastructure;component/Resources/BaseButtonStyle.xaml?�6%   6�   �� 5      $M Gpack://application:,,,/Ism.Infrastructure;component/Resources/Grid.xaml?�6%   6x   %�   &	        '       7  �� 5      6I   �� 5      "  6   8   .�5      ��  � ��$ 20(�6-   6:   �� 5       � ��$
 Bold"�6/   6>   �� 5       � ��$ 50��6+   68   5      5      .��5      �  � RowDefinitions 5!   
   �� 5"      $��* �6   6)   �� 5#      $
��Auto �6   6,   .+�5&   
   * 
 $Telerik.Windows.Controls.RadGridView  -   5        $
��list��5*      $��0��5&      ; 
 5Telerik.Windows.Controls.GridView.GridViewDataControl	   ShowSearchPanel	 . 
   AutoGenerateColumns
 .     ShowGroupPanel .     SelectionMode$ Single=�5,   6   #   CanUserSearchInHiddenColumns . * 	 $Telerik.Windows.Controls.DataControl   ItemsSource 5'      �� *List+   SelectedItem 5+      �� *
Selected+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�   Columns 5.      1 
 +Telerik.Windows.Controls.GridViewDataColumn  5/      6 
 0Telerik.Windows.Controls.GridViewBoundColumnBase   Header$ Projekt��6-   - 
 'Telerik.Windows.Controls.GridViewColumn   
IsReadOnly .    DataMemberBinding 6?   �� *	PointName+6|     50      $ Začetek��6-      DataFormatString$ 
dd.MM.yyyy��6h    .  6?   �� *
Start.Date+6�     51      $ Konec��6-   $ 
dd.MM.yyyy��6d    .  6=   �� *
End.Date+6�     52      $  ��6-    .    	IsVisible .   68   �� *ProjectNumber+6�     53      $  ��6-    .  .   68   �� *ExternalNumber+6�   55      ]� 57   
   $��1��6   ��.  y�67   �� *
IsSelect+ �� 	Converter# [   �� FallbackValue$ 	Collapsed��.+�58      �� $
��Left=�6   $��	Prekliči��6+   ��6>   �� *CancelCommand+��6`   Z� 6~   �� 59      $��Right=�6   $��Izberi��6,   ��6=   �� *SelectCommand+��6_   ��  �� ElementName$
 list�� �� Path$ SelectedItem�6�   5:      5<      5=      