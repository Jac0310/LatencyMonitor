using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LatencyMonitor
{

    //test class to cause a change to the primary db for latency detection through geo replication 


    class PrimaryUpdater
    {
        private Thread myThread;
        private SqlCommand eventupdate;
        private SqlCommand deleteStaleEntries;
        private string dbHostname;
        private string dBUserName;
        private string dBPassword;
        private string deploymentCode;

        public PrimaryUpdater()
        {
            eventupdate = new SqlCommand("exec evt.eventUpdate @event_id = 'B12E749C-C164-44B4-AAF1-0000DF76076C', @severity_id = 2, @description = 'External Subs/ Reds + USD 36,863.10 SKDJ TD: 11 Apr 2017 SD: 11 Apr 2017'");
            deleteStaleEntries = new SqlCommand("delete from dataChange where execTime < (dateadd(minute, -5, (select MAX(exectime) from dataChange)))");
            dbHostname = "rye-db-wus-8.database.windows.net";
            dBUserName = "LordOfTheHub";
            dBPassword = "RyedaleWarBand2926";
            deploymentCode = "JacTest";
        }

        public void startContinuousUpdate()
        {
            myThread = new Thread(Update);
            myThread.Start();
        }

        public void stopContinuousUpdate()
        {
            myThread.Abort();
        }

        private void Update()
        {
            Database DB = new Database(dbHostname, dBUserName, dBPassword, deploymentCode);
            while (true)
            {
                DB.executeNonQuery(eventupdate);
                DB.executeNonQuery(deleteStaleEntries);
                Thread.Sleep(100);
            }
        }
    }
}
