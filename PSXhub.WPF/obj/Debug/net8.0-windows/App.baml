   M S B A M L   `   `   `  ����  
PSXhub.WPF    PSXhub.WPF.App   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      H DPanuon.WPF.UI, Version=1.2.4.8, Culture=neutral, PublicKeyToken=null-pu$https://opensource.panuon.com/wpf-ui  5        �� 	Resources  5       �� MergedDictionaries'  !Panuon.WPF.UI.MessageBoxXSettings   Key   OKButtonContent   YesButtonContent   NoButtonContent   CancelButtonContent   WindowXStyle �� 
TargetType  Panuon.WPF.UI.WindowX  Panuon.WPF.UI.MessageBoxX   WindowXStyleKey	 �� BasedOn
 �� Property �� Value&   Panuon.WPF.UI.WindowXModalDialog   ButtonStyle   ButtonStyleKey   Panuon.WPF.UI.ButtonHelper �� Triggers P� Property P� Value   ContentTemplate �� TextWrapping �� 
IsReadOnly �� RelativeSource �� AncestorType �� Mode&   Panuon.WPF.UI.PendingBoxSettings   WindowStyle  Panuon.WPF.UI.PendingBox   WindowStyleKey"  Panuon.WPF.UI.WindowXCaption   	SpinStyle	  Panuon.WPF.UI.Spin   SpinStyleKey   ContentTemplate � RowDefinitions 	 Name ��Name �� TextAlignment  ��Name-
  'Panuon.WPF.UI.Resources.StyleDictionary1  +Panuon.WPF.UI.Resources.StyleDictionaryBase!   Includes�� 5   
    5      �� 5	        5
      messageSetting  $ OK��6D   $	 YES��6Y   $ NO��6p   $ CANCEL��6�    5      �� 5      "  6$   #	 [" 6<   .�5   "   �� 
 �@�SizeToContent$ Manual=�6B   �� 5   "   
 � ��$	 380��6:   �� 5   "   
 � ��$	 150��6;   �� 5   "   
 � �� �	#FF4D50576?   �� 5   "   
 � �� �#FFFFFF6?   �� 5   "   &
 �@ ButtonPanelHorizontalAlignment$ Center=�6i   5       5      �� 5      " ��6$   #	 [" 68   .�5   "   �� 
 �@ CornerRadius$ 4��6Q   �� 5   "   
 � ��$ 80��6:   �� 5   "   
 � ��$ 30��6;   �� 5   "   
 � ��$ 5q�6;   �� 5   "   
 � ��$ 12(�6=   �� 5   "   
 � ��$
 Bold"�6?   �� 5   "   
 � �� �#FFFFFF6?   �� 5   "   
 � �� �	#FF43464C6?    5   "   P� 5    &    �@��	IsDefault$
 True��6C   . �5!   *   �� 
 � �� �#FFFFFF6G   �� 5"   *   
 � �� �	#FF43464C6G   5#   '   5%       5'      �� 5(      .R�5)   "   � .+�5*   &   �� $
 Wrap=�5+   .   $��14(�6B   $
 True��6P   
������ $��0q�6{   $��Center=�6�   ��5*   .   �� *	Message+��6G   �� *
Foreground+ ��  
pu:WindowX$ OneWay=�5+   �   5,   #   5-      5/        50      pendingSettings   51      �� 52      " �6$   #	 [" 53   $   .�54   "   �� 
 � ��$ 
20,0,20,20q�55   )   �� 56   "   
 � ��$	 300��57   )   �� 58   "   
 � ��$	 180��59   )   �� 5:   "   
 � ��$	 300��5;   )   �� 5<   "   
 � ��$	 180��5=   )   �� 5>   "   
 �@�SizeToContent$ Manual=�5?   )   �� 5@   "   
 � ��
 �����5A   )   �� 5B   "   
 �@ 
Background
 ���� 5C   )   �� 5D   "   
 � ��
 �����5E   )   �� 5F   "   
 � ��$ 1q�5G   )   �� 5H   "   
 � �� �#1E1E1E5I   )   5J       5L      �� 5M      " 	 6$   #	 [" 5N   $   .�5O   "   �� 
 �@	 	SpinStyle$ Ring2=�5P   )   �� 5Q   "   
 � ��$ 50��5R   )   �� 5S   "   
 � ��$ 50��5T   )   �� 5U   "   
 � ��$ 0,0,0,20q�5V   )   �� 5W   "   
 �@	 	GlyphSize$ 2Q�5X   )   �� 5Y   "   
 �@	 
GlyphBrush �#1ABBEA5Z   )   5[       5]      �� 5^      .R�5_   "   � $��Center=�6'    5`   &   �� 5a   *   $
��Auto �68   �� 5b   *   $
��Auto �68   �� 5c   *   �� 5d   *   $
��Auto �68   .+�5f   &   	  $ 	PART_Spin��6.   �� 5g   &   $ PART_CaptionTextBlock��60   $��1��5h   0   $��5,5,0,0q�5i   0   $ Center=�5j   0   �� 5k   &   $ PART_MessageTextBlock��60   $��2��5l   0   $
��Wrap=�5m   0   $ Center=�5n   0   �� 5o   &   $  PART_CancelButton��6-   $��3��5p   -   $y�	Collapsed=�5q   -   $��Center=�5r   -   ��5s   -   Z� 5t   #   5u      5w      5|      
  5}      $	! All=�6%   %    5      5�      