   M S B A M L   `   `   `  ���� 	  Amigo#    Amigo.ScenarioRuntimeSettings   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008       � 	Resources   �� MergedDictionaries �� Source��  �� $ OfficeTab.xaml?�%    .��� .+���    ZeroBorderStyle#���   ._��� -   $��scenarioTabs�� �� TabStripPlacement$
 Left=�  BreadCrumbTabControl#���  
������ .@��� $��Scenario Objective��  BreadCrumbThinTabItem#���  .����   BorderStyle#���  ._���  �� Orientation$ Vertical=�.+��� $��Center=�$��10q�
�������#!Load based or goal based scenario�� $��10q�	� -   $��	goalBased��$��10q�.���� $��Achieve certain TPS load��
�������	� -   $��	loadBased��$��10q�.���� $*��$Achieve certain Concurrent User load��
��������� $��10,30,10,10q��� $��Explanation:��$
��Bold"�
�������$��10,10q��� $��VUsers and Load��#���  .���� #���  ._��� $ Vertical=�.+��� $��Center=�$��10q�+)Set number of users or target load in TPS�� $��10q��� $ 
Horizontal=�$��20��$��	10,20,0,0q�.+��� 2  ,System.Windows.Markup.XmlAttributeProperties  XmlSpace preserve$��
TPS load :��
�������$	��100��$��Stretch=��� -   $��	tps_value��$	��100��$��20,0,0,0q�$��0���� $ 
Horizontal=�$��20��$��	10,20,0,0q�.+���  preserve$��Number of Users :��
�������$	��100��$��Stretch=��� -   $��noof_user_value��$	��100��$��20,0,0,0q�$��0���� $��10,30,10,10q��� $��Explanation:��$
��Bold"�
�������$��10,10q��� -   $��Test Duration��#���  .���� #���  ._��� $ Vertical=�.+��� $��Center=�$��10q�Specify duration of load test�� $��10q��� $ 
Horizontal=�$��25��$��	10,20,0,0q�.+���  preserve$��Test Duration :��
�������$
��Left=��� -   $��duration_hr_value��$	��100��$��20,0,0,5q�$��25����  preserve$��Hour(s)��
�������$
��Left=��� -   $��duration_min_value��$��50��$��10,0,0,5q�$��25����  preserve$��
Minutes(s)��
�������$
��Left=��� $��10,30,10,10q��� $��Explanation:��$
��Bold"�
�������$��10,10q��� -	   $��Ramp Up / Down��#���  .���� #���  ._��� $ Vertical=�.+��� $��Center=�$��10q�Specify Ramp Up and Ramp Down�� $ 
Horizontal=�$��25��$��	10,20,0,0q�.+���  preserve$��Ramp Up Steps :��
�������$��Stretch=�$	��150���� -
   $��ramp_step_user_value��$��30��$��20,0,0,5q�$��25����  preserve$��User(s) in every ��
�������$��Stretch=��� -   $"��ramp_step_duration_min_value��$��30��$��20,0,0,5q�$��25����  preserve$��Minute��
�������$��Stretch=��� -   $"��ramp_Down_duration_sec_value��$��30��$��10,0,0,5q�$��25����  preserve$��
and Second��
�������$��Stretch=��� $��10,30,10,10q��� $ 
Horizontal=�$��25��$��	10,20,0,0q�.+���  preserve$��Ramp Up Duration :��
�������$��Stretch=�$	��150���� -   $��ramp_up_duration_hr_value��$��30��$��20,0,0,5q�$��25�� �� 
IsReadOnly . ��  preserve$��Hour(s)��
�������$��Stretch=��� -   $ ��ramp_up_duration_min_value��$��30��$��10,0,0,5q�$��25�� . ��  preserve$��
Minutes(s)��
�������$��Stretch=��� -   $��validate_rampup��  
BlueButton#���  $�� Calculate RampUp ��$��15,0,0,0q�$��25��$	��120��
�������$��10(��� $ 
Horizontal=�$��25��$��	10,20,0,0q�.+���  preserve$��Ramp Down Duration :��
�������$��Stretch=�$	��150���� -   $!��ramp_Down_duration_hr_value��$��30��$��20,0,0,5q�$��25�� . ��  preserve$��Hour(s)��
�������$��Stretch=��� -   $"��ramp_Down_duration_min_value��$��30��$��10,0,0,5q�$��25�� . ��  preserve$��
Minutes(s)��
�������$��Stretch=��� $��10q��� $��Explanation:��$
��Bold"�
�������$��10,10q��� $��Load Distribution��#���  .���� #���  ._��� $ Vertical=�.+��� $��Center=�$��10q�
�������53Load distribution in % or in absolute virtual users�� $��10q�	� -   $��
percentage��$��10q�.���� $$��Distribute in % of total Users��
�������	� -   $��absolute��$��10q�.���� $2��,Distribute in absolute number of total Users��
��������� $��10,30,10,10q��� $��Explanation:��$
��Bold"�
�������$��10,10q��� $��Logging��#���  .���� #���  ._��� $ Vertical=�.+��� $��Center=�$��10q�
�������Log Settings�� $��10q�	� -   $��	logLevel0��$��10q�.���� $��Do not log anything��
�������	� -   $��	logLevel1��$��10q�.���� $ ��Log only errors/exceptions��
�������	� -   $��	logLevel2��$��10q�.���� $:��4Log errors, parameter substitutions and request body��
�������	� -   $��	logLevel3��$��10q�.���� $N��HLog errors, parameters, request body and response body(Not Recomomended)��
���  ���� $��10,30,10,10q��� $��Explanation:��$
��Bold"�
�������$��10,10q��� $��Target Server��#���  .���� #���  ._��� $ Vertical=�.+��� $��Center=�$��10q�)'Specify the target server host and port�� $��10q��� $ 
Horizontal=�$��20��$��	10,20,0,0q�.+���  preserve$��Host Name :��
�������$	��100��$��Stretch=��� -   $��host_name_text��$	��300��$��20,0,0,0q��� $ 
Horizontal=�$��20��$��	10,20,0,0q�.+���  preserve$��Port Number :��
�������$	��100��$��Stretch=��� -   $��port_number_text��$	��100��$��20,0,0,0q��� $��10,30,10,10q��� $��Explanation:��$
��Bold"�
�������$��10,10q��� $��Error Handling��#���  .���� #���  ._��� $ Vertical=�.+��� $��Center=�$��10q�
�������)'Specify how to deal with runtime errors�� $��10q�	� -   $��errorLevel0��$��10q�.���� $7��1Ignore all errors and mark transactions as passed��
�������	� -   $��errorLevel1��$��10q�.���� $8��2Log errors, fail the transaction and continue test��
�������	� -   $��errorLevel2��$��10q�.���� $3��-Log errors and fail the test (Zero Tolerence)��
��������� $��10,30,10,10q��� $��Explanation:��$
��Bold"�
�������$��10,10q��� $��Network Emulation��#���  .���� #���  ._��� $ Vertical=�.+��� $
��Left=�$��10q�
�������&$Simulate bandwidth for WAN emulation�� $��10q��� $ 
Horizontal=�$��	10,20,0,0q�.+���  preserve$(��"Select the bandwidth to simulate :��
�������$
��Left=�$��	20,0,10,0q��� -   $��band_width_combo��.@���  �� 
IsSelected . .���� $��LAN (Use Max available)���� .���� $��56 Kbps���� .���� $��128 Kbps���� .���� $��256 Kbps���� .���� $��512 Kbps���� .���� $��1 Mbps���� .���� $��2 Mbps���� .���� $��10 Mbps���� $��10,30,10,10q��� $��Explanation:��$
��Bold"�
�������$��10,10q��� $��Cache Settings��#���  .���� #���  ._��� $ Vertical=�.+��� $��Center=�$��10q�
�������$"Simulate browser's cache behaviour�� $��10q�	� -   $��cacheEnabled��$��10q�.���� $3��-Enable to Cache to simulate browser behaviour��
�������	� -   $��cacheDisabled��$��10q�.���� $3��-Disable Cache and download all content always��
��������� $��10,30,10,10q��� $��Explanation:��$
��Bold"�
�������$��10,10q��� $��Proxy Settings��#���  .���� #���  ._��� $ Vertical=�.+��� $��Center=�$��10q�Proxy Settings�� $��10q��� $ 
Horizontal=�$��20��$��	10,20,0,0q�.+���  preserve$��Proxy Host Name :��
�������$	��120��$��Stretch=��� -    $��proxy_host_name_text��$	��300��$��20,0,0,0q��� $ 
Horizontal=�$��20��$��	10,20,0,0q�.+���  preserve$��Proxy Port No. :��
�������$	��120��$��Stretch=��� -!   $��proxy_port_number_text��$	��100��$��20,0,0,0q��� $��10,30,10,10q��� $��Explanation:��$
��Bold"�
�������$��10,10q��� -"   $��Review and Save��#���  .���� #���  ._��� $ Vertical=�.+��� $��Center=�$��10q�
�������Review and Save�� $��10q�� -#   $��runtime_summary_grid��$
��Left=�$��	20,0,10,2q� � RowDefinitions �� $
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
��auto �	 � ColumnDefinitions	 �� $	��150 ��� $	��350 �.+��� $��0��$��0��$��Scenario Objective:��$��13(�
�������
�������$��.1q��� $��0��$��1��$��Value:��$��13(�
�������
�������$��.1q��� $��1��$��0��$��Test Duration:��$��13(�
�������
�������$��.1q��� $��1��$��1��$��Value:��$��13(�
�������
�������$��.1q��� $��2��$��0��$��Ramp Up:��$��13(�
�������
�������$��.1q��� $��2��$��1��$��Value:��$��13(�
�������
�������$��.1q��� $��3��$��0��$��
Ramp Down:��$��13(�
�������
�������$��.1q��� $��3��$��1��$��Value:��$��13(�
�������
�������$��.1q��� $��4��$��0��$��Ramp User Step:��$��13(�
�������
�������$��.1q��� $��4��$��1��$��Value:��$��13(�
�������
�������$��.1q��� $��5��$��0��$��Load Distribution:��$��13(�
�������
�������$��.1q��� $��5��$��1��$��Value:��$��13(�
�������
�������$��.1q��� $��6��$��0��$��Logging:��$��13(�
�������
�������$��.1q��� $��6��$��1��$��Value:��$��13(�
�������
�������$��.1q��� $��7��$��0��$��Network Emulation:��$��13(�
�������
�������$��.1q��� $��7��$��1��$��Value:��$��13(�
�������
�������$��.1q��� $��8��$��0��$��Enable Cache:��$��13(�
�������
�������$��.1q��� $��8��$��1��$��Value:��$��13(�
�������
�������$��.1q��� $��9��$��0��$��Target Server:��$��13(�
�������
�������$��.1q��� $��9��$��1��$��Value:��$��13(�
�������
�������$��.1q��� $��10��$��0��$��Error Handling:��$��13(�
�������
�������$��.1q��� $��10��$��1��$��Value:��$��13(�
�������
�������$��.1q��� $��11��$��0��$��Proxy Settings:��$��13(�
�������
�������$��.1q��� $��11��$��1��$��Value:��$��13(�
�������
�������$��.1q��� -$   $��save_scenario_runtime��#���  $�� Save and Close ��$��30��$	��150��
�������$
��Bold"�