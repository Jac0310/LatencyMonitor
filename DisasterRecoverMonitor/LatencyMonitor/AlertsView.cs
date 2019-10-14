using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media.Imaging;

namespace LatencyMonitor
{
    public class AlertsView : AbstractStatusSubscriber, INotifyPropertyChanged
    {
        //An AbstractStatusSubscriber, extended to derive alerts from the latency and status readings
        //And display this in the FailOverControl

        //This collection is the item source for the alerts grid in the FailOverControl.
        public ObservableCollection<Alert> alerts { get; set; } = new ObservableCollection<Alert>();

        
        private Severity _overallSeverity;
        public Severity overallSeverity { get { return _overallSeverity; } set { _overallSeverity = value; overAllSeverityImage = SeverityImageConverter.convert(overallSeverity);  } }

        //This is fired when any property changes to maintain the UI
        public event PropertyChangedEventHandler PropertyChanged;

        public AlertsView(FailOverGroup fg)
        {
            isLatencySubscriber = true;
            failOverGroup = fg;
            latencies = new ObservableCollection<LatencyReading>();
        }

        public override void Update()
        {
            base.Update();
            dispatcher.BeginInvoke((Action)delegate
            {
                alerts?.Clear();
                getAlerts().ForEach(a => alerts.Add(a));
                if (alerts.Any())
                {
                    overallSeverity = alerts.OrderBy(a => a.severity).Last().severity;
                }
                else { overallSeverity = Severity.OK; }
            });         
        }

        private List<Alert> getAlerts()
        {
            IEnumerable<Alert> statusAlerts = RuleSet<DatabaseReplicationStatus>.
                checkRules(statuses.ToList()).Where(a => a != null);
            IEnumerable<Alert> latencyAlert = RuleSet<LatencyReading>.
                checkRules(latencies.ToList()).Where(a => a != null);
            return statusAlerts.Union(latencyAlert).ToList();
        }

        //This image summarises the status in the Alerts tab
        private BitmapImage _overallSeverityImage;
        public BitmapImage overAllSeverityImage
        {
            get { return _overallSeverityImage; }
            set
            {
                _overallSeverityImage = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(overAllSeverityImage)));
            }
        }
    }
}
