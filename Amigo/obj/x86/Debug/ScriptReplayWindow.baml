   M S B A M L   `   `   `  ����	  Amigo    Amigo.ScriptReplayWindow   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -   5        $��replay_window��5      .��5      � .+�5   
   ]� $��Stretch=�6   $	��550��60   .+�5	      �� $
��Left=�6      BorderStyle#���   6+   $	��300��6]   $	��300��6l   ._�5
      �� $��Stretch=�6   $
��Auto��69   $��Center=�6G     BreadCrumbTabControl#���  6i   $��Stretch=�6�   $��Stretch=�6�   
������ .@�5      �� $��Stretch=�6   $'��! Chosen Script (Set Of Requests) ��6A     LeftTabItem#���  6l   $��16(�6�   
������ .��5      R� -   5        $��script_root_tree1��5   #   
�������
������ $��12(�6h   .@�5      Q� -   5        $��verify_script_tree��5   +   
�������  Q� 
IsExpanded  . ��5   "   �� 5   &    �� Orientation$ 
Horizontal=�61   .+�5   *   �� $.��(pack://application:,,,/icons/session.png��60   $��20��6b   $��20��6n   6{   �� 5   *   -   5        $��recording_session_name��5   0   
�������6c   5   '   5      ��5      �� 5   "    �� 
TargetType" Q�6(   .�5   &   -   5        5   #   5      5      5      5      �� 5      -   5        $��center_pane��5      #���   6(   $��Stretch=�6Z   ._�5      �� $ Vertical=�6   .+�5       �� #���   6   $��Stretch=�6O   $��Center=�6m   ._�5!      �� $ 
Horizontal=�6%   $��45��6>   .+�5"      �� -   5        $��test_now_button��5"   %     
BlueButton#���  6<   
�������$��40��6t   $��50,0,0,0q�6�   {�6�   �� *test_now_button_status+ �� Mode$ TwoWay=�.��5#   "   �� $-��'pack://application:,,,/icons/replay.png��6(   $��40��6Y   $��40��6e   6r   5$      �� 5%      -   5        $��stop_now_button��5%   %   #���  6<   
�������$��40��6t   $��50,0,0,0q�6�   {�6�   �� *stop_now_button_status+$ TwoWay=�.��5&   "   �� -	   5         ��Name$ 
stop_image��5&   (   $+��%pack://application:,,,/icons/stop.png��6<   $��40��6k   $��40��6w   6�   5'      �� 5(      $��Center=�6%   $��50,0,0,0q�6@   ._�5)   "   �� .+�5*   &   �� -
   5        $��
Run Slowly��5*   /   $��14(�6|   $��Center=�6�   
������� d� 	IsChecked$
 TrueY�6�   �� 5+   &   -   5        $��Enhance Cookie handling��5+   /   $��14(�6�   $��Center=�6�   
�������$
 TrueY�6�   5,   #   5-      �� 5.      ���	#60000000$��50,0,0,0q�6>   ._�5/   "   � $��1q�6'    � ColumnDefinitions 50   &   �� 51   *   $��* �6;   �� 52   *   $��* �6;   .+�54   &   �� -   5        $��iteration_text_box��54   .    �� TextAlignment$ Right=�6H   $��20��6^   $��50��6j   $��0��6u   $��1��6�    �� 
IsReadOnly 6�   �� *stop_now_button_status+$ TwoWay=��� 55   &   $ Vertical=�61   $��1��6H   $��Center=�6X   $��5,0,0,0q�6s   .+�56   *   �� -   5        	 ��Name$	 Increase��56   1   {�6Z   �� *test_now_button_status+$ TwoWay=�.��57   .   �� $)��#pack://application:,,,/icons/up.png��64   $��15��6a   $��15��6m   58   +   �� 59   *   -   5        $	 Decrease��59   1   {�6Z   �� *test_now_button_status+$ TwoWay=�.��5:   .   �� $+��%pack://application:,,,/icons/down.png��64   $��15��6c   $��15��6o   5;   +   5<   '   5=   #   5>      � 5?      .+�5@      � -   5        $��ItrProgressBar��5@   *   $	��150��6@   $��30��6L   $��20,0,0,0q�6X   �� 5A   "   -   5        $��	itrNumber��5A   (   $��Processing Iteration: ��69   
�������$��14(�6m   $��Center=�6{   $��20,0,0,0q�6�   5B      5C      5D      �� 5E      #���   6   $��Stretch=�6O   ._�5F      (  "System.Windows.Controls.WebBrowser  -   5        $��preview_browser��5F   &   $��10,10,10,10q�6=   
 � UseLayoutRounding
 . $	��400��6k   6z   5G      �� 5H      #���   6   $��Stretch=�6O   ._�5I      �� $ Vertical=�6%   .+�5J      �� -   5        $��log_text_box��5J   &   $��10,10,10,10q�6:   $ �� HorizontalScrollBarVisibility$ Disabled=�6O   " �� VerticalScrollBarVisibility$
 Auto=�6x   5K      5L      5M      5N      5O      5P      5Q      