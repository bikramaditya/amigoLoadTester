   M S B A M L   `   `   `  ���� 	  Amigo    Amigo.ScenarioStudio   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     .��� .+�]� $��Stretch=�$	��650��.+��� -   $��
LeftBorder��$
��Left=�   ZeroBorderStyle#���   ._�R� -   $��script_root_tree��
�������
������   J� 	AllowDrop  . $��0q�$��0,10,0,0q�.@�Q� -   $��recording_sessions��
������� Q� 
IsSelected .  $��0q����� *RecordingSessions+ �� Mode$ TwoWay=� Q� 
IsExpanded �� *
toExpanded+$ OneWay=�����  �� Orientation$ 
Horizontal=�.+��� $6��0pack://application:,,,/icons/projects_folder.png��$��30��$��30��(� .���� -   $��parent_check_box�� d� 	IsChecked$ FalseY�$��5q�{��� *
toEnable+$ OneWay=��� -   $��project_name��
�������$��16(�$��Normal"����� *ProjectName+$ OneWay=�����  �� DataType$ RecordingSession��.R��� $ 
Horizontal=�$��Center=�$��Center=�.+��� $.��(pack://application:,,,/icons/session.png��$��22��$��22��$��15,0,0,0q��� 
�������$��5,0,0,0q�$��14(�$��Normal"�����  �� Path$ SessionName�$ TwoWay=��� $
��Left=�  BorderStyle#���  $��35��._��� $ Vertical=�$��Center=�$��2q�.+��� -   $��button_right_arrow��  
BlueButton#���  
�������{��� *
toEnable+$ OneWay=�.��]� .+��� $9��3pack://application:,,,/icons/arrow_double_right.png��$��35���� -   $��RightBorder��$��Right=�#���  ._��� .���� $ Vertical=�.+�R� -   $��scenario_root_tree��
�������
������   . $��0q�$��0,10,0,0q�$	��600��.@�Q� -	   $��scenario_tree��
������� .  $��0q� � ToolTip$ Expand to Open Scenarios������ *	Scenarios+$ TwoWay=� �� *
toEnable+$ OneWay=�{��� *
toEnable+$ OneWay=����� $ 
Horizontal=�.+��� $/��)pack://application:,,,/icons/scenario.png��$��30��$��30���� -
   $��scenarios_parent_lable��
�������$��Scenarios List��$��16(�$��Normal"����� $ 	Scenarios��.R��� $ 
Horizontal=�$��Center=�$��Center=�$��	-15,0,0,0q�.+��� $/��)pack://application:,,,/icons/scenario.png��$��20��$��20��$��1,0,0,0q��� 
�������$��2,0,0,0q�$��14(�$��Normal"����� $ ScenarioName�$ TwoWay=��� #���   ._��� $ Vertical=�$	��Top=�.+��� -   $��scenario_stack_panel��$ 
Horizontal=�$��Center=�$��0,10,1,1q�{��� *
toEnable+$ OneWay=�.+��� #���  
�������$��30��$��2,10,1,1q�$	��130��.��]� $��1q�.+��� $+��%pack://application:,,,/icons/help.png��$��28��$��28���� $�� Quick Help ��$��25��$��Center=�$��12(�$��10,5,0,0q��� -   #���  
�������$��30��$��	20,10,1,1q�$	��130��.��]� $��1q�.+��� $2��,pack://application:,,,/icons/new_project.png��$��28��$��28���� $�� New Scenario ��$��25��$��Center=�$��12(�$��0,5,0,0q��� -   #���  {�.  
�������$��30��$��	20,10,1,1q�$	��130��.��]� $��1q�.+��� $.��(pack://application:,,,/icons/runtime.png��$��28��$��28���� $�� Runtime Settings ��$��25��$��Center=�$��12(�$��0,5,0,0q��� -   #���  
�������$��30��$��	20,10,1,1q�$	��170��{��� *" isEnableInitializeLoadTestButton+$ TwoWay=�.��]� $��1q�.+��� $1��+pack://application:,,,/icons/start_load.png��$��28��$��28���� $�� Initialize Load Test ��$��25��$��Center=�$��12(�$��0,5,0,0q��� $��5,20,5,5q�.+�&   System.Windows.Controls.DataGrid  -   $��scenario_data_grid��	   AlternatingRowBackground
	 ��ح�
   AutoGenerateColumns
 .  
������    ColumnHeaderHeight$ 50Q�   CanUserReorderColumns .     CanUserAddRows .     CanUserResizeColumns .    CanUserResizeRows .  $   HorizontalScrollBarVisibility$ Disabled=�"   VerticalScrollBarVisibility$
 Auto=�$	��550��$��13(�
������ $��0q�$��Normal"����� *	Scripts+$ TwoWay=�{��� $ toEnable�$ TwoWay=�   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Script Name��   Width5  /System.Windows.Controls.DataGridLengthConverter$	 140    
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� $ 
ScriptName�  $ Start After�� �� $ 
StartAfter�  $ No. / % Of Users�� �� $ NumberOfUsers�  $ 
No.of Itrs�� �� $ NumberOfIterations�  $ Itr Delay Min�� �� $ DelayBetweenIterationMin�  $ Itr Delay Max�� �� $ DelayBetweenIterationMax�  $ Think Time Min�� �� $ ThinkTimeMin�  $ Think Time Max�� �� $ ThinkTimeMax�