using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;

namespace LatencyMonitor
{
    public class FailOverMap : AbstractStatusSubscriber, INotifyPropertyChanged
    {
        //A statusSubscriber that builds and renders an image from a collection
        //of DatabaseReplicationStatus. To show the current configuration of the failOverGroup
        //in the UI
        //Makes of Msagl library
        private String fileName;
        private List<String> files;
        private String filePath;
        private String _image;
        public String image
        {
            get { return _image; }
            set { _image = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(image))); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public FailOverMap(FailOverGroup fg)
        {
            //files = new Dictionary<DateTime, String>();
            files = new List<String>();
            failOverGroup = fg;
        }

        public override void Update()
        {
            base.Update();
            image = draw();
        }

        private String draw()
        {
            Graph g = GetGraph(statuses.ToList(), new Graph("Fail Over Map"));
            clearFile();
            GraphRenderer renderer = new GraphRenderer(g);
            renderer.CalculateLayout();
            int width = 700;
            Bitmap bitmap = new Bitmap(width, (int)(g.Height * (width / g.Width)));
            renderer.Render(bitmap);
            fileName = Guid.NewGuid().ToString() + ".png";
            filePath = AppDomain.CurrentDomain.BaseDirectory + fileName;
            bitmap.Save(fileName);
            files.Add(filePath);
            return filePath;
        }

        private void clearFile()
        {
            ////perhaps a seperate filemanager class?
            //if (files.Count() > 10)
            //{
            //    foreach (string path in files)
            //    {
            //        try
            //        {
            //            FileAttributes fa = File.GetAttributes(path);
            //            DateTime t = File.GetCreationTime(path);
            //            if (t < DateTime.Now.AddMinutes(-0.5))
            //            {
            //                //File.Delete(path);
            //               // files.Remove(path);
            //            }
            //        }
            //        catch (Exception e)
            //        {
            //            MessageBox.Show(e.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            //        }
            //    }
            //}
        }
        private Mutex mutex = new Mutex();
        private Graph GetGraph(List<DatabaseReplicationStatus> statuses, Graph g)
        {
            mutex.WaitOne();
            if (statuses.All(s => s.server != null))
            {
                //add a node to the graph for each status, create list of fgnodes
                statuses.ForEach(s => g.AddNode(new FgNode(s.server, s.type, s.partnerServer)));
                List<FgNode> nodes = g.Nodes.Select(n => (n as FgNode)).ToList();
                
                //identify primary node
                FgNode primaryNode = nodes.Where(n => n.type.Equals("PRIMARY")).FirstOrDefault();

                //for every node draw a line representing replication to its partner, in (primary -> secondary direction)
                foreach (FgNode fgn in nodes)
                {
                    //if node is primary then don't draw edge as data does not flow that way
                    FgNode partnerNode = nodes.Where(n => n.server.Equals(fgn.partner)).FirstOrDefault();
                    if (partnerNode != null && fgn != primaryNode)
                    {
                        g.AddEdge(partnerNode.server, " Geo-Replication", fgn.server);
                    }
                }
            }
            mutex.ReleaseMutex();
            return g;
        }

        //MSAGL node extension to include specific info of geo replication
        public class FgNode : Node
        {
            public string type;
            public string partner;
            public string server;
            public FgNode(string id, string role, string partner) : base(id)
            {
                this.type = role;
                this.LabelText += " (" + role + ")";
                this.partner = partner;
                this.server = id;
            }
        }
    }
}
