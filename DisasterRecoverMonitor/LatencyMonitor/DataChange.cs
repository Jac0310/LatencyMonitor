using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatencyMonitor
{
    class DataChange
    {
        public int sequence_id { get; }
        public String storedProcedureName { get; }
        public DateTime executionTime { get; }

        public DataChange(int sequence_id, String storedProcedureName, DateTime executionTime)
        {
            this.sequence_id = sequence_id;
            this.storedProcedureName = storedProcedureName;
            this.executionTime = executionTime;
        }


        public static DataChange fromRaw(IDataReader rs)
        {
            //capital letter column names
            int id = (int)rs["sequence_id"];
            String storedProcecdureName = (String)rs["storedProcedureName"];
            DateTime execTime = (DateTime)rs["execTime"];
            return new DataChange(id, storedProcecdureName, execTime);
        }

        public override bool Equals(Object ob)
        {
            DataChange dc = (DataChange)ob;
            if (dc != null)
            {
                return dc.sequence_id == sequence_id
                    && dc.storedProcedureName.Equals(storedProcedureName)
                    && dc.executionTime.ToString().Equals(executionTime.ToString());
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked 
            {
                int hash = 17;
                hash = hash * 23 + sequence_id.GetHashCode();
                hash = hash * 23 + storedProcedureName.GetHashCode();
                hash = hash * 23 + executionTime.GetHashCode();
                return hash;
            }
        }
    }
}
