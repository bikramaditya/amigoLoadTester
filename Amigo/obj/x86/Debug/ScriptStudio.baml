   M S B A M L   `   `   `  ����@  <Amigo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:AmigoAmigo  5      3clr-namespace:Amigo.ViewModelsAmigo.ViewModels  5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5          Amigo.ScriptStudio   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      %localclr-namespace:Amigo     5      1myEnumclr-namespace:Amigo.ViewModels     5      9System&clr-namespace:System;assembly=mscorlib     5        � 	Resources  5
         EnumParamSources S� 
MethodName S� 
ObjectType  System.Enum S� MethodParameters M� TypeName  EnumIterationType��%  &	        &	 x     S� 5   
   $ 	GetValues��5      "  5       5      M� 5      $ myEnum:ParamSources��6   5      S� 5   
   $ 	GetValues��5      "  5       5      M� 5      $ myEnum:IterationType��6   5      5      .��5      � .+�5   
   ]� $��Stretch=�6   .+�5      �� -   5         ��Name$ 
LeftBorder��5      $
��Left=�6)     BorderStyle#���  6?   ._�5      �� $��Stretch=�6   $
��Auto��69   $��Center=�6G     BreadCrumbTabControl#���  5      $��Stretch=�6L   $��Stretch=�5      
������ $��0q�6T   .@�5!      �� $��Stretch=�6   $��Script Perspective��6A     LeftTabItem#���  5"      $��15(�6D   
������ $��0q�6k   .��5#      R� -   5        $��script_root_tree��5#   #   
�������
������  J� 	AllowDrop . $��0q�5$   o   $��0,10,0,0q�6�   .@�5%   "   Q� -   5        $��recording_sessions��5%   /   
������� Q� 
IsSelected .  ��6\   �� *RecordingSessions+ �� Mode$ TwoWay=�	 Q� 
IsExpanded	 6�   �� *
toExpanded+$ OneWay=�
 � ContextMenu
 5&   &   �� 5'   *    �� 	StaysOpen . .@�5(   .   m� -   5        $'��!Record a script into this project��5(   7    m� Icon 5)   2   �� 5*   6   $1��+pack://application:,,,/icons/microphone.png��6<   $��20��6q   $��20��6}   5,   /   m� 5-   .   -   5        $��Rename Project��5-   7    5.   2   �� 5/   6   $-��'pack://application:,,,/icons/rename.png��6<   $��20��6m   $��20��6y   51   /   m� 52   .   -   5        $��Close Project��52   7    53   2   �� 54   6   $,��&pack://application:,,,/icons/close.png��6<   $��20��6l   $��20��6x   56   /   m� 57   .   $��Export Project��67    58   2   �� 59   6   $-��'pack://application:,,,/icons/export.png��6<   $��20��6m   $��20��6y   5;   /   m� 5<   .   $��Import Project��67    5=   2   �� 5>   6   $-��'pack://application:,,,/icons/export.png��6<   $��20��6m   $��20��6y   5@   /   m� 5A   .   -   5        $��Delete Project��5A   7    5B   2   �� 5C   6   $-��'pack://application:,,,/icons/delete.png��6<   $��20��6m   $��20��6y   5E   /   5F   +   ��5H   &   �� 5I   *    �� Orientation$ 
Horizontal=�65   .+�5J   .   �� $6��0pack://application:,,,/icons/projects_folder.png��64   $��30��6n   $��30��6z   �� 5K   .   -   5        $��script_name��5K   4   
�������$��16(�6�   ��6Z   �� *ProjectName+$ OneWay=�6�   5L   +   ��5N   &   �� 5O   *    �� DataType$ RecordingSession��6C   .R�5P   .   �� $ 
Horizontal=�69   .+�5Q   2   �� $.��(pack://application:,,,/icons/session.png��68   $��30��6j   $��25��6v   �� 5R   2   
���������6K   ��  �� Path$ SessionName�$ TwoWay=�
 5S   6   �� 5T   :   .@�5U   >   m� -	   5        $'��!Delete Script along with requests��5U   G    5V   B   �� 5W   F   $-��'pack://application:,,,/icons/delete.png��6L   $��20��6}   $��20��6�   5Y   ?   m� 5Z   >   -
   5        $��Expand��5Z   G    5[   B   �� 5\   F   $-��'pack://application:,,,/icons/expand.png��6L   $��20��6}   $��20��6�   5^   ?   m� 5_   >   -   5        $��Collapse��5_   G    5`   >   �� 5a   B   $/��)pack://application:,,,/icons/collapse.png��6H   $��20��6{   $��20��6�   5c   ;   5d   7   5f   3   5g   /   5h   +   5j   #   ��5k      �� 5l   "    �� 
TargetType" Q�6(   .�5m   &   -   5        5q   #   5s      5t      �� 5u      $��Stretch=�6   $��Server Perspective��6A     BreadCrumbTabItem#���  6]   $��15(�6�   
������ $��0q�6�   .��5v      R� -   5        $��	root_tree��5v   #   
�������
������  � ToolTip$" Please create/open a project��6`   $��0q�6�   .@�5w      Q� -   5        $��request_referers��5w   +   
���������6V   �� *RequestReferers+$ TwoWay=�	 6�   �� *
toExpanded+$ OneWay=���5x   "   �� 5y   &   $ 
Horizontal=�61   .+�5z   *   �� $6��0pack://application:,,,/icons/projects_folder.png��60   $��30��6j   $��30��6v   6�   �� 5{   *   -   5        $��project_name��5{   0   
�������$��16(�6�   ��6X   �� *ProjectName+$ OneWay=�5|   +   5}   '   ��5   "   �� 5�   &   $ RequestReferer��6?   .R�5�   *   �� $ 
Horizontal=�65   .+�5�   .   �� $-��'pack://application:,,,/icons/server.png��64   $��30��6e   $��30��6q   6~   �� 5�   .   
���������6G   �� $ Referer�$ TwoWay=�
 5�   2   �� 5�   6    . .@�5�   :   m� -   5        $+��%Delete all requests under this server��5�   C    5�   >   �� 5�   B   $-��'pack://application:,,,/icons/delete.png��6H   $��20��6y   $��20��6�   5�   ;   5�   7   5�   /   5�   +   5�   '   5�      5�      5�      5�      5�      �� 5�      -   5        $ RightBorder��5�      $��Right=�6*   #���  6A   $	��290��6s   ._�5�      �� .+�5�      �� -   5        $��help_topics_stack_panel��5�   !   y�6@   �� $ InfoPanelsVisibility�$ TwoWay=�.+�5�      �� $	��Top=�6!   $	��700��66   $	��290��6C   #���  6O   ._�5�      �� .+�5�   "   �� $��Help Topics��6,   $��18(�6?   
�������$��Center=�6`   $	��Top=�6}   $��	0,20,0,20q�6�   R� 5�   "   
������ .@�5�   &   Q� $��Choose a topic below��63   	 . 
�������.@�5�   *   Q� $��Architecture of this tool��67   
�������6Z   Q� 5�   *   $��How this tool works��67   
�������6T   Q� 5�   *   $��Parameterization��67   
�������6Q   Q� 5�   *   $��What is yummy cookies��67   
�������6V   5�   '   5�   #   5�      5�      5�      �� 5�      -   5        $��header_stack_panel��5�   !   y�6;   �� $ ParamPanelsVisibility�$ TwoWay=�.+�5�      �� $	��Top=�6!   #���  66   $	��700��6h   ._�5�      �� .+�5�   "   �� $��Center=�6,   $	��Top=�6I   $��Headers��6a   $��18(�6p   $
��Bold"�6~   &   System.Windows.Controls.DataGrid  5�   "   -   5        $��header_data_grid��5�   +      AutoGenerateColumns . 
������    CanUserReorderColumns .  $   HorizontalScrollBarVisibility$
 Auto=�5�   #   "   VerticalScrollBarVisibility$
 Auto=�5�   #   ��5�   #   �� *RequestHeaders+$ TwoWay=�5�   H   5�      5�      5�      5�      5�      �� 5�      -   5        $��center_pane��5�      #���  6(   ._�5�      �� .+�5�      �� $
��Auto=�6#   .��5�      �� -   5        $��information_stack_panel��5�   2   $	��600��6%   y�6Q   �� $ InfoPanelsVisibility�$ TwoWay=�.+�5�      �� $"��Welcome to Amigo Load Tester��6$   $��32(�6H   
�������$��Center=�5�   $   $	��Top=�6A   $��0,5,0,5q�6Y   �� 5�      $A��;The friendliest, smartest and efficient way of load testing��6$   $��18(�6g   
�������$��Center=�5�   $   $	��Top=�6A   $��0,0,0,5q�6Y   �� 5�      $��100,5,100,5q�6$   �� 5�      $-��'Top 10 Important capabilities of Amigo:��6$   $��20(�6S   
�������$��Center=�5�   %   $	��Top=�6B   $��10,1,0,1q�6Z   �� 5�      $-��'pack://application:,,,/icons/winner.png��6    $	��100��6Q   $	��100��6^   �� 5�      $a��[1. There is no script. Hence no language, no syntax, no compiling and hence no script error��6$   $��16(�5�   $   
�������$��Center=�5�   $   $	��Top=�6A   $��30,5,0,5q�6Y   �� 5�      $C��=2. Supports All browsers. Yes! All browsers and all versions.��6$   $��16(�6i   
�������$��Center=�5�   $   $	��Top=�6A   $��30,5,0,5q�6Y   �� 5�      $T��N3. Supports thick clients, thin clients and any client that speaks in HTTP(s).��6$   $��16(�6z   
�������$��Center=�5�   $   $	��Top=�6A   $��30,5,0,5q�6Y   �� 5�      $8��24. Supports HTTP and HTTPS traffic including AJAX.��6$   $��16(�6^   
�������$��Center=�5�   $   $	��Top=�6A   $��30,5,0,5q�6Y   �� 5�      $U��O5. Automatic proxy retrieve and setting, no worry about changing Browser Proxy.��6$   $��16(�6{   
�������$��Center=�5�   $   $	��Top=�6A   $��30,5,0,5q�6Y   �� 5�      $@��:6. Automatic parameter creation and Automatic correlation.��6$   $��16(�6f   
�������$��Center=�5�   $   $	��Top=�6A   $��30,5,0,5q�6Y   �� 5�      $?��97. Script replay and verification with live browser view.��6$   $��16(�6e   
�������$��Center=�5�   $   $	��Top=�6A   $��30,5,0,5q�6Y   �� 5�      $X��R8. Script portability. i.e Change the target server without modifying the scripts.��6$   $��16(�6~   
�������$��Center=�5�   $   $	��Top=�6A   $��30,5,0,5q�6Y   �� 5�      $%��9. One click scenario creation.��6$   $��16(�6K   
�������$��Center=�5�   $   $	��Top=�6A   $��30,5,0,5q�6Y   �� 5�      $B��<10. One click report generation from historical test result.��6$   $��16(�6h   
�������$��Center=�5�   $   $	��Top=�6A   $��30,5,0,5q�6Y   5�      5�      �� 5�      -   5        $��parameter_stack_panel��5�   !   y�6>   �� $ ParamPanelsVisibility�$ TwoWay=�.+�5�      �� $ 
Horizontal=�6%   $��50��6>   .+�5�      �� -   5        $"��host_parameterization_button��5�   %     
BlueButton#���  6I   
�������$��40��6�   {�.  $��50,5,5,5q�6�   .��5�   "   ]� $��5q�6,   .+�5�   &   �� $3��-pack://application:,,,/icons/parameterize.png��6,   $��32��6c   $��32��6o   6|   �� 5�   &   $�� Parameterize Host/Port  ��60   $��25��6Q   $��Center=�6]   5�   #   5�      �� 5�      -   5        $&�� validate_parameterization_button��5�   %   #���  6M   
�������$��40��6�   {�.  $��50,5,5,5q�6�   .��5�   "   ]� $��5q�6,   .+�5�   &   �� $3��-pack://application:,,,/icons/save_project.png��6,   $��32��6c   $��32��6o   6|   �� 5�   &   $��  Save Parameterization  ��60   $��25��6Q   $��Center=�6]   5�   #   5�      �� 5�      -   5        $��check_whole_script_button��5�   %   #���  6F   
�������$��40��6~   $��50,5,5,5q�6�   {�6�   �� $ ReplayButtonStatus�.��5�   "   ]� $��5q�6,   .+�5�   &   �� $-��'pack://application:,,,/icons/verify.png��6,   $��32��6]   $��32��6i   6v   �� 5�   &   $��  Replay and Verify  ��60   $��25��6M   $��Center=�6Y   5�   #   5�      5�        5�      -   5        $��param_data_grid��5�   #      AlternatingRowBackground
 ��ح� .  
������    ColumnHeaderHeight$ 50Q�5�   #    .  $
 Auto=�5�   #   $
 Auto=�5�   #   $	��600��5�   #   ��5�   #   �� *
Parameters+$ TwoWay=�   Columns 5�      0  *System.Windows.Controls.DataGridTextColumn  5�   "   ,  &System.Windows.Controls.DataGridColumn   Header$ 
Param Name��6X   1  +System.Windows.Controls.DataGridBoundColumn   Binding 65   �� $ 	ParamName�6X     5�   "   $ Original Param Value��6a    65   �� $ OriginalParamValue�6a   4  .System.Windows.Controls.DataGridTemplateColumn  5�   "   -   5        param_data_source��$ Parameter Data Source��5�   T      CellTemplate 5�   &   �� 5   *   .R�5  .   �� -   5        
������ ��5  7   ��  �� Source# [   �� SelectedItem 6�   �� $ ParameterizationSource�$ TwoWay=�5  /   5  +   5  #     5  "   $ Substituted value / CSV��6d    65   �� $ SubstututedParamValue�6d     5  "   -   5        csv_column_name��$ CSV Column Name��5  R    5  &   �� 5	  *   .R�5
  .   �� -   5        
������ ��5
  7   �� $ 
CSVColumns�$ TwoWay=� 6�   �� $ SelectedCSVColumnName�$ TwoWay=�5  /   5  +   5  #     5  "   -    5        iteration_mode��$ Iteration Mode��5  Q    5  &   �� 5  *   .R�5  .   �� -!   5        
������ ��5  7   �� # [  6�   �� $ SelectedIterationType�$ TwoWay=�5  /   5  +   5  #   5     5     5     5     5     5     5     