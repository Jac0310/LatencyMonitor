using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LatencyMonitor
{
    /// <summary>
    /// Interaction logic for FailOverControl.xaml
    /// This class is a UserControl which binds each AbstractStatusSubscriber
    /// To a visual element within the view.
    /// It asks subscribers to update when the FailOverGroup status changes
    /// </summary>
    public partial class FailOverControl : UserControl
    {
        private FailOverMap failOverMap { get; set; }
        private FailoverStatus failOverStatus { get; set; }
        private FailOverGroup failoverGroup { get; set; }
        private DBWriter writer;
        private AlertsView alertsView;
        private List<AbstractStatusSubscriber> subscribers = new List<AbstractStatusSubscriber>();

        //this stuff is copied from polling tree view, perhaps have a general button superclass
        private Brush originalButtonBackground;
        private Brush pressedBackground = new SolidColorBrush(Colors.AliceBlue);

        public FailOverControl(FailOverGroup fg)
        {
            InitializeComponent();
            this.failoverGroup = fg;

            originalButtonBackground = Sender.Background;

            //create subscribers
            failOverMap = new FailOverMap(fg);
            failOverStatus = new FailoverStatus(fg);
            alertsView = new AlertsView(fg);
            failOverMap.PropertyChanged += (s, e) => { };

            //set data context for binding
            alertsGrid.DataContext = alertsView;
            laGrid.DataContext = failOverStatus;
            foGrid.DataContext = failOverStatus;
            map.DataContext = failOverMap;
            OverallSeverity.DataContext = alertsView;

            //cache subscribers
            subscribers.Add(failOverMap);
            subscribers.Add(failOverStatus);
            subscribers.Add(alertsView);

            //subscribe subscribers
            failoverGroup.statusChanged += UpdateSubscribers;

            failOverStatus.latencies.CollectionChanged += (s, e) => 
            {
                int pollRate = writer == null ? 0 : writer.pollRate;
                DatabaseDAO.logLatency(failOverStatus.latencies, pollRate, failoverGroup.getPrimary());
            };

        }

        private void UpdateSubscribers()
        {
            subscribers.ForEach(s => s.Update());
        }

        private void startWriting(Object sender, RoutedEventArgs args)
        {
            //failoverGroup.p
            Database primary = failoverGroup.getPrimary();
            if (Int32.TryParse(Rate.Text, out int rate)) 
            {
                writer = new DBWriter(primary, rate);
                writer.start();
            }
            Sender.Background = pressedBackground;
        }

        private void stopWriting(Object sender, RoutedEventArgs args)
        {
            writer?.stop();
            Sender.Background = originalButtonBackground;
        }

        private void onClose(Object sender, RoutedEventArgs args)
        {
            DockPanel d = (DockPanel)this.Parent;
            writer?.stop();
            d.Children.Remove(this);
        }
    }
}
