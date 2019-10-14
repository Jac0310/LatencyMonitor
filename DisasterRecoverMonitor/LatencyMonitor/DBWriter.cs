using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace LatencyMonitor
{
    public class DBWriter : IPoller
    {

        //This IPoller writes to a given database at given intervals,
        //for the purpose of modelling input traffic
        private Database database;
        private Thread thread;
        private double interval;
        private const double msPerSec = 1000;
        private bool polling;
        public int pollRate { get; }

        public void start()
        {
            thread.Start();
        }

        public void stop()
        {
            polling = false;
            DatabaseDAO.clearInbound(database);
        }

        public DBWriter(Database d, Double rate)
        {
            database = d;
            this.interval = (Double)(1/rate) * msPerSec;
            this.thread = new Thread(send);
            this.pollRate = (int)rate;
        }

        private void send()
        {
            polling = true;
            while (polling)
            {
                try
                {
                    DatabaseDAO.write(database);
                    Thread.Sleep((int)interval);
                    Debug.WriteLine($"Writer poll {database.hostName}");
                }
                catch (Exception e)
                {
                    //polling = false;
                    //MessageBox.Show(e.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    Thread.Sleep((int)interval);
                    Debug.WriteLine("Writer poll exception");
                }              
            }
        }
    }
}
