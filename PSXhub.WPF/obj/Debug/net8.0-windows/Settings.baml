   M S B A M L   `   `   `  ����E  APSXhub.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:PSXhubPSXhub  5   	   5clr-namespace:PSXhub.PropertiesPSXhub.Properties  5   	       PSXhub.WPF.Settings   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:PSXhub     5   	   H DPanuon.WPF.UI, Version=1.2.4.8, Culture=neutral, PublicKeyToken=null-pu$https://opensource.panuon.com/wpf-ui  5   	   0
propertiesclr-namespace:PSXhub.Properties  5   	     � WindowStartupLocation$  CenterScreen=�5
   	   $	��400��5   	   $	��480��6   ���#F0F6FC���	#00ffffff � AllowsTransparency$
 True��5   	    � WindowStyle$
 None=�5   	    � 
ResizeMode$ NoResize=�5   	   $��0q�5   	   ��5   	   �� *Font+ �� Mode$ TwoWay=� � 	Resources 5         BoolToVisConverter��%   &	        �� 5   
   6'   5      '  !System.Windows.Shell.WindowChrome   WindowChrome 5        5   
      GlassFrameThickness$ 0q�6      NonClientFrameEdges$
 None=�5      	   CaptionHeight$	 0Q�5      
   CornerRadius$
 0��5         ResizeBorderThickness$ 0q�5      .��5      �� ���#3d444d$��2q�5       �� CornerRadius$ 12��5       ���#151b23._�5"   
   �  � RowDefinitions 5#      �� 5$      $
��Auto �6    60   �� 5%      $��* �6    6-   �� 5&      $
��Auto �6    60   .+�5(      �� -   5        $��Header��5*      $��0��5(      ���#3d444d$��0,0,0,2q�5)      ._�5,      � 
������  � ColumnDefinitions 5-      �� 5.      $��* �6+   67   �� 5/      $
��Auto �6+   6:   .+�51      �� $��0��6!    �� Orientation$ 
Horizontal=�61   $
��8,12q�52   !   $��Center=�6/   .+�53      �� $��16(�54      ���#F0F6FC$��SemiBold"�55      
Settings57      58      �� 59      $��1��6!   $��Center=�61   $ 
Horizontal=�5:   !   $��
0,12,10,12q�5;   !   .+�5<      �� -   5        $��Close��5=   F   
�������
������ $��25��6!   $��25��6,   $��24(�68   $��Center=�5>   !    � Cursor$
 Hand��6P   $��0q�5?   !      Panuon.WPF.UI.ButtonHelper   HoverBackground �#212830   CornerRadius$ 4��5A   !   .��5B      (� $��10��6&   $��10��61   $��0,0,0,0q�5C   &   .��5D   "   F� ���#9198A1���������� ��q���@@ H,�^� �K7�A8@@� р�XK� �fT6 � �:24 �� ��1 z ��.  � S� , ��x�@e�?��( s ��% k �zZ! � �� �a � n� ��X �x j : �� �N �t� f�! �� 2*& �}8gD�?T�* �|s v/ r� (3 �� �T6 ��J� �Q�� �K7�A8@@ӌ� ��� �St$_@@N� ;�O��v@@��[[�?�/L�
�@@�� �4�8E�@@�� `D���� �O|y U"� �&��� x�
�z T�ơ ����� ���X 8�h* $�������� @��r# Tk�& �c�x) �[�, �,�. ��̱1 �Mz84 ،fT6 XZр�3��K7�A8@@XZӤ�، ���M�j���`6���4�8E�@@�[�:p��@@�cӬOTk�T���&$��x�
8���������H�T�8�x�
���&Ӭ�UTk�O c`D�SP[�4�8E�@@��/L�
�@@��;�O��v@@SHM�St$_@@t����Y�K7�A8@@�3���Y�T6 �L��"3 t��/ x��~* ���l�% �hZH! ��� �,�~� ���# H�� �$ ���� ����� 	z�5F   &   �� 5G   *    �� ScaleX$ 1Q�69    �� ScaleY$ 1Q�6D   5I   #   5J      5K      5L      5M      5N      �� 5O      $��1��6   .��5P      �� $ Vertical=�6   $��20,10q�64   .+�5Q      � $	��0,5q�6     � Orientation$ Vertical=�5R       .+�5S      �� -   5        $$��DownloadByInternetInAutoFinder��5^      ���#F0F6FC���#828388���#3c3a3b"  Panuon.WPF.UI.CheckBoxHelper   CornerRadius$ 2��5W         CheckedBackground �#1f6feb   CheckedGlyphBrush �#fff   GlyphThickness$	 1.5Q�5Z      $
 Hand��5[      $��14(�5\      $��SemiBold"�5]       d� 	IsChecked 5_      �� * DownloadByInternetInAutoFinder+$ TwoWay=�.��6_   )'Enable Internet Download for AutoFinder5a      �� 5b      $��12(�6$   ���#9198A1$��23,5,0,0q�5d   $   $
��Wrap=�5e   $    �� TextAlignment$ Justify=�5f   $   ��Allows PS4/PS5 to download missing PKG files from the Sony servers during transfer (this process requires an active internet connection and may use significant internet data).5h      5i      �� 5j      $��Stretch=�6!   $��1��5k   !   ���#3d444d$
��0,10q�5m   !   61   � 5n      $	��0,5q�6    $ Vertical=�5o       .+�5p      �� -   5        $"��DownloadByInternetInDatabase��5{      ���#F0F6FC���#828388���#3c3a3b$ 2��5t       �#1f6feb �#fff$	 1.5Q�5w      $
 Hand��5x      $��14(�5y      $��SemiBold"�5z       5|      �� *DownloadByInternetInDatabase+$ TwoWay=�.��6]   -+Enable Internet Download for local Database5~      �� 5      $��12(�6$   ���#9198A1$��23,5,0,0q�5�   $   $
��Wrap=�5�   $   $ Justify=�5�   $   ��Allows PS4/PS5 to download missing PKG files from the Sony servers during transfer (this process requires an active internet connection and may use significant internet data).5�      5�      �� 5�      $��Stretch=�6!   $��1��5�   !   ���#3d444d$
��0,10q�5�   !   61   � 5�      $	��0,5q�6    $ Vertical=�5�       .+�5�      �� -   5        $��ConfirmScanDatabaseActive��5�      ���#F0F6FC���#828388���#3c3a3b$ 2��5�       �#1f6feb �#fff$	 1.5Q�5�      $
 Hand��5�      $��14(�5�      $��SemiBold"�5�       5�      �� *ConfirmScanDatabaseActive+$ TwoWay=�.��6Z    Ask Before Refreshing Database5�      �� 5�      $��12(�6$   ���#9198A1$��23,5,0,0q�5�   $   $
��Wrap=�5�   $   $ Justify=�5�   $   USEnable this option to receive a confirmation prompt before refreshing the database.5�      5�      �� 5�      $��Stretch=�6!   $��1��5�   !   ���#3d444d$
��0,10q�5�   !   61   � 5�      $	��0,5q�6    $ Vertical=�5�       .+�5�      �� -   5        $
��Tray��5�      ���#F0F6FC���#828388���#3c3a3b$ 2��5�       �#1f6feb �#fff$	 1.5Q�5�      $
 Hand��5�      $��14(�5�      $��SemiBold"�5�       5�      �� *
ActiveTray+$ TwoWay=�.��6K   Show Tray icon5�      �� 5�      $��12(�6$   ���#9198A1$��23,5,0,0q�5�   $   B@Keeps the app running in the background with a system tray icon.5�      5�      �� 5�      $��Stretch=�6!   $��1��5�   !   ���#3d444d$
��0,10q�5�   !   61   � 5�      $	��0,5q�6    $ Vertical=�5�       .+�5�      �� -   5        $��Update��5�      ���#F0F6FC���#828388���#3c3a3b$ 2��5�       �#1f6feb �#fff$	 1.5Q�5�      $
 Hand��5�      $��14(�5�      $��SemiBold"�5�       5�      �� *UpdateAutomatically+$ TwoWay=�.��6T   Update automatically5�      �� 5�      $��12(�6$   ���#9198A1$��23,5,0,0q�5�   $   version 1.00 protable5�      �� 5�      -   5         ��Name$ CheckUpdate��5�      $��30��5�      $��0,10,0,0q�5�      $
��Left=�5�      $��Center=�5�      ���#222931���#FFFFFF$��14(�5�      $��Regular"�5�      $��1q�5�      ���#3d444d$
 Hand��5�      $ 4��5�         ShadowColor 5�      Z� y�5�      �� *ShowUpdateBtn+ �� 	Converter# [  $ TwoWay=�.��6~   Check for Update5�      5�      �� 5�      $��Stretch=�6!   $��1��5�   !   ���#3d444d$
��0,10q�5�   !   61   � 5�      $	��0,5q�6    $ Vertical=�5�       .+�5�      �� -	   5        $��Debugger��5      ���#F0F6FC���#828388���#3c3a3b$ 2��5�       �#1f6feb �#fff$	 1.5Q�5�      $
 Hand��5�      $��14(�5�      $��SemiBold"�5�       5     �� *
Debugger+$ TwoWay=�.��6I   
Debugger5     �� 5     $��12(�6$   ���#9198A1$��23,5,0,0q�5  $   IGProvides detailed logs when crashes or errors occur to help fix issues.5     5	     �� 5
     $��Stretch=�6!   $��1��5  !   ���#3d444d$
��0,10q�5  !   61   � 5     $	��0,5q�6    $ Vertical=�5      $��Center=�5      .+�5     �� $��14(�6$   $��SemiBold"�5  $   
Language5     �� 5     -
   5         ��Name$	 IPs��5  #   $	��100��5  #   $��30��5  #   $��0,10,0,0q�5  #   $
��Left=�5  #   $��Center=�5  #   ���#222931���#FFFFFF$��14(�5  #   $��Regular"�5   #   $��1q�5!  #   ���#3d444d"  Panuon.WPF.UI.ComboBoxHelper    CornerRadius$  4��5#  #   !   ItemsSelectedBackground! �#222931"   ItemsHoverBackground" �#252c34# �� DisplayMemberPath$# Title��5)  #   "  Panuon.WPF.UI.DropDownHelper$   ShadowColor$ 5&  #   Z� ��5'  #   �� *	Languages+% �� SelectedItem% 5(  #   �� *
Language+$ TwoWay=�5)  #   5*     5+     5,     �� 5-     $��2��6   ���#3d444d$��0,2,0,0q�5.     & �� Padding$
& 5,10q�5/     ._�50     �� $��Right=�6   $ 
Horizontal=�51     .+�52     �� -   5        $��Cancel��5?     $	��0,8q�53     $��Cancel��54     $��80��55     $��35��56     ���#222931���#FFFFFF$��14(�59     $
 Hand��5:     $��1q�5;     ���#3d444d$ 5��5=      �#252c345?  '   �� 5@     -   5        $
��Save��5K     $��10q�5A     $
��Save��5B     $��80��5C     $��35��5D     ���#238636���#FFFFFF$��14(�5G     $��0q�5H     $
 Hand��5I     $ 5��5J     5L     5M     5N     5O     5P     5Q     