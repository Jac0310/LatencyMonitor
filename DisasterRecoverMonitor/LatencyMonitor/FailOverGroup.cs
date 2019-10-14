using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatencyMonitor
{
    public class FailOverGroup
    {
        //A collection of databases
        //With one primary and multiple subordinate secondaries
        //Subscribes to any change in the Database.all collection
        //And fires the statusChanged event when changed
        private Guid primary;
        public string name;
        private Dictionary<Guid, DatabaseReplicationStatus> statusMap = new Dictionary<Guid, DatabaseReplicationStatus>();
        public event Action statusChanged;
        public FailOverGroup(Database primary)
        {
            this.primary = primary.guid;
            name = primary.name;
            statusMap.Add(primary.guid, primary.status);
            Database.all.CollectionChanged += (s, e) => updateStatus();
        }

        public Database getPrimary()
        {
            return DatabaseDAO.get(primary);
        }

        private void updateStatus()
        {          
            statusMap.Keys.ToList().ForEach(g => statusMap[g] = DatabaseDAO.get(g).status);     
            if (statusMap.Values.All(s => s != null))
            {
                Guid newPrimary = statusMap.Keys.Where(g => DatabaseDAO.get(g).status.type == "PRIMARY").FirstOrDefault();
                //if current statuses do not indicate which is primary maintain the current
                if (newPrimary != Guid.Empty) primary = newPrimary; 
            }
            statusChanged?.Invoke();
        }

        public List<DatabaseReplicationStatus> getStatus()
        {
            return statusMap.Values.ToList();
        }

        public List<Database> getDatabases()
        {
            return statusMap.Keys.Select(g => DatabaseDAO.get(g)).ToList();
        }

        public void AddSecondary(Database secondary)
        {
            statusMap.Add(secondary.guid, secondary.status);
        }
    }
}
