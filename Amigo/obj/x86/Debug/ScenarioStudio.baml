   M S B A M L   `   `   `  ����	  Amigo    Amigo.ScenarioStudio   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .��5      � .+�5	   
   ]� $��Stretch=�6   $	��650��60   .+�5
      �� -   5        $��
LeftBorder��5
      $
��Left=�6'      ZeroBorderStyle#���   6=   ._�5      R� -   5        $��script_root_tree��5      
�������
������   J� 	AllowDrop  . $��0q�6p   $��0,10,0,0q�6�   .@�5      Q� -   5        $��recording_sessions��5   #   
������� Q� 
IsSelected .  $��0q�6�   ��6P   �� *RecordingSessions+ �� Mode$ TwoWay=� Q� 
IsExpanded 6�   �� *
toExpanded+$ OneWay=���5      �� 5       �� Orientation$ 
Horizontal=�6)   .+�5   "   �� $6��0pack://application:,,,/icons/projects_folder.png��6(   $��30��6b   $��30��6n   (� 5   "   .��5   &   �� -   5        $��parent_check_box��5   /    d� 	IsChecked$ FalseY�6G   $��5q�6Y   {�6d   �� *
toEnable+$ OneWay=�5   #   �� 5   "   -   5        $��project_name��5   (   
�������$��16(�6|   $��Normal"�6�   ��6O   �� *ProjectName+$ OneWay=�6�   5      ��5      �� 5       �� DataType$ RecordingSession��67   .R�5   "   �� $ 
Horizontal=�6-   $��Center=�6F   $��Center=�6a   .+�5   &   �� $.��(pack://application:,,,/icons/session.png��6,   $��22��6^   $��22��6j   $��15,0,0,0q�6u   �� 5   &   
�������$��5,0,0,0q�6q   $��14(�6�   $��Normal"�6�   ��6?   ��  �� Path$ SessionName�$ TwoWay=�6�   5   #   5      5      5      5       �� 5!      $
��Left=�6     BorderStyle#���  6+   $��35��6]   ._�5"      �� $ Vertical=�6   $��Center=�64   $��2q�6O   .+�5#      �� -   5        $��button_right_arrow��5#        
BlueButton#���  67   
�������{�6o   �� *
toEnable+$ OneWay=�.��5$      ]� .+�5%      �� $9��3pack://application:,,,/icons/arrow_double_right.png��6$   $��35��6a   5&      5'      5(      5)      �� 5*      -   5        $��RightBorder��5*      $��Right=�6(   #���  6?   ._�5+      �� .��5,      �� $ Vertical=�6!   .+�5-      R� -   5        $��scenario_root_tree��5-   #   
�������
������   . $��0q�5.   #   $��0,10,0,0q�67   $	��600��6I   .@�5/      Q� -	   5        $��scenario_tree��5/   +   
������� .  $��0q�6�    � ToolTip$ Expand to Open Scenarios��50   M   ��5/   S   �� *	Scenarios+$ TwoWay=� 6�   �� *
toEnable+$ OneWay=�{�50   p   �� *
toEnable+$ OneWay=���51   "   �� 52   &   $ 
Horizontal=�61   .+�53   *   �� $/��)pack://application:,,,/icons/scenario.png��60   $��30��6c   $��30��6o   �� 54   *   -
   5        $��scenarios_parent_lable��54   0   
�������$��Scenarios List��6a   $��16(�6z   $��Normal"�6�   55   '   ��57   "   �� 58   &   $ 	Scenarios��6?   .R�59   *   �� $ 
Horizontal=�65   $��Center=�6N   $��Center=�6i   $��	-15,0,0,0q�6�   .+�5:   .   �� $/��)pack://application:,,,/icons/scenario.png��64   $��20��6g   $��20��6s   $��1,0,0,0q�6~   �� 5;   .   
�������$��2,0,0,0q�6z   $��14(�6�   $��Normal"�6�   ��6G   �� $ ScenarioName�$ TwoWay=�6�   5<   +   5=   '   5?      5@      5A      5B      5C      �� 5D      #���   6   ._�5E      �� $ Vertical=�6   $	��Top=�64   .+�5F      �� -   5        $��scenario_stack_panel��5F   !   $ 
Horizontal=�6=   $��Center=�6�   $��0,10,1,1q�6�   {�6V   �� *
toEnable+$ OneWay=�.+�5G      �� #���  6!   
�������$��30��6Y   $��2,10,1,1q�6e   $	��130��6w   .��5H      ]� $��1q�6(   .+�5I   "   �� $+��%pack://application:,,,/icons/help.png��6(   $��28��6W   $��28��6c   6p   �� 5J   "   $�� Quick Help ��6,   $��25��6@   $��Center=�6L   $��12(�6g   $��10,5,0,0q�6u   5K      5L      �� 5M      -   5        #���  5M   !   
�������$��30��6Y   $��	20,10,1,1q�6f   $	��130��6y   .��5N      ]� $��1q�6(   .+�5O   "   �� $2��,pack://application:,,,/icons/new_project.png��6(   $��28��6^   $��28��6j   6w   �� 5P   "   $�� New Scenario ��6,   $��25��6B   $��Center=�6N   $��12(�6i   $��0,5,0,0q�6w   5Q      5R      �� 5S      -   5        #���  5S   !   {�.  
�������$��30��6k   $��	20,10,1,1q�6w   $	��130��6�   .��5T      ]� $��1q�6(   .+�5U   "   �� $.��(pack://application:,,,/icons/runtime.png��6(   $��28��6Z   $��28��6f   6s   �� 5V   "   $�� Runtime Settings ��6,   $��25��6F   $��Center=�6R   $��12(�6m   $��0,5,0,0q�6{   5W      5X      �� 5Y      -   5        #���  5Y   !   
�������$��30��6�   $��	20,10,1,1q�6�   $	��170��6�   {�6F   �� *" isEnableInitializeLoadTestButton+$ TwoWay=�.��5Z      ]� $��1q�6(   .+�5[   "   �� $1��+pack://application:,,,/icons/start_load.png��6(   $��28��6]   $��28��6i   6v   �� 5\   "   $�� Initialize Load Test ��6,   $��25��6J   $��Center=�6V   $��12(�6q   $��0,5,0,0q�6   5]      5^      5_      �� 5`      $��5,20,5,5q�6!   .+�5a      &   System.Windows.Controls.DataGrid  -   5        $��scenario_data_grid��5a   #   	   AlternatingRowBackground
	 ��ح�
   AutoGenerateColumns
 .  
������    ColumnHeaderHeight$ 50Q�5f   -      CanUserReorderColumns .     CanUserAddRows .     CanUserResizeColumns .    CanUserResizeRows .  $   HorizontalScrollBarVisibility$ Disabled=�5k   -   "   VerticalScrollBarVisibility$
 Auto=�5l   -   $	��550��5m   -   $��13(�5n   -   
������ $��0q�5o   G   $��Normal"�6[   ��5c   -   �� *	Scripts+$ TwoWay=�{�5n   ;   �� $ toEnable�$ TwoWay=�   Columns 5q      0  *System.Windows.Controls.DataGridTextColumn  5r   "   ,  &System.Windows.Controls.DataGridColumn   Header$ Script Name��6Y      Width5  /System.Windows.Controls.DataGridLengthConverter$	 140 6n      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 65   �� $ 
ScriptName�6z     5s   "   $ Start After��6Y    65   �� $ 
StartAfter�6Y     5t   "   $ No. / % Of Users��6\    65   �� $ NumberOfUsers�6\     5u   "   $ 
No.of Itrs��6a    65   �� $ NumberOfIterations�6a     5v   "   $ Itr Delay Min��6g    65   �� $ DelayBetweenIterationMin�6g     5w   "   $ Itr Delay Max��6g    65   �� $ DelayBetweenIterationMax�6g     5x   "   $ Think Time Min��6[    65   �� $ ThinkTimeMin�6[     5y   "   $ Think Time Max��6[    65   �� $ ThinkTimeMax�6[   5{      5|      5}      5~      5      5�      5�      