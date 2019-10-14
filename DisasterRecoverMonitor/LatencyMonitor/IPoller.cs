using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatencyMonitor
{
    //Interface for any start stop poller
    public interface IPoller
    {
        void start();
        void stop();
    }
}
