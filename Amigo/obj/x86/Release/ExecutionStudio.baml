   M S B A M L   `   `   `  ���� |  xSystem.Windows.Controls.DataVisualization.Toolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35�{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit2System.Windows.Controls.DataVisualization.Charting  	 Amigo   Amigo.ExecutionStudio   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     �chartingToolkit{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit       � 	Resources     DashedPolyLine �� 
TargetType �� Property �� Value  ThickPolyLine�� %�   &	        &	 A     �� " �.���  �@J�Opacity$	 0.5Q���  � ��$ 2���� " �.���  �@J�Opacity$ 1Q���  � ��$ 4��.��� .+�]� $��Stretch=�$	��650��.+��� $
��Left=�  BorderStyle#���  $	��350��._��� #���  ._��� .����  �� Orientation$ Vertical=�.+��� $"��Approx. Load Pattern Preview��$��Center=�$��5q�>   8System.Windows.Controls.DataVisualization.Charting.Chart  -   $��loadPatternPreviewChart��$	��Top=�$��
0,-40,10,0q�$	��250��
�������
�������$	��0.1q�  WhiteLegendStyle   LegendStyle# [    Series. C   =System.Windows.Controls.DataVisualization.Charting.LineSeries  -   $��loadPatternPreviewSeries��H   BSystem.Windows.Controls.DataVisualization.Charting.DataPointSeries   DependentValuePath$ Value��   IndependentValuePath$	 Key��	   IsSelectionEnabled	 . 
���   �
   ItemsSource
 ��    Axes C   =System.Windows.Controls.DataVisualization.Charting.LinearAxis  -   $��loadPatternAxis��=   7System.Windows.Controls.DataVisualization.Charting.Axis   Orientation$ X=�  -   $��loadPatternYAxis��$ Y=�D   >System.Windows.Controls.DataVisualization.Charting.NumericAxis   Minimum�   �System.Windows.Controls.DataVisualization.Charting.NullableConverter`1[[System.Double, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]$ 0 �� -   $��NumberOfUsers��$��Center=�$��10,-110,10,10q�
����  �����  �� Path$ NumberOfUsers� �� Mode$ TwoWay=��� $��Center=�$��10,20,10,10q�
�������Applied Runtime Settings�� $��
10,0,10,10q�� -   $#��loadtest_runtime_summary_grid��$
��Left=�$��
20,10,10,5q� � RowDefinitions �� $
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
��auto � � ColumnDefinitions �� $	��150 ��� $	��170 �.+��� $��0��$��0��$��Scenario Objective:��$��12(�
�������
�������$��.1q��� $��0��$��1��$��Not Initialized��$��12(�
�������
�������$��.1q��� $��1��$��0��$��Test Duration:��$��12(�
�������
�������$��.1q��� $��1��$��1��$��Not Initialized��$��12(�
�������
�������$��.1q��� $��2��$��0��$��Ramp Up:��$��12(�
�������
�������$��.1q��� $��2��$��1��$��Not Initialized��$��12(�
�������
�������$��.1q��� $��3��$��0��$��
Ramp Down:��$��12(�
�������
�������$��.1q��� $��3��$��1��$��Not Initialized��$��12(�
�������
�������$��.1q��� $��4��$��0��$��Ramp User Step:��$��12(�
�������
�������$��.1q��� $��4��$��1��$��Not Initialized��$��12(�
�������
�������$��.1q��� $��5��$��0��$��Load Distribution:��$��12(�
�������
�������$��.1q��� $��5��$��1��$��Not Initialized��$��12(�
�������
�������$��.1q��� $��6��$��0��$��Logging:��$��12(�
�������
�������$��.1q��� $��6��$��1��$��Not Initialized��$��12(�
�������
�������$��.1q��� $��7��$��0��$��Network Emulation:��$��12(�
�������
�������$��.1q��� $��7��$��1��$��Not Initialized��$��12(�
�������
�������$��.1q��� $��8��$��0��$��Enable Cache:��$��12(�
�������
�������$��.1q��� $��8��$��1��$��Not Initialized��$��12(�
�������
�������$��.1q��� $��9��$��0��$��Target Server:��$��12(�
�������
�������$��.1q��� $��9��$��1��$��Not Initialized��$��12(�
�������
�������$��.1q��� $��10��$��0��$��Error Handling:��$��12(�
�������
�������$��.1q��� $��10��$��1��$��Not Initialized��$��12(�
�������
�������$��.1q��� $��11��$��0��$��Proxy Settings:��$��12(�
�������
�������$��.1q��� $��11��$��1��$��Not Initialized��$��12(�
�������
�������$��.1q��� $��Right=�#���  $	��290��._��� .+��� -   $$��current_test_status_stackPanel��$ Vertical=�$y�Visible=�.+��� $��Center=�$��10q�
�������Current Test Status�� $��
10,0,10,10q�� -   $!��current_status_summary_grid��$
��Left=�$��20,10,10,10q� �� $
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
��auto � �� $
��auto ��� $	��170 �.+��� $��0��$��0��$��Active VUsers:��$��12(�
�������
�������$��.1q��� -	   $��ActiveVUsers��$��0��$��1��$��12(�
�������
�������$��.1q����� $ ActiveVUsers�$ TwoWay=��� $��1��$��0��$��Elaspsed Time:��$��12(�
�������
�������$��.1q��� -
   $��ElaspsedTime��$��1��$��1��$��12(�
�������
�������$��.1q����� $ ElaspsedTime�$ TwoWay=��� $��2��$��0��$��Time Remaining:��$��12(�
�������
�������$��.1q��� -   $��TimeRemaining��$��2��$��1��$��12(�
�������
�������$��.1q����� $ TimeRemaining�$ TwoWay=��� -   $��avgRespTimeRow��$��3��$��0��$��Avg Resp Time��$��12(�
�������
�������$��.1q��� -   $��AvgRespTime��$��3��$��1��$��12(�
�������
�������$��.1q����� $ AvgRespTime�$ TwoWay=��� $��4��$��0��$
��TPS:��$��12(�
�������
�������$��.1q��� -   $��LableTPS��$��4��$��1��$��12(�
�������
�������$��.1q����� $ LableTPS�$ TwoWay=��� $��5��$��0��$��Throughput:��$��12(�
�������
�������$��.1q��� -   $��LblThroughput��$��5��$��1��$��12(�
�������
�������$��.1q����� $ LblThroughput�$ TwoWay=��� $��6��$��0��$��Total Txns:��$��12(�
�������
�������$��.1q��� -   $��	TotalTxns��$��6��$��1��$��12(�
�������
�������$��.1q����� $ 	TotalTxns�$ TwoWay=��� $��7��$��0��$��Total Error:��$��12(�
�������
�������$��.1q��� -   $��
TotalError��$��7��$��1��$��12(�
�������
�������$��.1q����� $ 
TotalError�$ TwoWay=��� $��8��$��0��$��Total Pass:��$��12(�
�������
�������$��.1q��� -   $��	TotalPass��$��8��$��1��$��12(�
�������
�������$��.1q����� $ 	TotalPass�$ TwoWay=��� $��9��$��0��$��Pass Percentage:��$��12(�
�������
�������$��.1q��� -   $��PassPercentage��$��9��$��1��$��12(�
�������
�������$��.1q����� $ PassPercentage�$ TwoWay=��� $��
10,15,10,5q��� -   $��start_loadtest_now��  
BlueButton#���  
�������$��	10,15,1,1q�$	��180��$��Center=�$��Center=�$��30��{��� *StartLoadTestButtonStatus+$ TwoWay=�.��]� .+��� $1��+pack://application:,,,/icons/start_load.png��$��28��$��28���� $�� Start Load Test Now ��$��25��$��Center=�$��12(�$��0,1,0,0q��� -   $��stop_loadtest_now��#���  
�������$��	10,15,1,1q�$	��180��$��Center=�$��Center=�$��30��{��� *StopLoadTestButtonStatus+$ TwoWay=�.��]� .+��� $+��%pack://application:,,,/icons/stop.png��$��28��$��28���� $�� Abort Test Now ��$��25��$��Center=�$��12(�$��0,5,0,0q�� -   $��testProgressBar��$	��180��$��30��$��	10,35,1,1q� � Value �� *ProgressPercentage+$ TwoWay=��� -   $��testProgressText��
�������$��14(�$��SemiBold"�$��Center=�$��	10,10,1,1q�$��Center=�$��Center=����� *ProgressMessage+$ TwoWay=��� -   $ ��server_resource_stackPanel��$ Vertical=�$y�Hidden=�.+��� $��Center=�$��10q�
�������%#Add servers for resource monitoring�� $��
10,0,10,10q��� -   $��add_new_server_button��#���  
�������$��40��$��Center=�.��]� $��5q�.+��� $2��,pack://application:,,,/icons/new_project.png��$��32��$��32���� $�� Add a New Monitor ��$��25��$��Center=��� $��
10,0,10,10q�R� -   $��monitors_tree��
�������
������ $��0q�$��0,10,0,0q�.@�Q� 
������� � ContextMenu ��  �� 	StaysOpen . .@�m� -   $��Add a New Monitor��$��28�� m� Icon �� $2��,pack://application:,,,/icons/new_project.png��$��24��$��24������ $ 
Horizontal=�.+��� $6��0pack://application:,,,/icons/projects_folder.png��$��30��$��30���� $��Active Monitors��
�������$��16(��� #���  ._��� -   $"��execution_center_stack_panel��$y�Hidden=�.+��� -   $	��Top=�$��Stretch=�$��Center=�  BreadCrumbTabControl#���  $��Stretch=�$��Stretch=�
������ .@��� $��Stretch=�  LeftTabItem#���  $��12(�$��38������ .R��� $ 
Horizontal=�.+��� $.��(pack://application:,,,/icons/session.png��$��24��$��24���� $&�� Load Test Metrics and Statistics��$��10,2,0,0q�$��12(�$��DemiBold"�.���� $	��650��.���� .+��� $$��Average Response Time (Top 10)��$��Center=�$��20q�  -   $��averageRespChart��$	��Top=�$��30,-60,50,0q�$	��350��
���   �
�������$	��0.1q�# [ . E   ?System.Windows.Controls.DataVisualization.Charting.ColumnSeries  -   $��averageRespSeries��$ Value��$	 Key��
 �� �	   �System.Windows.Controls.DataVisualization.Charting.ColumnBarBaseSeries`1[[System.Windows.Controls.DataVisualization.Charting.ColumnDataPoint, System.Windows.Controls.DataVisualization.Toolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35]] 	  IndependentAxis E
   ?System.Windows.Controls.DataVisualization.Charting.CategoryAxis
  $ X=�D   >System.Windows.Controls.DataVisualization.Charting.DisplayAxis   AxisLabelStyle �� B   <System.Windows.Controls.DataVisualization.Charting.AxisLabel"  .���  � ��$ Normal"���  � ��$ 12(���  � �� ��  �� 
TargetType"  .R���    FormattedContent#��z  � LayoutTransform ��  �� Angle$	 -75Q�   $ Y=�   Title$ Response Time��   ShowGridLines . $ 0 �� $��Response Time vs time��$��Center=�$��5q�  -      Name$ responseTimeChart��$	��Top=�$��30,-50,50,0q�$	��250��
���   �
�������$	��0.1q�# [    -!   $��RespTimeXaxis��$ X=�$ Elapsed Time��  -"   $��RespTimeYaxis��$ Y=�$ Response Time�� . $ 0 �� $��Summary Table��$��Center=�$��5q�&   System.Windows.Controls.DataGrid  -#   $��test_summary_table_grid��   AutoGenerateColumns .  
������     ColumnHeaderHeight$  30Q�!   CanUserReorderColumns! .  "   CanUserAddRows" .  #   CanUserResizeColumns# . $   CanUserResizeRows$ .  $%   HorizontalScrollBarVisibility$% Disabled=�"&   VerticalScrollBarVisibility$
& Auto=�$	��250��$��12(�
������ $	��0.1q�$��Normal"�$��	80,0,50,0q����� *TransactionMetrices+$ TwoWay=�{��� $ StopLoadTestButtonStatus�$ TwoWay=�'   Columns' 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn(   Header$( Transaction Name��)   
IsReadOnly) . *   Width5  /System.Windows.Controls.DataGridLengthConverter$	* 250 1  +System.Windows.Controls.DataGridBoundColumn+   Binding+ �� $ DisplayName�  $( Min(s)��) . $* 60 + �� $ RespTimeMin�  $( Max(s)��) . $* 60 + �� $ RespTimeMax�  $( Avg.(s)��) . $* 60 + �� $ RespTimeAvg�  $( 85%(s)��) . $* 60 + �� $ 
RespTime85�  $( 90%(s)��) . $* 60 + �� $ 
RespTime90�  $( 95%(s)��) . $* 60 + �� $ 
RespTime95��� $)��#Transaction per second(TPS) vs Time��$��Center=�$��20q�  -$   $��TPSSeriesChart��$	��Top=�$��30,-60,50,0q�$	��250��
���   �
�������$	��0.1q�# [ .   -%   $��	TPSSeries��$ Value��$	 Key��	 . 
���   �
 ��    -&   $��TPSSeriesXAxis��$ X=�$ Elapsed Time��  $ Y=�$ TPS (per sec)�� . $ 0 �� $��ThroughPut vs Time��$��Center=�$��20q�  -'   $��throughPutSeriesChart��$	��Top=�$��30,-60,50,0q�$	��250��
���   �
�������$	��0.1q�# [ .   -(   $��throughPutSeries��$ Value��$	 Key��	 . 
���   �
 ��    -)   $��throughPutSeriesXaxis��$ X=�$ Elapsed Time��  $ Y=�$
 Kbps�� . $ 0 �� $��Stretch=�  BreadCrumbTabItem#���  $��12(�$��38������ .R��� $ 
Horizontal=�.+��� $.��(pack://application:,,,/icons/session.png��$��24��$��24���� $&�� Server Resource Usage Statistics��$��10,2,0,0q�$��12(�$��DemiBold"�.���� $	��650��.���� -*   $)��#server_resource_monitors_stackPanel��