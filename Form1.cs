using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace YazGelLab
{
    public partial class Form1 : Form
    {
        GraphManager graphManager;
        Random rnd = new Random();

        // Seçim Değişkenleri
        UserNode seciliDugum = null; // Sol Tık
        UserNode hedefDugum = null;  // Sağ Tık

        public Form1()
        {
            InitializeComponent();
            graphManager = new GraphManager();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            if (gridSonuclar != null)
            {
                gridSonuclar.Columns.Clear();
                gridSonuclar.Columns.Add("Sira", "Sıra");
                gridSonuclar.Columns.Add("Bilgi", "Sonuç");
                gridSonuclar.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }
        private void pnlGraph_MouseClick(object sender, MouseEventArgs e)
        {
            Point tiklananYer = e.Location;
            bool birineTiklandi = false;

            foreach (var node in graphManager.Nodes)
            {
                double mesafe = Math.Sqrt(
                    Math.Pow(node.Konum.X - tiklananYer.X, 2) +
                    Math.Pow(node.Konum.Y - tiklananYer.Y, 2)
                );

                if (mesafe < 20)
                {
                    if (e.Button == MouseButtons.Left)
                        seciliDugum = node;
                    else if (e.Button == MouseButtons.Right)
                        hedefDugum = node;

                    birineTiklandi = true;
                    pnlGraph.Invalidate();
                    break;
                }
            }

            if (!birineTiklandi)
            {
                seciliDugum = null;
                hedefDugum = null;
                pnlGraph.Invalidate();
            }
        }
    }
}
    