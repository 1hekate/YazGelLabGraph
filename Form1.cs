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

        // Se�im De�i�kenleri
        UserNode seciliDugum = null; // Sol T�k (Ye�il)
        UserNode hedefDugum = null;  // Sa� T�k (K�rm�z�)

        public Form1()
        {
            InitializeComponent();
            graphManager = new GraphManager();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Tabloyu Haz�rla
            if (gridSonuclar != null)
            {
                gridSonuclar.Columns.Clear();
                gridSonuclar.Columns.Add("Sira", "S�ra");
                gridSonuclar.Columns.Add("Bilgi", "Sonu�");
                gridSonuclar.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        // =============================================================
        // 1. MOUSE TIKLAMA VE SE��M (SOL/SA� TIK)
        // =============================================================
        private void pnlGraph_MouseClick(object sender, MouseEventArgs e)
        {
            Point tiklananYer = e.Location;
            bool birineTiklandi = false;

            foreach (var node in graphManager.Nodes)
            {
                // T�klanan koordinat bir d���m�n i�inde mi?
                double mesafe = Math.Sqrt(Math.Pow(node.Konum.X - tiklananYer.X, 2) + Math.Pow(node.Konum.Y - tiklananYer.Y, 2));

                if (mesafe < 20)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        seciliDugum = node; // Sol t�k: Ba�lang��

                        // Bilgileri TextBox'lara aktar (Varsa)
                        if (txtAd != null) txtAd.Text = node.Ad;
                        if (txtAktiflik != null) txtAktiflik.Text = node.Aktiflik.ToString();
                        if (txtEtkilesim != null) txtEtkilesim.Text = node.Etkilesim.ToString();
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        hedefDugum = node; // Sa� t�k: Hedef
                    }

                    birineTiklandi = true;
                    pnlGraph.Invalidate();
                    break;
                }
            }

            // Bo�lu�a t�kland�ysa se�imleri kald�r
            if (!birineTiklandi)
            {
                seciliDugum = null;
                hedefDugum = null;
                pnlGraph.Invalidate();
            }
        }

        // =============================================================
        // 2. ��Z�M ��LEM� (PAINT)
        // =============================================================
        private void pnlGraph_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Kenarlar� �iz
            Pen edgePen = new Pen(Color.Gray, 2);
            foreach (var edge in graphManager.Edges)
            {
                g.DrawLine(edgePen, edge.BaslangicDugumu.Konum, edge.BitisDugumu.Konum);

                // A��rl��� Yaz
                float midX = (edge.BaslangicDugumu.Konum.X + edge.BitisDugumu.Konum.X) / 2;
                float midY = (edge.BaslangicDugumu.Konum.Y + edge.BitisDugumu.Konum.Y) / 2;
                g.DrawString(edge.Agirlik.ToString("F1"), this.Font, Brushes.Red, midX, midY);
            }

            // D���mleri �iz
            int nodeSize = 40;
            foreach (var node in graphManager.Nodes)
            {
                Rectangle rect = new Rectangle(node.Konum.X - nodeSize / 2, node.Konum.Y - nodeSize / 2, nodeSize, nodeSize);

                // Renk Belirleme
                Brush firca = new SolidBrush(node.Renk);
                if (node == seciliDugum) firca = Brushes.LimeGreen;
                else if (node == hedefDugum) firca = Brushes.Red;

                g.FillEllipse(firca, rect);
                g.DrawEllipse(Pens.Black, rect);
                g.DrawString(node.Ad, this.Font, Brushes.Black, node.Konum.X - 15, node.Konum.Y - 30);
            }
        }

        // =============================================================
        // 3. YEN� BUTONLAR (EKLE / S�L / G�NCELLE)
        // =============================================================

        // Bilgileri G�ncelle
        private void btnUpdateNode_Click(object sender, EventArgs e)
        {
            if (seciliDugum == null) { MessageBox.Show("L�tfen sol t�kla bir d���m se�in."); return; }
            try
            {
                seciliDugum.Ad = txtAd.Text;
                seciliDugum.Aktiflik = double.Parse(txtAktiflik.Text);
                seciliDugum.Etkilesim = double.Parse(txtEtkilesim.Text);
                pnlGraph.Invalidate();
                MessageBox.Show("Bilgiler g�ncellendi.");
            }
            catch { MessageBox.Show("Say�sal de�er hatas�."); }
        }

        // D���m� Sil
        private void btnDeleteNode_Click(object sender, EventArgs e)
        {
            if (seciliDugum == null) { MessageBox.Show("Silinecek d���m� se�in."); return; }

            // Ba�lant�lar� temizle
            for (int i = graphManager.Edges.Count - 1; i >= 0; i--)
            {
                if (graphManager.Edges[i].BaslangicDugumu == seciliDugum || graphManager.Edges[i].BitisDugumu == seciliDugum)
                    graphManager.Edges.RemoveAt(i);
            }
            graphManager.Nodes.Remove(seciliDugum);
            seciliDugum = null;
            pnlGraph.Invalidate();
        }

        // Manuel D���m Ekle
        private void btnAddNodeManuel_Click(object sender, EventArgs e)
        {
            // 1. Kutucuklar bo� mu kontrol et
            if (string.IsNullOrWhiteSpace(txtAd.Text) ||
                string.IsNullOrWhiteSpace(txtAktiflik.Text) ||
                string.IsNullOrWhiteSpace(txtEtkilesim.Text))
            {
                MessageBox.Show("L�tfen Ad, Aktiflik ve Etkile�im kutular�n� doldurunuz!", "Eksik Bilgi");
                return;
            }

            try
            {
                // 2. Textbox'lardaki verileri al
                string ad = txtAd.Text;
                double aktiflik = double.Parse(txtAktiflik.Text);   // Say�ya �evir
                double etkilesim = double.Parse(txtEtkilesim.Text); // Say�ya �evir
                double baglantiSayisi = 1; // Yeni eklenen ki�inin ba�ta 1 varsayal�m (veya 0)

                // 3. Yeni d���m� olu�tur
                UserNode newNode = new UserNode(ad, aktiflik, etkilesim, baglantiSayisi);

                // 4. Ekranda rastgele bo� bir yere koy
                newNode.Konum = new Point(rnd.Next(50, pnlGraph.Width - 50), rnd.Next(50, pnlGraph.Height - 50));

                // 5. Y�neticiye ekle ve �iz
                graphManager.AddNode(newNode);
                pnlGraph.Invalidate();

                MessageBox.Show($"'{ad}' ba�ar�yla eklendi.");
            }
            catch
            {
                MessageBox.Show("L�tfen Aktiflik ve Etkile�im kutular�na sadece SAYI giriniz (�rn: 12 veya 5.5).");
            }
        }

        // Ba�lant� Ekle (+)
        private void btnAddEdge_Click(object sender, EventArgs e)
        {
            if (seciliDugum == null || hedefDugum == null) { MessageBox.Show("�ki d���m se�melisiniz."); return; }

            bool varMi = graphManager.Edges.Any(ed => (ed.BaslangicDugumu == seciliDugum && ed.BitisDugumu == hedefDugum) || (ed.BaslangicDugumu == hedefDugum && ed.BitisDugumu == seciliDugum));
            if (!varMi)
            {
                graphManager.AddEdge(seciliDugum.Ad, hedefDugum.Ad);
                pnlGraph.Invalidate();
            }
        }

        // Ba�lant� Sil (-)
        private void btnRemoveEdge_Click(object sender, EventArgs e)
        {
            if (seciliDugum == null || hedefDugum == null) return;
            var edge = graphManager.Edges.FirstOrDefault(ed => (ed.BaslangicDugumu == seciliDugum && ed.BitisDugumu == hedefDugum) || (ed.BaslangicDugumu == hedefDugum && ed.BitisDugumu == seciliDugum));
            if (edge != null)
            {
                graphManager.Edges.Remove(edge);
                pnlGraph.Invalidate();
            }
        }

        // =============================================================
        // 4. ESK� �ZELL�KLER VE ALGOR�TMALAR
        // =============================================================

        // Rastgele Graf
        private void btnRandomGraph_Click(object sender, EventArgs e)
        {
            graphManager = new GraphManager();
            int nodeCount = rnd.Next(5, 10);
            for (int i = 0; i < nodeCount; i++)
            {
                UserNode n = new UserNode("U" + (i + 1), rnd.Next(1, 10), rnd.Next(10, 100), rnd.Next(1, 20));
                n.Konum = new Point(rnd.Next(50, pnlGraph.Width - 50), rnd.Next(50, pnlGraph.Height - 50));
                graphManager.AddNode(n);
            }
            foreach (var n in graphManager.Nodes)
            {
                if (rnd.Next(2) == 0) continue;
                var friend = graphManager.Nodes[rnd.Next(graphManager.Nodes.Count)];
                if (n != friend) graphManager.AddEdge(n.Ad, friend.Ad);
            }
            pnlGraph.Invalidate();
        }

        // CSV Y�kle
        private void btnLoadCSV_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "CSV|*.csv";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                graphManager = new GraphManager();
                string[] lines = File.ReadAllLines(openFileDialog1.FileName);
                if (lines.Length <= 1) return;

                // 1. D���mleri Oku
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] parts = lines[i].Split(',');
                    if (parts.Length >= 4)
                    {
                        UserNode n = new UserNode(parts[0], double.Parse(parts[1]), double.Parse(parts[2]), double.Parse(parts[3]));
                        n.Konum = new Point(rnd.Next(50, pnlGraph.Width - 50), rnd.Next(50, pnlGraph.Height - 50));
                        graphManager.AddNode(n);
                    }
                }
                // 2. Ba�lant�lar� Oku
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] parts = lines[i].Split(',');
                    if (parts.Length >= 5 && !string.IsNullOrEmpty(parts[4]))
                    {
                        foreach (var k in parts[4].Split('-')) graphManager.AddEdge(parts[0], k.Trim());
                    }
                }
                pnlGraph.Invalidate();
                MessageBox.Show("CSV Y�klendi.");
            }
        }

        // Temizle
        private void btnClear_Click(object sender, EventArgs e)
        {
            graphManager = new GraphManager();
            if (gridSonuclar != null) gridSonuclar.Rows.Clear();
            pnlGraph.Invalidate();
        }

        // BFS Algoritmas�
        private void btnBFS_Click(object sender, EventArgs e)
        {
            if (seciliDugum == null) { MessageBox.Show("Bir d���m se�in."); return; }
            var result = Algorithms.BFS(graphManager, seciliDugum);

            // Tabloya Yaz
            gridSonuclar.Rows.Clear();
            gridSonuclar.Rows.Add("BA�LANGI�", seciliDugum.Ad);
            for (int i = 0; i < result.Count; i++) gridSonuclar.Rows.Add(i + 1, result[i]);
            MessageBox.Show("BFS Sonucu tabloya yaz�ld�.");
        }

        // En K�sa Yol (Dijkstra)
        private void btnShortestPath_Click(object sender, EventArgs e)
        {
            if (seciliDugum == null || hedefDugum == null) { MessageBox.Show("�ki d���m se�in."); return; }
            var path = Algorithms.Dijkstra(graphManager, seciliDugum, hedefDugum);

            gridSonuclar.Rows.Clear();
            if (path != null)
            {
                string yolStr = "";
                foreach (var p in path)
                {
                    gridSonuclar.Rows.Add("ADIM", p.Ad);
                    yolStr += p.Ad + " -> ";
                }
                MessageBox.Show("Yol: " + yolStr);
            }
            else
            {
                MessageBox.Show("Yol bulunamad�.");
            }
        }

        // Merkezilik (Centrality)
        private void btnCentrality_Click(object sender, EventArgs e)
        {
            var result = Algorithms.EnEtkiliKullanicilar(graphManager);
            gridSonuclar.Rows.Clear();
            foreach (var r in result) gridSonuclar.Rows.Add("Pop�ler", r);
        }

        // Renklendirme (Welsh-Powell)
        private void btnColoring_Click(object sender, EventArgs e)
        {
            int renkSayisi = Algorithms.WelshPowellRenklendirme(graphManager);
            pnlGraph.Invalidate();
            gridSonuclar.Rows.Clear();
            gridSonuclar.Rows.Add("SONU�", $"Toplam {renkSayisi} renk kullan�ld�.");
            MessageBox.Show($"Boyama tamamland�. {renkSayisi} renk kullan�ld�.");
        }

        private void btnDFS_Click(object sender, EventArgs e)
        {
            if (seciliDugum == null)
            {
                MessageBox.Show("L�tfen haritadan ba�lang�� d���m�n� se�in (Sol T�k).");
                return;
            }

            // Algoritmay� �a��r
            var result = Algorithms.DFS(graphManager, seciliDugum);

            // Tabloya Yaz
            gridSonuclar.Rows.Clear();
            gridSonuclar.Rows.Add("BA�LANGI�", seciliDugum.Ad);

            for (int i = 0; i < result.Count; i++)
            {
                gridSonuclar.Rows.Add(i + 1, result[i]);
            }

            MessageBox.Show("DFS (Derinlik �ncelikli) sonucu tabloya yaz�ld�.");
        }

        private void btnSaveCSV_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "CSV Dosyas�|*.csv";
            saveFileDialog1.Title = "Graf� Kaydet";
            saveFileDialog1.FileName = "ProjeVerisi.csv"; // Varsay�lan isim

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // CSV sat�rlar�n� olu�turaca��m�z liste
                    var satirlar = new List<string>();

                    // 1. Ba�l�k Sat�r�
                    satirlar.Add("Ad,Aktiflik,Etkilesim,BaglantiSayisi,Komsular");

                    // 2. Her d���m i�in sat�r olu�tur
                    foreach (var node in graphManager.Nodes)
                    {
                        // Bu d���m�n kom�ular�n� bul
                        var komsular = new List<string>();

                        foreach (var edge in graphManager.Edges)
                        {
                            // E�er bu kenar�n bir ucu bizim d���mse, di�er ucu kom�udur
                            if (edge.BaslangicDugumu == node) komsular.Add(edge.BitisDugumu.Ad);
                            else if (edge.BitisDugumu == node) komsular.Add(edge.BaslangicDugumu.Ad);
                        }

                        // Kom�ular� "User2-User3" �eklinde tire ile birle�tir
                        string komsularString = string.Join("-", komsular);

                        // CSV format�nda sat�r� haz�rla: Ad,Aktiflik,Etkile�im,Ba�lant�Say�s�,Kom�ular
                        string satir = $"{node.Ad},{node.Aktiflik},{node.Etkilesim},{node.BaglantiSayisi},{komsularString}";
                        satirlar.Add(satir);
                    }

                    // 3. Dosyay� diske yaz
                    File.WriteAllLines(saveFileDialog1.FileName, satirlar);
                    MessageBox.Show("Dosya ba�ar�yla kaydedildi!", "Kay�t Ba�ar�l�");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kaydederken hata olu�tu: " + ex.Message);
                }
            }
        }

        private void btnAStar_Click(object sender, EventArgs e)
        {
            if (seciliDugum == null || hedefDugum == null) { MessageBox.Show("Ba�lang�� (Sol T�k) ve Hedef (Sa� T�k) se�melisiniz."); return; }

            var path = Algorithms.AStar(graphManager, seciliDugum, hedefDugum);

            gridSonuclar.Rows.Clear();
            if (path != null)
            {
                string yolStr = "";
                foreach (var p in path)
                {
                    gridSonuclar.Rows.Add("A* ADIM", p.Ad);
                    yolStr += p.Ad + " -> ";
                }
                MessageBox.Show("A* ile bulunan yol:\n" + yolStr);
            }
            else
            {
                MessageBox.Show("Yol bulunamad�.");
            }
        }

        private void btnComponents_Click(object sender, EventArgs e)
        {
            // Algoritmay� �al��t�r
            var bilesenler = Algorithms.BagliBilesenleriBul(graphManager);

            // Tabloyu Temizle ve Haz�rla
            gridSonuclar.Rows.Clear();

            // Sonu�lar� Yazd�r
            for (int i = 0; i < bilesenler.Count; i++)
            {
                // O topluluktaki ki�ileri virg�lle birle�tir (�rn: Ali, Veli, Ay�e)
                string uyeler = string.Join(", ", bilesenler[i]);

                gridSonuclar.Rows.Add($"TOPLULUK {i + 1}", uyeler);
            }

            MessageBox.Show($"Analiz Tamamland�.\nToplam {bilesenler.Count} adet ayr�k topluluk (ada) bulundu.", "Ba�l� Bile�en Analizi");
        }
    }
}