   M S B A M L   `   `   `  ���� 	  Amigo!    Amigo.AutoCorrelationWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     -   $��autoCorrelationUC��.����    BorderStyle#���   ._��� .+��� $=��7Select the boundary set from following eligible matches��$��Center=�
�������$��14(��� -   $��	paramName��$�� ��$��Center=�
�������$��14(��� -   $��
paramValue��$+��%Scanning for possible correlations...��$��Center=�
�������$��14(�� -   $��corrProgressBar��$	��300��$��Center=�$y�Visible=�$��20��$��5q�&   System.Windows.Controls.DataGrid  -   $��autoCorrelationGrid��$��10q�    AlternatingRowBackground
  ��ح�   AutoGenerateColumns .     CanUserResizeColumns .    CanUserAddRows .  
������    ColumnHeaderHeight$ 30Q�   CanUserReorderColumns .  $   HorizontalScrollBarVisibility$ Disabled=�"   VerticalScrollBarVisibility$
 Auto=�$	��400������    Columns 4  .System.Windows.Controls.DataGridTemplateColumn  ,  &System.Windows.Controls.DataGridColumn	   Header$	  ��
   Width5  /System.Windows.Controls.DataGridLengthConverter$
 25    CellTemplate �� .R�	�  d� 	IsChecked ��  �� Path$ 	ISChecked�0  *System.Windows.Controls.DataGridTextColumn  $	 Left Boundary��   
Foreground
 �   �   FontSize$ 14Q�$	
 200 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� $ LB�  $	 Right Boundary��
 �   �$ 14Q�$	
 200  �� $ RB��� -   $��select_button��  
BlueButton#���  {�. $��40��$	��200��$��	20,0,10,0q�.��]� .+��� -    ��Name$ new_project_image��$3��-pack://application:,,,/icons/save_project.png��$��38��$��38��$
��Left=��� $��Select and Close��$��14(�
�������$��Center=�$��Right=�$��20,0,0,0q�