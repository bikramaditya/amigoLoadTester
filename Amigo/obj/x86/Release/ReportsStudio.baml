   M S B A M L   `   `   `  ���� @  <Amigo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:AmigoAmigo  | xSystem.Windows.Controls.DataVisualization.Toolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35�{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit2System.Windows.Controls.DataVisualization.Charting     Amigo.ReportsStudio   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     %localclr-namespace:Amigo     �chartingToolkit{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit     .��� -   $��
LayoutRoot��.+�]� $��Stretch=�$	��640��.+��� $
��Left=�   BorderStyle#���   $	��350��._�R� -   $��execution_root_tree��
�������
������   J� 	AllowDrop  . $��0q�$��0,10,0,0q�.@�Q� -   $��test_exec_tree_item��
������� Q� 
IsExpanded .  Q� 
IsSelected .  $��0q����� *TestExecutions+ �� Mode$ TwoWay=�����  �� Orientation$ Vertical=�.+��� $ 
Horizontal=�.+��� $6��0pack://application:,,,/icons/projects_folder.png��$��30��$��30���� -   $��project_name��
�������$��16(�$��Normal"����� *ProjectName+$ OneWay=��� -   $��retrivalCounter��$��Retrieving...    Count: ��$��35,10q�$y�Hidden=�����  �� DataType$ KeyValuePair��.R��� $ 
Horizontal=�$��Center=�$��Center=�.+��� $.��(pack://application:,,,/icons/execute.png��$��22��$��22��$��15,0,0,0q��� 
�������$��5,0,0,0q�$��12(�$��Normal"�����  �� Path$ Value�$ OneWay=�����  �� 
TargetType" Q�.�-   �� $��Right=�#���   $	��290��._��� $ Vertical=�.+��� $	��540��.+�R� -   $��folder_root_tree��
�������
������   . $��0q�$��0,10,0,0q�.@�Q� -   $��folder_tree_item��
������� .  .  $��0q����� $ 
Horizontal=�.+��� $-��'pack://application:,,,/icons/folder.png��$��36��$��36���� 
�������$��Documents Vault��$��16(�$��Normal"�� -	   $��reportProgressBar��$	��180��$��30��$��	10,35,1,1q� � Value �� *ProgressPercentage+$ TwoWay=��� $ 
Horizontal=�.+��� $��Report Generation Status ��$��14(�$��Center=�$��50,5,5,5q��� -
   $��reportProgressText��$��16(�$��Center=�$��5q����� *ProgressPercentage+$ TwoWay=��� $	�� % ��$��14(�$��Center=�$��5q��� #���   ._��� .���� $ Vertical=�.+��� $��Report Generation Screen��$��20(�$��Center=�$��10q�� -   $#��loadtest_runtime_summary_grid��$
��Left=�$��10q�$y�Hidden=�	 � RowDefinitions	 �� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto �
 � ColumnDefinitions
 �� $	��250 ��� $	��500 �.+��� $��0��$��0��$��Name of the report:��$��14(�
�������
�������$��.1q��� -   $��projectnameTextBox��$��0��$��1��$0��*Please enter a logicaliy identifiable name��$��14(�$��30��
���   �
�������$��1.q�$��Italic$�$��Center=�$��Center=�$��2q����#CECEF6�� $��1��$��0��$%��Upload ur and Customer's Logos:��$��14(�
�������
�������$��.1q��� $ 
Horizontal=�$��1��$��1��$��2q�.+��� -   $��yourLogo��$��Choose Your Logo��$��14(�
���   �
�������$��.1q�$	��250��$��Italic$�$��Center=�$��Center=�$��30�����#CECEF6�� $��	10,0,10,0q��� -   $��customerLogo��$��Choose Customer's Logo��$��14(�
���   �
�������$��.1q�$	��250��$��Italic$�$��Center=�$��Center=�$��30�����#CECEF6�� $��2��$��0��$!��Description about the test:��$��14(�
�������
�������$��.1q��� -   $��testDescription��$��2��$��1��$c��]e.g. This test was carried out to verify the XYZEcomm Application supports...n users at x TPS��$��12(�
���   �
�������$��.1q� �� TextWrapping$
 Wrap=�$	��100��$��2q����#CECEF6�� $��4��$��0��$&�� Deployment Architecture Diagram:��$��14(�
�������
�������$��.1q��� -   $��deploymentDiagramTextBox��$��4��$��1��$$��Click here to choose the image��$��14(�$��30��
���   �
�������$��1.q�$��Italic$�$��Center=�$��Center=�$��2q����#CECEF6�� $��5��$��0��$��Web Servers:��$��14(�
�������
�������$��.1q��� -   $��webServersTextBox��$��5��$��1��$N��He.g. 2 Web Servers each having 8 Intel Xeon Cores 2.66 GHz and 16 GB RAM��$��12(�$��30��
���   �
�������$��1.q�$��Italic$�$��Center=�$��Center=�$��2q����#CECEF6�� $��6��$��0��$��Application Servers:��$��14(�
�������
�������$��.1q��� -   $��appServersTextBox��$��6��$��1��$N��He.g. 4 App Servers each having 8 Intel Xeon Cores 2.66 GHz and 16 GB RAM��$��12(�$��30��
���   �
�������$��1.q�$��Italic$�$��Center=�$��Center=�$��2q����#CECEF6�� $��7��$��0��$��DB Servers:��$��14(�
�������
�������$��.1q��� -   $��dbServersTextBox��$��7��$��1��$H��Be.g. 1 DB Server having 16 Intel Xeon Cores 2.66 GHz and 32 GB RAM��$��12(�$��30��
���   �
�������$��1.q�$��Italic$�$��Center=�$��Center=�$��2q����#CECEF6�� $��8��$��0��$��Other Servers:��$��14(�
�������
�������$��.1q��� -   $��otherServersTextBox��$��8��$��1��$^��Xe.g. 1 LDAP Server and 1 SSO server each having 4 Intel Xeon Cores 2.66 GHz and 8 GB RAM��$��12(�$��30��
���   �
�������$��1.q�$��Italic$�$��Center=�$��Center=�$��2q����#CECEF6�� $��10��$��0��$��Choose report format:��$��14(�
�������
�������$��.1q��� $��10��$��1��
�������$��.5q�$��2q�$��30��._��� $ 
Horizontal=�$��Center=�.+�	� -   $��RadioPDF��$��14(�
�������$��2,2,0,0q�{�.  .��]� .+��� $*��$pack://application:,,,/icons/pdf.png��$��28��$��28���� $��	Adobe PDF��$��12(�
�������$��Center=�$��5,0,0,0q�	� -   $��	RadioDOCX��$��14(�
�������$��35,2,0,0q� d� 	IsChecked$
 TrueY�.��]� .+��� $+��%pack://application:,,,/icons/docx.png��$��28��$��28���� $��MS DOCX��$��12(�
�������$��Center=�$��5,0,0,0q�	� -   $��	RadioHTML��$��14(�
�������$��35,2,0,0q�{�.  .��]� .+��� $/��)pack://application:,,,/icons/web_page.png��$��28��$��28���� $
��HTML��$��12(�
�������$��Center=�$��5,0,0,0q��� -   $��save_button��$��11��$��0��$��18(�
�������  
BlueButton#���  {�. $��40��$	��200��$
��Left=�$��	40,20,0,0q�.��]� $��5q�.+��� $3��-pack://application:,,,/icons/save_project.png��$��32��$��32���� $��Save Above Details��$��15(�
�������$��Center=�$��5,0,0,0q��� -   $��generate_button��$��11��$��1��$��18(�
�������#���  {�.  $��40��$	��200��$��0,20,0,0q�$��Center=�$��Center=�.��]� $��5q�.+��� $-��'pack://application:,,,/icons/report.png��$��32��$��32���� $��Generate Report��$��15(�
�������$��Center=�$��10,0,0,0q�