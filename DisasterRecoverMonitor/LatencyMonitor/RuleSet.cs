using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatencyMonitor
{
    public class RuleSet <T>
    {
        //A collection of rules of a specific type
        static IEnumerable<Rule> rules = Rule.getRules(typeof(T));

        private static List<Alert> checkRules(T t)
        {
            return rules.Select(r => r.apply(t)).ToList();
        }

        public static IEnumerable<Alert> checkRules(List<T> ts)
        {
            return ts.SelectMany(t => checkRules(t));
        }
    }
}
