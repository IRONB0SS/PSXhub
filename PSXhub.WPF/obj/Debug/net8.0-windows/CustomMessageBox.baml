   M S B A M L   `   `   `  ����E  APSXhub.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:PSXhubPSXhub  5   	   !    PSXhub.WPF.CustomMessageBox   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   H DPanuon.WPF.UI, Version=1.2.4.8, Culture=neutral, PublicKeyToken=null-pu$https://opensource.panuon.com/wpf-ui  5   	    localclr-namespace:PSXhub  5   	   -   5        
������ 
�������  � 
ResizeMode$  NoResize=�5   	    � AllowsTransparency$
 True��5   	    � WindowStyle$
 None=�5   	    � Title$ 
MessageBox��5   	   $��0q�5   	   ��5   	   �� *Font+ �� Mode$ TwoWay=�'  !System.Windows.Shell.WindowChrome   WindowChrome 5        5   
      GlassFrameThickness$ 0q�6      NonClientFrameEdges$
 None=�5         CaptionHeight$ 0Q�5      	   CornerRadius$	 0��5      
   ResizeBorderThickness$
 0q�5      .��5      �� ���#3d444d$��2q�5       �� CornerRadius$ 12��5      ���#151b23._�5   
   �  � RowDefinitions 5      �� 5       $
��Auto �6    60   �� 5!      $��* �6    6-   �� 5"      $
��Auto �6    60   .+�5%      �� -   5        $��TitleBar��5'      $��0��5%      ���#3d444d$��	0,0,0,0.5q�5&      ._�5)      � 
������  � ColumnDefinitions 5*      �� 5+      $��* �6+   67   �� 5,      $
��Auto �6+   6:   .+�5.      �� $��0��6!    �� Orientation$ 
Horizontal=�61   $��10,12q�5/   !   $��Center=�60   .+�50      �� -   5        $��CTitle��52   3   $��16(�51      ���#F0F6FC$��SemiBold"�52      6C   53      �� 54      $��1��6!   $��Center=�61   $ 
Horizontal=�55   !   $��
0,12,10,12q�56   !   .+�57      �� -   5        $��Close��58   F   
�������
������ $��25��6!   $��25��6,   $��24(�68   $��Center=�59   !    � Cursor$
 Hand��6R   $��0q�5:   !      Panuon.WPF.UI.ButtonHelper   HoverBackground �#212830   CornerRadius$ 4��5<   !   .��5=      (� $��10��6&   $��10��61   $��0,0,0,0q�5>   &   .��5?   "   F� ���#9198A1���������� ��q���@@ H,�^� �K7�A8@@� р�XK� �fT6 � �:24 �� ��1 z ��.  � S� , ��x�@e�?��( s ��% k �zZ! � �� �a � n� ��X �x j : �� �N �t� f�! �� 2*& �}8gD�?T�* �|s v/ r� (3 �� �T6 ��J� �Q�� �K7�A8@@ӌ� ��� �St$_@@N� ;�O��v@@��[[�?�/L�
�@@�� �4�8E�@@�� `D���� �O|y U"� �&��� x�
�z T�ơ ����� ���X 8�h* $�������� @��r# Tk�& �c�x) �[�, �,�. ��̱1 �Mz84 ،fT6 XZр�3��K7�A8@@XZӤ�، ���M�j���`6���4�8E�@@�[�:p��@@�cӬOTk�T���&$��x�
8���������H�T�8�x�
���&Ӭ�UTk�O c`D�SP[�4�8E�@@��/L�
�@@��;�O��v@@SHM�St$_@@t����Y�K7�A8@@�3���Y�T6 �L��"3 t��/ x��~* ���l�% �hZH! ��� �,�~� ���# H�� �$ ���� ����� 	z�5A   &   �� 5B   *    �� ScaleX$ 1Q�69    �� ScaleY$ 1Q�6D   5D   #   5E      5F      5G      5H      5I      � 5K      -   5        $��Message��5K   @   $��1��6   $��Center=�6%   $��
20,10,20,5q�5L      6.   �� 5N      $��2��6    �� Padding$ 10,10q�6"   $��	0,0.5,0,0q�5O      ���#3d444d._�5Q      �� $ 
Horizontal=�5R      $��Right=�5S      .+�5T      �� -   5        $��Ok��5a      $	��0,8q�5U      $��OK��5V      $��95��5W      $
��10,5q�5X      ���#222931���#FFFFFF$��14(�5[      $
 Hand��5\      $��1q�5]      ���#3d444d$ 5��5_       �#252c345a   #   5b      5c      5d      5e      5f      