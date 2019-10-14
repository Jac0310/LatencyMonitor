using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LatencyMonitor
{
    public class LatencyReading : INotifyPropertyChanged
    {
        //This data object is derived from the difference between two DatabaseReplicationStatus objects
        //It raises a property changed event when updated prompt any interested user interface 
        //component to update
        private TimeSpan _lastCommitLatency;
        public TimeSpan lastCommitLatency
        {
            get { return _lastCommitLatency; }
            set { _lastCommitLatency = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(lastCommitLatency))); }
        }

        private TimeSpan _lastReplicationLatency;
        public TimeSpan lastReplicationLatency
        {
            get { return _lastReplicationLatency; }
            set { _lastReplicationLatency = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(lastReplicationLatency))); }
        }

        private Boolean _transactionsEqual;
        public Boolean transactionsEqual
        {
            get { return _transactionsEqual; }
            set { _transactionsEqual = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(transactionsEqual))); }
        }


        private int _transactionDifference;
        public int transactionDifference
        {
            get { return _transactionDifference; }
            set { _transactionDifference = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(transactionDifference))); }

        }

        private string _pairing;
        public string pairing
        {
            get { return _pairing; }
            set { _pairing = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(pairing))); }
        }

        private LatencyReading(TimeSpan lastCommitLatency, TimeSpan lastReplicationLatency, string pairing, int eventDifference, bool equal)
        {
            this.lastCommitLatency = lastCommitLatency;
            this.lastReplicationLatency = lastReplicationLatency;
            this.transactionDifference = eventDifference;
            this.pairing = pairing;
            this.transactionsEqual = equal;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static LatencyReading getReading(DatabaseReplicationStatus primaryStatus, DatabaseReplicationStatus secondaryStatus)
        {
            if (primaryStatus != null && secondaryStatus != null)
            {
                if (primaryStatus?.partnerServer != "DISCONNECTED" && secondaryStatus?.partnerServer != "DISCONNECTED")
                {
                    return new LatencyReading
                        (primaryStatus.lastCommit - secondaryStatus.lastCommit, // last commit latency
                        DateTime.UtcNow - primaryStatus.lastReplication, // last replication latency
                        secondaryStatus.partnerServer.Substring(7) + " -> " + secondaryStatus.server.Substring(7), // pairing
                        primaryStatus.transactionCount - secondaryStatus.transactionCount, //transaction difference
                        areTransactionsEqual(primaryStatus.transactions, secondaryStatus.transactions)); // transactions equal?
                }
            }              
            return new LatencyReading(new TimeSpan(), new TimeSpan(), "", 0, false);    
        }

        private static bool areTransactionsEqual(IEnumerable<Transaction> t1, IEnumerable<Transaction> t2)
        {
            if (t1 != null && t2 != null)
            {
                return t1.OrderBy(t => t.EndTime).SequenceEqual(
             t2.OrderBy(t => t.EndTime));
            }
            else if (t1 == null && t2 == null) { return true; }
            return false;
        }
    }
}
