   M S B A M L   `   `   `  ����   LatencyMonitor$    LatencyMonitor.FailOverControl   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     .���   � RowDefinitions  �� $��30 ��� $��* �.+�� $��0��.+��� -   $��Sender��$
��Send�� � ToolTip$, &Send write traffic to primary database���� -   $
��Stop��$
��Stop��$4 .Stop sending write traffic to primary database���� -   $
��Rate��$��60��$��Center=��� $��	(write/s)��� $��0��$��Right=�.+��� -   $��Close��$��X��$��Right=��� $��1��.@��� $��Status��.��� .+�&   System.Windows.Controls.DataGrid  -   $��foGrid��   AutoGenerateColumns .  ���� *
statuses+ �� Mode$ TwoWay=�   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$
 Role��   Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *type+  $ Server��$
 Auto  �� *server+  $ Partner Server��$
 Auto  �� *partnerServer+  $ State��$
 Auto  �� *state+  $ Last Commit��$
 Auto  �� *
lastCommit+ �� StringFormat$  {0:dd.MM.yy HH:mm:ss:fff }��  $ Last Replication��$
 Auto  �� *lastReplication+$  {0:dd.MM.yy HH:mm:ss:fff }��  $ Transaction Count��$
 Auto  �� *transactionCount+  $ Last Transaction��$
 Auto  �� *lastTransaction+  -   $��laGrid�� .  ���� *	latencies+$ TwoWay=�   $ Pairing��$
 Auto  �� *	pairing+  $ Last Commit Latency��$
 Auto  �� *lastCommitLatency+$ c��  $! Time Since Last Replication��$
 Auto  �� *lastReplicationLatency+$ c��  $" Transaction Count Difference��$
 Auto  �� *transactionDifference+4  .System.Windows.Controls.DataGridCheckBoxColumn  $ Transactions Equal��	   
IsReadOnly	 . $
 Auto  �� *transactionsEqual+�� -   
 ��Name$
 fgmap��$	��Map��.��� .+��� -    ��Name$	 map��$	��400��$	��300��$��0��$��0������  �� Path$ image��� ��� .+��� $��Alerts���� -	   $ OverallSeverity��$��20��$��20������ *overAllSeverityImage+.��  -
   $��
alertsGrid�� .  ���� *alerts+$ TwoWay=�   $ Alerts��$ *  �� *	message+4  .System.Windows.Controls.DataGridTemplateColumn  $ Severity��$ SizeToCells 	 .    CellTemplate �� .R��� ���� *severityImage+