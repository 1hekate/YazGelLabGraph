using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

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
        // Rastgele Graf Oluþtur (Süre Ölçümlü)
        private void btnRandomGraph_Click(object sender, EventArgs e)
        {
            // 1. Temizle ve Hazýrla
            graphManager = new GraphManager();
            // 8 ile 12 arasýnda kiþi olsun (Çok kalabalýk olmasýn ama baðlar sýký olsun)
            int nodeCount = rnd.Next(8, 13);

            // 2. Kiþileri Oluþtur
            for (int i = 0; i < nodeCount; i++)
            {
                UserNode n = new UserNode("User" + (i + 1), rnd.Next(1, 10), rnd.Next(10, 100), rnd.Next(1, 20));

                // Rastgele konum (Kenarlara çok yapýþmasýn)
                n.Konum = new Point(rnd.Next(100, pnlGraph.Width - 100), rnd.Next(100, pnlGraph.Height - 100));

                graphManager.AddNode(n);
            }

            // 3. BAÐLANTI OLUÞTURMA (GÜNCELLENMÝÞ KISIM)
            // Herkesi diðer herkesle kýyasla
            for (int i = 0; i < graphManager.Nodes.Count; i++)
            {
                for (int j = i + 1; j < graphManager.Nodes.Count; j++)
                {
                    // %35 Ýhtimalle Baðlantý Kur (Bu oran artarsa graf çorba olur, azalýrsa kopuk olur)
                    if (rnd.Next(100) < 35)
                    {
                        var k1 = graphManager.Nodes[i];
                        var k2 = graphManager.Nodes[j];

                        // Zaten baðlý deðillerse baðla
                        graphManager.AddEdge(k1.Ad, k2.Ad);
                    }
                }
            }

            // 4. Çiz ve Bilgi Ver
            pnlGraph.Invalidate();

            // Baðlantý sayýsýný kontrol edelim
            int toplamBaglanti = graphManager.Edges.Count;
            MessageBox.Show($"Graf Oluþturuldu!\nKiþi Sayýsý: {nodeCount}\nToplam Baðlantý: {toplamBaglanti}\n");
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

            // 1. Kronometreyi Baþlat
            Stopwatch sw = Stopwatch.StartNew();

            // 2. Algoritmayý Çalýþtýr 
            var result = Algorithms.BFS(graphManager, seciliDugum);

            // 3. Kronometreyi Durdur
            sw.Stop();

            // Tabloya Yaz
            gridSonuclar.Rows.Clear();
            gridSonuclar.Rows.Add("BAÞLANGIÇ", seciliDugum.Ad);
            for (int i = 0; i < result.Count; i++) gridSonuclar.Rows.Add(i + 1, result[i]);

            // Mesajda süreyi göster (Milisaniye cinsinden)
            // Ticks: Ýþlemci vuruþ sayýsýdýr (Çok daha hassastýr)
            string sureBilgisi = $"Süre: {sw.Elapsed.TotalMilliseconds} ms ({sw.ElapsedTicks} Ticks)";

            MessageBox.Show("BFS Sonucu tabloya yazýldý.\n" + sureBilgisi, "Performans");
        }

        // En Kýsa Yol (Dijkstra)
        private void btnShortestPath_Click(object sender, EventArgs e)
        {
            // 1. Kontrol: Baþlangýç ve Hedef seçili mi?
            // Sol týkla baþlangýç (Yeþil), Sað týkla hedef (Kýrmýzý) seçilmeli.
            if (seciliDugum == null || hedefDugum == null)
            {
                MessageBox.Show("Lütfen haritadan iki kiþi seçin:\n1. Baþlangýç için Sol Týk (Yeþil)\n2. Hedef için Sað Týk (Kýrmýzý)");
                return;
            }

            // 2. Kronometreyi Baþlat
            Stopwatch sw = Stopwatch.StartNew();

            // 3. Algoritmayý Çalýþtýr
            // Algorithms sýnýfýndaki Dijkstra metodunu çaðýrýyoruz
            var path = Algorithms.Dijkstra(graphManager, seciliDugum, hedefDugum);

            // 4. Kronometreyi Durdur
            sw.Stop();

            // 5. Sonuçlarý Tabloya Yaz
            gridSonuclar.Rows.Clear(); // Önceki sonuçlarý temizle

            if (path != null && path.Count > 0)
            {
                string yolMetni = "";

                // Listeyi dön ve tabloya ekle
                for (int i = 0; i < path.Count; i++)
                {
                    gridSonuclar.Rows.Add($"{i + 1}. ADIM", path[i].Ad);
                    yolMetni += path[i].Ad + (i < path.Count - 1 ? " -> " : "");
                }

                // 6. Sonucu ve Süreyi Mesajla Göster
                string mesaj = $"En Kýsa Yol Bulundu!\n\n" +
                               $"Yol: {yolMetni}\n" +
                               $"Adým Sayýsý: {path.Count - 1}\n" +
                               $"Süre: {sw.Elapsed.TotalMilliseconds} ms ({sw.ElapsedTicks} Ticks)";

                MessageBox.Show(mesaj, "Dijkstra Sonucu");
            }
            else
            {
                MessageBox.Show("Bu iki kiþi arasýnda herhangi bir baðlantý (yol) bulunamadý.", "Yol Yok");
            }
        }

        // Merkezilik (Centrality)
        private void btnCentrality_Click(object sender, EventArgs e)
        {
            // 1. Kontrol: Graf boþ mu?
            if (graphManager.Nodes.Count == 0)
            {
                MessageBox.Show("Analiz için önce graf oluþturmalýsýnýz.");
                return;
            }

            // 2. Kronometreyi Baþlat
            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();

            // 3. Hesaplama Yap (Garanti olsun diye anlýk sayýyoruz)
            // Her düðümün kaç baðlantýsý var say ve özelliðine yaz
            foreach (var node in graphManager.Nodes)
            {
                // Bu düðümün dahil olduðu (baþlangýç veya bitiþ) kenarlarý say
                int derece = graphManager.Edges.Count(edge => edge.BaslangicDugumu == node || edge.BitisDugumu == node);
                node.BaglantiSayisi = derece;
            }

            // 4. Sýrala ve Ýlk 5'i Al (LINQ Kullanýyoruz)
            var top5List = graphManager.Nodes
                            .OrderByDescending(n => n.BaglantiSayisi) // Büyükten küçüðe sýrala
                            .Take(5)                                  // Ýlk 5 tanesini al
                            .ToList();

            // 5. Kronometreyi Durdur
            sw.Stop();

            // 6. Sonuçlarý Tabloya Yaz
            gridSonuclar.Rows.Clear();

            // Baþlýk
            gridSonuclar.Rows.Add("SIRA", "KULLANICI (BAÐLANTI)");

            for (int i = 0; i < top5List.Count; i++)
            {
                var node = top5List[i];
                // Örn: 1. Ahmet (15)
                gridSonuclar.Rows.Add($"{i + 1}. EN POPÜLER", $"{node.Ad} ({node.BaglantiSayisi} Arkadaþ)");
            }

            // 7. Performans Mesajý
            string mesaj = $"Merkezilik Analizi Tamamlandý!\n" +
                           $"Ýncelenen Kiþi: {graphManager.Nodes.Count}\n" +
                           $"Süre: {sw.Elapsed.TotalMilliseconds} ms ({sw.ElapsedTicks} Ticks)";

            MessageBox.Show(mesaj, "En Etkili 5 Kiþi");
        }

        // Renklendirme (Welsh-Powell)
        private void btnColoring_Click(object sender, EventArgs e)
        {
            // 1. Kontrol: Graf boþ mu?
            if (graphManager.Nodes.Count == 0)
            {
                MessageBox.Show("Renklendirme yapmak için önce graf oluþturmalýsýnýz.");
                return;
            }

            // 2. Kronometreyi Baþlat
            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();

            // 3. Algoritmayý Çalýþtýr
            // (Bu metot düðümlerin .Renk özelliðini deðiþtiriyor)
            Algorithms.WelshPowellRenklendirme(graphManager);

            // 4. Kronometreyi Durdur
            sw.Stop();

            // 5. Ekraný Yenile (Boyanan düðümleri görmek için þart!)
            pnlGraph.Invalidate();

            // 6. Sonuçlarý Hesapla (Kaç farklý renk kullanýldý?)
            // Linq kullanarak kaç farklý renk olduðunu sayýyoruz
            int renkSayisi = graphManager.Nodes.Select(n => n.Renk).Distinct().Count();

            // 7. Tabloya Yaz
            gridSonuclar.Rows.Clear();
            gridSonuclar.Rows.Add("DURUM", "Renklendirme Tamamlandý");
            gridSonuclar.Rows.Add("TOPLAM RENK", renkSayisi + " Adet");

            // Ýstersen her düðümün rengini de listeleyebilirsin
            foreach (var node in graphManager.Nodes)
            {
                // Rengin adýný (Red, Blue vs) veya kodunu yaz
                gridSonuclar.Rows.Add(node.Ad, node.Renk.Name);
            }

            // 8. Sonuç Mesajý
            string mesaj = $"Welsh-Powell Renklendirme Bitti!\n" +
                           $"Toplam Kullanýlan Renk: {renkSayisi}\n" +
                           $"Süre: {sw.Elapsed.TotalMilliseconds} ms ({sw.ElapsedTicks} Ticks)";

            MessageBox.Show(mesaj, "Renklendirme Performansý");
        }

        private void btnDFS_Click(object sender, EventArgs e)
        {
            // 1. Kontrol: Bir kiþi seçili mi? (Sol Týk - Yeþil)
            if (seciliDugum == null)
            {
                MessageBox.Show("Lütfen haritadan baþlangýç düðümünü seçin (Sol Týk).");
                return;
            }

            // 2. Kronometreyi Baþlat
            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();

            // 3. Algoritmayý Çalýþtýr
            // Algorithms sýnýfýndaki DFS metodunu çaðýrýyoruz
            // (Bize ziyaret edilen isimlerin listesini dönüyor)
            var result = Algorithms.DFS(graphManager, seciliDugum);

            // 4. Kronometreyi Durdur
            sw.Stop();

            // 5. Sonuçlarý Tabloya Yaz
            gridSonuclar.Rows.Clear();
            gridSonuclar.Rows.Add("BAÞLANGIÇ", seciliDugum.Ad);

            for (int i = 0; i < result.Count; i++)
            {
                // Tabloya: 1. ADIM | Ahmet
                gridSonuclar.Rows.Add($"{i + 1}. ADIM", result[i]);
            }

            // 6. Performans Raporu
            string mesaj = $"DFS Taramasý Tamamlandý!\n" +
                           $"Toplam Eriþilen Kiþi: {result.Count}\n" +
                           $"Süre: {sw.Elapsed.TotalMilliseconds} ms ({sw.ElapsedTicks} Ticks)";

            MessageBox.Show(mesaj, "DFS Performans Sonucu");
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
            // 1. Seçim Kontrolü: Ýki nokta seçilmiþ mi?
            // Sol Týk (Yeþil) = Baþlangýç, Sað Týk (Kýrmýzý) = Hedef
            if (seciliDugum == null || hedefDugum == null)
            {
                MessageBox.Show("A* algoritmasý için iki kiþi seçmelisiniz:\n1. Baþlangýç (Sol Týk)\n2. Hedef (Sað Týk)");
                return;
            }

            // 2. Kronometreyi Baþlat
            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();

            // 3. Algoritmayý Çalýþtýr
            // Algorithms sýnýfýndaki AStar metodunu çaðýrýyoruz
            var path = Algorithms.AStar(graphManager, seciliDugum, hedefDugum);

            // 4. Kronometreyi Durdur
            sw.Stop();

            // 5. Sonuçlarý Tabloya Yaz
            gridSonuclar.Rows.Clear();

            if (path != null && path.Count > 0)
            {
                string yolMetni = "";

                // Listeyi dön ve tabloya ekle
                for (int i = 0; i < path.Count; i++)
                {
                    gridSonuclar.Rows.Add($"A* {i + 1}. ADIM", path[i].Ad);
                    // Yol metnini oluþtur (A -> B -> C þeklinde)
                    yolMetni += path[i].Ad + (i < path.Count - 1 ? " -> " : "");
                }

                // 6. Performans Mesajý
                string mesaj = $"A* Ýle Hedefe Ulaþýldý!\n\n" +
                               $"Güzergah: {yolMetni}\n" +
                               $"Toplam Adým: {path.Count - 1}\n" +
                               $"Süre: {sw.Elapsed.TotalMilliseconds} ms ({sw.ElapsedTicks} Ticks)";

                MessageBox.Show(mesaj, "A* Performans Sonucu");
            }
            else
            {
                MessageBox.Show("Seçilen kiþiler arasýnda gidilebilecek bir yol yok.", "Sonuç Bulunamadý");
            }
        }

        private void btnComponents_Click(object sender, EventArgs e)
        {
            // 1. Kontrol: Graf boþ mu?
            if (graphManager.Nodes.Count == 0)
            {
                MessageBox.Show("Analiz yapabilmek için ekranda düðümler olmalýdýr.");
                return;
            }

            // 2. Kronometreyi Baþlat
            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();

            // 3. Algoritmayý Çalýþtýr
            // Algorithms sýnýfýndaki BagliBilesenleriBul metodunu çaðýrýyoruz
            // Bize List<List<string>> dönecek (Her liste bir ada)
            var bilesenler = Algorithms.BagliBilesenleriBul(graphManager);

            // 4. Kronometreyi Durdur
            sw.Stop();

            // 5. Sonuçlarý Tabloya Yaz
            gridSonuclar.Rows.Clear();
            gridSonuclar.Rows.Add("DURUM", "Analiz Tamamlandý");

            for (int i = 0; i < bilesenler.Count; i++)
            {
                // O gruptaki kiþilerin isimlerini yan yana yaz (Ali, Veli, Ayþe...)
                string grupUyeleri = string.Join(", ", bilesenler[i]);

                // Tabloya ekle: "TOPLULUK 1", "Ali, Veli..."
                gridSonuclar.Rows.Add($"TOPLULUK {i + 1}", grupUyeleri);
            }

            // 6. Performans Mesajý
            string mesaj = $"Topluluk Analizi Bitti!\n" +
                           $"Bulunan Ayrýk Ada Sayýsý: {bilesenler.Count}\n" +
                           $"Süre: {sw.Elapsed.TotalMilliseconds} ms ({sw.ElapsedTicks} Ticks)";

            MessageBox.Show(mesaj, "Baðlý Bileþen Performansý");
        }

        // BÜYÜK GRAF OLUÞTUR (50-100 KÝÞÝLÝK)
        private void btnRandomLarge_Click(object sender, EventArgs e)
        {
            // 1. Kronometreyi Baþlat
            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();

            // 2. Temizle
            graphManager = new GraphManager();

            // 50 ile 100 arasýnda rastgele kiþi sayýsý
            int nodeCount = rnd.Next(50, 101);

            // 3. Kiþileri Oluþtur
            for (int i = 0; i < nodeCount; i++)
            {
                // Ýsimler: User_1, User_2...
                UserNode n = new UserNode("User_" + (i + 1), rnd.Next(1, 10), rnd.Next(10, 100), rnd.Next(1, 20));

                // Konum (Kenarlardan biraz boþluk býrakarak)
                n.Konum = new Point(rnd.Next(50, pnlGraph.Width - 50), rnd.Next(50, pnlGraph.Height - 50));

                graphManager.AddNode(n);
            }

            // 4. BAÐLANTI OLUÞTURMA (GÜNCELLENMÝÞ MANTIK)
            // Herkesi herkesle kontrol et ama olasýlýðý "Büyük Graf" olduðu için biraz kýsalým
            // (Çok kalabalýk olunca %35 çok fazla çizgi yapar, ekran kararýr. %10-15 idealdir)

            int baglantiSansi = 15; // %15 Ýhtimal (Bu bile 100 kiþide ortalama 750 çizgi yapar!)

            for (int i = 0; i < graphManager.Nodes.Count; i++)
            {
                for (int j = i + 1; j < graphManager.Nodes.Count; j++)
                {
                    // %15 ihtimalle baðla
                    if (rnd.Next(100) < baglantiSansi)
                    {
                        var k1 = graphManager.Nodes[i];
                        var k2 = graphManager.Nodes[j];
                        graphManager.AddEdge(k1.Ad, k2.Ad);
                    }
                }
            }

            // 5. Kronometreyi Durdur
            sw.Stop();

            // 6. Çiz
            pnlGraph.Invalidate();

            // 7. Rapor Ver
            string mesaj = $"DEV Graf Oluþturuldu!\n" +
                           $"Kiþi Sayýsý: {nodeCount}\n" +
                           $"Toplam Baðlantý: {graphManager.Edges.Count}\n" +
                           $"Oluþturma Süresi: {sw.Elapsed.TotalMilliseconds} ms";

            MessageBox.Show(mesaj, "Yüksek Performans Testi");
        }
    }
}