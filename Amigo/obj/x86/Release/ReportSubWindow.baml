   M S B A M L   `   `   `  ���� |  xSystem.Windows.Controls.DataVisualization.Toolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35�{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit2System.Windows.Controls.DataVisualization.Charting  	 Amigo   Amigo.ReportSubWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     �chartingToolkit{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit     .��� .+���    BorderStyle#���   ._��� .���� -   $��
LayoutRoot��  �� Orientation$  Vertical=�$
��2000��.+��� $��Report Quick Over View��$��15(�
�������$��Center=�$��	0,10,0,10q�$��Center=��� -   $��Close_button��$��18(�
�������  
BlueButton#���  {�. $��40��$	��350��$��0,20,0,0q�$��Center=�$��Center=�.��]� $��5q�.+��� $1��+pack://application:,,,/icons/start_load.png��$��32��$��32���� $'��!Close Window and Open Full Report��$��15(�
�������$��Center=�$��10,0,0,0q��� $��Summary Table��$��Center=�$��5q�
�������$��14(�&   System.Windows.Controls.DataGrid  -   $��test_summary_table_grid��   AutoGenerateColumns .  
������    ColumnHeaderHeight$ 30Q�   CanUserReorderColumns .     CanUserAddRows .     CanUserResizeColumns .    CanUserResizeRows .  $   HorizontalScrollBarVisibility$ Disabled=�"   VerticalScrollBarVisibility$
 Auto=�$
��auto��$��12(�{�. 
������ $	��0.1q�$��Normal"�$��	80,0,50,0q����� 	   Columns	 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn
   Header$
 Transaction Name��   
IsReadOnly .    Width5  /System.Windows.Controls.DataGridLengthConverter$	 250 1  +System.Windows.Controls.DataGridBoundColumn   Binding ��  �� Path$ DisplayName�  $
 Min(s)�� . $ 60  �� $ RespTimeMin�  $
 Max(s)�� . $ 60  �� $ RespTimeMax�  $
 Avg.(s)�� . $ 60  �� $ RespTimeAvg�  $
 85%(s)�� . $ 60  �� $ 
RespTime85�  $
 90%(s)�� . $ 60  �� $ 
RespTime90�  $
 95%(s)�� . $ 60  �� $ 
RespTime95�>   8System.Windows.Controls.DataVisualization.Charting.Chart  -   $��responseTimeSeriesChart��   Title$$ Response Time(All Txn) Vs Time��$	��Top=�$��80,30,50,30q�$	��350��
���   �
���   �
�������$	��0.1q�  BlackLegendStyle   LegendStyle# [    Axes C   =System.Windows.Controls.DataVisualization.Charting.LinearAxis  =   7System.Windows.Controls.DataVisualization.Charting.Axis   Orientation$ Y=�D	   >System.Windows.Controls.DataVisualization.Charting.DisplayAxis 	  Title$ Response Time(s)�� 	  ShowGridLines . D
   >System.Windows.Controls.DataVisualization.Charting.NumericAxis 
  Minimum�   �System.Windows.Controls.DataVisualization.Charting.NullableConverter`1[[System.Double, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]$ 0   -   $��analysisPieSeriesChart��$* $Dynamic contents Vs. Static Contents��$	��Top=�$��80,30,50,30q�$	��350��
���   �
���   �
�������$	��0.1q�# [    Series. B   <System.Windows.Controls.DataVisualization.Charting.PieSeries  -   $��analysisPieSeries��H   BSystem.Windows.Controls.DataVisualization.Charting.DataPointSeries   DependentValuePath$ Value��   IndependentValuePath$	 Key��   ItemsSource ��   -   $��staticPieSeriesChart��$" Static contents distribution��$	��Top=�$��80,30,50,30q�$	��350��
���   �
���   �
�������$	��0.1q�# [ .   -   $��staticPieSeries��$ Value��$	 Key�� �� 