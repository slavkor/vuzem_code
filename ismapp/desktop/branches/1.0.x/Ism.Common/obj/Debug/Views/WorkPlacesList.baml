   M S B A M L   `   `   `  ����E  AIsm.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Ism.Common.ViewsIsm.Common.Views  5      'clr-namespace:Ism.Common
Ism.Common  5      M IIsm.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS<clr-namespace:Ism.Infrastructure;assembly=Ism.InfrastructureIsm.Infrastructure 5      %    Ism.Common.Views.WorkPlacesList   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:Ism.Common.Views     5      -localrgnclr-namespace:Ism.Common     5      Linf<clr-namespace:Ism.Infrastructure;assembly=Ism.Infrastructure     5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5	      w sTelerik.Windows.Controls.DataVisualization, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a ]Telerik.Windows.Data, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n	 jTelerik.Windows.Controls.GridView, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k
 gTelerik.Windows.Documents.Core, Version=2018.2.619.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Input, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l hTelerik.Windows.Scheduling.Core, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.Navigation, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7j fTelerik.Windows.Controls.Data, Version=2018.2.620.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Otelerik1http://schemas.telerik.com/2008/xaml/presentation	   	 
      5
      P LPrism.Wpf, Version=6.3.0.0, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59$prismhttp://prismlibrary.com/  5      !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel  .  � 	Resources 5       �� MergedDictionaries �� Source; 	 5Telerik.Windows.Controls.GridView.GridViewSearchPanel �� 
TargetType   GW �� BasedOn �� Property �� Value  VisibilityConverter�� 5   
    5      �� 5      $X Rpack://application:,,,/Ism.Infrastructure;component/Resources/BaseButtonStyle.xaml?�6%   6�   �� 5      $M Gpack://application:,,,/Ism.Infrastructure;component/Resources/Grid.xaml?�6%   6x   %�   '        7   &	 �     �� 5      "  6   8   .�5      ��  � ��$ 20(�6-   6:   �� 5       � ��$
 Bold"�6/   6>   �� 5       � ��$ 50��6+   68   5      �� 5      6I   5      .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   6)   �� 5      $
��Auto �6   6,   .+�5"   
   * 	 $Telerik.Windows.Controls.RadGridView  -   5        	  Name$
	 list��5"   j   $��0��6   ; 	 5Telerik.Windows.Controls.GridView.GridViewDataControl
   AutoGenerateColumns
 .     ShowGroupPanel .     ShowSearchPanel . *  $Telerik.Windows.Controls.DataControl   ItemsSource 6+   �� *
WorkPlaces+   SelectedItem 6x   �� *SelectedWorkPlace+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�   Columns 5#      1 	 +Telerik.Windows.Controls.GridViewDataColumn  5$      6 	 0Telerik.Windows.Controls.GridViewBoundColumnBase   Header$ Šifra��6-   - 	 'Telerik.Windows.Controls.GridViewColumn   
IsReadOnly .    DataMemberBinding 6=   �� *Code+6u     5%      $ Delovno mestp��6-    .  6D   �� *WorkPlaceName+6�   5	  /Telerik.Windows.Controls.EventToCommandBehavior 	  EventBindings 5'      +
  %Telerik.Windows.Controls.EventBinding
  5(       
  	EventName$ MouseDoubleClick��6J    
  RaiseOnHandledEvents .  
  Command 6(   �� *SelectCommand+ 
  CommandParameter 6g   ��  �� ElementName$
 list�� �� Path$ SelectedItem�6�   5*      ]� 5,   
   $��1��6   ��.  y�67   �� *
IsSelect+ �� 	Converter# [  �� FallbackValue$ 	Collapsed��.+�5-      �� $
��Left=�6   $��	Prekliči��6+   ��6>   �� *CancelCommand+��6`   Z� 6~   �� 5.      $��Right=�6   $��Izberi��6,   ��6=   �� *SelectCommand+��6_   �� $
 list��$ SelectedItem�6�   5/      50      51      