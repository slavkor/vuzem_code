   M S B A M L   `   `   `  ���� K  GIsm.Construction, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:Ism.Construction.ViewsIsm.Construction.Views  M IIsm.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullY?clr-namespace:Ism.Infrastructure.Ui;assembly=Ism.InfrastructureIsm.Infrastructure.Ui 3clr-namespace:Ism.ConstructionIsm.Construction  S<clr-namespace:Ism.Infrastructure;assembly=Ism.InfrastructureIsm.Infrastructure iGclr-namespace:Ism.Infrastructure.Converters;assembly=Ism.InfrastructureIsm.Infrastructure.Converters w sTelerik.Windows.Controls.DataVisualization, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7�cclr-namespace:Telerik.Windows.Controls.Timeline;assembly=Telerik.Windows.Controls.DataVisualization!Telerik.Windows.Controls.Timeline 2    ,Ism.Construction.Views.ConstructionSitesList   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     6local$clr-namespace:Ism.Construction.Views     Nui?clr-namespace:Ism.Infrastructure.Ui;assembly=Ism.Infrastructure     3localrgnclr-namespace:Ism.Construction     Linf<clr-namespace:Ism.Infrastructure;assembly=Ism.Infrastructure     ]	converterGclr-namespace:Ism.Infrastructure.Converters;assembly=Ism.Infrastructure     c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  P LPrism.Wpf, Version=6.3.0.0, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59$prismhttp://prismlibrary.com/  X	 TXceed.Wpf.Toolkit, Version=3.2.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f43xct)http://schemas.xceed.com/wpf/xaml/toolkit 	 n
 jTelerik.Windows.Controls.GridView, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Chart, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a ]Telerik.Windows.Data, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Documents.Core, Version=2019.1.215.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Input, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l hTelerik.Windows.Scheduling.Core, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7| xTelerik.Windows.Controls.FixedDocumentViewersUI, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7z vTelerik.Windows.Controls.FixedDocumentViewers, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.Navigation, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7o kTelerik.Windows.Controls.GanttView, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7j fTelerik.Windows.Controls.Data, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Wtelerik1http://schemas.telerik.com/2008/xaml/presentation 
             �	timelinescclr-namespace:Telerik.Windows.Controls.Timeline;assembly=Telerik.Windows.Controls.DataVisualization 
             !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel  .    UcStyle#��[   � 	Resources  �� MergedDictionaries �� Source?  9Ism.Infrastructure.Converters.DayModelToDateTimeConverter  DayToDateTime<  6Ism.Infrastructure.Converters.SolidColorBrushConverter  BrushConverter8  2Ism.Infrastructure.Converters.RangeSliderConverter  RangeSliderConverte;  5Ism.Infrastructure.Converters.TimelinePeriodConverter  TimelinePeriodConverter@  :Telerik.Windows.Controls.Timeline.TimelineItemGroupControl �� 
TargetTypeF  @Telerik.Windows.Controls.Timeline.ExpandableHeaderedItemsControl �� Property �� Value; 
 5Telerik.Windows.Controls.GridView.GridViewSearchPanel  GW �� BasedOn:	  4Ism.Infrastructure.Ui.CompabyBackgroundColorSelector  collorStyle  ContentTemplate)
  #Telerik.Windows.Controls.RadTimeBar 
 Name	 
  IsSnapToIntervalEnabled$
 
  EnableSparklineVirtualization*  $Telerik.Windows.Controls.TimeBarBase   PeriodStart �� Mode �� UpdateSourceTrigger   	PeriodEnd   VisiblePeriodStart   VisiblePeriodEnd 
  SelectionStart 
  SelectionEnd   	Intervals3  -Telerik.Windows.Controls.TimeBar.YearInterval4  .Telerik.Windows.Controls.TimeBar.MonthInterval3  -Telerik.Windows.Controls.TimeBar.WeekInterval2  ,Telerik.Windows.Controls.TimeBar.DayInterval0  *Telerik.Windows.Controls.RadCartesianChart   VerticalAxis3  -Telerik.Windows.Controls.ChartView.LinearAxis   HorizontalAxis?  9Telerik.Windows.Controls.ChartView.DateTimeContinuousAxis   	MajorStep   MajorStepUnit   Minimum   Maximum   Series2  ,Telerik.Windows.Controls.ChartView.BarSeries:  4Telerik.Windows.Controls.ChartView.CategoricalSeries   CombineMode   ValueBindingJ  DTelerik.Windows.Controls.ChartView.StringToDataPointBindingConverter>  8Telerik.Windows.Controls.ChartView.CategoricalSeriesBase   CategoryBinding   PaletteMode4  .Telerik.Windows.Controls.ChartView.ChartSeries   
ShowLabels    ItemsSource<  6Telerik.Windows.Controls.ChartView.PointTemplateSeries!   DefaultVisualStyle"   LabelDefinitionsC  =Telerik.Windows.Controls.ChartView.ChartSeriesLabelDefinition#   HorizontalAlignment$   VerticalAlignment%   Template& (� StretchDirection' (� Stretch( � LayoutTransform) �� Angle3  -Telerik.Windows.Controls.ChartView.LineSeriesA  ;Telerik.Windows.Controls.ChartView.CategoricalStrokedSeries*   Stroke+   StrokeThickness5  /Telerik.Windows.Controls.EventToCommandBehavior,   EventBindings+  %Telerik.Windows.Controls.EventBinding-   	EventName.   RaiseOnHandledEvents/   Command0   CommandParameter1 `� 	Converter2 �� Path3 �� ElementName  RowPlanDetailsTemplate* 
 $Telerik.Windows.Controls.RadGridView; 
 5Telerik.Windows.Controls.GridView.GridViewDataControl4   AutoGenerateColumns5   ShowGroupPanel6   ShowColumnFooters7   IsFilteringAllowed*   $Telerik.Windows.Controls.DataControl8    ItemsSource9   Columns1! 
 +Telerik.Windows.Controls.GridViewDataColumn6" 
 0Telerik.Windows.Controls.GridViewBoundColumnBase: "  Header-# 
 'Telerik.Windows.Controls.GridViewColumn; #  
IsReadOnly< "  DataMemberBinding= #  AggregateFunctions&$   Telerik.Windows.Data.SumFunction,%  &Telerik.Windows.Data.AggregateFunction> %  Caption? "  DataFormatString 	 RowDetailsTemplate@   RowDetailsTemplate=& 
 7Telerik.Windows.Controls.GridViewToggleRowDetailsColumn 
 tmItemTemplate*'  $Telerik.Windows.Controls.RadTimelineA ' NameB '  	StartPathC '  DurationPathD '  SelectionModeE '  IsSelectionEnabled  SiteToolTipTemplateF '  ToolTipTemplate  Site_CustomG '  TimelineItemTemplateH   
ScrollModeI '  ItemsSource��  �� $X Rpack://application:,,,/Ism.Infrastructure;component/Resources/BaseButtonStyle.xaml?��� $Z Tpack://application:,,,/Ism.Infrastructure;component/Resources/ConstructionSites.xaml?��� $M Gpack://application:,,,/Ism.Infrastructure;component/Resources/Grid.xaml?�%�
  &	       &	      &	 
     &	      '       '  @     7  &	 �     &	 �     7  &	 $    &		 ~    7  &	
 �    7  7          �� "  .���  �@ 
IsExpanded . �� "  8   .���  � ��$ 20(���  � ��$
 Bold"���  � ��$ 50��	  �� .R�� .+�
  $ timebar��$��2��$	��Top=�	 . 
 .   �� *	MinDate+$ TwoWay=�$ PropertyChanged=� �� *	MaxDate+$ TwoWay=�$ PropertyChanged=� �� *VisibleStart+$ TwoWay=�$ PropertyChanged=� �� *
VisibleEnd+$ TwoWay=�$ PropertyChanged=� �� *SelectionStart+$ TwoWay=�$ PropertyChanged=� �� *SelectionEnd+$ TwoWay=�$ PropertyChanged=�         .��� .+�  $��0 0 0 0 q�   $y�	Collapsed=�   $y�	Collapsed=�$ 1Q�$	 Day=� �� *	MinDate+ �� *	MaxDate+   $ Stack=�$ Limit $
 Date $ Series=� .   �� *Dates+! �� " ��.���  � ��
 � � �"   $# Center=�$$ Center=�% �� .R�(� $
& Both=�$' Uniform=�.���� 
���   �$
��Bold"����� *DataItem.Limit+( V� ~��� $	) -90Q�  $ Stack=�$ LimitOverflow $
 Date $ Series=� .   �� *Dates+! �� " ��.���  � ��
 �  ��"   $# Center=�$	$ Top=�% �� .R��� .+��� $��10(�
���  ��$
��Bold"����� *DataItem.LimitOverflow+( V� ~��� $	) -90Q�  $ Stack=�$	 Max $
 Date 
* �  ��$+ 1Q�$	��0.5q�  �� *Dates+,   $- SelectionChanged��. . / �� *SelectionChangedEventCommand+0 `� 81   .1��� $2 SelectionStart�$3 timebar���� $2 SelectionEnd�$3 timebar���� .R�  $��planGrid��4 .  5 .  6 . 7 .  8 �� *WorkPeriods+9 !  $: Št. zaposlenih��; . < �� *Plan+= $  $> Število zaposlenih: ��!  $: Stopnja��; . < �� *Description+!  $: Začetek��$? 
dd.MM.yyyy��; . < �� *Start+!  $: Konec��$? 
dd.MM.yyyy��; . < �� *End+�� .R�  $��projectsGrid��4 .  5 .  6 . 7 .  8@   8 �� *
Projects+9 &  !  $: Št. zaposlenih��; . < �� *EstimatedWorkers+= $  $> Število zaposlenih: ��!  $: Projekt��; . < �� *Name+!  $: Začetek��$? 
dd.MM.yyyy��; . < �� *
Start.Date+!  $: Konec��$? 
dd.MM.yyyy��; . < �� *
End.Date+�� .R�'  $A timeline��$B Start��$C Duration��$D Single=�E .  8F   8G  $
H None=� �� *Start+ �� *End+ �� *Start+ �� *End+I �� *DataItem.Projects+       ,   $- MouseDoubleClick��. . / �� *EditConstructionSiteCommand+0 �� $3 timeline��$2 SelectedItem�.��� J � RowDefinitionsJ �� $
��Auto ��� $��* �K � ColumnDefinitionsK �� $	��300 ��� $��* �.+�*(  $Telerik.Windows.Controls.RadCalendar(  $��0��$��0��$	��300��L (  SelectionMode$L Single=�M (  DateSelectionMode$
M Year=�$	��Top=�N (  SelectedDateN �� *Date+$ TwoWay=�$ PropertyChanged=�� $��1��$��0��.+�))  #Telerik.Windows.Controls.RadListBox)  -   O ) Name$O listbox��
���   �$��1q�$��10q�$��Visible=���. ���� *ProjectsInSelection+���� .R��� P �� Orientation$P Vertical=�Q �� CanVerticallyScrollQ . ���� .R��� $��1q�
���  ��$	��300��._�� K �� $��* �J �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� $��0������ *	Site.Name+�� $��1������ *Site.Customer.Name+�� $��2������ *Name+�� $��3������ *Externalnumber+�� $��4������ *
Start.Date+R �� StringFormat$R 
dd.MM.yyyy���� $��5������ *
End.Date+$R 
dd.MM.yyyy��� $��1��$��0��$��3��J �� $
��1.5* ��� $
��Auto ��� $��* �.+�,*  &Telerik.Windows.Controls.RadTabControl*  -   $	��tab��$��0��.@�)+  #Telerik.Windows.Controls.RadTabItem+  $��
Časovnica��
S � Tag$S 0��.��'  -   $A timeline��$B Start��$C Duration��$D Single=�E . #F [ #G [ T '  GroupExpandMode$T Multiple=�U '  	GroupPath$U ConstructionStieState��"V '  VerticalScrollBarVisibility$
V Auto=�$
��Left=� �� *	MinDate+$ TwoWay=�$ PropertyChanged=� �� *	MaxDate+$ TwoWay=�$ PropertyChanged=� �� *VisibleStart+$ TwoWay=�$ PropertyChanged=� �� *
VisibleEnd+$ TwoWay=�$ PropertyChanged=�W '  SelectedItemW �� *
Selected+$ TwoWay=�$ PropertyChanged=�I �� *ConstructionSites+X '  ItemRowIndexGeneratorX 4,  .Ism.Infrastructure.Ui.NewLineRowIndexGenerator,         ,   $- MouseDoubleClick��. . / �� *EditConstructionSiteCommand+0 �� $3 timeline��$2 SelectedItem�+  $��Časovnica povezana��$y�	Collapsed=�$S 1��.��� .+�+-  %Telerik.Windows.Controls.RadGanttView-  -   Y - Name$Y 	ganttview�����	#FFD6D4D4$��1 0 1 1q�Z -  PixelLength$Z 1:00:00h�$��0 6 0 0q�[ -  TasksSource[ �� *Tasks+\ -  VisibleRange\ �� *VisibleRange+] -  Columns] =.  7Telerik.Windows.Controls.GanttView.TreeColumnDefinition.  =/  7Telerik.Windows.Controls.GanttView.ColumnDefinitionBase^ /  Header$^ Title��_ /  Width80  2Telerik.Windows.Controls.ColumnLengthTypeConverter$	_ 2400 91  3Telerik.Windows.Controls.GanttView.ColumnDefinition` 1  MemberBinding` �� *Title+a 1  CellEditTemplatea �� .R��� .2  (Telerik.Windows.Input.Touch.TouchManagerb 2  	TouchMode$
b None=����� *Title+1  $^ Start��$	_ 1200 c 1  MaxWidth$	c 120Q�` �� *Start+a �� .R�03  *Telerik.Windows.Controls.RadDateTimePicker3  d 3  SelectedValued �� *Start+e 1  CellTemplatee �� .R��� ���� *Start+$R 
dd.MM.yyyy��1  $	^ End��$	_ 1200 $	c 120Q�` �� *End+a �� .R�3  d �� *End+e �� .R��� ���� *End+$R 
dd.MM.yyyy��+  $��Seznam��$S 2��.��  -   $
��list��$��0��5 .  7 .  4 .  f   ShowSearchPanelf . 6 . g   RowStyleSelector#g [ #@ [	 8 �� *ConstructionSites+h    SelectedItemh �� *
Selected+$ TwoWay=�$ PropertyChanged=�9 &  !  $: Št. zaposlenih��; . < �� *EstimatedWorkers+= $  $> Število zaposlenih: ��!  $: Stranka��; . < �� *Site.Customer.Name+!  $: Gradbišče��; . < �� *	Site.Name+!  $: Začetek��$? 
dd.MM.yyyy��; . < �� *Start+!  $: Konec��$? 
dd.MM.yyyy��; . < �� *End+,   $- MouseDoubleClick��. . / �� *EditConstructionSiteCommand+0 �� $
3 list��$2 SelectedItem�+  $��Pregled in plan��$S 3��.���  ��%�   &		       �� .R�  $��employeesGrid��4 .  6 .  5 .  8 �� *	Employees+9 !  < �� *
LastName+!  < �� *Name+J �� $
��Auto ��� $
��Auto �K �� $��* ��� $��* ��� $��* �.+�  $��1��$��0��$��10q�4 .  #@ [	 8 �� *Sites+9 &  !  < �� *Name+!  < �� *Customer.Name+  $��1��$��1��$��10q�  $��1��$��2��$��10q�,   $- SelectionChanged��. . / �� *TabSelectionChangedCommand+0 �� $	3 tab��$2 SelectedItem��� $��1��$��Stretch=�$��5��
�������*  $��2��i *  TabOrientation$i Vertical=�j *  TabStripPlacement$
j Left=�.@�+  $��Po projektih��.��
  -   $ timebar��$��2��$	��Top=�	 . 
 .   �� *	MinDate+$ TwoWay=�$ PropertyChanged=� �� *	MaxDate+$ TwoWay=�$ PropertyChanged=� �� *VisibleStart+$ TwoWay=�$ PropertyChanged=� �� *
VisibleEnd+$ TwoWay=�$ PropertyChanged=� �� *SelectionStart+$ TwoWay=�$ PropertyChanged=� �� *SelectionEnd+$ TwoWay=�$ PropertyChanged=�         .��� .+�  $��0 0 0 0 q�   $y�	Collapsed=�   $y�	Collapsed=�$ 1Q�$	 Day=� �� *	MinDate+ �� *	MaxDate+   $ Stack=�$ Limit $
 Date $ Series=� .   �� *Dates+! �� " ��.���  � ��
 � � �"   $# Center=�$$ Center=�% �� .R�(� $
& Both=�$' Uniform=�.���� 
���   �$
��Bold"����� *DataItem.Limit+( V� ~��� $	) -90Q�  $ Stack=�$ LimitOverflow $
 Date $ Series=� .   �� *Dates+! �� " ��.���  � ��
 �  ��"   $# Center=�$	$ Top=�% �� .R��� .+��� $��10(�
���  ��$
��Bold"����� *DataItem.LimitOverflow+( V� ~��� $	) -90Q�  $ Stack=�$	 Max $
 Date 
* �  ��$+ 1Q�$	��0.5q�  �� *Dates+,   $- SelectionChanged��. . / �� *SelectionChangedEventCommand+0 `� #1 [ .1��� $2 SelectionStart�$3 timebar���� $2 SelectionEnd�$3 timebar��+  $��Po planu��.��*  -   k * Name$k radTabControl��$��8q�l �� DisplayMemberPath$l WorkPlace.WorkPlaceName��;4  5Telerik.Windows.Controls.TabControl.RadTabControlBasem 4  ContentTemplate#m [ ���� *WorkPlaceInfos+