   M S B A M L   `   `   `  ����|  xSystem.Windows.Controls.DataVisualization.Toolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35�{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit2System.Windows.Controls.DataVisualization.Charting  5      	 Amigo   Amigo.ReportSubWindow   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      �chartingToolkit{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit     5      .��5      � .+�5	   
   ��    BorderStyle#���   6   ._�5
      �� .��5      �� -   5        $��
LayoutRoot��5        �� Orientation$  Vertical=�6/   $
��2000��6F   .+�5      �� $��Report Quick Over View��6    $��15(�6>   
�������$��Center=�6_   $��	0,10,0,10q�6z   $��Center=�6�   �� 5      -   5        $��Close_button��5      $��18(�61   
�������  
BlueButton#���  6R   {�. $��40��5   )   $	��350��65   $��0,20,0,0q�6A   $��Center=�6S   $��Center=�6p   .��5      ]� $��5q�6$   .+�5      �� $1��+pack://application:,,,/icons/start_load.png��6$   $��32��6Y   $��32��6e   6r   �� 5      $'��!Close Window and Open Full Report��6(   $��15(�6Q   
�������$��Center=�6r   $��10,0,0,0q�6�   5      5      �� 5      $��Summary Table��6    $��Center=�65   $��5q�6R   
�������$��14(�6p   &   System.Windows.Controls.DataGrid  5      -   5        $��test_summary_table_grid��5         AutoGenerateColumns .  
������    ColumnHeaderHeight$ 30Q�5   +      CanUserReorderColumns .     CanUserAddRows .     CanUserResizeColumns .    CanUserResizeRows .  $   HorizontalScrollBarVisibility$ Disabled=�5   +   "   VerticalScrollBarVisibility$
 Auto=�5   +   $
��auto��5    +   $��12(�5!   +   {�. 
������ $	��0.1q�5"   E   $��Normal"�6[   $��	80,0,50,0q�5#   +   ��5   +   �� 	   Columns	 5$      0  *System.Windows.Controls.DataGridTextColumn  5%      ,  &System.Windows.Controls.DataGridColumn
   Header$
 Transaction Name��6V      
IsReadOnly .    Width5  /System.Windows.Controls.DataGridLengthConverter$	 250 6�   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   ��  �� Path$ DisplayName�6�     5&      $
 Min(s)��6V    . $ 60 6x    61   �� $ RespTimeMin�6x     5'      $
 Max(s)��6V    . $ 60 6x    61   �� $ RespTimeMax�6x     5(      $
 Avg.(s)��6V    . $ 60 6y    61   �� $ RespTimeAvg�6y     5)      $
 85%(s)��6U    . $ 60 6w    61   �� $ 
RespTime85�6w     5*      $
 90%(s)��6U    . $ 60 6w    61   �� $ 
RespTime90�6w     5+      $
 95%(s)��6U    . $ 60 6w    61   �� $ 
RespTime95�6w   5-      >   8System.Windows.Controls.DataVisualization.Charting.Chart  5.      -   5        $��responseTimeSeriesChart��5.   S      Title$$ Response Time(All Txn) Vs Time��6,   $	��Top=�6r   $��80,30,50,30q�6�   $	��350��6�   
���   �
���   �
�������$	��0.1q�5/   W     BlackLegendStyle   LegendStyle# [ 6m      Axes 51      C   =System.Windows.Controls.DataVisualization.Charting.LinearAxis  =   7System.Windows.Controls.DataVisualization.Charting.Axis   Orientation$ Y=�69   D	   >System.Windows.Controls.DataVisualization.Charting.DisplayAxis 	  Title$ Response Time(s)��6I    	  ShowGridLines . D
   >System.Windows.Controls.DataVisualization.Charting.NumericAxis 
  Minimum�   �System.Windows.Controls.DataVisualization.Charting.NullableConverter`1[[System.Double, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]$ 0 6w   53        54      -   5        $��analysisPieSeriesChart��54   Y   $* $Dynamic contents Vs. Static Contents��6,   $	��Top=�6w   $��80,30,50,30q�6�   $	��350��6�   
���   �
���   �
�������$	��0.1q�55   W   # [ 6m      Series. 56      B   <System.Windows.Controls.DataVisualization.Charting.PieSeries  -   5        $��analysisPieSeries��56   4   H   BSystem.Windows.Controls.DataVisualization.Charting.DataPointSeries   DependentValuePath$ Value��6M      IndependentValuePath$	 Key��6h      ItemsSource 6�   �� 57        58      -   5        $��staticPieSeriesChart��58   Q   $" Static contents distribution��6,   $	��Top=�6m   $��80,30,50,30q�6�   $	��350��6�   
���   �
���   �
�������$	��0.1q�59   W   # [ 6m   . 5:        -   5        $��staticPieSeries��5:   4   $ Value��6K   $	 Key��6f    6�   �� 5;      5<      5=      5>      5?      5@      