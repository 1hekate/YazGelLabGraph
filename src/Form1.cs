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

        // Seçim Deðiþkenleri
        UserNode seciliDugum = null; // Sol Týk (Yeþil)
        UserNode hedefDugum = null;  // Sað Týk (Kýrmýzý)

        public Form1()
        {
            InitializeComponent();
            graphManager = new GraphManager();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Tabloyu Hazýrla
            if (gridSonuclar != null)
            {
                gridSonuclar.Columns.Clear();
                gridSonuclar.Columns.Add("Sira", "Sýra");
                gridSonuclar.Columns.Add("Bilgi", "Sonuç");
                gridSonuclar.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        // =============================================================
        // 1. MOUSE TIKLAMA VE SEÇÝM (SOL/SAÐ TIK)
        // =============================================================
        private void pnlGraph_MouseClick(object sender, MouseEventArgs e)
        {
            Point tiklananYer = e.Location;
            bool birineTiklandi = false;

            foreach (var node in graphManager.Nodes)
            {
                // Týklanan koordinat bir düðümün içinde mi?
                double mesafe = Math.Sqrt(Math.Pow(node.Konum.X - tiklananYer.X, 2) + Math.Pow(node.Konum.Y - tiklananYer.Y, 2));

                if (mesafe < 20)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        seciliDugum = node; // Sol týk: Baþlangýç

                        // Bilgileri TextBox'lara aktar (Varsa)
                        if (txtAd != null) txtAd.Text = node.Ad;
                        if (txtAktiflik != null) txtAktiflik.Text = node.Aktiflik.ToString();
                        if (txtEtkilesim != null) txtEtkilesim.Text = node.Etkilesim.ToString();
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        hedefDugum = node; // Sað týk: Hedef
                    }

                    birineTiklandi = true;
                    pnlGraph.Invalidate();
                    break;
                }
            }

            // Boþluða týklandýysa seçimleri kaldýr
            if (!birineTiklandi)
            {
                seciliDugum = null;
                hedefDugum = null;
                pnlGraph.Invalidate();
            }
        }

        // =============================================================
        // 2. ÇÝZÝM ÝÞLEMÝ (PAINT)
        // =============================================================
        private void pnlGraph_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Kenarlarý Çiz
            Pen edgePen = new Pen(Color.Gray, 2);
            foreach (var edge in graphManager.Edges)
            {
                g.DrawLine(edgePen, edge.BaslangicDugumu.Konum, edge.BitisDugumu.Konum);

                // Aðýrlýðý Yaz
                float midX = (edge.BaslangicDugumu.Konum.X + edge.BitisDugumu.Konum.X) / 2;
                float midY = (edge.BaslangicDugumu.Konum.Y + edge.BitisDugumu.Konum.Y) / 2;
                g.DrawString(edge.Agirlik.ToString("F1"), this.Font, Brushes.Red, midX, midY);
            }

            // Düðümleri Çiz
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
        // 3. YENÝ BUTONLAR (EKLE / SÝL / GÜNCELLE)
        // =============================================================

        // Bilgileri Güncelle
        private void btnUpdateNode_Click(object sender, EventArgs e)
        {
            if (seciliDugum == null) { MessageBox.Show("Lütfen sol týkla bir düðüm seçin."); return; }
            try
            {
                seciliDugum.Ad = txtAd.Text;
                seciliDugum.Aktiflik = double.Parse(txtAktiflik.Text);
                seciliDugum.Etkilesim = double.Parse(txtEtkilesim.Text);
                pnlGraph.Invalidate();
                MessageBox.Show("Bilgiler güncellendi.");
            }
            catch { MessageBox.Show("Sayýsal deðer hatasý."); }
        }

        // Düðümü Sil
        private void btnDeleteNode_Click(object sender, EventArgs e)
        {
            if (seciliDugum == null) { MessageBox.Show("Silinecek düðümü seçin."); return; }

            // Baðlantýlarý temizle
            for (int i = graphManager.Edges.Count - 1; i >= 0; i--)
            {
                if (graphManager.Edges[i].BaslangicDugumu == seciliDugum || graphManager.Edges[i].BitisDugumu == seciliDugum)
                    graphManager.Edges.RemoveAt(i);
            }
            graphManager.Nodes.Remove(seciliDugum);
            seciliDugum = null;
            pnlGraph.Invalidate();
        }

        // Manuel Düðüm Ekle
        private void btnAddNodeManuel_Click(object sender, EventArgs e)
        {
            // 1. Kutucuklar boþ mu kontrol et
            if (string.IsNullOrWhiteSpace(txtAd.Text) ||
                string.IsNullOrWhiteSpace(txtAktiflik.Text) ||
                string.IsNullOrWhiteSpace(txtEtkilesim.Text))
            {
                MessageBox.Show("Lütfen Ad, Aktiflik ve Etkileþim kutularýný doldurunuz!", "Eksik Bilgi");
                return;
            }

            try
            {
                // 2. Textbox'lardaki verileri al
                string ad = txtAd.Text;
                double aktiflik = double.Parse(txtAktiflik.Text);   // Sayýya çevir
                double etkilesim = double.Parse(txtEtkilesim.Text); // Sayýya çevir
                double baglantiSayisi = 1; // Yeni eklenen kiþinin baþta 1 varsayalým (veya 0)

                // 3. Yeni düðümü oluþtur
                UserNode newNode = new UserNode(ad, aktiflik, etkilesim, baglantiSayisi);

                // 4. Ekranda rastgele boþ bir yere koy
                newNode.Konum = new Point(rnd.Next(50, pnlGraph.Width - 50), rnd.Next(50, pnlGraph.Height - 50));

                // 5. Yöneticiye ekle ve çiz
                graphManager.AddNode(newNode);
                pnlGraph.Invalidate();

                MessageBox.Show($"'{ad}' baþarýyla eklendi.");
            }
            catch
            {
                MessageBox.Show("Lütfen Aktiflik ve Etkileþim kutularýna sadece SAYI giriniz (Örn: 12 veya 5.5).");
            }
        }

        // Baðlantý Ekle (+)
        private void btnAddEdge_Click(object sender, EventArgs e)
        {
            if (seciliDugum == null || hedefDugum == null) { MessageBox.Show("Ýki düðüm seçmelisiniz."); return; }

            bool varMi = graphManager.Edges.Any(ed => (ed.BaslangicDugumu == seciliDugum && ed.BitisDugumu == hedefDugum) || (ed.BaslangicDugumu == hedefDugum && ed.BitisDugumu == seciliDugum));
            if (!varMi)
            {
                graphManager.AddEdge(seciliDugum.Ad, hedefDugum.Ad);
                pnlGraph.Invalidate();
            }
        }

        // Baðlantý Sil (-)
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
        // 4. ESKÝ ÖZELLÝKLER VE ALGORÝTMALAR
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

        // CSV Yükle
        private void btnLoadCSV_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "CSV|*.csv";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                graphManager = new GraphManager();
                string[] lines = File.ReadAllLines(openFileDialog1.FileName);
                if (lines.Length <= 1) return;

                // 1. Düðümleri Oku
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
                // 2. Baðlantýlarý Oku
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] parts = lines[i].Split(',');
                    if (parts.Length >= 5 && !string.IsNullOrEmpty(parts[4]))
                    {
                        foreach (var k in parts[4].Split('-')) graphManager.AddEdge(parts[0], k.Trim());
                    }
                }
                pnlGraph.Invalidate();
                MessageBox.Show("CSV Yüklendi.");
            }
        }

        // Temizle
        private void btnClear_Click(object sender, EventArgs e)
        {
            graphManager = new GraphManager();
            if (gridSonuclar != null) gridSonuclar.Rows.Clear();
            pnlGraph.Invalidate();
        }

        // BFS Algoritmasý
        private void btnBFS_Click(object sender, EventArgs e)
        {
            if (seciliDugum == null) { MessageBox.Show("Bir düðüm seçin."); return; }
            var result = Algorithms.BFS(graphManager, seciliDugum);

            // Tabloya Yaz
            gridSonuclar.Rows.Clear();
            gridSonuclar.Rows.Add("BAÞLANGIÇ", seciliDugum.Ad);
            for (int i = 0; i < result.Count; i++) gridSonuclar.Rows.Add(i + 1, result[i]);
            MessageBox.Show("BFS Sonucu tabloya yazýldý.");
        }

        // En Kýsa Yol (Dijkstra)
        private void btnShortestPath_Click(object sender, EventArgs e)
        {
            if (seciliDugum == null || hedefDugum == null) { MessageBox.Show("Ýki düðüm seçin."); return; }
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
                MessageBox.Show("Yol bulunamadý.");
            }
        }

        // Merkezilik (Centrality)
        private void btnCentrality_Click(object sender, EventArgs e)
        {
            var result = Algorithms.EnEtkiliKullanicilar(graphManager);
            gridSonuclar.Rows.Clear();
            foreach (var r in result) gridSonuclar.Rows.Add("Popüler", r);
        }

        // Renklendirme (Welsh-Powell)
        private void btnColoring_Click(object sender, EventArgs e)
        {
            int renkSayisi = Algorithms.WelshPowellRenklendirme(graphManager);
            pnlGraph.Invalidate();
            gridSonuclar.Rows.Clear();
            gridSonuclar.Rows.Add("SONUÇ", $"Toplam {renkSayisi} renk kullanýldý.");
            MessageBox.Show($"Boyama tamamlandý. {renkSayisi} renk kullanýldý.");
        }

        private void btnDFS_Click(object sender, EventArgs e)
        {
            if (seciliDugum == null)
            {
                MessageBox.Show("Lütfen haritadan baþlangýç düðümünü seçin (Sol Týk).");
                return;
            }

            // Algoritmayý çaðýr
            var result = Algorithms.DFS(graphManager, seciliDugum);

            // Tabloya Yaz
            gridSonuclar.Rows.Clear();
            gridSonuclar.Rows.Add("BAÞLANGIÇ", seciliDugum.Ad);

            for (int i = 0; i < result.Count; i++)
            {
                gridSonuclar.Rows.Add(i + 1, result[i]);
            }

            MessageBox.Show("DFS (Derinlik Öncelikli) sonucu tabloya yazýldý.");
        }

        private void btnSaveCSV_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "CSV Dosyasý|*.csv";
            saveFileDialog1.Title = "Grafý Kaydet";
            saveFileDialog1.FileName = "ProjeVerisi.csv"; // Varsayýlan isim

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // CSV satýrlarýný oluþturacaðýmýz liste
                    var satirlar = new List<string>();

                    // 1. Baþlýk Satýrý
                    satirlar.Add("Ad,Aktiflik,Etkilesim,BaglantiSayisi,Komsular");

                    // 2. Her düðüm için satýr oluþtur
                    foreach (var node in graphManager.Nodes)
                    {
                        // Bu düðümün komþularýný bul
                        var komsular = new List<string>();

                        foreach (var edge in graphManager.Edges)
                        {
                            // Eðer bu kenarýn bir ucu bizim düðümse, diðer ucu komþudur
                            if (edge.BaslangicDugumu == node) komsular.Add(edge.BitisDugumu.Ad);
                            else if (edge.BitisDugumu == node) komsular.Add(edge.BaslangicDugumu.Ad);
                        }

                        // Komþularý "User2-User3" þeklinde tire ile birleþtir
                        string komsularString = string.Join("-", komsular);

                        // CSV formatýnda satýrý hazýrla: Ad,Aktiflik,Etkileþim,BaðlantýSayýsý,Komþular
                        string satir = $"{node.Ad},{node.Aktiflik},{node.Etkilesim},{node.BaglantiSayisi},{komsularString}";
                        satirlar.Add(satir);
                    }

                    // 3. Dosyayý diske yaz
                    File.WriteAllLines(saveFileDialog1.FileName, satirlar);
                    MessageBox.Show("Dosya baþarýyla kaydedildi!", "Kayýt Baþarýlý");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kaydederken hata oluþtu: " + ex.Message);
                }
            }
        }

        private void btnAStar_Click(object sender, EventArgs e)
        {
            if (seciliDugum == null || hedefDugum == null) { MessageBox.Show("Baþlangýç (Sol Týk) ve Hedef (Sað Týk) seçmelisiniz."); return; }

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
                MessageBox.Show("Yol bulunamadý.");
            }
        }

        private void btnComponents_Click(object sender, EventArgs e)
        {
            // Algoritmayý çalýþtýr
            var bilesenler = Algorithms.BagliBilesenleriBul(graphManager);

            // Tabloyu Temizle ve Hazýrla
            gridSonuclar.Rows.Clear();

            // Sonuçlarý Yazdýr
            for (int i = 0; i < bilesenler.Count; i++)
            {
                // O topluluktaki kiþileri virgülle birleþtir (Örn: Ali, Veli, Ayþe)
                string uyeler = string.Join(", ", bilesenler[i]);

                gridSonuclar.Rows.Add($"TOPLULUK {i + 1}", uyeler);
            }

            MessageBox.Show($"Analiz Tamamlandý.\nToplam {bilesenler.Count} adet ayrýk topluluk (ada) bulundu.", "Baðlý Bileþen Analizi");
        }
    }
}