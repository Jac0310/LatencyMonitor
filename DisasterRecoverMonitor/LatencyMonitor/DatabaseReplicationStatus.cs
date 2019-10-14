using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatencyMonitor
{
    public class DatabaseReplicationStatus : INotifyPropertyChanged
    {
        //This data object describes the current status of a database
        //With regard to geo replication

        public event PropertyChangedEventHandler PropertyChanged;
        private int _transactionCount;
        public int transactionCount
        {
            get { return _transactionCount; }
            set { _transactionCount = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(transactionCount))); }
        }

        public IEnumerable<Transaction> transactions { get; set; }

        private string _partnerServer;
        public string partnerServer
        {
            get { return _partnerServer; }
            set { _partnerServer = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(partnerServer))); }
        }

        private DateTime _lastTransaction;
        public DateTime lastTransaction
        {
            get { return _lastTransaction; }
            set { _lastTransaction = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(lastTransaction))); }
        }

        private string _server;
        public string server
        {
            get { return _server; }
            set { _server = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(server))); }
        }


        //The time of last transaction committed to the database.
        //If retrieved on the primary database, it indicates the last commit time on the primary database. 
        //If retrieved on the secondary database, it indicates the last commit time on the secondary database.
        //If retrieved on the secondary database when the primary of the replication link is down, it indicates until what point the secondary has caught up.
        private DateTime _lastCommit;
        public DateTime lastCommit
        {
            get { return _lastCommit; }
            set { _lastCommit = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(lastCommit))); }
        }

        //The timestamp of the last transaction’s acknowledgement by the secondary based on the primary database clock. 
        //This value is available on the primary database only.

        private DateTime _lastReplication;
        public DateTime lastReplication
        {
            get { return _lastReplication; }
            set { _lastReplication = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(lastReplication))); }
        }



        private string _type;
        public string type 
        {
            get { return _type; }
            set { _type = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(type))); }
        }

        private string _state;
        public string state
        {
            get { return _state; }
            set { _state = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(state))); }
        }

        public DatabaseReplicationStatus(string partnerServer, String type, string state, DateTimeOffset lastCommit, DateTimeOffset lastReplication = new DateTimeOffset())
        {
            this.state = state;
            this.partnerServer = partnerServer;
            this.type = type;
            this.lastCommit = lastCommit.DateTime;
            this.lastReplication = lastReplication.DateTime;
        }

        public static DatabaseReplicationStatus FromRaw(IDataReader rs)
        {
            var lastRep = rs["last_replication"];
            if (lastRep.GetType() != typeof(DBNull)) //Primary
            {
                return new DatabaseReplicationStatus((string)rs["partner_server"], (string)rs["role_desc"], (string)rs["replication_state_desc"], (DateTimeOffset)rs["last_commit"], (DateTimeOffset)lastRep);
            }
            //Secondary
            return new DatabaseReplicationStatus((string)rs["partner_server"], (string)rs["role_desc"], (string)rs["replication_state_desc"], (DateTimeOffset)rs["last_commit"]);

        }       
    }
}
