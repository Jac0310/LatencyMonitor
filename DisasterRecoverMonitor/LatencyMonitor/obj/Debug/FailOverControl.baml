   M S B A M L   `   `   `  ����  LatencyMonitor$    LatencyMonitor.FailOverControl   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .��5      �   � RowDefinitions  5	   
   �� 5
      $��30 �6   �� 5      $��* �6   .+�5   
   � $��0��6   .+�5      �� -   5        $��Sender��5      $
��Send��68    � ToolTip$, &Send write traffic to primary database��6G   �� 5      -   5        $
��Stop��5      $
��Stop��6!   $4 .Stop sending write traffic to primary database��6D   �� 5      -   5        $
��Rate��5      $��60��6#   $��Center=�6.   �� 5      $��	(write/s)��6   5      � 5   
   $��0��6   $��Right=�6!   .+�5      �� -   5        $��Close��5      $��X��62   $��Right=�6>   5      �� 5      $��1��6   .@�5      �� $��Status��6   .��5      � .+�5      &   System.Windows.Controls.DataGrid  -   5        $��foGrid��5         AutoGenerateColumns .  ��6-   �� *
statuses+ �� Mode$ TwoWay=�   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$
 Role��61      Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6?   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6M   �� *type+6h     5       $ Server��61   $
 Auto 6A    6O   �� *server+6l     5!      $ Partner Server��61   $
 Auto 6I    6W   �� *partnerServer+6{     5"      $ State��61   $
 Auto 6@    6N   �� *state+6j     5#      $ Last Commit��61   $
 Auto 6F    6T   �� *
lastCommit+ �� StringFormat$  {0:dd.MM.yy HH:mm:ss:fff }��6�     5$      $ Last Replication��61   $
 Auto 6K    6Y   �� *lastReplication+$  {0:dd.MM.yy HH:mm:ss:fff }��6�     5%      $ Transaction Count��61   $
 Auto 6M    6[   �� *transactionCount+6�     5&      $ Last Transaction��61   $
 Auto 6K    6Y   �� *lastTransaction+6   5(        5)      -   5        $��laGrid��5)       .  ��6-   �� *	latencies+$ TwoWay=� 5*        5+      $ Pairing��61   $
 Auto 6B    6P   �� *	pairing+6n     5,      $ Last Commit Latency��61   $
 Auto 6N    6\   �� *lastCommitLatency+$ c��6�     5-      $! Time Since Last Replication��61   $
 Auto 6V    6d   �� *lastReplicationLatency+$ c��6�     5.      $" Transaction Count Difference��61   $
 Auto 6W    6e   �� *transactionDifference+6�   4  .System.Windows.Controls.DataGridCheckBoxColumn  5/      $ Transactions Equal��65   	   
IsReadOnly	 . $
 Auto 6c    6q   �� *transactionsEqual+6�   51      52      53      �� 54      -   5        
 ��Name$
 fgmap��54   #   $	��Map��6   .��55      � .+�56      �� -   5         ��Name$	 map��56      $	��400��6*   $	��300��66   $��0��6C   $��0��6S   ��6`   ��  �� Path$ image�57      58      �� 59      ��5:      � 5;      .+�5<      �� $��Alerts��6$   64   �� 5=      -	   5        $ OverallSeverity��5=       $��20��69   $��20��6D   ��6P   �� *overAllSeverityImage+5>      .��5@        -
   5        $��
alertsGrid��5@       .  ��6-   �� *alerts+$ TwoWay=� 5A        5B      $ Alerts��6-   $ * 6=    6H   �� *	message+6g   4  .System.Windows.Controls.DataGridTemplateColumn  5C      $ Severity��61   $ SizeToCells 6C   	 .    CellTemplate 5D      �� 5E   "   .R�5F   &   �� ��6,   �� *severityImage+5G   #   5I      5K      5L      5M      5N      5O      