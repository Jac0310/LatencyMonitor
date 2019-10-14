using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LatencyMonitor
{
    class Monitor : INotifyPropertyChanged
    {
        private Thread myThread;
        private String deploymentCode;
        private DataChange primaryDataChange;
        private DataChange secondaryDataChange;
        private string primaryDBHostName;
        private string secondaryDBHostName;
        private string dBUserName;
        private string dBPassword;
        private SqlCommand LastEntryCommand;
        private DateTime lastSynchronised;
        public LatencyHistory latencyHistory { get; } // unnecessary complication


        //time since last synchronised - meaningless until lastsynchronised initialised
        string lastSyncLatency;
        public String LastSyncLatency
        {
            get { return this.lastSyncLatency; }
            set { this.lastSyncLatency = value; Notify("LastSyncLatency"); }
        }


        //dataChange last entry time difference across DBs
        string timeDiffLatency;
        public String TimeDiffLatency
        {
            get { return this.timeDiffLatency; }
            set { this.timeDiffLatency = value; Notify("TimeDiffLatency"); }
        }

        string lagDiff;
        public String LagDiff
        {
            get { return this.lagDiff; }
            set { this.lagDiff = value; Notify("LagDiff"); }
        }

        string window;
        public String Window
        {
            get { return this.window; }
            set { this.window = value; Notify("Window"); }
        }


        //Difference in entries to DataChange table across primary/Secondary. Inspection showed table updated every 200ms with sleep(50ms) in PrimaryUpdater
        string missedEntries;
        public String MissedEntries
        {
            get { return this.missedEntries; }
            set { this.missedEntries = value; Notify("MissedEntries"); }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void Notify(String propName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }


        public void startPoll()
        {
            myThread = new Thread(Poll);
            myThread.Start();
        }

        public void stopPoll()
        {
            myThread.Abort();
           
        }

        public Monitor()
        {
            deploymentCode = "JacTest";
            //change query to Riebe's table name.
            LastEntryCommand = new SqlCommand("select * from dataChange where sequence_id = (select MAX(sequence_id) from dataChange)");
            primaryDBHostName = "rye-db-wus-8.database.windows.net";
            secondaryDBHostName = "rye-db-eus-1.database.windows.net";
            dBUserName = "LordOfTheHub";
            dBPassword = "RyedaleWarBand2926";
            latencyHistory = new LatencyHistory();
        }

        private void Poll()
        {
            //connects to two db's using Database class
            // azure test geo - replicated db's

            Database primaryDB = new Database(primaryDBHostName, dBUserName, dBPassword, deploymentCode);
            Database secondaryDB = new Database(secondaryDBHostName, dBUserName, dBPassword, deploymentCode);

            while (true)
            {
                
                secondaryDataChange = getLatestDataChange(secondaryDB);
                primaryDataChange = getLatestDataChange(primaryDB);

                if (primaryDataChange.Equals(secondaryDataChange))
                {
                    Window = LastSyncLatency;
                    lastSynchronised = DateTime.Now;
                    if (LastSyncLatency != null)
                    {
                        latencyHistory.Add(TimeSpan.Parse(LastSyncLatency));
                    }
                    
                }

                if (lastSynchronised.Year != 1)
                {
                    LastSyncLatency = formatSpan(getLatency(DateTime.Now, lastSynchronised));
                    
                }
      
                MissedEntries = (primaryDataChange.sequence_id - secondaryDataChange.sequence_id).ToString();
                TimeDiffLatency = formatSpan(getLatency(primaryDataChange.executionTime, secondaryDataChange.executionTime));
                LagDiff = formatSpan(getLatency(primaryDataChange, secondaryDataChange));
                latencyHistory.Add(Convert.ToInt32(MissedEntries));
                Thread.Sleep(1000);
            }

        }

        private TimeSpan getLatency(DateTime t1, DateTime t2)
        {
            TimeSpan span = t1.Subtract(t2);
            return span;
        }

        private TimeSpan getLatency(TimeSpan t1, TimeSpan t2)
        {
            TimeSpan span = t1.Subtract(t2);
            return span;
        }

        private TimeSpan getLatency(DataChange d1, DataChange d2)
        {
            DateTime now = DateTime.Now;
            return getLatency(getLatency(now, d2.executionTime), getLatency(now, d1.executionTime));
        }

        private String formatSpan(TimeSpan t)
        {
            return t.ToString(@"hh\:mm\:ss\.ff");
        }


        private DataChange getLatestDataChange(Database db)
        {
            return (DataChange)db.executeReader(LastEntryCommand, rs =>
            {
                while (rs.Read())
                {
                    return DataChange.fromRaw(rs);
                }
                return null;
            });
        }
    }
}
