   M S B A M L   `   `   `  ����M  IIsm.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:Ism.Infrastructure.ResourcesIsm.Infrastructure.Resources  5      M+clr-namespace:Ism.Infrastructure.ConvertersIsm.Infrastructure.Converters  5      " Telerik.Windows.Controls.ChartXclr-namespace:Telerik.Windows.Controls.ChartView;assembly=Telerik.Windows.Controls.Chart"Telerik.Windows.Controls.ChartView 5      w sTelerik.Windows.Controls.DataVisualization, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7�cclr-namespace:Telerik.Windows.Controls.Timeline;assembly=Telerik.Windows.Controls.DataVisualization!Telerik.Windows.Controls.Timeline 5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   SiteToolTipTemplate  �� RadiusY �� RadiusX �� Orientation  PorjectToolTipTemplate J� Opacity  DefaultShiftDayTemplate  ShiftDayTemplate �� Stretch  Timeline_TimelineItem_Fill �� EndPoint �� 
StartPoint � Color � Offset   Timeline_TimelineItem_Stroke	 �� Color ( $Timeline_TimelineItem_Fill_MouseOver * &Timeline_TimelineItem_Stroke_MouseOver & "Timeline_TimelineItem_Fill_Pressed (	 $Timeline_TimelineItem_Stroke_Pressed  
 SiteTimelineItemControlStyle
 �� 
TargetType;   5Telerik.Windows.Controls.Timeline.TimelineItemControl �� 	Resources2   ,Ism.Infrastructure.Converters.ColorConverter  Key<   6Ism.Infrastructure.Converters.SolidColorBrushConverter �� Property �� Value  BrushConverter �� 	Converter �� 
TargetType � 	Resources'  !System.Windows.VisualStateManager   VisualStateGroups%  System.Windows.VisualStateGroup   States   System.Windows.VisualState   
Storyboard �� 
TargetName �� TargetProperty g� Duration R� KeyTime R� Value ��Name # ProjectTimelineItemControlStyle  Site_Default  Site_Custom  Project_Custom�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      4local*clr-namespace:Ism.Infrastructure.Resources  5      9	convertes+clr-namespace:Ism.Infrastructure.Converters  5      mtelerikChartViewXclr-namespace:Telerik.Windows.Controls.ChartView;assembly=Telerik.Windows.Controls.Chart  5      q	timelinescclr-namespace:Telerik.Windows.Controls.Timeline;assembly=Telerik.Windows.Controls.DataVisualization  5      %�  &	        &	 �    &	 �    &	     &	 �    &	 `    &	 �    &	     &	 B    &		 �    &	
 	    7  7  7  7  7  7 	 7  7  7  &	 *    7  7  7  7  7  7 	 &	 4    &	 \    7 
 &	 �    7  �� 5   
   .R�5   
   � .+�5	      �� $��0q�6   $  2��6#   $ 2��6/   
�������6;   �� 5
      $ 
Horizontal=�6   $��8,8,8,12q�62   .+�5      �� 
���   �$��12(�6Q   $
��Bold"�6_   $��Center=�6q   $��Center=�6�   ��6   �� *EstimatedWorkers+6�   �� 5      $�� ��6   
���   �$��12(�68   $��Center=�6F   $��Center=�6c   �� 5      
���   �$��12(�6S   $
��Bold"�6a   $��Center=�6s   $��Center=�6�   ��6   �� *Site.Customer.Name+6�   �� 5      $�� ��6   
���   �$��12(�68   $��Center=�6F   $��Center=�6c   �� 5      
���   �$��12(�6J   $��Center=�6X   $��Center=�6u   ��6   �� *	Site.Name+6u   �� 5      $�� ��6   
���   �$��12(�68   $��Center=�6F   $��Center=�6c   �� 5      
���   �$��12(�6E   $��Center=�6S   $��Center=�6p   ��6   �� *Name+6p   5      5      5      �� 5      .R�5   
   � .+�5      �� $��0q�6   $  2��6#   $ 2��6/   
�������$	 0.7Q�6H   �� 5      $ 
Horizontal=�6   $��8,8,8,12q�62   .+�5      �� $��Segoe UI)�6   $��Projekt ��62   �� 5       $��Segoe UI)�6   ��62   �� *Name+5!      5"      5#      �� 5%      .R�5&   
   �� ��6   �� *Text+5'      �� 5)      .R�5*   
   � .+�5+      F� ����Ѐ�� @fр�[ @]� �Pq pٰ р�� �7� Ѱ$� `�s р�@T� � � @B ���- @KL ���- �ا ���- p�� �^h �K � ��^�@��K@o@p�� @o@�ا �@o@@KL � � @B 	
���   �$	 0.3Q�6�   $ UniformToFill=�6�   �� 5,      $��0 6 0 0q�5-      
���   �$
��Bold"�5/      $��Center=�50      $��Center=�51      ��5,      �� *Text+51      52      53      �� 54      $ 0.5,1&�6=   $ 0.5,0&�6N   .��55   
   � $ 	#66FFFFFF��6   $ 0Q�6)   � 56   
   $ 	#B2A1A1A1��6   $ 1Q�6)   57      �� 58      $	 	#CC636363��6;   �� 59      $ 0.5,1&�6G   $ 0.5,0&�6X   .��5:   
   � $ 	#66FFFFFF��6   $ 0Q�6)   � 5;   
   $ 	#B2A1A1A1��6   $ 1Q�6)   5<      �� 5=      $	 	#CC909090��6E   �� 5>      $ 0.5,1&�6E   $ 0.5,0&�6V   .��5?   
   � $ 	#E5FFFFFF��6   $ 0Q�6)   � 5@   
   $ 	#66FFFFFF��6   $ 1Q�6)   5A      �� 5B      $	 	#CC727272��6C   �� 5D      "
   61    5E   
     5F      ColorConverter 5G        5I      BrushConverter 5J      .�5L   
   ��  � ��$ 20��6#   60   �� 5M   
    � �� 6(   �� *DataItem.Site.Company.Color+8   �� 5N   
    � ��$ 0 5 0 0q�6#   �� 5O   
    � �� 6'   �� *DataItem.Site.Company.Color+8  �� 5P   
    � �� 5Q      �� 5R      "   6"    5S        5T      ColorConverter 5U      .R�5W      �� ���	#01FFFFFF 5X        5Y      CommonStates��. 5Z   "     
Normal��6.     5[   "   	MouseOver��. 5\   &   �� .��5]   *   T� $ timelineItem��6H   $
 Fill�6m   $ 0.00:00:00.05D�6�   ..�5^   .   i� $ 0��6E   8  6Q   5_   +   T� 5`   *   $ timelineItem��6H   $ Stroke�6m   $ 0.00:00:00.05D�6�   ..�5a   .   i� $ 0��6E   8  6Q   5b   +   5c   '   5d   #     5e   "   Pressed��. 5f   &   �� .��5g   *   T� $ timelineItem��6H   $
 Fill�6m   $ 0.00:00:00.05D�6�   ..�5h   .   i� $ 0��6E   8  6Q   5i   +   T� 5j   *   $ timelineItem��6H   $ Stroke�6m   $ 0.00:00:00.05D�6�   ..�5k   .   i� $ 0��6E   8  6Q   5l   +   5m   '   5n   #   5o      ._�5q      � ��6   �� *DataItem.Site.Company.Color+8  .+�5r      �� $ timelineItem��6(   $
 Fill=�6�   ��6>   �� *DataItem.Site.Company.Color+8  ��6�   �� *DataItem.Site.Company.Color+8  6�   �� 5s      $��0��6)   $ 
Horizontal=�66   .+�5t   "   �� 
���   �$��12(�6j   $
��Bold"�6x   $��Center=�6�   $��Center=�6�   ��6,   �� *DataItem.EstimatedWorkers+6�   �� 5u   "   $�� ��6,   
���   �$��12(�6H   $��Center=�6V   $��Center=�6s   �� 5v   "   
���   �$��12(�6l   $
��Bold"�6z   $��Center=�6�   $��Center=�6�   ��6,   �� *DataItem.Site.Customer.Name+6�   �� 5w   "   $�� ��6,   
���   �$��12(�6H   $��Center=�6V   $��Center=�6s   �� 5x   "   
���   �$��12(�6c   $��Center=�6q   $��Center=�6�   ��6,   �� *DataItem.Site.Name+6�   �� 5y   "   $�� ��6,   
���   �$��12(�6H   $��Center=�6V   $��Center=�6s   �� 5z   "   
���   �$��12(�6^   $��Center=�6l   $��Center=�6�   ��6,   �� *DataItem.Name+6�   5{      5|      5}      5~      5�      5�      �� 5�      "
   64   .�5�   
   ��  � ��$ 20��6#   �� 5�   
    � ��8   6(   �� 5�   
    � ��$ 0 5 0 0q�6#   �� 5�   
    � ��8  6'   �� 5�   
    � �� 5�      �� 5�      "   6"   .R�5�      �� ���	#01FFFFFF$��20��64    5�        5�      CommonStates��. 5�   "     
Normal��6.     5�   "   	MouseOver��. 5�   &   �� .��5�   *   T� $ timelineItem��6H   $
 Fill�6m   $ 0.00:00:00.05D�6�   ..�5�   .   i� $ 0��6E   8  6Q   5�   +   T� 5�   *   $ timelineItem��6H   $ Stroke�6m   $ 0.00:00:00.05D�6�   ..�5�   .   i� $ 0��6E   8  6Q   5�   +   5�   '   5�   #     5�   "   Pressed��. 5�   &   �� .��5�   *   T� $ timelineItem��6H   $
 Fill�6m   $ 0.00:00:00.05D�6�   ..�5�   .   i� $ 0��6E   8  6Q   5�   +   T� 5�   *   $ timelineItem��6H   $ Stroke�6m   $ 0.00:00:00.05D�6�   ..�5�   .   i� $ 0��6E   8  6Q   5�   +   5�   '   5�   #   5�      ._�5�      � $��20��6   .+�5�      �� $ timelineItem��64   $��20��6(   #��z��6J   #��z��6n   $
 Fill=�6�   �� 5�      
���   �$��12(�6Z   $��Center=�6h   $��Center=�6�   ��6(   �� *DataItem.Name+6�   5�      5�      5�      5�      5�      �� 5�      .R�5�   
      5�      �� 5�      .R�5�   
      8��  6(   5�      �� 5�      .R�5�   
      8��  6(   5�      5�      