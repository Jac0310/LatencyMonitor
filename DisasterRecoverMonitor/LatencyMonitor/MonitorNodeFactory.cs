using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;

namespace LatencyMonitor
{
    public class MonitorNodeFactory
    {
        //This factory builds TreeviewItems
        //Containing Application specific objects 
        //From XNode
        public static TreeViewItem getMonitorElemet(XNode xn)
        {
            //if xn is failovergroup, create failover group and construct xelement with it.
            TreeViewItem t = new TreeViewItem();
            t.IsExpanded = true;
            XElement xe = xn as XElement;           
            if (xe.Name.LocalName.Equals("FailOverGroup")) 
            {
                
                FailOverGroup fg = parseFailOver(xe);
                t = new PollingTreeviewItem(fg);
                return t;
            }
            Database d = parseDatabase(xe);
            t.Header = d.name + " (" + xe.Name.LocalName + "): " + d.hostName;
            t.Tag = d;
            return t;
        }

        private static FailOverGroup parseFailOver(XElement xe)
        {
            FailOverGroup fg = null; 
       
            foreach (XNode x in xe.Nodes())
            {
                XElement x1 = x as XElement;
                Database d = parseDatabase(x1);
                if (x1.Name.LocalName.Equals("Primary"))
                {
                    fg = new FailOverGroup(d);                
                }
                else if (x1.Name.LocalName.Equals("Secondary"))
                {
                    fg.AddSecondary(d);
                }
                Database.all.InstertAndWatch(d);
            }
            var dbs = Database.all;
            return fg;
        }

        private static Database parseDatabase(XElement xe)
        {
            string hostName = null;
            string userName = null;
            string passWord = null;
            string deploymentCode = null;
            foreach (XNode x in xe.Nodes())
            {
                XElement x1 = x as XElement;
                switch (x1.Name.LocalName)
                {
                    case "HostName":
                        hostName = x1.Value;
                        break;
                    case "UserName":
                        userName = x1.Value;
                        break;
                    case "Phrase":
                        passWord = x1.Value;
                        break;
                    case "DeploymentCode":
                        deploymentCode = x1.Value;
                        break;
                }
            }
            return new Database(hostName, userName, passWord, deploymentCode);
        }

        class PollingTreeviewItem : TreeViewItem
        {
            private Button poll;
            private Button stop;
            private FailOverGroupPoller poller;
            private FailOverGroup failOverGroup;
            private TextBox rate;
            private Brush originalButtonBackground;
            private Brush pressedBackground = new SolidColorBrush(Colors.AliceBlue);

            public PollingTreeviewItem(FailOverGroup fg) : base()
            {
                this.Tag = fg;
                poll = new Button();
                stop = new Button();

                this.failOverGroup = fg;

                rate = new TextBox();
                rate.Width = 40;
                rate.Name = "Rate";
                rate.ToolTip = "Poll Interval (Seconds)";

                poll.Content = "Poll";
                stop.Content = "Stop";

                originalButtonBackground = poll.Background;

                poll.Click += StartPoll;
                stop.Click += StopPoll;
                TextBlock l = new TextBlock();
                l.Text = fg.name;
                l.Width = 60;
                this.Header = new WrapPanel { Children = { l, poll, stop, rate } };
            }

            private void StartPoll(object sender, RoutedEventArgs args)
            {
                if (Int32.TryParse(rate.Text, out int r))
                {
                    //double interval = (Double)(1 / r) * msPerSec;
                    poller = new FailOverGroupPoller(failOverGroup, (Double)r);
                }
                else
                {
                    poller = new FailOverGroupPoller(failOverGroup);
                }
                poller.start();
                poll.Background = pressedBackground;             
            }

            private void StopPoll(object sender, RoutedEventArgs args)
            {
                poller.stop();
                poll.Background = originalButtonBackground;
            }
        }
    }
}
