   M S B A M L   `   `   `  ����G  CUchetZayavok, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:UchetZayavokUchetZayavok  5   	       UchetZayavok.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:UchetZayavok     5   	     � Title$7  1Учет заявок на мероприятия��5   	   $	��450��6,   $	��800��69   .��5	      �  � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   $��10q�6;   .+�5      �� $'��!Фильтр по статусу:��6   $��Center=�61   �� 5      -   5         ��Name$ StatusFilterComboBox��5      $	��150��65   $	��5,0q�6A    �� DisplayMemberPath$ 
StatusName��5       �� SelectedValuePath$ StatusID��66   5      �� 5      $/��)Фильтр по мероприятию:��6   $��Center=�65   $��10,0,0,0q�6P   �� 5      -   5        $ EventFilterComboBox��5      $	��150��64   $	��5,0q�6@   $ 	EventName��5      $ EventID��65   5      �� 5      -   5         ��Name$ ClearFiltersButton��5      $��Сбросить��61   $��80��6D   $��10,0,0,0q�6O   5      5      &   System.Windows.Controls.DataGrid  5!   
   -   5          Name$ ApplicationsDataGrid��5!      $��1��61   $��10q�6>      AutoGenerateColumns .  	   Columns	 5"      0  *System.Windows.Controls.DataGridTextColumn  5#      ,  &System.Windows.Controls.DataGridColumn
   Header$
 ID��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *ApplicationID+  5$      $
 ФИО��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6O    62   �� *
FullName+6O     5%      $
 Контакты��6%    67   �� *ContactInfo+  5&      $
 Мероприятие��6%    6:   �� *	EventName+  5'      $
 Статус��6%    65   �� *
StatusName+  5(      $
 Дата подачи��6%    6:   �� *ApplicationDate+5*      �� 5-   
   $��2��6   $ 
Horizontal=�6"   $��Right=�6;   $��Bottom=�6W   $��10q�6r   .+�5.      �� -   5        $ 	AddButton��5.      $��Добавить��6(   $	��100��6;   $��5q�6G   6R   �� 5/      -   5        $ 
EditButton��5/      $ ��Редактировать��6)   $	��100��6A   $��5q�6M   6X   �� 50      -   5        $ DeleteButton��50      $��Удалить��6+   $	��100��6=   $��5q�6I   6T   51      52      53      