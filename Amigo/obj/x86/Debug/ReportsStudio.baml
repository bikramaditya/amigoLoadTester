   M S B A M L   `   `   `  ����@  <Amigo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:AmigoAmigo  5      | xSystem.Windows.Controls.DataVisualization.Toolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35�{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit2System.Windows.Controls.DataVisualization.Charting 5          Amigo.ReportsStudio   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      %localclr-namespace:Amigo     5      �chartingToolkit{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit     5      .��5
      � -   5        $��
LayoutRoot��5
      .+�5   
   ]� $��Stretch=�6   $	��640��60   .+�5      �� $
��Left=�6      BorderStyle#���   6+   $	��350��6]   ._�5      R� -   5        $��execution_root_tree��5      
�������
������   J� 	AllowDrop  . $��0q�6s   $��0,10,0,0q�6�   .@�5      Q� -   5        $��test_exec_tree_item��5   #   
������� Q� 
IsExpanded .  Q� 
IsSelected .  $��0q�6�   ��6Q   �� *TestExecutions+ �� Mode$ TwoWay=���5      �� 5       �� Orientation$ Vertical=�6)   .+�5   "   �� $ 
Horizontal=�6-   .+�5   &   �� $6��0pack://application:,,,/icons/projects_folder.png��6,   $��30��6f   $��30��6r   �� 5   &   -   5        $��project_name��5   ,   
�������$��16(�6�   $��Normal"�6�   ��6S   �� *ProjectName+$ OneWay=�6�   5   #   �� 5   "   -   5        $��retrivalCounter��5   ,   $��Retrieving...    Count: ��6C   $��35,10q�6c   $y�Hidden=�6r   5      ��5      �� 5       �� DataType$ KeyValuePair��67   .R�5   "   �� $ 
Horizontal=�6-   $��Center=�6F   $��Center=�6a   .+�5   &   �� $.��(pack://application:,,,/icons/execute.png��6,   $��22��6^   $��22��6j   $��15,0,0,0q�6u   �� 5   &   
�������$��5,0,0,0q�6k   $��12(�6|   $��Normal"�6�   ��6?   ��  �� Path$ Value�$ OneWay=�6�   5   #   5      5       ��5!      �� 5"       �� 
TargetType" Q�6    .�5#      -   5        5$      5&      5'      �� 5(      $��Right=�6   #���   6,   $	��290��6^   ._�5)      �� $ Vertical=�6   .+�5*      �� $	��540��6!   .+�5+      R� -   5        $��folder_root_tree��5+   #   
�������
������   . $��0q�6x   $��0,10,0,0q�6�   .@�5,      Q� -   5        $��folder_tree_item��5,   +   
������� .  .  $��0q�6{   ��5-   "   �� 5.   &   $ 
Horizontal=�61   .+�5/   *   �� $-��'pack://application:,,,/icons/folder.png��60   $��36��6a   $��36��6m   �� 50   *   
�������$��Documents Vault��6C   $��16(�6]   $��Normal"�6k   51   '   53      54      55      � 56      -	   5        $��reportProgressBar��56   "   $	��180��6;   $��30��6G   $��	10,35,1,1q�6S    � Value 6f   �� *ProgressPercentage+$ TwoWay=��� 57      $ 
Horizontal=�6!   .+�58      �� $��Report Generation Status ��6$   $��14(�6E   $��Center=�6S   $��50,5,5,5q�6p   �� 59      -
   5        $��reportProgressText��59   $   $��16(�6p   $��Center=�6~   $��5q�6�   ��6?   �� *ProgressPercentage+$ TwoWay=�6�   �� 5:      $	�� % ��6$   $��14(�6/   $��Center=�6=   $��5q�6Z   5;      5<      5=      �� 5>      #���   6   ._�5?      �� .��5@      �� $ Vertical=�6   .+�5A      �� $��Report Generation Screen��6    $��20(�6@   $��Center=�6N   $��10q�6k   � 5B      -   5        $#��loadtest_runtime_summary_grid��5B      $
��Left=�6@   $��10q�6[   $y�Hidden=�6g   	 � RowDefinitions	 5C      �� 5D      $
��auto �6,   �� 5E      $
��auto �6,   �� 5F      $
��auto �6,   �� 5G      $
��auto �6,   �� 5H      $
��auto �6,   �� 5I      $
��auto �6,   �� 5J      $
��auto �6,   �� 5K      $
��auto �6,   �� 5L      $
��auto �6,   �� 5M      $
��auto �6,   �� 5N      $
��auto �6,   �� 5O      $
��auto �6,   
 � ColumnDefinitions
 5Q      �� 5R      $	��250 �6/   �� 5S      $	��500 �6/   .+�5U      �� $��0��6    $��0��6-   $��Name of the report:��6=   $��14(�6[   
�������
�������$��.1q�6�   �� 5V      -   5        $��projectnameTextBox��5V   "   $��0��6<   $��1��6I   $0��*Please enter a logicaliy identifiable name��6Y   $��14(�6�   $��30��6�   
���   �
�������$��1.q�5W   I   $��Italic$�6^   $��Center=�6q   $��Center=�6�   $��2q�5X   s   ���#CECEF66~   �� 5Y      $��1��6    $��0��6-   $%��Upload ur and Customer's Logos:��6=   $��14(�6h   
�������
�������$��.1q�6�   �� 5Z      $ 
Horizontal=�6%   $��1��6>   $��1��6K   $��2q�6[   .+�5[      �� -   5        $��yourLogo��5[   &   $��Choose Your Logo��66   $��14(�6O   
���   �
�������$��.1q�6�   $	��250��6�   $��Italic$�5\   &   $��Center=�69   $��Center=�6[   $��30��6v   ���#CECEF66�   �� 5]      $��	10,0,10,0q�6(   �� 5^      -   5        $��customerLogo��5^   &   $��Choose Customer's Logo��6:   $��14(�6Y   
���   �
�������$��.1q�6�   $	��250��6�   $��Italic$�5_   &   $��Center=�69   $��Center=�6[   $��30��6v   ���#CECEF66�   5`      �� 5a      $��2��6    $��0��6-   $!��Description about the test:��6=   $��14(�6d   
�������
�������$��.1q�6�   �� 5b      -   5        $��testDescription��5b   "   $��2��69   $��1��6F   $c��]e.g. This test was carried out to verify the XYZEcomm Application supports...n users at x TPS��6V   $��12(�5c   "   
���   �
�������$��.1q�6W    �� TextWrapping$
 Wrap=�6l   $	��100��6�   $��2q�6�   ���#CECEF66�   �� 5e      $��4��6    $��0��6-   $&�� Deployment Architecture Diagram:��6=   $��14(�6i   
�������
�������$��.1q�6�   �� 5f      -   5        $��deploymentDiagramTextBox��5f   "   $��4��6B   $��1��6O   $$��Click here to choose the image��6_   $��14(�6�   $��30��6�   
���   �
�������$��1.q�5g   I   $��Italic$�6^   $��Center=�6q   $��Center=�6�   $��2q�5h   P   ���#CECEF66[   �� 5j      $��5��6    $��0��6-   $��Web Servers:��6=   $��14(�6U   
�������
�������$��.1q�6�   �� 5k      -   5        $��webServersTextBox��5k   "   $��5��6;   $��1��6H   $N��He.g. 2 Web Servers each having 8 Intel Xeon Cores 2.66 GHz and 16 GB RAM��6X   $��12(�6�   $��30��6�   
���   �
�������$��1.q�5l   I   $��Italic$�6^   $��Center=�6q   $��Center=�6�   $��2q�5m   "   ���#CECEF66-   �� 5o      $��6��6    $��0��6-   $��Application Servers:��6=   $��14(�6]   
�������
�������$��.1q�6�   �� 5p      -   5        $��appServersTextBox��5p   "   $��6��6;   $��1��6H   $N��He.g. 4 App Servers each having 8 Intel Xeon Cores 2.66 GHz and 16 GB RAM��6X   $��12(�6�   $��30��6�   
���   �
�������$��1.q�5q   I   $��Italic$�6^   $��Center=�6q   $��Center=�6�   $��2q�5r   "   ���#CECEF66-   �� 5t      $��7��6    $��0��6-   $��DB Servers:��6=   $��14(�6T   
�������
�������$��.1q�6�   �� 5u      -   5        $��dbServersTextBox��5u   "   $��7��6:   $��1��6G   $H��Be.g. 1 DB Server having 16 Intel Xeon Cores 2.66 GHz and 32 GB RAM��6W   $��12(�6�   $��30��6�   
���   �
�������$��1.q�5v   I   $��Italic$�6^   $��Center=�6q   $��Center=�6�   $��2q�5w   "   ���#CECEF66-   �� 5y      $��8��6    $��0��6-   $��Other Servers:��6=   $��14(�6W   
�������
�������$��.1q�6�   �� 5z      -   5        $��otherServersTextBox��5z   "   $��8��6=   $��1��6J   $^��Xe.g. 1 LDAP Server and 1 SSO server each having 4 Intel Xeon Cores 2.66 GHz and 8 GB RAM��6Z   $��12(�6�   $��30��6�   
���   �
�������$��1.q�5{   I   $��Italic$�6^   $��Center=�6q   $��Center=�6�   $��2q�5|   "   ���#CECEF66-   �� 5~      $��10��6    $��0��6.   $��Choose report format:��6>   $��14(�6_   
�������
�������$��.1q�6�   �� 5      $��10��6!   $��1��6/   
�������$��.5q�6S   $��2q�6h   $��30��6s   ._�5�      �� $ 
Horizontal=�6)   $��Center=�6B   .+�5�   "   	� -   5        $��RadioPDF��5�   .   $��14(�6>   
�������$��2,2,0,0q�6_   {�.  .��5�   &   ]� .+�5�   *   �� $*��$pack://application:,,,/icons/pdf.png��60   $��28��6^   $��28��6j   6w   �� 5�   *   $��	Adobe PDF��64   $��12(�6E   
�������$��Center=�6f   $��5,0,0,0q�6�   5�   '   5�   #   	� 5�   "   -   5        $��	RadioDOCX��5�   .   $��14(�6?   
�������$��35,2,0,0q�6`    d� 	IsChecked$
 TrueY�6r   .��5�   &   ]� .+�5�   *   �� $+��%pack://application:,,,/icons/docx.png��60   $��28��6_   $��28��6k   6x   �� 5�   *   $��MS DOCX��64   $��12(�6C   
�������$��Center=�6d   $��5,0,0,0q�6   5�   '   5�   #   	� 5�   &   -   5        $��	RadioHTML��5�   2   $��14(�6C   
�������$��35,2,0,0q�6d   {�.  .��5�   &   ]� .+�5�   *   �� $/��)pack://application:,,,/icons/web_page.png��60   $��28��6c   $��28��6o   6|   �� 5�   *   $
��HTML��64   $��12(�6@   
�������$��Center=�6a   $��5,0,0,0q�6|   5�   '   5�   #   5�      5�      �� 5�      -   5        $��save_button��5�   !   $��11��65   $��0��6C   $��18(�6S   
�������  
BlueButton#���  6t   {�. $��40��5�   !   $	��200��6-   $
��Left=�69   $��	40,20,0,0q�6T   .��5�      ]� $��5q�6(   .+�5�   "   �� $3��-pack://application:,,,/icons/save_project.png��6(   $��32��6_   $��32��6k   6x   �� 5�   "   $��Save Above Details��6,   $��15(�6F   
�������$��Center=�6g   $��5,0,0,0q�6�   5�      5�      �� 5�      -   5        $��generate_button��5�   !   $��11��68   $��1��6F   $��18(�6V   
�������#���  6w   {�.  $��40��5�   !   $	��200��6-   $��0,20,0,0q�69   $��Center=�6K   $��Center=�6h   .��5�      ]� $��5q�6(   .+�5�   "   �� $-��'pack://application:,,,/icons/report.png��6(   $��32��6Y   $��32��6e   6r   �� 5�   "   $��Generate Report��6,   $��15(�6C   
�������$��Center=�6d   $��10,0,0,0q�6   5�      5�      5�      5�      5�      5�      5�      5�      5�      