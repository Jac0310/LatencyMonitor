using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace LatencyMonitor
{
    public abstract class AbstractStatusSubscriber
    {
        //Base class for any component that needs a live update of status and latency
        //Any status subscriber subscribes to status updates by default
        public ObservableCollection<DatabaseReplicationStatus> statuses { get; set; } = new ObservableCollection<DatabaseReplicationStatus>();
        public ObservableCollection<LatencyReading> latencies { get; set; } = new ObservableCollection<LatencyReading>();

        protected readonly Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

        protected FailOverGroup failOverGroup { get; set; }
        protected Boolean isLatencySubscriber { get; set; } = false; //deriving class can specifiy that it is interested in latency

        protected List<DatabaseReplicationStatus> getStatus()
        {
            return failOverGroup.getStatus();
        }

        //Parameter: collection of DatabseReplicationStatus objects
        //Returns a collection of LatencyReadings. Each reading is between the primary and
        //one of its secondary subordinate databases
        private List<LatencyReading> getLatency(IEnumerable<DatabaseReplicationStatus> currentStatus)
        {
            DatabaseReplicationStatus primaryStatus = currentStatus.Where(s => s.type == "PRIMARY").FirstOrDefault();
            List<DatabaseReplicationStatus> secondaryStatus = currentStatus.Where(s => !s.Equals(primaryStatus)).ToList();
            return secondaryStatus.Select(ss => LatencyReading.getReading(primaryStatus, ss)).ToList();
        }

        //This method updates the observable status and latency collections
        public virtual void Update()
        {
            dispatcher.BeginInvoke((Action)delegate
            {
                if (statuses.LongCount() > 0)
                {
                    //if (isStatusSubscriber) statuses?.Clear();
                    statuses?.Clear();
                    if (isLatencySubscriber) latencies?.Clear();
                }
                List<DatabaseReplicationStatus> currentStatus = getStatus();
                if (currentStatus.All(s => s != null))
                {
                    currentStatus.ForEach(s => statuses?.Add(s));
                    if (isLatencySubscriber)
                    {
                        List<LatencyReading> currentLatency = getLatency(currentStatus);
                        currentLatency.ForEach(l => latencies?.Add(l));
                    }            
                }
            });
        }
    }
}
