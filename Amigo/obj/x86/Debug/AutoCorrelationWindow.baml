   M S B A M L   `   `   `  ����	  Amigo!    Amigo.AutoCorrelationWindow   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -   5        $��autoCorrelationUC��5      .��5      ��    BorderStyle#���   6   ._�5	   
   �� .+�5
      �� $=��7Select the boundary set from following eligible matches��6   $��Center=�6V   
�������$��14(�6�   �� 5      -   5        $��	paramName��5      $�� ��6)   $��Center=�64   
�������$��14(�6d   �� 5      -   5        $��
paramValue��5      $+��%Scanning for possible correlations...��6*   $��Center=�6Z   
�������$��14(�6�   � 5      -   5        $��corrProgressBar��5      $	��300��65   $��Center=�6A   $y�Visible=�6^   $��20��6s   $��5q�6   &   System.Windows.Controls.DataGrid  5      -   5        $��autoCorrelationGrid��5      $��10q�66       AlternatingRowBackground
  ��ح�   AutoGenerateColumns .     CanUserResizeColumns .    CanUserAddRows .  
������    ColumnHeaderHeight$ 30Q�5         CanUserReorderColumns .  $   HorizontalScrollBarVisibility$ Disabled=�5      "   VerticalScrollBarVisibility$
 Auto=�5      $	��400��5      ��5      ��    Columns 5      4  .System.Windows.Controls.DataGridTemplateColumn  5      ,  &System.Windows.Controls.DataGridColumn	   Header$	  ��6-   
   Width5  /System.Windows.Controls.DataGridLengthConverter$
 25 67      CellTemplate 5      �� 5      .R�5   "   	�  d� 	IsChecked 6.   ��  �� Path$ 	ISChecked�5      5!      0  *System.Windows.Controls.DataGridTextColumn  5"      $	 Left Boundary��6)      
Foreground
 �   �   FontSize$ 14Q�6S   $	
 200 6a   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6m   �� $ LB�  5#      $	 Right Boundary��6)   
 �   �$ 14Q�6T   $	
 200 6b    6n   �� $ RB�5%      �� 5&      -   5        $��select_button��5&   K     
BlueButton#���  6   {�. $��40��6a   $	��200��6m   $��	20,0,10,0q�6y   .��5'      ]� .+�5(      �� -   5         ��Name$ new_project_image��5(      $3��-pack://application:,,,/icons/save_project.png��67   $��38��6n   $��38��6z   $
��Left=�6�   6�   �� 5)      $��Select and Close��6    $��14(�68   
�������$��Center=�6Y   $��Right=�6t   $��20,0,0,0q�6�   5*      5+      5,      5-      5.      