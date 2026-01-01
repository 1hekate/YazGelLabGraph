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

        public Form1()
        {
            InitializeComponent();
            graphManager = new GraphManager();
        }

        // -------------------------------
        // FORM LOAD → TABLO HAZIRLA
        // -------------------------------
        private void Form1_Load(object sender, EventArgs e)
        {
            gridSonuclar.Columns.Clear();
            gridSonuclar.Columns.Add("Sira", "Sıra");
            gridSonuclar.Columns.Add("Bilgi", "Ziyaret Edilen Düğüm");
            gridSonuclar.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // -------------------------------
        // MANUEL DÜĞÜM EKLE
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
        // BFS ÇALIŞTIR
        // -------------------------------
        private void btnBFS_Click(object sender, EventArgs e)
        {
            if (seciliDugum == null)
            {
                MessageBox.Show("Başlangıç düğümünü sol tık ile seçin.");
                return;
            }

            var sonuc = Algorithms.BFS(graphManager, seciliDugum);

            gridSonuclar.Rows.Clear();
            gridSonuclar.Rows.Add("0", seciliDugum.Ad);

            for (int i = 0; i < sonuc.Count; i++)
            {
                gridSonuclar.Rows.Add((i + 1).ToString(), sonuc[i]);
            }

            MessageBox.Show("BFS tamamlandı.");
        }

        // -------------------------------
        // KENAR EKLE
        // -------------------------------
        private void btnAddEdge_Click(object sender, EventArgs e)
        {
            if (seciliDugum == null || hedefDugum == null)
            {
                MessageBox.Show("İki düğüm seçmelisiniz.");
                return;
            }

            bool varMi = graphManager.Edges.Any(ed =>
                (ed.BaslangicDugumu == seciliDugum && ed.BitisDugumu == hedefDugum) ||
                (ed.BaslangicDugumu == hedefDugum && ed.BitisDugumu == seciliDugum));

            if (!varMi)
            {
                graphManager.AddEdge(seciliDugum.Ad, hedefDugum.Ad);
                pnlGraph.Invalidate();
            }
        }

        // -------------------------------
        // KENAR SİL
        // -------------------------------
        private void btnRemoveEdge_Click(object sender, EventArgs e)
        {
            if (seciliDugum == null || hedefDugum == null) return;

            var edge = graphManager.Edges.FirstOrDefault(ed =>
                (ed.BaslangicDugumu == seciliDugum && ed.BitisDugumu == hedefDugum) ||
                (ed.BaslangicDugumu == hedefDugum && ed.BitisDugumu == seciliDugum));

            if (edge != null)
            {
                graphManager.Edges.Remove(edge);
                pnlGraph.Invalidate();
            }
        }

        // -------------------------------
        // MOUSE SEÇİM
        // -------------------------------
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

        // -------------------------------
        // ÇİZİM
        // -------------------------------
        private void pnlGraph_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            foreach (var edge in graphManager.Edges)
                g.DrawLine(Pens.Gray,
                    edge.BaslangicDugumu.Konum,
                    edge.BitisDugumu.Konum);

            foreach (var node in graphManager.Nodes)
            {
                Brush b = Brushes.LightBlue;
                if (node == seciliDugum) b = Brushes.Lime;
                if (node == hedefDugum) b = Brushes.Red;

                Rectangle r = new Rectangle(
                    node.Konum.X - 20,
                    node.Konum.Y - 20,
                    40, 40);

                g.FillEllipse(b, r);
                g.DrawEllipse(Pens.Black, r);
                g.DrawString(node.Ad, Font, Brushes.Black,
                    node.Konum.X - 10,
                    node.Konum.Y - 30);
            }
        }
    }
}