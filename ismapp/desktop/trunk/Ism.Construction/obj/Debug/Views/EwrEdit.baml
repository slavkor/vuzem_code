   M S B A M L   `   `   `  ����K  GIsm.Construction, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:Ism.Construction.ViewsIsm.Construction.Views  5      3clr-namespace:Ism.ConstructionIsm.Construction  5      M IIsm.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS<clr-namespace:Ism.Infrastructure;assembly=Ism.InfrastructureIsm.Infrastructure 5      Y?clr-namespace:Ism.Infrastructure.Ui;assembly=Ism.InfrastructureIsm.Infrastructure.Ui 5	      iGclr-namespace:Ism.Infrastructure.Converters;assembly=Ism.InfrastructureIsm.Infrastructure.Converters 5
      G CIsm.Controls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA0clr-namespace:Ism.Controls;assembly=Ism.ControlsIsm.Controls 5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5      $    Ism.Construction.Views.EwrEdit   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6local$clr-namespace:Ism.Construction.Views     5      3localrgnclr-namespace:Ism.Construction     5      Linf<clr-namespace:Ism.Infrastructure;assembly=Ism.Infrastructure     5      Nui?clr-namespace:Ism.Infrastructure.Ui;assembly=Ism.Infrastructure     5	      ^
convertersGclr-namespace:Ism.Infrastructure.Converters;assembly=Ism.Infrastructure     5
      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5      P	 LPrism.Wpf, Version=6.3.0.0, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59$prismhttp://prismlibrary.com/ 	 5      ;ctrl0clr-namespace:Ism.Controls;assembly=Ism.Controls 	 5      3system&clr-namespace:System;assembly=mscorlib 	 5      n
 jTelerik.Windows.Controls.GridView, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Chart, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a ]Telerik.Windows.Data, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Documents.Core, Version=2019.1.215.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Input, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l hTelerik.Windows.Scheduling.Core, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7| xTelerik.Windows.Controls.FixedDocumentViewersUI, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7z vTelerik.Windows.Controls.FixedDocumentViewers, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.Navigation, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7o kTelerik.Windows.Controls.GanttView, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7j fTelerik.Windows.Controls.Data, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7w sTelerik.Windows.Controls.DataVisualization, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Wtelerik1http://schemas.telerik.com/2008/xaml/presentation 
             5      ! 	 Prism.Mvvm.ViewModelLocator    AutoWireViewModel  .    UcStyle#��[  5       � 	Resources 5       �� MergedDictionaries �� Source  addPath �� Stretch  
removePath  WS �� 
TargetType  CustomWindowStyle �� BasedOn �� Property �� Value  True 	 False8  2Ism.Infrastructure.Converters.EnumBooleanConverter  EnumBooleanConverterF  @Ism.Infrastructure.Converters.SecurityScopeToVisibilityConverter  SecurityToVisbility�� 5   
    5      �� 5      $X Rpack://application:,,,/Ism.Infrastructure;component/Resources/BaseButtonStyle.xaml?�6%   6�   �� 5      $Y Spack://application:,,,/Ism.Infrastructure;component/Resources/BaseTextBoxStyle.xaml?�6%   6�   �� 5      $M Gpack://application:,,,/Ism.Infrastructure;component/Resources/Grid.xaml?�6%   6x   %�  &	       &	 �    &	 s    7  &	 �    &	     &	 )    &	 <    F� 5      $
 Fill=�6#   ���	#FF000000������S��р���р��S��2
�S��2
��рu��рu�2
��2
�2
��2
�uр��uр��2
��S�2
��S�����@x}р�C@x}р�C���� ����� �@x}р��@x}р������� ������ �@�4р�C@�4р�C������������@x}����S�@x}�S�@x} H�р�� H�р���S�@�4�S�@�4 c�р�� c�р���2
�@x}�2
�@x} c�р� c�р��S6C   F� 5      $��31.6666��6&   $��31.6667��66   $
 Fill=�6G   ���	#FF000000|��������<R��C�Z��@��=R��m5�����O���C��m5��Ѥ���n5��C0P������n5�H<R�����Z���CѬ<R\�������<R6g   �� 5       " �6   8   .�5!      ��  � ��$	 400��6-   6;   �� 5"       � ��$	 400��6.   6<   5#      ��5$      True��60   ��5%      	False��62     5&      6M     5'      6Z   5(      .��5*      � 	 � RowDefinitions	 5,   
   �� 5-      $��* �6   6)   �� 5.      $
��Auto �6   6,   
 � ColumnDefinitions
 50   
   �� 51      $��* �6   6+   �� 52      $��* �6   6+   .+�55   
   � $��0��6   $��0��6   
 56      �� 57       z� SharedSizeGroup$	 lbl��6#   $
��Auto �69   6H   �� 58      6%   	 5;      �� 5<      $
��Auto �6    60   �� 5=      $
��Auto �6    60   �� 5>      $
��Auto �6    60   �� 5?      $
��Auto �6    60   �� 5@      $
��Auto �6    60   �� 5A      $��* �6    6-   �� 5B      $��* �6    6-   .+�5E      �� $��0��6   $��0��6$    � IsSharedSizeScope . $��Datum��6O   6a   �� 5F      $��0��6   $��1��6$    . $��	Številka��6O   6d   �� 5G      $��0��6   $��2��6$    . $��Zunanja številka��6O   6l   �� 5H      $��0��6   $��3��6$    . $��Število ur��6O   6f   �� 5I      $��0��6   $��4��6$    . $��Strošek materiala��6O   6m   �� 5J      $��0��6   $��5��6$    . $
��Opis��6O   6`   ,  &Telerik.Windows.Controls.RadDatePicker  5L      $��0��6$   $��1��61   0  *Telerik.Windows.Controls.RadDateTimePicker   SelectedValue 6A   �� *
Ewr.Date+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=��� 5M      $��1��6   $��1��6&    .  �� 
IsReadOnly . {�.  ��6Q   �� *
Ewr.Number+$ PropertyChanged=�$ TwoWay=�6�   �� 5N      $��1��6   $��2��6&    . ��6Q   �� *Ewr.ExternalNumber+$ PropertyChanged=�$ TwoWay=�6�   �� 5O      $��1��6   $��3��6&    . ��6Q   �� *	Ewr.Hours+$ PropertyChanged=�$ TwoWay=�6�   �� 5P      $��1��6   $��4��6&    . ��6Q   �� *Ewr.MaterialCosts+$ PropertyChanged=�$ TwoWay=�6�   �� 5Q       �� TextWrapping$
 Wrap=�6    �� AcceptsReturn . " �� VerticalScrollBarVisibility$
 Auto=�6A   $��1��6d   $��5��6t    . ��6�   �� *Ewr.Description+$ PropertyChanged=�$ TwoWay=�6�   � 5S      $��6��6   $��0��6    $��2��60   
 5T      �� 5U      6)   �� 5V      6)   �� 5W      6)   �� 5X      6)   .+�5[      �� $��1q�6   
���   � �� Padding$ 2q�6A   $��0��6M   ._�5\      � 	 5]      �� 5^      $
��Auto �6,   6<   �� 5_      $��* �6,   69   .+�5a      ]� $��0��6$   ��.  .+�5b      �� $
��Left=�6(   $��
Delovodja ��6>   6R   ,  &Telerik.Windows.Controls.RadPathButton  5c      $
��Left=�64   $��30��6J   $��30��6U   $��foreman��6�   ��6a   �� *EmployeeSelectCommand+   PathGeometry 6�   ��  �� Source# [  �� Path$
 Data�   	PathStyle 5d   "   �� 5e   &   " F�6,   .�5f   *   ��  � ��
 � � �6Q   �� 5g   *    �@��Stretch$ UniformToFill=�6D   �� 5h   *    � ��$ Center=�6P   �� 5i   *    � ��$ Center=�6N   5j   '   5l        5m      $
��Left=�64   $��30��6J   $��30��6U   $��foreman��6�   ��6a   �� *ForemanRemoveCommand+ 6�   �� # [ $
 Data� 5n   "   �� 5o   &   " F�6,   .�5p   *   ��  � ��
 �  ��6O   �� 5q   *    �@��Stretch$ UniformToFill=�6D   �� 5r   *    � ��$ Center=�6P   �� 5s   *    � ��$ Center=�6N   5t   '   5v      5w      * 
 $Telerik.Windows.Controls.RadGridView  5x      $��1��6.   ; 
 5Telerik.Windows.Controls.GridView.GridViewDataControl   ShowGroupPanel .     IsFilteringAllowed .     AutoGenerateColumns .  *	  $Telerik.Windows.Controls.DataControl 	  ItemsSource 6�   �� *
ForeMans+$ TwoWay=�$ PropertyChanged=� 	  SelectedItem 6�   �� *SelectedForeman+$ TwoWay=�$ PropertyChanged=�   Columns 5y      1
 
 +Telerik.Windows.Controls.GridViewDataColumn
  5z   "   6 
 0Telerik.Windows.Controls.GridViewBoundColumnBase   Header$ 	Delovodja��6=   - 
 'Telerik.Windows.Controls.GridViewColumn    
IsReadOnly  . !   DataMemberBinding! 6Q   �� "   AggregateFunctions" 5{   &   (  "Telerik.Windows.Data.CountFunction  5|   *   ,  &Telerik.Windows.Data.AggregateFunction#   Caption$# Število izvajalcev: ��6@   5~   #   5�      5�      5�      �� 5�      $��1q�6   
���   �$ 2q�6A   $��1��6M   ._�5�      � $��1��6   	 5�      �� 5�      $
��Auto �6,   6<   �� 5�      $��* �6,   69   .+�5�      ]� $��0��6$   ��.  .+�5�      �� $
��Left=�6(   $��Varilec ��6>   6P     5�      $
��Left=�64   $��30��6J   $��30��6U   $��welder��6�   ��6a   �� *EmployeeSelectCommand+ 6�   �� # [ $
 Data� 5�   "   �� 5�   &   " F�6,   .�5�   *   ��  � ��
 � � �6Q   �� 5�   *    �@��Stretch$ UniformToFill=�6D   �� 5�   *    � ��$ Center=�6P   �� 5�   *    � ��$ Center=�6N   5�   '   5�        5�      $
��Left=�64   $��30��6J   $��30��6U   $��welder��6�   ��6a   �� *WelderRemoveCommand+ 6�   �� # [ $
 Data� 5�   "   �� 5�   &   " F�6,   .�5�   *   ��  � ��
 �  ��6O   �� 5�   *    �@��Stretch$ UniformToFill=�6D   �� 5�   *    � ��$ Center=�6P   �� 5�   *    � ��$ Center=�6N   5�   '   5�      5�        5�      $��1��6.    .   .   .   6�   �� *	Welders+$ TwoWay=�$ PropertyChanged=� 6�   �� *SelectedWelder+$ TwoWay=�$ PropertyChanged=� 5�      
  5�   "   $ Varilec��6=     . ! 6O   �� " 5�   &     5�   *   $# Število izvajalcev: ��6@   5�   #   5�      5�      5�      �� 5�      $��1q�6   
���   �$ 2q�6A   $��2��6M   ._�5�      � $��2��6   	 5�      �� 5�      $
��Auto �6,   6<   �� 5�      $��* �6,   69   .+�5�      ]� $��0��6$   ��.  .+�5�      �� $
��Left=�6(   $��Monter ��6>   6O     5�      $
��Left=�64   $��30��6J   $��30��6U   $��	installer��6�   ��6a   �� *EmployeeSelectCommand+ 6�   �� # [ $
 Data� 5�   "   �� 5�   &   " F�6,   .�5�   *   ��  � ��
 � � �6Q   �� 5�   *    �@��Stretch$ UniformToFill=�6D   �� 5�   *    � ��$ Center=�6P   �� 5�   *    � ��$ Center=�6N   5�   '   5�        5�      $
��Left=�64   $��30��6J   $��30��6U   $��	installer��6�   ��6a   �� *InstallerRemoveCommand+ 6�   �� # [ $
 Data� 5�   "   �� 5�   &   " F�6,   .�5�   *   ��  � ��
 �  ��6O   �� 5�   *    �@��Stretch$ UniformToFill=�6D   �� 5�   *    � ��$ Center=�6P   �� 5�   *    � ��$ Center=�6N   5�   '   5�      5�        5�      $��1��6.    .   .   .   6�   �� *
Installers+$ TwoWay=�$ PropertyChanged=� 6�   �� *SelectedInstaller+$ TwoWay=�$ PropertyChanged=� 5�      
  5�   "   $ Monter��6=     . ! 6N   �� " 5�   &     5�   *   $# Število izvajalcev: ��6@   5�   #   5�      5�      5�      �� 5�      $��1q�6   
���   �$ 2q�6A   $��3��6M   ._�5�      � $��3��6   	 5�      �� 5�      $
��Auto �6,   6<   �� 5�      $��* �6,   69   .+�5�      ]� $��0��6$   ��.  .+�5�      �� $
��Left=�6(   $��Cevar ��6>   6N     5�      $
��Left=�64   $��30��6J   $��30��6U   $��piper��6�   ��6a   �� *EmployeeSelectCommand+ 6�   �� # [ $
 Data� 5�   "   �� 5�   &   " F�6,   .�5�   *   ��  � ��
 � � �6Q   �� 5�   *    �@��Stretch$ UniformToFill=�6D   �� 5�   *    � ��$ Center=�6P   �� 5�   *    � ��$ Center=�6N   5�   '   5�        5�      $
��Left=�64   $��30��6J   $��30��6U   $��piper��6�   ��6a   �� *PiperRemoveCommand+ 6�   �� # [ $
 Data� 5�   "   �� 5�   &   " F�6,   .�5�   *   ��  � ��
 �  ��6O   �� 5�   *    �@��Stretch$ UniformToFill=�6D   �� 5�   *    � ��$ Center=�6P   �� 5�   *    � ��$ Center=�6N   5�   '   5�      5�        5�      $��1��6.    .   .   .   6�   �� *Pipers+$ TwoWay=�$ PropertyChanged=� 6�   �� *SelectedPiper+$ TwoWay=�$ PropertyChanged=� 5�      
  5�   "   $ Cevar��6=     . ! 6M   �� " 5�   &     5�   *   $# Število izvajalcev: ��6@   5�   #   5�      5�      5�      5�      5�      � 5   
   $��0��6   $��1��6   .+�5     � 	 5     �� 5     $��* �6$   61   �� 5     $
��Auto �6$   64   .+�5     �� $��0��6!   "   Ism.Construction.RegionNames$   EwrDocumentsRegion! 	 Prism.Regions.RegionManager%   
RegionName#% Z$ 6.   6�   �� 5     $��1��6!   &   EwrExSiteManagerRegion#% Z& 6.   6�   5     5	     ]� 5  
   $��1��6   $��0��6"   $��2��62   ��.  .+�5     �� $
��Left=�6   $��	Prekliči��6+   ��6>   �� *CancelCommand+�� 5     $��Right=�6   $��Shrani��6,   #��[ 6]   ��6=   �� *SaveCommand+6�   5     5     5     