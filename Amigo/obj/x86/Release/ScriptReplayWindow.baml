   M S B A M L   `   `   `  ���� 	  Amigo    Amigo.ScriptReplayWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     -   $��replay_window��.��� .+�]� $��Stretch=�$	��550��.+��� $
��Left=�   BorderStyle#���   $	��300��$	��300��._��� $��Stretch=�$
��Auto��$��Center=�  BreadCrumbTabControl#���  $��Stretch=�$��Stretch=�
������ .@��� $��Stretch=�$'��! Chosen Script (Set Of Requests) ��  LeftTabItem#���  $��16(�
������ .��R� -   $��script_root_tree1��
�������
������ $��12(�.@�Q� -   $��verify_script_tree��
�������  Q� 
IsExpanded  . ����  �� Orientation$ 
Horizontal=�.+��� $.��(pack://application:,,,/icons/session.png��$��20��$��20���� -   $��recording_session_name��
�����������  �� 
TargetType" Q�.�-   �� -   $��center_pane��#���   $��Stretch=�._��� $ Vertical=�.+��� #���   $��Stretch=�$��Center=�._��� $ 
Horizontal=�$��45��.+��� -   $��test_now_button��  
BlueButton#���  
�������$��40��$��50,0,0,0q�{��� *test_now_button_status+ �� Mode$ TwoWay=�.���� $-��'pack://application:,,,/icons/replay.png��$��40��$��40���� -   $��stop_now_button��#���  
�������$��40��$��50,0,0,0q�{��� *stop_now_button_status+$ TwoWay=�.���� -	    ��Name$ 
stop_image��$+��%pack://application:,,,/icons/stop.png��$��40��$��40���� $��Center=�$��50,0,0,0q�._��� .+��� -
   $��
Run Slowly��$��14(�$��Center=�
������� d� 	IsChecked$
 TrueY��� -   $��Enhance Cookie handling��$��14(�$��Center=�
�������$
 TrueY��� ���	#60000000$��50,0,0,0q�._�� $��1q� � ColumnDefinitions �� $��* ��� $��* �.+��� -   $��iteration_text_box�� �� TextAlignment$ Right=�$��20��$��50��$��0��$��1�� �� 
IsReadOnly �� *stop_now_button_status+$ TwoWay=��� $ Vertical=�$��1��$��Center=�$��5,0,0,0q�.+��� -   	 ��Name$	 Increase��{��� *test_now_button_status+$ TwoWay=�.���� $)��#pack://application:,,,/icons/up.png��$��15��$��15���� -   $	 Decrease��{��� *test_now_button_status+$ TwoWay=�.���� $+��%pack://application:,,,/icons/down.png��$��15��$��15��� .+�� -   $��ItrProgressBar��$	��150��$��30��$��20,0,0,0q��� -   $��	itrNumber��$��Processing Iteration: ��
�������$��14(�$��Center=�$��20,0,0,0q��� #���   $��Stretch=�._�(  "System.Windows.Controls.WebBrowser  -   $��preview_browser��$��10,10,10,10q�
 � UseLayoutRounding
 . $	��400���� #���   $��Stretch=�._��� $ Vertical=�.+��� -   $��log_text_box��$��10,10,10,10q�$ �� HorizontalScrollBarVisibility$ Disabled=�" �� VerticalScrollBarVisibility$
 Auto=�