using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace YazGelLab
{
    public partial class Form1 : Form
    {
        GraphManager graphManager;
        Random rnd = new Random();

        UserNode seciliDugum = null;
        UserNode hedefDugum = null;

        // 🔴 YENİ: Aktif yol (Dijkstra / A*)
        List<UserNode> aktifYol = null;

        public Form1()
        {
            InitializeComponent();
            graphManager = new GraphManager();
        }

        // -------------------------------
        // FORM LOAD
        // -------------------------------
        private void Form1_Load(object sender, EventArgs e)
        {
            gridSonuclar.Columns.Clear();
            gridSonuclar.Columns.Add("Sira", "Sıra");
            gridSonuclar.Columns.Add("Bilgi", "Sonuç");
            gridSonuclar.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // -------------------------------
        // NODE EKLE
        // -------------------------------
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

        // -------------------------------
        // NODE SİL
        // -------------------------------
        private void btnDeleteNode_Click(object sender, EventArgs e)
        {
            if (seciliDugum == null) return;

            for (int i = graphManager.Edges.Count - 1; i >= 0; i--)
            {
                if (graphManager.Edges[i].BaslangicDugumu == seciliDugum ||
                    graphManager.Edges[i].BitisDugumu == seciliDugum)
                {
                    graphManager.Edges.RemoveAt(i);
                }
            }

            graphManager.Nodes.Remove(seciliDugum);
            seciliDugum = null;
            hedefDugum = null;
            aktifYol = null;

            pnlGraph.Invalidate();
        }

        // -------------------------------
        // RANDOM GRAPH
        // -------------------------------
        private void btnRandomGraph_Click(object sender, EventArgs e)
        {
            graphManager = new GraphManager();
            seciliDugum = null;
            hedefDugum = null;
            aktifYol = null;

            int nodeSayisi = rnd.Next(5, 10);

            for (int i = 0; i < nodeSayisi; i++)
            {
                UserNode n = new UserNode(
                    "U" + (i + 1),
                    rnd.Next(1, 10),
                    rnd.Next(10, 100),
                    1);

                n.Konum = new Point(
                    rnd.Next(50, pnlGraph.Width - 50),
                    rnd.Next(50, pnlGraph.Height - 50));

                graphManager.AddNode(n);
            }

            foreach (var node in graphManager.Nodes)
            {
                int baglanti = rnd.Next(1, 3);
                for (int i = 0; i < baglanti; i++)
                {
                    var hedef = graphManager.Nodes[rnd.Next(graphManager.Nodes.Count)];
                    if (hedef != node)
                        graphManager.AddEdge(node.Ad, hedef.Ad);
                }
            }

            pnlGraph.Invalidate();
        }

        // -------------------------------
        // BFS
        // -------------------------------
        private void btnBFS_Click(object sender, EventArgs e)
        {
            if (seciliDugum == null) return;

            aktifYol = null;

            var sonuc = Algorithms.BFS(graphManager, seciliDugum);

            gridSonuclar.Rows.Clear();
            gridSonuclar.Rows.Add("0", seciliDugum.Ad);

            for (int i = 0; i < sonuc.Count; i++)
                gridSonuclar.Rows.Add(i + 1, sonuc[i]);

            pnlGraph.Invalidate();
        }

        // -------------------------------
        // DFS
        // -------------------------------
        private void btnDFS_Click(object sender, EventArgs e)
        {
            if (seciliDugum == null) return;

            aktifYol = null;

            var sonuc = Algorithms.DFS(graphManager, seciliDugum);

            gridSonuclar.Rows.Clear();
            gridSonuclar.Rows.Add("0", seciliDugum.Ad);

            for (int i = 0; i < sonuc.Count; i++)
                gridSonuclar.Rows.Add(i + 1, sonuc[i]);

            pnlGraph.Invalidate();
        }

        // -------------------------------
        // DİJKSTRA
        // -------------------------------
        private void btnShortestPath_Click(object sender, EventArgs e)
        {
            if (seciliDugum == null || hedefDugum == null) return;

            aktifYol = Algorithms.Dijkstra(graphManager, seciliDugum, hedefDugum);

            gridSonuclar.Rows.Clear();
            if (aktifYol == null) return;

            for (int i = 0; i < aktifYol.Count; i++)
                gridSonuclar.Rows.Add(i + 1, aktifYol[i].Ad);

            pnlGraph.Invalidate();
        }

        // -------------------------------
        // 🔴 A* (YENİ)
        // -------------------------------
        private void btnAStar_Click(object sender, EventArgs e)
        {
            if (seciliDugum == null || hedefDugum == null) return;

            aktifYol = Algorithms.AStar(graphManager, seciliDugum, hedefDugum);

            gridSonuclar.Rows.Clear();
            if (aktifYol == null) return;

            for (int i = 0; i < aktifYol.Count; i++)
                gridSonuclar.Rows.Add(i + 1, aktifYol[i].Ad);

            pnlGraph.Invalidate();
        }

        // -------------------------------
        // EDGE EKLE
        // -------------------------------
        private void btnAddEdge_Click(object sender, EventArgs e)
        {
            if (seciliDugum == null || hedefDugum == null) return;

            graphManager.AddEdge(seciliDugum.Ad, hedefDugum.Ad);
            pnlGraph.Invalidate();
        }

        // -------------------------------
        // MOUSE
        // -------------------------------
        private void pnlGraph_MouseClick(object sender, MouseEventArgs e)
        {
            Point p = e.Location;

            foreach (var node in graphManager.Nodes)
            {
                double mesafe = Math.Sqrt(
                    Math.Pow(node.Konum.X - p.X, 2) +
                    Math.Pow(node.Konum.Y - p.Y, 2));

                if (mesafe < 20)
                {
                    if (e.Button == MouseButtons.Left) seciliDugum = node;
                    if (e.Button == MouseButtons.Right) hedefDugum = node;

                    pnlGraph.Invalidate();
                    return;
                }
            }

            seciliDugum = null;
            hedefDugum = null;
            pnlGraph.Invalidate();
        }

        // -------------------------------
        // ÇİZİM
        // -------------------------------
        private void pnlGraph_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Kenarlar
            foreach (var edge in graphManager.Edges)
                g.DrawLine(Pens.Gray,
                    edge.BaslangicDugumu.Konum,
                    edge.BitisDugumu.Konum);

            // 🔴 Aktif yol
            if (aktifYol != null && aktifYol.Count > 1)
            {
                Pen yolKalemi = new Pen(Color.Red, 3);
                for (int i = 0; i < aktifYol.Count - 1; i++)
                    g.DrawLine(yolKalemi,
                        aktifYol[i].Konum,
                        aktifYol[i + 1].Konum);
            }

            // Node’lar
            foreach (var node in graphManager.Nodes)
            {
                Brush b = Brushes.LightBlue;
                if (node == seciliDugum) b = Brushes.Lime;
                if (node == hedefDugum) b = Brushes.Red;

                Rectangle r = new Rectangle(
                    node.Konum.X - 20,
                    node.Konum.Y - 20, 40, 40);

                g.FillEllipse(b, r);
                g.DrawEllipse(Pens.Black, r);
                g.DrawString(node.Ad, Font, Brushes.Black,
                    node.Konum.X - 10, node.Konum.Y - 30);
            }
        }
    }
}