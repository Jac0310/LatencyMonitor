using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LatencyMonitor
{
    public class FailOverGroupPoller : IPoller
    {
        //A facade interface to a group of databaseStatusPollers within a failOverGroup
        private HashSet<DatabaseStatusPoller> pollers = new HashSet<DatabaseStatusPoller>();

        private FailOverGroup failOverGroup;
        private Double interval;

        public FailOverGroupPoller(FailOverGroup fg, Double interval = 5000)
        {
            this.failOverGroup = fg;
            this.interval = interval;
        }

        public void start()
        {
            failOverGroup.getDatabases().ForEach(d => startDatabasePoller(d));
        }

        public void stop()
        {
            pollers.ToList().ForEach(dstp => dstp.stop());
        }

        private void startDatabasePoller(Database d)
        {
            DatabaseStatusPoller dstp = new DatabaseStatusPoller(d, interval);
            pollers.Add(dstp);
            dstp.start();
        }       
    }
}
