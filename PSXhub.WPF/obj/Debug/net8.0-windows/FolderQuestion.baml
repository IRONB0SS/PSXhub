   M S B A M L   `   `   `  ����  
PSXhub.WPF    PSXhub.WPF.FolderQuestion   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   H DPanuon.WPF.UI, Version=1.2.4.8, Culture=neutral, PublicKeyToken=null-pu$https://opensource.panuon.com/wpf-ui  5   	     � WindowStartupLocation$  CenterScreen=�5   	   $	��450��5	   	   $	��600��6   ���#151b23
������� � WindowStyle$ SingleBorderWindow=�5   	    � 
ResizeMode$ NoResize=�5   	   ���#1e4273$��2q�5   	   ��5   	   �� *Font+ �� Mode$ TwoWay=�'  !System.Windows.Shell.WindowChrome   WindowChrome 5        5   
      GlassFrameThickness$ 0q�6      CaptionHeight$ 0Q�6/      CornerRadius$ 4��6A   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   6,   �� 5      $��* �6   6)   �� 5      $
��Auto �6   6,   .+�5   
   � $��0��6   ���#121d2f	 � ColumnDefinitions	 5      �� 5      $��* �6#   6/   .+�5      �� $��0��6   
 �� Orientation$
 
Horizontal=�6)   $��10,20q�5      .+�5       �� $
��Bold"�5!      $��14(�6'   Choose Browser for AutoFinder5#      5$      5%      � 5&   
   $��1��6   ���#0d1017	 5'      �� 5(      $��* �6#   6/   �� 5)      $��* �6#   6/   .+�5+      �� -   5        $��OldBrowserBorder��5,   :   $��0��5+      ���#1e4273$	��1.2q�6;   $��
20,15,5,15q�5,       �� CornerRadius$ 4��6)   ._�5.      �  5/      �� 50      $
��Auto �6(   68   �� 51      $��* �6(   65   .+�53      �� $��0��6   ���#121d2f$ 4,4,0,0��54      ���#1e4273$��	0,0,0,1.2q�56      ._�57      	� -   5        $��OldBrowserRadioButton��57   5   $��10,10q�6&    	� 	GroupName$ browser��58   &   
 � Tag$
 true��6:    d� 	IsChecked$
 trueY�59   &    � Cursor$
 Hand��67   ���#3b3b3b���#9198a1���#858585$	��1.5q�5;   <   %  Panuon.WPF.UI.RadioButtonHelper   HoverBorderBrush �#acacac   CheckedBorderBrush �#99c8ff   CheckedBackground �#3b3b3b   CheckedToggleBrush �#99c8ff   CheckedForeground �#c9d1d9$��16(�5A   &   $��Medium"�5B   &   .��6:   Old Browser6G   5C      ]� 5D      $��1��6    ���#151b23.+�5E      �� $��Images/OldBrowser.jpg��6    $��Center=�5F       $��Center=�6;   $
��0,10q�5G       5H      5I      5J      �� 5K      -   5        $��ModernBrowserBorder��5L   :   $��1��5K      ���#1e4273$	��1.2q�6;   $��
5,15,20,15q�5L      $ 4��6)   ._�5N      �  5O      �� 5P      $
��Auto �6(   68   �� 5Q      $��* �6(   65   .+�5S      �� $��0��6   ���#121d2f$ 4,4,0,0��5T      ���#1e4273$��	0,0,0,1.2q�5V      ._�5W      	� -   5        $��ModernBrowserRadioButton��5W   5   $��10,10q�6&   $ browser��5X   &   $ false��6:   $ falseY�5Y   &   $
 Hand��68   ���#3b3b3b���#9198a1���#858585$	��1.5q�5[   <    �#acacac �#99c8ff �#3b3b3b �#99c8ff �#c9d1d9$��16(�5a   &   $��Medium"�5b   &   .��6:   Modern Browser6J   5c      ]� 5d      $��1��6    ���#151b23.+�5e      �� $��Images/ModernBrowser.jpg��6    $��Center=�5f       $��Center=�6;   $
��20,0q�5g       5h      5i      5j      5k      � 5l   
   $��2��6   .+�5m      �� $
 
Horizontal=�6   $��Right=�62   .+�5n      �� -   5        $
��Save��5z   C   $
��Save��5n      $��10q�5o      $
��10,5q�5p      $
��Left=�5q      $��Center=�5r      ���#238636���#FFFFFF ^� ShowOnDisabled$
 True��5u      $��14(�5v      $��1q�5w      ���#3f9b4f$
 Hand��5y         Panuon.WPF.UI.ButtonHelper   HoverBackground �#29903b   CornerRadius$ 4��5{      $��Medium"�5|      5}      5~      5      5�      