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

        private void btnAddNodeManuel_Click(object sender, EventArgs e)
        {
            UserNode n = new UserNode(
                txtAd.Text,
                double.Parse(txtAktiflik.Text),
                double.Parse(txtEtkilesim.Text),
                1);

            n.Konum = new Point(
                rnd.Next(50, pnlGraph.Width - 50),
                rnd.Next(50, pnlGraph.Height - 50));

            graphManager.AddNode(n);
            pnlGraph.Invalidate();
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

        private void pnlGraph_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            foreach (var edge in graphManager.Edges)
                g.DrawLine(Pens.Gray, edge.BaslangicDugumu.Konum, edge.BitisDugumu.Konum);

            foreach (var node in graphManager.Nodes)
            {
                Brush b = Brushes.LightBlue;
                if (node == seciliDugum) b = Brushes.Lime;
                if (node == hedefDugum) b = Brushes.Red;

                Rectangle r = new Rectangle(node.Konum.X - 20, node.Konum.Y - 20, 40, 40);
                g.FillEllipse(b, r);
                g.DrawEllipse(Pens.Black, r);
                g.DrawString(node.Ad, Font, Brushes.Black, node.Konum.X - 10, node.Konum.Y - 30);
            }
        }
    }
}