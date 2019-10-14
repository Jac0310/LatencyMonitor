using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace LatencyMonitor
{
    public class SeverityImageConverter
    {
        //This class converts an alerts severity into an image, to be displayed in the UI

        private static BitmapImage error = new BitmapImage(new Uri("images/error.PNG", UriKind.Relative));
        private static BitmapImage warning = new BitmapImage(new Uri("images/warning.PNG", UriKind.Relative));
        private static BitmapImage info = new BitmapImage(new Uri("images/info.PNG", UriKind.Relative));
        private static BitmapImage ok = new BitmapImage(new Uri("images/ok.PNG", UriKind.Relative));

        private static Dictionary<Severity, BitmapImage> imageMap = new Dictionary<Severity, BitmapImage>()
        {
            { Severity.WARNING, warning },
            { Severity.ERROR, error },
            { Severity.INFO, info },
            { Severity.OK, ok }
        };

        public static BitmapImage convert(Severity severity)
        {
            return imageMap[severity];
        }
    }
}
