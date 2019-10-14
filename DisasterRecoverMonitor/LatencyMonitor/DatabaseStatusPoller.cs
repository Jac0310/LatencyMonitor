using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace LatencyMonitor
{
    class DatabaseStatusPoller : IPoller
    {
        //An IPoller that updates the replication status of a given database,
        //at a defined interval.
        //Errors in connection result in updating the database to a disconnected status
        //With the error message that caused the problem.

        private Database d;
        private Double interval;
        private Thread t;
        public DatabaseStatusPoller(Database d, Double interval)
        {
            this.d = d;
            this.interval = interval;
        }

        public void start()
        {
            t = new Thread(poll);
            t.Start();
        }

        public void stop()
        {
            t.Abort();
        }

        private void poll()
        {
            while (true)
            {
                try
                {
                    DatabaseDAO.updateReplicationStatus(d);
                }
                catch (Exception e)
                {
                    DatabaseDAO.showDisconnectedStatus(d, e.Message);
                }
                try
                {
                    Thread.Sleep((int)interval);
                }
                catch (Exception e)
                {
                    DatabaseDAO.showDisconnectedStatus(d, e.Message);
                }
            }
        }
    }
}
