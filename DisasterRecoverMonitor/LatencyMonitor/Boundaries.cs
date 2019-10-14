using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatencyMonitor
{
    public class Boundaries<T> where T : IComparable
    {
        //A generic class used to differentiate the severity of any problem
        // For any paramName an inner and outer breach can be defined

        private static Dictionary<string, Boundaries<IComparable>> all = new Dictionary<string, Boundaries<IComparable>>();
        public T outer { get; }
        public T inner { get; }
        private String paramName { get; }
        private Boundaries(T outer, T inner, String parameterName)
        {
            this.outer = outer;
            this.inner = inner;
            this.paramName = parameterName;
        }

        public Boolean outerExceeded(T obj)
        {
            return obj.CompareTo(outer) > 0;
        }

        public Boolean innerExceeded(T obj)
        {
            return obj.CompareTo(inner) > 0 && obj.CompareTo(outer) < 0;
        }

        public static Boundaries<IComparable> GetBoundaries(String parameterName)
        {
            return all[parameterName];          
        }

        //Static initialiser
        static Boundaries()
        {
            Boundaries<IComparable> b1 = new Boundaries<IComparable>(20, 5, "transactionDifference");
            all.Add("transactionDifference", b1);

            Boundaries<IComparable> b = new Boundaries<IComparable>(new TimeSpan(0, 2, 0), new TimeSpan(0, 1, 0), "lastCommitLatency");
            all.Add("lastCommitLatency", b);        
        }
    }
}
