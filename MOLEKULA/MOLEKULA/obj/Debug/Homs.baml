   M S B A M L   `   `   `  ����C  ?MOLEKULA, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:MOLEKULAMOLEKULA  5   	       MOLEKULA.Homs   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:MOLEKULA     5   	   b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   -   5        $	��575��5      $	��900��6'   $��0q�63   $��0q�6>     � WindowStartupLocation$  CenterScreen=�5   	    � ShowActivated .   � Title$ MOLECULE��5	        � Topmost .  � 
ResizeMode$ NoResize=�5
   	    � AllowsTransparency .  � WindowStyle$
 None=�69      MaterialDesignBody#���   5   	   $��Regular"�5   	   $��13(�5   	   &   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�5   	      TextRenderingMode$
 Auto=�5   	     MaterialDesignPaper#���  5   	     MaterialDesignFont#���  5   	   	 � 	Resources	 5        ButtonTemplate
 �� 
TargetType �� CornerRadius � Offset � Color �� 
TargetType  BackNull �� Property �� Value��%   &	       &	 -    �� 5   
   "
 ��61   .R�5      �� ���	#FFFF7800$��3q�6-   $ 5��6A   
���������5      �� 5      ��5      � � 5   "   $ 0Q�6/   $ 	#FFFF7800��6:   � 5   "   $ 1Q�6/   $ 	#FFFF7800��6:   5      5       ._�5"      �� ��. 6#   5#      5$      �� 5%   
   " ��6   .�5&      ��  � ��$ ./Fonts/#Raider)�6+   �� 5'       � �� 5(      �� 5)      "
 ��6&   .R�5*      �� #��z��6+   5+      5-      5.      52      .��53      � ��54   
   �� 55       �� EndPoint$ 0.5,1&�6"    �� 
StartPoint$	 1,0&�63   .��56      � $ 	#FF1C0534��6   $ 0Q�61   � 57      $ 	#FF2F0A4D��6   $ 1Q�61   58      .+�5:   
   A� ���#7D52BC$��Right=�6!   $��0,185,101,120q�6=   
���   �$	��270��6c   $	��270��6o    J� Effect 5;      -  'System.Windows.Media.Effects.BlurEffect  5<      5>      A� 5?   
   ���	#66876CCE$��170,0,0,-60q�6#   ���#876CCE$	��150��6I   $
��Left=�6U   $	��150��6p   $��Bottom=�6}    5@        5A      5C      A� 5D   
   ���	#7F6240A2$��0,81,-15,424q�6#   ���#6240A2$��70��6J   $
��70  ��6U   $��Right=�6c    5E        5F      5H      A� 5I   
   ���	#66876CCE$��270,120,0,435q�6#   ���#876CCE$��20��6K   $��20��6V   $
��Left=�6b    5J        5K      5M       � 5T   
   -   5          �Name$ CONTENTS��5U      $�� ��5V      $
��Left=�5W      $��0,75,0,0q�5X      $	��Top=�5Y      $	��900��5Z      $	��490��6   $-��'/MOLEKULA;component/Fonts/#Roboto Black)�5[        � NavigationUIVisibility$ Hidden=�6B   �� 5^   
   -   5         K� ZIndex$ 10��5^      $��34��5_      $��34��6   $��Right=�5`      $��	0,16,10,0q�5a      $��0q�6    $	��Top=�5b      +  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 20��5c      .��5d      '  !MaterialDesignThemes.Wpf.PackIcon  $��30��6&   $��30��61      Kind$ Close=�6=   5e      �� 5f   
   -   5        $ 10��5f      $��34��5g      $��34��6   $��Right=�5h      $��	0,16,56,0q�5i      $��0q�6    $	��Top=�5j      $ 20��5k      .��5l        $��30��6&   $��30��61   $ WindowMinimize=�6=   5m      �� 5o   
   -   5         ��Name$ 
BorderHome��5r      $��MOLECULE.co��5o      $
��Left=�5p      $��Center=�5q      $��21(�5r      
��� ���$��Italic$�5t      $-��'/MOLEKULA;component/Fonts/#Roboto Black)�5u      $��30,20,0,529q�5v      $	��152��6"   $��26��6.   �� 5w   
   -   5        $
��Left=�5w      $��30,16,0,525q�5x      $	��147��6%    � Cursor$
 Hand��5y      $��20(�6   $��34��6,   $
��Bold"�5z      $��0q�6"      TextHintingMode$ Fixed=�5{       J� RenderTransformOrigin$ 0.5,0.5&�5|      
�������$��0q�5}   $   $��Center=�5~      
������ .��5�      �� $��MOLECULE.co��6   $��Center=�5�      $��21(�5�      
�������$��Italic$�5�      $-��'/MOLEKULA;component/Fonts/#Roboto Black)�5�      $	��142��6F   5�      �� 5�   
   -   5        $��202,15,559,526q�5�      $��34��6(   $��Microsoft Sans Serif)�5�      $
 Hand��5�      $��11(�6   $
��Bold"�5�      #��[ 6"   $ Fixed=�5�      $ 0.5,0.5&�5�      
���������5�   $   Z� .��5�      �� $��ПРОДУКТ��6   $��Center=�5�      $��Center=�5�      $��11(�5�      
�������$
 Hand��5�   +   $-��'/MOLEKULA;component/Fonts/#Roboto Black)�69   5�      5�      �� 5�   
   -   5        $��347,15,414,526q�5�      $��34��6(   $��Microsoft Sans Serif)�5�      $
 Hand��5�      $��11(�6   $
��Bold"�5�      #��[ 6"   $ Fixed=�5�      $ 0.5,0.5&�5�      
���������5�   :   Z� .��5�      �� $��КОНТАКТЫ��6   $��Center=�5�      $��Center=�5�      $��11(�5�      
�������$
 Hand��5�   +   $-��'/MOLEKULA;component/Fonts/#Roboto Black)�69   5�      5�      �� 5�   
   -	   5        $��493,16,268,525q�5�      $��34��6(   $��Microsoft Sans Serif)�5�      $
 Hand��5�      $��11(�6   $
��Bold"�5�      #��[ 6"   $ Fixed=�5�      $ 0.5,0.5&�5�      
���������5�   :   Z� .��5�      �� $��ПОМОЩЬ��6   $��Center=�5�      $��Center=�5�      $��11(�5�      
�������$
 Hand��5�   +   $-��'/MOLEKULA;component/Fonts/#Roboto Black)�69   5�      5�      �� 5�   
   -
   5         ��Name$ 
StartBuild��5�      #��[ 5�      $	��113��5�      $��34��6   $+��%/MOLEKULA;component/Fonts/#Montserrat)�5�      $
��Left=�5�      $��Bottom=�5�      $
 Hand��5�      $��
30,0,0,158q�5�      .��5�      �� $��Начать��6   $��Center=�5�      $��Center=�5�      $��20(�5�      
�������$
��Bold"�5�   $   $.��(/MOLEKULA;component/Fonts/#Roboto Medium)�66   5�      �� 5�   
   -   5        $��637,15,124,526q�5�      $��34��5�      $��Microsoft Sans Serif)�5�      $
 Hand��5�      $��11(�6   $
��Bold"�5�      $ Fixed=�5�      $ 0.5,0.5&�5�      
�������#��[ 5�      .��5�      �� $��ТАБЛИЦА��6   $��Center=�5�      $��Center=�5�      $��11(�5�      
�������$
 Hand��5�   $   $-��'/MOLEKULA;component/Fonts/#Roboto Black)�62   5�      5�      5�      