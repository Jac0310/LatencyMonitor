using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using System.Xml;


namespace LatencyMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Builds a failOverGroup tree from config, displays it
    /// and binds necessary action to nodes
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            PopulateTree(tree, XDocument.Load(@"..\..\MonitorConfig.xml"));
            
        }

        private void PopulateTree(TreeView t, XDocument x)
        {
            TreeViewItem root = new TreeViewItem();
            root.Header = "Fail Over Groups:";
            root.IsExpanded = true;
            t.Items.Add(root);
            RecursiveBuild(root, x.Root);
        }

        private void RecursiveBuild(TreeViewItem ti, XElement xe, int level = 3)
        {
            level--;
            foreach (XNode xn in xe.Nodes())
            {
                if (level > 0)
                {
                    if (xn.NodeType.Equals(XmlNodeType.Element))
                    {
                        XElement xei = xn as XElement;
                        TreeViewItem n = MonitorNodeFactory.getMonitorElemet(xn);
                        
                        n.MouseDoubleClick += launchFGGrid;                     
                        ti.Items.Add(n);
                        RecursiveBuild(n, xei, level);
                    }
                }             
            }
        }

        private void launchFGGrid(object sender, RoutedEventArgs e)
        {
            TreeViewItem t = (TreeViewItem)sender;
            if (t.Tag is FailOverGroup fg)
            {
                //FailOverGroup fg = (FailOverGroup)t.Tag;
                FailOverControl foc = new FailOverControl(fg);
                foPanel.Children.Add(foc);
            }
        }
    }
}
