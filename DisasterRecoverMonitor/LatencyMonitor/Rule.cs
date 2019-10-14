using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatencyMonitor
{
    public class Rule
    {
        //A rule is applied to an object
        //And returns an alert, with a message
        //and a specific severity, depending on boundaries set
        private string name;
        private Type type { get; }
        private Func<object, Alert> func;
        private static Dictionary<String, Rule> nameRuleMap = new Dictionary<string, Rule>();

        public static  IEnumerable<Rule> getRules(Type t)
        {
            return nameRuleMap.Values.Where(r => r.getObjectType() == t);
        }

        private Rule(string name, string parameterName, Type type, Func<Object, Alert> func)
        {
            this.name = name;
            this.type = type;
            this.func = func;
        }

        public Type getObjectType()
        {
            return type;
        }

        public Alert apply(object o)
        {
            Alert alert = func(o);
            if (alert != null) { return alert; }
            return null;
        }

        static Rule()
        {
            Rule r;
            Boundaries<IComparable> lastCommitBoundaries = Boundaries<IComparable>.GetBoundaries("lastCommitLatency");
            Boundaries<IComparable> transDiffBoundaries = Boundaries<IComparable>.GetBoundaries("transactionDifference");
            r = new Rule("last Commit Latency Exceeds inner tolerance", "lastCommitLatency",  typeof(LatencyReading), l => 
            {
                LatencyReading lr = l as LatencyReading;
                if (lastCommitBoundaries.innerExceeded(lr.lastCommitLatency.Duration()))
                {
                    return new Alert ($"last Commit Latency between {lr.pairing} Exceeds inner tolerance of {lastCommitBoundaries.inner}", Severity.WARNING);
                }
                return null;
            } );
            nameRuleMap.Add(r.name, r);

            r = new Rule("last Commit Latency Exceeds outer tolerance", "lastCommitLatency", typeof(LatencyReading), l =>
            {
                LatencyReading lr = l as LatencyReading;
                if (lastCommitBoundaries.outerExceeded(lr.lastCommitLatency.Duration()))
                {
                    return new Alert($"last Commit Latency between {lr.pairing} Exceeds outer tolerance of {lastCommitBoundaries.outer}", Severity.ERROR);
                }
                return null;
            });
            nameRuleMap.Add(r.name, r);

            r = new Rule("Transaction difference Exceeds outer tolerance", "transactionDifference", typeof(LatencyReading), l =>
            {         
                LatencyReading lr = l as LatencyReading;
                if (transDiffBoundaries.outerExceeded(Math.Abs(lr.transactionDifference)))
                {
                    return new Alert($"Transaction difference of {lr.transactionDifference} between {lr.pairing} Exceeds outer tolerance of {transDiffBoundaries.outer}", Severity.ERROR);
                }
                return null;
            });
            nameRuleMap.Add(r.name, r);

            r = new Rule("Transaction difference Exceeds inner tolerance", "transactionDifference", typeof(LatencyReading), l =>
            {
                LatencyReading lr = l as LatencyReading;
                if (transDiffBoundaries.innerExceeded(Math.Abs(lr.transactionDifference)))
                {
                    return new Alert($"Transaction difference of {lr.transactionDifference} between {lr.pairing} Exceeds inner tolerance of {transDiffBoundaries.outer}", Severity.WARNING);
                }
                return null;
            });
            nameRuleMap.Add(r.name, r);

            r = new Rule("Database is disconnected", "databaseRepicationStatus", typeof(DatabaseReplicationStatus), s =>
            {
                DatabaseReplicationStatus st = s as DatabaseReplicationStatus;
                if (st.partnerServer.Equals("DISCONNECTED"))
                {
                    return new Alert(st.state, Severity.ERROR);
                }
                return null;
            });
            nameRuleMap.Add(r.name, r);
        }
    }
}
