using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace YazGelLab
{
    public partial class Form1 : Form
    {
        GraphManager graphManager;
        Random rnd = new Random();

        UserNode seciliDugum = null;
        UserNode hedefDugum = null;

        public Form1()
        {
            InitializeComponent();
            graphManager = new GraphManager();
        }

        private void pnlGraph_MouseClick(object sender, MouseEventArgs e)
        {
            Point tiklananYer = e.Location;

            foreach (var node in graphManager.Nodes)
            {
                double mesafe = Math.Sqrt(
                    Math.Pow(node.Konum.X - tiklananYer.X, 2) +
                    Math.Pow(node.Konum.Y - tiklananYer.Y, 2));

                if (mesafe < 20)
                {
                    if (e.Button == MouseButtons.Left)
                        seciliDugum = node;
                    else if (e.Button == MouseButtons.Right)
                        hedefDugum = node;

                    pnlGraph.Invalidate();
                    return;
                }
            }

            seciliDugum = null;
            hedefDugum = null;
            pnlGraph.Invalidate();
        }
    }
}