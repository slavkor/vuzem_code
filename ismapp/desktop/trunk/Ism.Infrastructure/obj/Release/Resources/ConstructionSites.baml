   M S B A M L   `   `   `  ���� M  IIsm.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:Ism.Infrastructure.ResourcesIsm.Infrastructure.Resources  M+clr-namespace:Ism.Infrastructure.ConvertersIsm.Infrastructure.Converters  " Telerik.Windows.Controls.ChartXclr-namespace:Telerik.Windows.Controls.ChartView;assembly=Telerik.Windows.Controls.Chart"Telerik.Windows.Controls.ChartView w sTelerik.Windows.Controls.DataVisualization, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7�cclr-namespace:Telerik.Windows.Controls.Timeline;assembly=Telerik.Windows.Controls.DataVisualization!Telerik.Windows.Controls.Timeline R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   SiteToolTipTemplate  �� RadiusY �� RadiusX �� Orientation  PorjectToolTipTemplate J� Opacity  DefaultShiftDayTemplate  ShiftDayTemplate �� Stretch  Timeline_TimelineItem_Fill �� EndPoint �� 
StartPoint � Color � Offset   Timeline_TimelineItem_Stroke	 �� Color ( $Timeline_TimelineItem_Fill_MouseOver * &Timeline_TimelineItem_Stroke_MouseOver & "Timeline_TimelineItem_Fill_Pressed (	 $Timeline_TimelineItem_Stroke_Pressed  
 SiteTimelineItemControlStyle
 �� 
TargetType;   5Telerik.Windows.Controls.Timeline.TimelineItemControl �� 	Resources2   ,Ism.Infrastructure.Converters.ColorConverter  Key<   6Ism.Infrastructure.Converters.SolidColorBrushConverter �� Property �� Value  BrushConverter �� 	Converter �� 
TargetType � 	Resources'  !System.Windows.VisualStateManager   VisualStateGroups%  System.Windows.VisualStateGroup   States   System.Windows.VisualState   
Storyboard �� 
TargetName �� TargetProperty g� Duration R� KeyTime R� Value ��Name # ProjectTimelineItemControlStyle  Site_Default  Site_Custom  Project_Custom�� D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     4local*clr-namespace:Ism.Infrastructure.Resources  9	convertes+clr-namespace:Ism.Infrastructure.Converters  mtelerikChartViewXclr-namespace:Telerik.Windows.Controls.ChartView;assembly=Telerik.Windows.Controls.Chart  q	timelinescclr-namespace:Telerik.Windows.Controls.Timeline;assembly=Telerik.Windows.Controls.DataVisualization  %]  &	        &	 Z    &	     &	 ,    &	 e    &	 �    &	 �    &	 .    &	 C    &		 �    &	
 �    7  7  7  7  7  7 	 7  7  7  &	 |    7  7  7  7  7  7 	 &	 R    &	 _    7 
 &	 q    7  �� .R�� .+��� $��0q�$  2��$ 2��
��������� $ 
Horizontal=�$��8,8,8,12q�.+��� 
���   �$��12(�$
��Bold"�$��Center=�$��Center=����� *EstimatedWorkers+�� $�� ��
���   �$��12(�$��Center=�$��Center=��� 
���   �$��12(�$
��Bold"�$��Center=�$��Center=����� *Site.Customer.Name+�� $�� ��
���   �$��12(�$��Center=�$��Center=��� 
���   �$��12(�$��Center=�$��Center=����� *	Site.Name+�� $�� ��
���   �$��12(�$��Center=�$��Center=��� 
���   �$��12(�$��Center=�$��Center=����� *Name+�� .R�� .+��� $��0q�$  2��$ 2��
�������$	 0.7Q��� $ 
Horizontal=�$��8,8,8,12q�.+��� $��Segoe UI)�$��Projekt ���� $��Segoe UI)����� *Name+�� .R��� ���� *Text+�� .R�� .+�F� ����Ѐ�� @fр�[ @]� �Pq pٰ р�� �7� Ѱ$� `�s р�@T� � � @B ���- @KL ���- �ا ���- p�� �^h �K � ��^�@��K@o@p�� @o@�ا �@o@@KL � � @B 	
���   �$	 0.3Q�$ UniformToFill=��� $��0 6 0 0q�
���   �$
��Bold"�$��Center=�$��Center=����� *Text+�� $ 0.5,1&�$ 0.5,0&�.��� $ 	#66FFFFFF��$ 0Q�� $ 	#B2A1A1A1��$ 1Q��� $	 	#CC636363���� $ 0.5,1&�$ 0.5,0&�.��� $ 	#66FFFFFF��$ 0Q�� $ 	#B2A1A1A1��$ 1Q��� $	 	#CC909090���� $ 0.5,1&�$ 0.5,0&�.��� $ 	#E5FFFFFF��$ 0Q�� $ 	#66FFFFFF��$ 1Q��� $	 	#CC727272���� "
      ColorConverter   BrushConverter .���  � ��$ 20����  � �� �� *DataItem.Site.Company.Color+8   ��  � ��$ 0 5 0 0q���  � �� �� *DataItem.Site.Company.Color+8  ��  � �� �� "      ColorConverter .R��� ���	#01FFFFFF   CommonStates��.   
Normal��  	MouseOver��. �� .��T� $ timelineItem��$
 Fill�$ 0.00:00:00.05D�..�i� $ 0��8  T� $ timelineItem��$ Stroke�$ 0.00:00:00.05D�..�i� $ 0��8    Pressed��. �� .��T� $ timelineItem��$
 Fill�$ 0.00:00:00.05D�..�i� $ 0��8  T� $ timelineItem��$ Stroke�$ 0.00:00:00.05D�..�i� $ 0��8  ._�� ���� *DataItem.Site.Company.Color+8  .+��� $ timelineItem��$
 Fill=����� *DataItem.Site.Company.Color+8  ���� *DataItem.Site.Company.Color+8  �� $��0��$ 
Horizontal=�.+��� 
���   �$��12(�$
��Bold"�$��Center=�$��Center=����� *DataItem.EstimatedWorkers+�� $�� ��
���   �$��12(�$��Center=�$��Center=��� 
���   �$��12(�$
��Bold"�$��Center=�$��Center=����� *DataItem.Site.Customer.Name+�� $�� ��
���   �$��12(�$��Center=�$��Center=��� 
���   �$��12(�$��Center=�$��Center=����� *DataItem.Site.Name+�� $�� ��
���   �$��12(�$��Center=�$��Center=��� 
���   �$��12(�$��Center=�$��Center=����� *DataItem.Name+�� "
   .���  � ��$ 20����  � ��8   ��  � ��$ 0 5 0 0q���  � ��8  ��  � �� �� "   .R��� ���	#01FFFFFF$��20��   CommonStates��.   
Normal��  	MouseOver��. �� .��T� $ timelineItem��$
 Fill�$ 0.00:00:00.05D�..�i� $ 0��8  T� $ timelineItem��$ Stroke�$ 0.00:00:00.05D�..�i� $ 0��8    Pressed��. �� .��T� $ timelineItem��$
 Fill�$ 0.00:00:00.05D�..�i� $ 0��8  T� $ timelineItem��$ Stroke�$ 0.00:00:00.05D�..�i� $ 0��8  ._�� $��20��.+��� $ timelineItem��$��20��#��z��#��z��$
 Fill=��� 
���   �$��12(�$��Center=�$��Center=����� *DataItem.Name+�� .R�   �� .R�   8��  �� .R�   8��  