   M S B A M L   `   `   `  ���� @  <Amigo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:AmigoAmigo  3clr-namespace:Amigo.ViewModelsAmigo.ViewModels  O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem     Amigo.ScriptStudio   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     %localclr-namespace:Amigo     1myEnumclr-namespace:Amigo.ViewModels     9System&clr-namespace:System;assembly=mscorlib       � 	Resources     EnumParamSources S� 
MethodName S� 
ObjectType  System.Enum S� MethodParameters M� TypeName  EnumIterationType��%�   &	        &	 =     S� $ 	GetValues��"   M� $ myEnum:ParamSources��S� $ 	GetValues��"   M� $ myEnum:IterationType��.��� .+�]� $��Stretch=�.+��� -    ��Name$ 
LeftBorder��$
��Left=�  BorderStyle#���  ._��� $��Stretch=�$
��Auto��$��Center=�  BreadCrumbTabControl#���  $��Stretch=�$��Stretch=�
������ $��0q�.@��� $��Stretch=�$��Script Perspective��  LeftTabItem#���  $��15(�
������ $��0q�.��R� -   $��script_root_tree��
�������
������  J� 	AllowDrop . $��0q�$��0,10,0,0q�.@�Q� -   $��recording_sessions��
������� Q� 
IsSelected .  ���� *RecordingSessions+ �� Mode$ TwoWay=�	 Q� 
IsExpanded	 �� *
toExpanded+$ OneWay=�
 � ContextMenu
 ��  �� 	StaysOpen . .@�m� -   $'��!Record a script into this project�� m� Icon �� $1��+pack://application:,,,/icons/microphone.png��$��20��$��20��m� -   $��Rename Project�� �� $-��'pack://application:,,,/icons/rename.png��$��20��$��20��m� -   $��Close Project�� �� $,��&pack://application:,,,/icons/close.png��$��20��$��20��m� $��Export Project�� �� $-��'pack://application:,,,/icons/export.png��$��20��$��20��m� $��Import Project�� �� $-��'pack://application:,,,/icons/export.png��$��20��$��20��m� -   $��Delete Project�� �� $-��'pack://application:,,,/icons/delete.png��$��20��$��20������  �� Orientation$ 
Horizontal=�.+��� $6��0pack://application:,,,/icons/projects_folder.png��$��30��$��30���� -   $��script_name��
�������$��16(����� *ProjectName+$ OneWay=�����  �� DataType$ RecordingSession��.R��� $ 
Horizontal=�.+��� $.��(pack://application:,,,/icons/session.png��$��30��$��25���� 
�����������  �� Path$ SessionName�$ TwoWay=�
 �� .@�m� -	   $'��!Delete Script along with requests�� �� $-��'pack://application:,,,/icons/delete.png��$��20��$��20��m� -
   $��Expand�� �� $-��'pack://application:,,,/icons/expand.png��$��20��$��20��m� -   $��Collapse�� �� $/��)pack://application:,,,/icons/collapse.png��$��20��$��20������  �� 
TargetType" Q�.�-   �� $��Stretch=�$��Server Perspective��  BreadCrumbTabItem#���  $��15(�
������ $��0q�.��R� -   $��	root_tree��
�������
������  � ToolTip$" Please create/open a project��$��0q�.@�Q� -   $��request_referers��
����������� *RequestReferers+$ TwoWay=�	 �� *
toExpanded+$ OneWay=����� $ 
Horizontal=�.+��� $6��0pack://application:,,,/icons/projects_folder.png��$��30��$��30���� -   $��project_name��
�������$��16(����� *ProjectName+$ OneWay=����� $ RequestReferer��.R��� $ 
Horizontal=�.+��� $-��'pack://application:,,,/icons/server.png��$��30��$��30���� 
����������� $ Referer�$ TwoWay=�
 ��  . .@�m� -   $+��%Delete all requests under this server�� �� $-��'pack://application:,,,/icons/delete.png��$��20��$��20���� -   $ RightBorder��$��Right=�#���  $	��290��._��� .+��� -   $��help_topics_stack_panel��y��� $ InfoPanelsVisibility�$ TwoWay=�.+��� $	��Top=�$	��700��$	��290��#���  ._��� .+��� $��Help Topics��$��18(�
�������$��Center=�$	��Top=�$��	0,20,0,20q�R� 
������ .@�Q� $��Choose a topic below��	 . 
�������.@�Q� $��Architecture of this tool��
�������Q� $��How this tool works��
�������Q� $��Parameterization��
�������Q� $��What is yummy cookies��
��������� -   $��header_stack_panel��y��� $ ParamPanelsVisibility�$ TwoWay=�.+��� $	��Top=�#���  $	��700��._��� .+��� $��Center=�$	��Top=�$��Headers��$��18(�$
��Bold"�&   System.Windows.Controls.DataGrid  -   $��header_data_grid��   AutoGenerateColumns . 
������    CanUserReorderColumns .  $   HorizontalScrollBarVisibility$
 Auto=�"   VerticalScrollBarVisibility$
 Auto=����� *RequestHeaders+$ TwoWay=��� -   $��center_pane��#���  ._��� .+��� $
��Auto=�.���� -   $��information_stack_panel��$	��600��y��� $ InfoPanelsVisibility�$ TwoWay=�.+��� $"��Welcome to Amigo Load Tester��$��32(�
�������$��Center=�$	��Top=�$��0,5,0,5q��� $A��;The friendliest, smartest and efficient way of load testing��$��18(�
�������$��Center=�$	��Top=�$��0,0,0,5q��� $��100,5,100,5q��� $-��'Top 10 Important capabilities of Amigo:��$��20(�
�������$��Center=�$	��Top=�$��10,1,0,1q��� $-��'pack://application:,,,/icons/winner.png��$	��100��$	��100���� $a��[1. There is no script. Hence no language, no syntax, no compiling and hence no script error��$��16(�
�������$��Center=�$	��Top=�$��30,5,0,5q��� $C��=2. Supports All browsers. Yes! All browsers and all versions.��$��16(�
�������$��Center=�$	��Top=�$��30,5,0,5q��� $T��N3. Supports thick clients, thin clients and any client that speaks in HTTP(s).��$��16(�
�������$��Center=�$	��Top=�$��30,5,0,5q��� $8��24. Supports HTTP and HTTPS traffic including AJAX.��$��16(�
�������$��Center=�$	��Top=�$��30,5,0,5q��� $U��O5. Automatic proxy retrieve and setting, no worry about changing Browser Proxy.��$��16(�
�������$��Center=�$	��Top=�$��30,5,0,5q��� $@��:6. Automatic parameter creation and Automatic correlation.��$��16(�
�������$��Center=�$	��Top=�$��30,5,0,5q��� $?��97. Script replay and verification with live browser view.��$��16(�
�������$��Center=�$	��Top=�$��30,5,0,5q��� $X��R8. Script portability. i.e Change the target server without modifying the scripts.��$��16(�
�������$��Center=�$	��Top=�$��30,5,0,5q��� $%��9. One click scenario creation.��$��16(�
�������$��Center=�$	��Top=�$��30,5,0,5q��� $B��<10. One click report generation from historical test result.��$��16(�
�������$��Center=�$	��Top=�$��30,5,0,5q��� -   $��parameter_stack_panel��y��� $ ParamPanelsVisibility�$ TwoWay=�.+��� $ 
Horizontal=�$��50��.+��� -   $"��host_parameterization_button��  
BlueButton#���  
�������$��40��{�.  $��50,5,5,5q�.��]� $��5q�.+��� $3��-pack://application:,,,/icons/parameterize.png��$��32��$��32���� $�� Parameterize Host/Port  ��$��25��$��Center=��� -   $&�� validate_parameterization_button��#���  
�������$��40��{�.  $��50,5,5,5q�.��]� $��5q�.+��� $3��-pack://application:,,,/icons/save_project.png��$��32��$��32���� $��  Save Parameterization  ��$��25��$��Center=��� -   $��check_whole_script_button��#���  
�������$��40��$��50,5,5,5q�{��� $ ReplayButtonStatus�.��]� $��5q�.+��� $-��'pack://application:,,,/icons/verify.png��$��32��$��32���� $��  Replay and Verify  ��$��25��$��Center=�  -   $��param_data_grid��   AlternatingRowBackground
 ��ح� .  
������    ColumnHeaderHeight$ 50Q� .  $
 Auto=�$
 Auto=�$	��600������ *
Parameters+$ TwoWay=�   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ 
Param Name��1  +System.Windows.Controls.DataGridBoundColumn   Binding �� $ 	ParamName�  $ Original Param Value�� �� $ OriginalParamValue�4  .System.Windows.Controls.DataGridTemplateColumn  -   param_data_source��$ Parameter Data Source��   CellTemplate �� .R��� -   
������ ����  �� Source# [   �� SelectedItem �� $ ParameterizationSource�$ TwoWay=�  $ Substituted value / CSV�� �� $ SubstututedParamValue�  -   csv_column_name��$ CSV Column Name�� �� .R��� -   
������ ���� $ 
CSVColumns�$ TwoWay=� �� $ SelectedCSVColumnName�$ TwoWay=�  -    iteration_mode��$ Iteration Mode�� �� .R��� -!   
������ ���� # [  �� $ SelectedIterationType�$ TwoWay=�