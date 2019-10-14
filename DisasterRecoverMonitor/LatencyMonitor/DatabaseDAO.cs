using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatencyMonitor
{
    internal static class DatabaseDAO
    {
        //Facade object to handle database queries

        private static String fetchServerName(Database d, List<String> servers)
        {
            SqlCommand sc = new SqlCommand("SELECT @@SERVERNAME AS 'Server Name'");

            d.executeReader(sc, rs =>
            {
                while (rs.Read())
                {
                    servers.Add((String)rs["Server Name"]);
                }
                return null;

            });
            return servers.FirstOrDefault();
        }

        public static Database get(Guid guid)
        {
            return Database.all.Where(d => d.guid == guid).FirstOrDefault();
        }

        //testing facility

        public static void logLatency(IEnumerable<LatencyReading> latencies, int pollRate, Database primary)
        {
            List<SqlCommand> queries = latencies.Select(lr => new SqlCommand($"insert into DRM.LatencyLog (pairing, transactionDiff, lastCommitLatency, lastReplicationLatency, pollPerSec)" +
                $" values ('{lr.pairing}', {lr.transactionDifference}, {lr.lastCommitLatency.Ticks}, {lr.lastReplicationLatency.Ticks}, {pollRate})")).ToList();
            queries.ForEach(q => primary.executeNonQuery(q));
        }

        private static IEnumerable<Transaction> fetchTransactions(Database d, List<Transaction> ts)
        {
            SqlCommand sc = new SqlCommand("select [Current LSN], [Transaction ID], [Xact ID], Operation, [End Time] from sys.fn_dblog(null, null) where[End Time] is not null");

            d.executeReader(sc, rs =>
            {
                while (rs.Read())
                {
                    ts.Add(Transaction.FromRaw(rs));
                }
                return null;
            });
            return ts;
        }

        public static void write(Database d)
        {
            SqlCommand spWrite = new SqlCommand("DRM.Write") { CommandType = CommandType.StoredProcedure };
            d.executeNonQuery(spWrite);
        }

        public static void clearInbound(Database d)
        {
            SqlCommand sc = new SqlCommand("delete from drm.Inbound where sequential_id > 0");
            d.executeNonQuery(sc);
        }

        public static void updateReplicationStatus(Database d)
        {
            StatusUpdater su = new StatusUpdater(d);
            su.updateDatabaseStatus();
        }

        private static DatabaseReplicationStatus getStatus(Database d)
        {
            SqlCommand sc = new SqlCommand("select * from sys.dm_geo_replication_link_status");
            try
            {
                return (DatabaseReplicationStatus)d.executeReader(sc, rs =>
                {
                    while (rs.Read())
                    {
                        return DatabaseReplicationStatus.FromRaw(rs);
                    }
                    return null;
                });
            }
            catch (Exception e)
            {
                showDisconnectedStatus(d, e.Message);
                return null;
            }         
        }

        private static DatabaseReplicationStatus enrichStatus(DatabaseReplicationStatus s, IEnumerable<Transaction> ts, String serverName)
        {
            if (ts.Count() > 0)
            {
                s.transactionCount = ts.Count();
                s.transactions = ts;
                s.lastTransaction = ts.Last().EndTime;
            }
            s.server = serverName;
            return s;
        }

        public static void showDisconnectedStatus(Database d, String message)
        {
            d.status = new DatabaseReplicationStatus("DISCONNECTED", "DISCONNECTED", message, new DateTimeOffset());
        }

        class StatusUpdater // threading fix
        {
            //constructs a status object and then updates the database
            private Database database;
            public StatusUpdater(Database d)
            {
                this.database = d;
            }

            public void updateDatabaseStatus()
            {
                DatabaseReplicationStatus status = constructStatus();
                database.status = status;
            }

            private DatabaseReplicationStatus constructStatus()
            {
                DatabaseReplicationStatus status = getStatus(database);
                IEnumerable<Transaction> ts = fetchTransactions(database, new List<Transaction>());
                String serverName = fetchServerName(database, new List<string>());
                status = enrichStatus(status, ts, serverName);
                return status;
            }
        }
    }
}
