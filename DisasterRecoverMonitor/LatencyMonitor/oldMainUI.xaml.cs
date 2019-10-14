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
    /// Interaction logic for oldMainUI.xaml
    /// </summary>
    public partial class oldMainUI : UserControl
    {
        private Monitor latencyMonitor;
        private PrimaryUpdater primaryUpdater;
        public oldMainUI()
        {
            InitializeComponent();
            latencyMonitor = new Monitor();
            DataContext = latencyMonitor;
            primaryUpdater = new PrimaryUpdater();
        }

        public void onStart(object sender, RoutedEventArgs e)
        {
            latencyMonitor.startPoll();
            primaryUpdater.startContinuousUpdate();
        }

        public void onStop(object sender, RoutedEventArgs e)
        {
            latencyMonitor.stopPoll();
            primaryUpdater.stopContinuousUpdate();
            MessageBox.Show(String.Format("Average entry difference: {0} \n Max unsynchronised window: {1} (HH:MM:SS)", Convert.ToDouble(latencyMonitor.latencyHistory.GapHistory.Sum()) / Convert.ToDouble(latencyMonitor.latencyHistory.GapHistory.Count), latencyMonitor.latencyHistory.WindowHistory.Max()));

        }

    }
}
