   M S B A M L   `   `   `  ���� K  GIsm.Construction, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:Ism.Construction.ViewsIsm.Construction.Views  M IIsm.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS<clr-namespace:Ism.Infrastructure;assembly=Ism.InfrastructureIsm.Infrastructure ,    &Ism.Construction.Views.ForemanProjects   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     6local$clr-namespace:Ism.Construction.Views     Linf<clr-namespace:Ism.Infrastructure;assembly=Ism.Infrastructure     c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  P LPrism.Wpf, Version=6.3.0.0, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59$prismhttp://prismlibrary.com/  n jTelerik.Windows.Controls.GridView, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k	 gTelerik.Windows.Controls.Chart, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a
 ]Telerik.Windows.Data, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Documents.Core, Version=2019.1.215.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Input, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l hTelerik.Windows.Scheduling.Core, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7| xTelerik.Windows.Controls.FixedDocumentViewersUI, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7z vTelerik.Windows.Controls.FixedDocumentViewers, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.Navigation, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7o kTelerik.Windows.Controls.GanttView, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7j fTelerik.Windows.Controls.Data, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7w sTelerik.Windows.Controls.DataVisualization, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Wtelerik1http://schemas.telerik.com/2008/xaml/presentation  	 
           !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel  .    UcStyle#��[   � 	Resources  �� MergedDictionaries �� Source��  �� $X Rpack://application:,,,/Ism.Infrastructure;component/Resources/BaseButtonStyle.xaml?�%    .���  � RowDefinitions �� $��* ��� $
��Auto �.+�)  #Telerik.Windows.Controls.RadListBox  -     Name$
 list��$��0��
���   �$��1q�$��10q�$
��Auto=����� *
Projects+>  8Telerik.Windows.Controls.Primitives.ItemsControlSelector   SelectedItem �� *SelectedProject+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=����� .R�+  %Telerik.Windows.Controls.RadWrapPanel  	   Orientation$	 
Horizontal=����� .R��� $��1q�
���  ��._�� 
 � ColumnDefinitions
 ��  z� SharedSizeGroup$ ll����  �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� $��0��$��0��$��Gradbišče���� $��0��$��1��$��Stranka���� $��0��$��2��$��Projekt���� $��0��$��3��$��Številka projekta���� $��0��$��4��$��Začetek���� $��0��$��5��$��Konec���� $��1��$��0������  �� ElementName$ csite�� �� Path$
 Text��� $��1��$��1������ $
 cust��$
 Text��� $��1��$��2������ *Name+�� $��1��$��3������ *Externalnumber+�� $��1��$��4������ *
Start.Date+ �� StringFormat$ 
dd.MM.yyyy���� $��1��$��5������ *
End.Date+$ 
dd.MM.yyyy������  �� 
TargetType-  'Telerik.Windows.Controls.RadListBoxItem"  .��� 2  ,Telerik.Windows.Controls.Primitives.ListItem �� Property �@ 
IsSelected �� Value �� *
IsSelected+$ TwoWay=�$ PropertyChanged=��� $��1��$y�	Collapsed=�.+��� -   $��csite������ * SiteData.ConstructionSite.Name+�� -   $
��cust������ *)'SiteData.ConstructionSite.Customer.Name+