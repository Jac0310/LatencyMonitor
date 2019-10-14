using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatencyMonitor
{
    class LatencyHistory
    {
        public ICollection<int> GapHistory { get;  }
        public ICollection<TimeSpan> WindowHistory;

        public LatencyHistory()
        {
            GapHistory = new List<int>();
            WindowHistory = new List<TimeSpan>();
        }

        public void Add(int gap)
        {  
            GapHistory.Add(gap);
        }

        public void Add(TimeSpan span)
        {
            WindowHistory.Add(span);
        }

    }
}
