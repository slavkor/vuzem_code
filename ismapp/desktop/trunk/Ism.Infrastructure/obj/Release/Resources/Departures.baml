   M S B A M L   `   `   `  ���� M  IIsm.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:Ism.Infrastructure.ResourcesIsm.Infrastructure.Resources  w sTelerik.Windows.Controls.DataVisualization, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7�cclr-namespace:Telerik.Windows.Controls.Timeline;assembly=Telerik.Windows.Controls.DataVisualization!Telerik.Windows.Controls.Timeline R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089n jTelerik.Windows.Controls.GridView, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a ]Telerik.Windows.Data, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Documents.Core, Version=2019.1.215.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k	 gTelerik.Windows.Controls.Input, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l
 hTelerik.Windows.Scheduling.Core, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.Navigation, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7o kTelerik.Windows.Controls.GanttView, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7j fTelerik.Windows.Controls.Data, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7 %  !Timeline_TimelineInstantItem_Fill  �� EndPoint �� 
StartPoint � Color � Offset / +Timeline_TimelineInstantItem_Fill_MouseOver - )Timeline_TimelineInstantItem_Fill_Pressed  Departure_Default_template'   !System.Windows.VisualStateManager    VisualStateGroups%  System.Windows.VisualStateGroup   States   System.Windows.VisualState   
Storyboard �� 
TargetName �� TargetProperty	 g� Duration
 R� KeyTime R� Value ��Name �� Angle �� CenterX �� CenterY  Car_Inbound_Confirmed  Car_Inbound_InProgress  Car_Outbound_Confirmed  Car_Outbound_InProgress  internal 	 addPath �� Stretch 
 
removePath  steeringPath  
notifyPath  confirmPath + 'TimelineInstantItemControlStyle_Default �� 
TargetTypeB  <Telerik.Windows.Controls.Timeline.TimelineInstantItemControl �� Property �� Value �� 
TargetType 5 1TimelineInstantItemControlStyle_Inbound_Confirmed J� OpacityMask !� Visual j� Stretch 4 0TimelineInstantItemControlStyle_Inbound_Progress 6 2TimelineInstantItemControlStyle_Outbound_Confirmed 5 1TimelineInstantItemControlStyle_Outbound_Progress , (TimelineInstantItemControlStyle_Internal   AddDepartureEmployeeTemplate,  &Telerik.Windows.Controls.RadPathButton   PathGeometry �� Source �� Path   	PathStyle  DepartureEmployeeTemplate � ColumnDefinitions �� Padding � RowDefinitions.  (Telerik.Windows.Controls.RadToggleButton d� 	IsChecked5  /Telerik.Windows.Controls.EventToCommandBehavior    EventBindings+  %Telerik.Windows.Controls.EventBinding!   	EventName"   RaiseOnHandledEvents#   Command$   CommandParameter  AddDepartureCarTemplate  DepartureCarTemplate  ToolTipTemplate% �� RadiusY& �� RadiusX' J� Opacity( �� Orientation) �� StringFormat  Departure_Default  Departure_Inboud_Confirmed  Departure_Outboud_Confirmed  Departure_Inboud_InProgress   Departure_Outboud_InProgress  Departure_Internal�� D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     4local*clr-namespace:Ism.Infrastructure.Resources  q	timelinescclr-namespace:Telerik.Windows.Controls.Timeline;assembly=Telerik.Windows.Controls.DataVisualization  Qtelerik1http://schemas.telerik.com/2008/xaml/presentation
    	 
      %(  &	        &	 V     &	 �     &	 �     7  7  7   &	 �    &	 W    &	     &	 +    &	 7
    &		 �    &	
 m    &	 '    &	     &	 �    &	 &    7  7  7   &	 �    7  7  7  &	 �    7  7  7  &	 �    7  7  7  &	 G    7  7  7  &	     7  7  7  &	 �    7 	 &	 �    7 
 &	 '!    7 	 &	 :"    7 
 &	 a$    &	 �%    7  &	 &    7  &	 &    7  &	 *&    7  &	 <&    7  &	 N&    7  �� $	  1,1&�$	 0,0&�.��� $ 	#FF747474��$ 0Q�� $ 	#FF1C1C1C��$ 1Q��� $	  1,1&�$	 0,0&�.��� $
 Gray��$ 0Q�� $ Black��$ 1Q��� $	  1,1&�$	 0,0&�.��� $ Black��$ 0Q�� $
 Gray��$ 1Q��� .R��� ���	#01FFFFFF$��10��$��10��   CommonStates��.   
Normal��  	MouseOver��. �� .��T� $ timelineItem��$
 Fill�$	 0.00:00:00.05D�..�i� $
 0��8     Pressed��. �� .��T� $ timelineItem��$
 Fill�$	 0.00:00:00.05D�..�i� $
 0��8  ._��� $ timelineItem��$��Center=�$��Center=�$��7��$��7��8�� z��� $ 45Q�$	 3.5Q�$	 3.5Q�F� �����@KL �ا Ѡ.c �.c �`�.c ���!�ا �` $� � $� �@� �`� `� �`@]� �`� `� ���!�@� �`� `� �` $� �`� `� Р.c  $� �@KL �@� �`� `� נ.c @]� �`� `� � z �@� �`� `� נ.c  $� �`� `� �@� ��[ � ��R �@KL `@KL Ѡ.c @KL ӀY @KL  �P �R ��M ��[ ���-  � ���-  -1� 	= @o@ �@KL @o@׀�[  -1 р�[ ��!р���!р� -1���!@o@ � -1@o@�@o@ -1 �@o@ � �@� ��[ 	
���   �F� �����@KL �ا Ѡ.c �.c �`�.c ���!�ا �` $� � $� �@� �`� `� �`@]� �`� `� ���!�@� �`� `� �` $� �`� `� Р.c  $� �@KL �@� �`� `� נ.c @]� �`� `� � z �@� �`� `� נ.c  $� �`� `� �@� ��[ � ��R �@KL `@KL Ѡ.c @KL ӀY @KL  �P �R ��M ��[ ���-  � ���-  -1� 	= @o@ �@KL @o@׀�[  -1 р�[ ��!р���!р� -1���!@o@ � -1@o@�@o@ -1 �@o@ � �@� ��[ 	
���   �F� �������- ��[ � $� ��[ ���!��� �@o@��� �0_Q��� ��^+� ��^ � ���^��� �@o@��� ׀������- ��- ���� ��� ���- ��- �@T� ��� ׀�[ �����- ��- ���- ��� ���- ��- �@B ��� �@B  z �@B "i �� ��[ ��- ��[ Р%& �pr Ѡ%& ��� Ѡ7� ��� Ѡ7� �pr Ѡ%& �pr � � �pr � � ��� � ���� �P�� �pr � � �pr Ѐ�[ `�� � �D ���  `� `� ׀�[  ��  `� `� ��pr ���  `� `� ׀�[ `��  `� `� ���`�� � �� ���  `� `� ׀� ��  `� `� ���)���  `� `� ׀�`��  `� `� 	
���   �F� �������- ��[ � $� ��[ ���!��� �@o@��� �0_Q��� ��^+� ��^ � ���^��� �@o@��� ׀������- ��- ���� ��� ���- ��- �@T� ��� ׀�[ �����- ��- ���- ��� ���- ��- �@B ��� �@B  z �@B "i �� ��[ ��- ��[ Р%& �pr Ѡ%& ��� Ѡ7� ��� Ѡ7� �pr Ѡ%& �pr � � �pr � � ��� � ���� �P�� �pr � � �pr Ѐ�[ `�� � �D ���  `� `� ׀�[  ��  `� `� ��pr ���  `� `� ׀�[ `��  `� `� ���`�� � �� ���  `� `� ׀� ��  `� `� ���)���  `� `� ׀�`��  `� `� 	
���   �F� ����� � ��� �`�s ���  	= �1  	= ��!� 	= @o@� -1@o@� -1��!� -1�1 �Q��k0@���  � ��� � z @T� � � @]�   	=  	= � $� @T�   	=  	= ��y� �� � � �� �ا и! �ا �%& ��ا `�S р�� `�S р�� ��- Ӏ�� ��- pAv  �: pAv 0�f �pAv 0�f ��j "i ��j  z �@f z ��"i ��� 0�f ��� 0�f Ӑ��  �: ��� ��- ��� ��- р�� `�S �@]� `�S �@]� �%& �@]� и! w� ��  �� �� ��y� �� 	
���   �F� $
 Fill=����	#FF000000������S��р���р��S��2
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
 Fill=����	#FF000000|��������<R��C�Z��@��=R��m5�����O���C��m5��Ѥ���n5��C0P������n5�H<R�����Z���CѬ<R\�������<RF� �����@]� ��/Ӏ��  �,�����Y0@p'!PQPQSp'!�����Y0@ �,��� ��/@]� ��-@]� �`�� ��� =
ףp=0@�p�  �  � S�p� =
ףp=0@��� `�� @]� �-�@]� ��/Ѐ��  z р��  z �@f�ا р�/�ا ��#,�� p� �p ����[ Ӏz�  G ���  	=  �  	= Ӏ��  	= ��}  G �f ��[ ӐO  �p B �� �A> �ا ���j �ا р��  z ��ا ��/��ا �-Ӏ�� `�� @ŋ =
ףp=0@��  � Ӏhv �p� �co ��� @l @]� рA> @]� ���A P_� �M �� ��`  ��кt  d �a� �#,�ا ��/� � �� �0� �� ����- �"��K �@o@ Ii ��O�$� ��O � Ӏ�O0� @o@���"�"���@o@0� ��O � ��O��$� ��O Ii @o@��K �"���- ���� 0� ��  � Ӏ� �$� ��-  Ii ��K ��K � Ii ��- �$� ��  � �� 	
���   �F� ����� -1�� � 	= �� ׀�  	=  �� �� р� ��Oр�[ ��� -1��׀�O $�  �� �� р�O 	= � -1��  �� �� � -1 $� р�[  $� � 	= ��� 	=  	= � -1 	= 	
���   �F� K����@o@��j �@T� ��!��g5 `�� Ѱ�J �z� Q@T� �Q��+0@�p�*�KU �@o@��j 	
���   ��� "  .���  � ��$ 0 5 0 0q���  � �� �� "  .R��� $��10��$��10��   CommonStates��.   
Normal��  	MouseOver��. �� .��T� $ timelineItem��$
 Fill�$	 0.00:00:00.05D�..�i� $
 0��8     Pressed��. �� .��T� $ timelineItem��$
 Fill�$	 0.00:00:00.05D�..�i� $
 0��8  ._��� $ timelineItem��$��Center=�$��Center=�$��7��$��7��8�� z��� $ 45Q�$	 3.5Q�$	 3.5Q��� "  .���  � ��$ 0 5 0 0q���  � �� �� "  .R��� $��20��$��20��   CommonStates��.   
Normal��  	MouseOver��. �� .��T� $ timelineItem��$
 Fill�$	 0.00:00:00.05D�..�i� $
 0��8     Pressed��. �� .��T� $ timelineItem��$
 Fill�$	 0.00:00:00.05D�..�i� $
 0��8  ._��� $ timelineItem��$��Center=�$��Center=�$��20��$��20��
��� � � !� 8  $
 Fill=��� "  .���  � ��$ 0 5 0 0q���  � �� �� "  .R��� $��20��$��20��   CommonStates��.   
Normal��  	MouseOver��. �� .��T� $ timelineItem��$
 Fill�$	 0.00:00:00.05D�..�i� $
 0��8     Pressed��. �� .��T� $ timelineItem��$
 Fill�$	 0.00:00:00.05D�..�i� $
 0��8  ._��� $ timelineItem��$��Center=�$��Center=�$��20��$��20��
���r��� !� 8  $
 Fill=��� "  .���  � ��$ 0 5 0 0q���  � �� �� "  .R��� $��30��$��20��   CommonStates��.   
Normal��  	MouseOver��. �� .��T� $ timelineItem��$
 Fill�$	 0.00:00:00.05D�..�i� $
 0��8     Pressed��. �� .��T� $ timelineItem��$
 Fill�$	 0.00:00:00.05D�..�i� $
 0��8  ._��� $ timelineItem��$��Center=�$��Center=�$��30��$��20��
��� � � !� 8  $
 Fill=��� "  .���  � ��$ 0 5 0 0q���  � �� �� "  .R��� $��30��$��20��   CommonStates��.   
Normal��  	MouseOver��. �� .��T� $ timelineItem��$
 Fill�$	 0.00:00:00.05D�..�i� $
 0��8     Pressed��. �� .��T� $ timelineItem��$
 Fill�$	 0.00:00:00.05D�..�i� $
 0��8  ._��� $ timelineItem��$��Center=�$��Center=�$��30��$��20��
���r��� !� 8  $
 Fill=��� "  .���  � ��$ 0 5 0 0q���  � �� �� "  .R��� $��20��$��30��   CommonStates��.   
Normal��  	MouseOver��. �� .��T� $ timelineItem��$
 Fill�$	 0.00:00:00.05D�..�i� $
 0��8     Pressed��. �� .��T� $ timelineItem��$
 Fill�$	 0.00:00:00.05D�..�i� $
 0��8  ._��� $ timelineItem��$��Center=�$��Center=�$��20��$��30��
������ !� 8  $
 Fill=��� .R��� $��1q�
��� � �._�� $	��140��$��70��.+�  $
��Left=�$��30��$��30������ *AddEmployee+ �� 8   $
 Data� �� " F�.���  � ��
 � � ���  �@��Stretch$ UniformToFill=���  � ��$ Center=���  � ��$ Center=��� .R��� $��1q�
����  �._�� $	��140��$��70�� �� $��60 ��� �� $��20 �.+��� $��Assets/placeholder.png��$��50��$��50��$
��Left=��� $��1��.+��� 
���   �$��15(�$��1��$��Center=�$ 0,0,54,0q����� *Employee.LastName+�� 
���   �$��15(�$��1��$��Center=�$ 0,0,54,0q����� *Employee.Name+� $��2�� �� $
��Auto ��� $
��Auto ��� $��* �.+�  $��0��$��20��$��20������ *RemoveEmployee+����  �� 8   $
 Data� �� " F�.���  � ��
 �  ����  �@��Stretch$ UniformToFill=���  � ��$ Center=���  � ��$ Center=�  $��1�� �� *Employee.IsDriver+    $! Checked��" . # �� *DriverCommand+$ ��   $! 	Unchecked��" . # �� *PasangerCommand+$ �� �� .R��� $��1q�
��� � �._�� $	��140��$��70��.+�  $
��Left=�$��30��$��30������ *AddCar+ �� 8   $
 Data� �� " F�.���  � ��
 � � ���  �@��Stretch$ UniformToFill=���  � ��$ Center=���  � ��$ Center=��� .R��� $��1q�
����  �._�� $	��140��$��70�� �� �� $��20 �.+��� $��0��.+��� 
���   �$��15(�$��1��$��Center=�$ 0,0,54,0q����� *Car.Registration+�� 
���   �$��15(�$��1��$��Center=�$ 0,0,54,0q����� *Employee.Name+� $��2�� �� $
��Auto ��� $��* �.+�  $��0��$��20��$��20������ *	RemoveCar+����  �� 8   $
 Data� �� " F�.���  � ��
 �  ����  �@��Stretch$ UniformToFill=���  � ��$ Center=���  � ��$ Center=��� .R�� .+��� $��0q�$% 2��$& 2��
�������$	' 0.7Q��� $( 
Horizontal=�$��8,8,8,12q�.+��� $��Segoe UI)�$	��Iz ���� $��Segoe UI)����� *Departure.Origin.PointName+�� $��Segoe UI)�$	�� v ���� $��Segoe UI)����� *!Departure.Destination.PointName+�� $��Segoe UI)�$�� dne ���� $��Segoe UI)����� *Departure.DepartTime+$) dd.MM.yyyy hh:mm:ss���� .R�  8��  �� .R�  8��  �� .R�  8��  �� .R�  8��  �� .R�  8��  �� .R�  8��  