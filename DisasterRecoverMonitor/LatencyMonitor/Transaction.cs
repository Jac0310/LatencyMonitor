using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatencyMonitor
{
    public class Transaction
    {
        //This data object represents an entry in the transaction log.

        public String CurrentLSN;
        public String TransactionID;
        public String Operation;
        public DateTime EndTime;
        public long XactID;


        public Transaction(String CurrentLSN, String TransactionID, String Operation, long XactID, DateTime EndTime)
        {
            this.CurrentLSN = CurrentLSN;
            this.TransactionID = TransactionID;
            this.Operation = Operation;
            this.XactID = XactID;
            this.EndTime = EndTime;
        }

        public static Transaction FromRaw(IDataReader rs)
        {
            DateTime end = DateTime.ParseExact((String)rs["End Time"], "yyyy/MM/dd HH:mm:ss:fff", null);
            return new Transaction((String)rs["Current LSN"], (String)rs["Transaction ID"], (String)rs["Operation"], (long)rs["Xact ID"], end);
        }

        public override bool Equals(object obj)
        {
            Transaction t2 = obj as Transaction;
            return t2.CurrentLSN.Equals(this.CurrentLSN) &&
                t2.TransactionID.Equals(this.TransactionID) &&
                t2.Operation.Equals(this.Operation) &&
                t2.EndTime.Equals(this.EndTime) &&
                t2.XactID.Equals(this.XactID);
        }

        public override int GetHashCode()
        {
            var hashCode = -2083669842;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CurrentLSN);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(TransactionID);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Operation);
            hashCode = hashCode * -1521134295 + EndTime.GetHashCode();
            hashCode = hashCode * -1521134295 + XactID.GetHashCode();
            return hashCode;
        }
    }
}
