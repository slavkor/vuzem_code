   M S B A M L   `   `   `  ���� >  :Ism, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:Ism.Views	Ism.Views  G CIsm.Controls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA0clr-namespace:Ism.Controls;assembly=Ism.ControlsIsm.Controls M IIsm.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS<clr-namespace:Ism.Infrastructure;assembly=Ism.InfrastructureIsm.Infrastructure     Ism.Views.Shell   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     )localclr-namespace:Ism.Views     P LPrism.Wpf, Version=6.3.0.0, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59$prismhttp://prismlibrary.com/  c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  ;ctrl0clr-namespace:Ism.Controls;assembly=Ism.Controls  Finf<clr-namespace:Ism.Infrastructure;assembly=Ism.Infrastructure    � WindowState$  	Maximized=�!  Prism.Mvvm.ViewModelLocator   AutoWireViewModel .    CustomWindowStyle#��[   � Title �� *WindowTitle+ � 	Resources  �� 
TargetType  CustomWindowStyleError �� BasedOn �� Property �� Value  CustomWindowStyleGreen  CustomPopupStyle �� 
TargetType	 �� Padding
 ��Name �� Orientation  InheritedStyle �� Path �� RelativeSource �� AncestorType  	BoolToVis��%�  &	       7   &	 *     7   &	 T     7   &	 �    �� " �8   .���  � ��
 �r����� " �8   .���  � ��
 ������� " �8   .���  � �� �� " �.R�� .+��� $	 5q�#��z��#��z��$��1,1,1,1q�._��� .u��� ]� $��30��$	��Top=���.  $y�Hidden=�.+��� $��5,0,0,0q�$��Center=�$
��Left=�$��16(�
�������#��z �� $
 	MainStack��$ 
Horizontal=�$��5q�$��Right=�$	��Top=�$��Right=�.+��� $��0��#���  ���� $ MinimizeCommand�����  �� " ��� $��1��#���  ���� $ MaximizeCommand�����  �� " ��� $��2��#���  ���� $ RestoreCommand�����  �� " ��� $��r��#���  ���� $ CloseCommand�����  �� " ��� .��� .  (System.Windows.Interactivity.Interaction   Triggers F  @Prism.Interactivity.InteractionRequest.InteractionRequestTrigger  3  -System.Windows.Interactivity.EventTriggerBase   SourceObject �� *AppBusyRequest+ �� Mode$ OneWay=�.  (System.Windows.Interactivity.TriggerBase   Actions. +  %Prism.Interactivity.PopupWindowAction     IsModal .    WindowStartupLocation$ CenterOwnerX�   WindowStyle# [    WindowContent    Ism.Views.BusyIndicator     �� *ExceptionRequest+$ OneWay=�.    . $ CenterOwnerX�# [     Ism.Views.ExceptionView     �� *ConfirmSaveRequest+$ OneWay=�.    . $ CenterOwnerX�# [  	   Ism.Views.ConfirmSaveView	   � ColumnDefinitions �� $
��Auto ��� $��* ��� $��* ��� $��50 � � RowDefinitions �� $��40 ��� $
��Auto ��� $��* �.+��� $��0��$��0��$
  Ism.Infrastructure.RegionNames 
  CompanyLogoRegion!  Prism.Regions.RegionManager   
RegionName# Z �� -    ��Name$ SysNavigationRegion��$��1q�$��1��$��0��$��3�� 
  SysNavRegion# Z ���� .R�]� ��.  $��Right=�$��Right=��� -   $ NavigationRegion��$��1q�$��0��$��1��$��4�� 
  NavigaionRegion# Z ���� .R��� $ 
Horizontal=��� -    ��Name$ 
MainRegion��$��0��$��2��$��4��$��1��
������� 
  MainContentRegion# Z $��Stretch=�$��Stretch=��� 
���   �$��1q����	#80000000$��3��y��� *AdornerVisible+  �� 	Converter#  [ ! �� FallbackValue$! 	Collapsed��._�� .+��� $��0q�$
��Wrap=�$��Please Wait...��$��Center=�$��Center=�$��24(�$
��Bold"����	#7EFFFFFF