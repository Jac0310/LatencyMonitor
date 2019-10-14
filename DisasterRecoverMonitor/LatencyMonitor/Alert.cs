using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace LatencyMonitor
{
    public class Alert : INotifyPropertyChanged
    {
        //An alert has a message, and a severity (ERROR/INFO/WARNING)

        public Alert(String message, Severity severity)
        {
            this.message = message;
            this.severity = severity;
        }

        //This event is raised when any property changes,
        //This allows the UI to be updated
        public event PropertyChangedEventHandler PropertyChanged;

        private string _message;
        public string message
        {
            get { return _message; }
            set { _message = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(message))); }
        }

        private Severity _severity;
        public Severity severity
        {
            get { return _severity; }
            set
            {
                _severity = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(severity)));
                severityImage = SeverityImageConverter.convert(severity);
            }
        }

        private BitmapImage _severityImage;
        public BitmapImage severityImage
        {
            get { return _severityImage; }
            set { _severityImage = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(severityImage))); }
        }
    }
}
