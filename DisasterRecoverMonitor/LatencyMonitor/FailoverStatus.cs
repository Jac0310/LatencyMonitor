using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace LatencyMonitor
{
    class FailoverStatus : AbstractStatusSubscriber
    {
        public FailoverStatus(FailOverGroup fg)
        {
            isLatencySubscriber = true;
            failOverGroup = fg;
        }
    }
}
