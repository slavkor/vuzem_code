   M S B A M L   `   `   `  ���� O  KIsm.BusinessPartners, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:Ism.BusinessPartners.ViewsIsm.BusinessPartners.Views  M IIsm.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nulliGclr-namespace:Ism.Infrastructure.Converters;assembly=Ism.InfrastructureIsm.Infrastructure.Converters 8    2Ism.BusinessPartners.Views.BusinessPartnerOverView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     :xctk)http://schemas.xceed.com/wpf/xaml/toolkit     :local(clr-namespace:Ism.BusinessPartners.Views     c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  P LPrism.Wpf, Version=6.3.0.0, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59$prismhttp://prismlibrary.com/  X
convertersGclr-namespace:Ism.Infrastructure.Converters;assembly=Ism.Infrastructure  n jTelerik.Windows.Controls.GridView, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a	 ]Telerik.Windows.Data, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k
 gTelerik.Windows.Controls.Input, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2019.1.220.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Etelerik1http://schemas.telerik.com/2008/xaml/presentation  	 
  !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel  .    UcStyle#��[   � 	Resources  �� MergedDictionaries �� Source  addPath �� Stretch  
removePath��  �� $X Rpack://application:,,,/Ism.Infrastructure;component/Resources/BaseButtonStyle.xaml?�%W  &	       &	 �    F� $
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
 Fill=����	#FF000000|��������<R��C�Z��@��=R��m5�����O���C��m5��Ѥ���n5��C0P������n5�H<R�����Z���CѬ<R\�������<R.��� .  (System.Windows.Interactivity.Interaction   Triggers F  @Prism.Interactivity.InteractionRequest.InteractionRequestTrigger  3  -System.Windows.Interactivity.EventTriggerBase   SourceObject �� *BusinessPartnerListRequest+ �� Mode$ OneWay=�.  (System.Windows.Interactivity.TriggerBase   Actions. +  %Prism.Interactivity.PopupWindowAction  	   IsModal	 . 
   WindowStartupLocation$
 CenterOwnerX�  CustomWindowStyle   WindowStyle# [    WindowContent ;   5Ism.BusinessPartners.Views.BusinessPartnersSelectList   � RowDefinitions �� $
��Auto ��� $��* �.+�]� $��0����.  .+�,  &Telerik.Windows.Controls.RadPathButton  $
��Left=�$��30��$��30��$��foreman������ *SelectBusinessPartner+   PathGeometry ��  �� Source# [  �� Path$
 Data�   	PathStyle ��  �� 
TargetType" F�.���  �� Property � �� �� Value
 � � ���  �@��Stretch$ UniformToFill=���  � ��$ Center=���  � ��$ Center=�  $
��Left=�$��30��$��30��$��foreman������ *RemoveBusinessPartner+ �� # [ $
 Data� �� " F�.���  � ��
 �  ����  �@��Stretch$ UniformToFill=���  � ��$ Center=���  � ��$ Center=��� $��1��.+��� ���� *BusinessPartner.Name+�� ���� *BusinessPartner.LastName+