   M S B A M L   `   `   `  ���� K  GIsm.Construction, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:Ism.Construction.ViewsIsm.Construction.Views  3clr-namespace:Ism.ConstructionIsm.Construction  M IIsm.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS<clr-namespace:Ism.Infrastructure;assembly=Ism.InfrastructureIsm.Infrastructure Y?clr-namespace:Ism.Infrastructure.Ui;assembly=Ism.InfrastructureIsm.Infrastructure.Ui iGclr-namespace:Ism.Infrastructure.Converters;assembly=Ism.InfrastructureIsm.Infrastructure.Converters G CIsm.Controls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA0clr-namespace:Ism.Controls;assembly=Ism.ControlsIsm.Controls O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem $    Ism.Construction.Views.EwrEdit   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     6local$clr-namespace:Ism.Construction.Views     3localrgnclr-namespace:Ism.Construction     Linf<clr-namespace:Ism.Infrastructure;assembly=Ism.Infrastructure     Nui?clr-namespace:Ism.Infrastructure.Ui;assembly=Ism.Infrastructure     ^
convertersGclr-namespace:Ism.Infrastructure.Converters;assembly=Ism.Infrastructure     c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  P	 LPrism.Wpf, Version=6.3.0.0, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59$prismhttp://prismlibrary.com/ 	 ;ctrl0clr-namespace:Ism.Controls;assembly=Ism.Controls 	 3system&clr-namespace:System;assembly=mscorlib 	 n
 jTelerik.Windows.Controls.GridView, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Chart, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a ]Telerik.Windows.Data, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Documents.Core, Version=2019.1.215.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Input, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l hTelerik.Windows.Scheduling.Core, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7| xTelerik.Windows.Controls.FixedDocumentViewersUI, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7z vTelerik.Windows.Controls.FixedDocumentViewers, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.Navigation, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7o kTelerik.Windows.Controls.GanttView, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7j fTelerik.Windows.Controls.Data, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7w sTelerik.Windows.Controls.DataVisualization, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Wtelerik1http://schemas.telerik.com/2008/xaml/presentation 
             ! 	 Prism.Mvvm.ViewModelLocator    AutoWireViewModel  .    UcStyle#��[   � 	Resources  �� MergedDictionaries �� Source  addPath �� Stretch  
removePath  WS �� 
TargetType  CustomWindowStyle �� BasedOn �� Property �� Value  True 	 False8  2Ism.Infrastructure.Converters.EnumBooleanConverter  EnumBooleanConverterF  @Ism.Infrastructure.Converters.SecurityScopeToVisibilityConverter  SecurityToVisbility��  �� $X Rpack://application:,,,/Ism.Infrastructure;component/Resources/BaseButtonStyle.xaml?��� $Y Spack://application:,,,/Ism.Infrastructure;component/Resources/BaseTextBoxStyle.xaml?��� $M Gpack://application:,,,/Ism.Infrastructure;component/Resources/Grid.xaml?�%�  &	       &	 �    &	 C    7  &	 �    &	 �    &	 �    &	 �    F� $
 Fill=����	#FF000000������S��р���р��S��2
�S��2
��рu��рu�2
��2
�2
��2
�uр��uр��2
��S�2
��S�����@x}р�C@x}р�C���� ����� �@x}р��@x}р������� ������ �@�4р�C@�4р�C������������@x}����S�@x}�S�@x} H�р�� H�р���S�@�4�S�@�4 c�р�� c�р���2
�@x}�2
�@x} c�р� c�р��SF� $��31.6666��$��31.6667��$
 Fill=����	#FF000000|��������<R��C�Z��@��=R��m5�����O���C��m5��Ѥ���n5��C0P������n5�H<R�����Z���CѬ<R\�������<R�� " �8   .���  � ��$	 400����  � ��$	 400����True����	False��    .��� 	 � RowDefinitions	 �� $��* ��� $
��Auto �
 � ColumnDefinitions
 �� $��* ��� $��* �.+�� $��0��$��0��
 ��  z� SharedSizeGroup$	 lbl��$
��Auto ��� 	 �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $��* ��� $��* �.+��� $��0��$��0�� � IsSharedSizeScope . $��Datum���� $��0��$��1�� . $��	Številka���� $��0��$��2�� . $��Zunanja številka���� $��0��$��3�� . $��Število ur���� $��0��$��4�� . $��Strošek materiala���� $��0��$��5�� . $
��Opis��,  &Telerik.Windows.Controls.RadDatePicker  $��0��$��1��0  *Telerik.Windows.Controls.RadDateTimePicker   SelectedValue �� *
Ewr.Date+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=��� $��1��$��1�� .  �� 
IsReadOnly . {�.  ���� *
Ewr.Number+$ PropertyChanged=�$ TwoWay=��� $��1��$��2�� . ���� *Ewr.ExternalNumber+$ PropertyChanged=�$ TwoWay=��� $��1��$��3�� . ���� *	Ewr.Hours+$ PropertyChanged=�$ TwoWay=��� $��1��$��4�� . ���� *Ewr.MaterialCosts+$ PropertyChanged=�$ TwoWay=���  �� TextWrapping$
 Wrap=� �� AcceptsReturn . " �� VerticalScrollBarVisibility$
 Auto=�$��1��$��5�� . ���� *Ewr.Description+$ PropertyChanged=�$ TwoWay=�� $��6��$��0��$��2��
 �� �� �� �� .+��� $��1q�
���   � �� Padding$ 2q�$��0��._�� 	 �� $
��Auto ��� $��* �.+�]� $��0����.  .+��� $
��Left=�$��
Delovodja ��,  &Telerik.Windows.Controls.RadPathButton  $
��Left=�$��30��$��30��$��foreman������ *EmployeeSelectCommand+   PathGeometry ��  �� Source# [  �� Path$
 Data�   	PathStyle �� " F�.���  � ��
 � � ���  �@��Stretch$ UniformToFill=���  � ��$ Center=���  � ��$ Center=�  $
��Left=�$��30��$��30��$��foreman������ *ForemanRemoveCommand+ �� # [ $
 Data� �� " F�.���  � ��
 �  ����  �@��Stretch$ UniformToFill=���  � ��$ Center=���  � ��$ Center=�* 
 $Telerik.Windows.Controls.RadGridView  $��1��; 
 5Telerik.Windows.Controls.GridView.GridViewDataControl   ShowGroupPanel .     IsFilteringAllowed .     AutoGenerateColumns .  *	  $Telerik.Windows.Controls.DataControl 	  ItemsSource �� *
ForeMans+$ TwoWay=�$ PropertyChanged=� 	  SelectedItem �� *SelectedForeman+$ TwoWay=�$ PropertyChanged=�   Columns 1
 
 +Telerik.Windows.Controls.GridViewDataColumn
  6 
 0Telerik.Windows.Controls.GridViewBoundColumnBase   Header$ 	Delovodja��- 
 'Telerik.Windows.Controls.GridViewColumn    
IsReadOnly  . !   DataMemberBinding! �� "   AggregateFunctions" (  "Telerik.Windows.Data.CountFunction  ,  &Telerik.Windows.Data.AggregateFunction#   Caption$# Število izvajalcev: ���� $��1q�
���   �$ 2q�$��1��._�� $��1��	 �� $
��Auto ��� $��* �.+�]� $��0����.  .+��� $
��Left=�$��Varilec ��  $
��Left=�$��30��$��30��$��welder������ *EmployeeSelectCommand+ �� # [ $
 Data� �� " F�.���  � ��
 � � ���  �@��Stretch$ UniformToFill=���  � ��$ Center=���  � ��$ Center=�  $
��Left=�$��30��$��30��$��welder������ *WelderRemoveCommand+ �� # [ $
 Data� �� " F�.���  � ��
 �  ����  �@��Stretch$ UniformToFill=���  � ��$ Center=���  � ��$ Center=�  $��1�� .   .   .   �� *	Welders+$ TwoWay=�$ PropertyChanged=� �� *SelectedWelder+$ TwoWay=�$ PropertyChanged=� 
  $ Varilec��  . ! �� "   $# Število izvajalcev: ���� $��1q�
���   �$ 2q�$��2��._�� $��2��	 �� $
��Auto ��� $��* �.+�]� $��0����.  .+��� $
��Left=�$��Monter ��  $
��Left=�$��30��$��30��$��	installer������ *EmployeeSelectCommand+ �� # [ $
 Data� �� " F�.���  � ��
 � � ���  �@��Stretch$ UniformToFill=���  � ��$ Center=���  � ��$ Center=�  $
��Left=�$��30��$��30��$��	installer������ *InstallerRemoveCommand+ �� # [ $
 Data� �� " F�.���  � ��
 �  ����  �@��Stretch$ UniformToFill=���  � ��$ Center=���  � ��$ Center=�  $��1�� .   .   .   �� *
Installers+$ TwoWay=�$ PropertyChanged=� �� *SelectedInstaller+$ TwoWay=�$ PropertyChanged=� 
  $ Monter��  . ! �� "   $# Število izvajalcev: ���� $��1q�
���   �$ 2q�$��3��._�� $��3��	 �� $
��Auto ��� $��* �.+�]� $��0����.  .+��� $
��Left=�$��Cevar ��  $
��Left=�$��30��$��30��$��piper������ *EmployeeSelectCommand+ �� # [ $
 Data� �� " F�.���  � ��
 � � ���  �@��Stretch$ UniformToFill=���  � ��$ Center=���  � ��$ Center=�  $
��Left=�$��30��$��30��$��piper������ *PiperRemoveCommand+ �� # [ $
 Data� �� " F�.���  � ��
 �  ����  �@��Stretch$ UniformToFill=���  � ��$ Center=���  � ��$ Center=�  $��1�� .   .   .   �� *Pipers+$ TwoWay=�$ PropertyChanged=� �� *SelectedPiper+$ TwoWay=�$ PropertyChanged=� 
  $ Cevar��  . ! �� "   $# Število izvajalcev: ��� $��0��$��1��.+�� 	 �� $��* ��� $
��Auto �.+��� $��0��"   Ism.Construction.RegionNames$   EwrDocumentsRegion! 	 Prism.Regions.RegionManager%   
RegionName#% Z$ �� $��1��&   EwrExSiteManagerRegion#% Z& ]� $��1��$��0��$��2����.  .+��� $
��Left=�$��	Prekliči������ *CancelCommand+�� $��Right=�$��Shrani��#��[ ���� *SaveCommand+