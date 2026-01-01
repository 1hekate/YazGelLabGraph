using System;
using System.Collections.Generic;
using System.Linq;

namespace YazGelLab
{
    public class Algorithms
    {
        // 1. BFS Algoritması (Genişlik Öncelikli Arama)
        // Başlangıç düğümünden gidilebilecek herkesi bulur ve listeler.
        public static List<string> BFS(GraphManager graph, UserNode startNode)
        {
            var visited = new List<string>();       // Ziyaret edilenlerin isimleri
            var queue = new Queue<UserNode>();      // Sırada bekleyenler

            visited.Add(startNode.Ad);
            queue.Enqueue(startNode);

            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();

                // Bu düğümün komşularını bul
                // (Edge listesinde Başlangıç veya Bitiş düğümü bu kişi olan tüm bağlantıları bul)
                var neighbors = graph.Edges
                    .Where(e => e.BaslangicDugumu == currentNode || e.BitisDugumu == currentNode)
                    .Select(e => e.BaslangicDugumu == currentNode ? e.BitisDugumu : e.BaslangicDugumu);

                foreach (var neighbor in neighbors)
                {
                    if (!visited.Contains(neighbor.Ad))
                    {
                        visited.Add(neighbor.Ad);
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return visited;
        }
        // 2. Dijkstra Algoritması (En Kısa Yol)
        public static List<UserNode> Dijkstra(GraphManager graph, UserNode startNode, UserNode endNode)
        {
            var distances = new Dictionary<UserNode, double>();
            var previous = new Dictionary<UserNode, UserNode>();
            var nodes = new List<UserNode>();

            // Başlangıç ayarları
            foreach (var node in graph.Nodes)
            {
                distances[node] = double.MaxValue; // Sonsuz uzaklık
                previous[node] = null;
                nodes.Add(node);
            }

            distances[startNode] = 0;

            while (nodes.Count > 0)
            {
                // En kısa mesafeye sahip düğümü seç
                nodes.Sort((x, y) => distances[x].CompareTo(distances[y]));
                var smallest = nodes[0];
                nodes.Remove(smallest);

                if (smallest == endNode) // Hedefe ulaştık
                {
                    var path = new List<UserNode>();
                    while (previous[smallest] != null)
                    {
                        path.Add(smallest);
                        smallest = previous[smallest];
                    }
                    path.Add(startNode);
                    path.Reverse();
                    return path;
                }

                if (distances[smallest] == double.MaxValue) break;

                // Komşuları gez
                var neighbors = graph.Edges
                    .Where(e => e.BaslangicDugumu == smallest || e.BitisDugumu == smallest)
                    .ToList();

                foreach (var edge in neighbors)
                {
                    var neighbor = (edge.BaslangicDugumu == smallest) ? edge.BitisDugumu : edge.BaslangicDugumu;

                    // Düğüm daha önce işlendiyse atla (nodes listesinde yoksa işlenmiştir)
                    if (!nodes.Contains(neighbor)) continue;

                    double alt = distances[smallest] + edge.Agirlik;
                    if (alt < distances[neighbor])
                    {
                        distances[neighbor] = alt;
                        previous[neighbor] = smallest;
                    }
                }
            }

            return null; // Yol bulunamadı
        }

        // 6. A* (A-Star) Algoritması
        public static List<UserNode> AStar(GraphManager graph, UserNode startNode, UserNode endNode)
        {
            var openSet = new List<UserNode> { startNode };
            var cameFrom = new Dictionary<UserNode, UserNode>();

            // gScore: Başlangıçtan buraya kadar olan gerçek maliyet
            var gScore = new Dictionary<UserNode, double>();
            foreach (var node in graph.Nodes) gScore[node] = double.MaxValue;
            gScore[startNode] = 0;

            // fScore: gScore + Heuristic (Tahmini kalan yol)
            var fScore = new Dictionary<UserNode, double>();
            foreach (var node in graph.Nodes) fScore[node] = double.MaxValue;
            fScore[startNode] = Heuristic(startNode, endNode);

            while (openSet.Count > 0)
            {
                // fScore değeri en düşük olanı seç
                var current = openSet.OrderBy(n => fScore[n]).First();

                if (current == endNode)
                    return ReconstructPath(cameFrom, current);

                openSet.Remove(current);

                // Komşuları bul
                var neighbors = graph.Edges
                    .Where(e => e.BaslangicDugumu == current || e.BitisDugumu == current)
                    .Select(e => new {
                        Node = (e.BaslangicDugumu == current) ? e.BitisDugumu : e.BaslangicDugumu,
                        Weight = e.Agirlik
                    });

                foreach (var neighborInfo in neighbors)
                {
                    var neighbor = neighborInfo.Node;
                    double tentativeGScore = gScore[current] + neighborInfo.Weight;

                    if (tentativeGScore < gScore[neighbor])
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeGScore;
                        fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, endNode);

                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }
            }
            return null; // Yol yok
        }

        // Yardımcı Metot: Kuş uçuşu mesafe (Öklid)
        private static double Heuristic(UserNode a, UserNode b)
        {
            // Koordinatlar arasındaki düz çizgi mesafesi
            return Math.Sqrt(Math.Pow(a.Konum.X - b.Konum.X, 2) + Math.Pow(a.Konum.Y - b.Konum.Y, 2));
        }

        // Yardımcı Metot: Yolu geriye doğru kurma
        private static List<UserNode> ReconstructPath(Dictionary<UserNode, UserNode> cameFrom, UserNode current)
        {
            var totalPath = new List<UserNode> { current };
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                totalPath.Add(current);
            }
            totalPath.Reverse();
            return totalPath;
        }
        // 3. Degree Centrality (En Popüler Kullanıcıları Bulma)
        public static List<string> EnEtkiliKullanicilar(GraphManager graph)
        {
            // Her kullanıcının kaç bağlantısı olduğunu sayalım
            var dereceler = new Dictionary<UserNode, int>();

            foreach (var node in graph.Nodes)
            {
                // Bu düğümün dahil olduğu tüm kenarları say
                int baglantiSayisi = graph.Edges.Count(e => e.BaslangicDugumu == node || e.BitisDugumu == node);
                dereceler.Add(node, baglantiSayisi);
            }

            // Bağlantı sayısına göre çoktan aza sırala ve ilk 5'i al
            var enIyiler = dereceler.OrderByDescending(x => x.Value)
                                    .Take(5)
                                    .Select(x => $"{x.Key.Ad} (Derece: {x.Value})")
                                    .ToList();

            return enIyiler;
        }

        public static int WelshPowellRenklendirme(GraphManager graph)
        {
            // 1. Düğümleri derecelerine (bağlantı sayılarına) göre çoktan aza sırala
            var siraliDugumler = graph.Nodes.OrderByDescending(n =>
                graph.Edges.Count(e => e.BaslangicDugumu == n || e.BitisDugumu == n)
            ).ToList();

            // Kullanılabilecek Renk Havuzu (İstersen buraya daha çok renk ekleyebilirsin)
            Color[] renkPaleti = {
        Color.Orange, Color.Purple, Color.Teal, Color.Pink,
        Color.Brown, Color.Gold, Color.Navy, Color.Olive
            };

            int renkIndex = 0;

            // Tüm düğümler boyanana kadar devam et
            while (siraliDugumler.Count > 0)
            {
                // Şu anki turda kullanacağımız renk
                // Eğer renk paleti biterse rastgele renk üret
                Color aktifRenk;
                if (renkIndex < renkPaleti.Length)
                    aktifRenk = renkPaleti[renkIndex];
                else
                    aktifRenk = Color.FromArgb(new Random().Next(256), new Random().Next(256), new Random().Next(256));

                // Listenin en başındaki (en yüksek dereceli) düğümü al ve boya
                var ilkDugum = siraliDugumler[0];
                ilkDugum.Renk = aktifRenk;

                // Bu renge boyadığımız düğümleri listeden ve geçici listeden takip edelim
                var buTurBoyananlar = new List<UserNode>();
                buTurBoyananlar.Add(ilkDugum);

                // Sıradaki diğer düğümlere bak:
                // Eğer bu düğüm, şu an boyadıklarımızdan HİÇBİRİNE komşu değilse, onu da aynı renge boya.
                for (int i = 1; i < siraliDugumler.Count; i++)
                {
                    var adayDugum = siraliDugumler[i];
                    bool komsuMu = false;

                    foreach (var boyanan in buTurBoyananlar)
                    {
                        // Komşuluk kontrolü
                        bool baglantiVar = graph.Edges.Any(e =>
                            (e.BaslangicDugumu == boyanan && e.BitisDugumu == adayDugum) ||
                            (e.BaslangicDugumu == adayDugum && e.BitisDugumu == boyanan));

                        if (baglantiVar)
                        {
                            komsuMu = true;
                            break;
                        }
                    }

                    if (!komsuMu)
                    {
                        adayDugum.Renk = aktifRenk;
                        buTurBoyananlar.Add(adayDugum);
                    }
                }

                // Boyananları listeden çıkar (Artık işleri bitti)
                foreach (var boyanan in buTurBoyananlar)
                {
                    siraliDugumler.Remove(boyanan);
                }

                renkIndex++; // Bir sonraki renge geç
            }

            return renkIndex; // Toplam kullanılan renk sayısını döndür
        }

        public static List<string> DFS(GraphManager graph, UserNode startNode)
        {
            var visited = new List<string>();
            var stack = new Stack<UserNode>(); // Queue yerine Stack

            stack.Push(startNode);

            while (stack.Count > 0)
            {
                var currentNode = stack.Pop();

                if (!visited.Contains(currentNode.Ad))
                {
                    visited.Add(currentNode.Ad);

                    // Komşuları bul
                    var neighbors = graph.Edges
                        .Where(e => e.BaslangicDugumu == currentNode || e.BitisDugumu == currentNode)
                        .Select(e => e.BaslangicDugumu == currentNode ? e.BitisDugumu : e.BaslangicDugumu)
                        .ToList(); // Listeye çevir ki ters çevirebilelim

                    // Stack yapısı gereği komşuları tersten eklersek daha doğal bir gezinme olur
                    neighbors.Reverse();

                    foreach (var neighbor in neighbors)
                    {
                        if (!visited.Contains(neighbor.Ad))
                        {
                            stack.Push(neighbor);
                        }
                    }
                }
            }
            return visited;
        }
        // 7. Bağlı Bileşenleri (Ayrık Toplulukları) Bulma
        public static List<List<string>> BagliBilesenleriBul(GraphManager graph)
        {
            var tumBilesenler = new List<List<string>>();
            var ziyaretEdilenler = new HashSet<string>();

            foreach (var node in graph.Nodes)
            {
                // Eğer bu düğüme daha önce hiç uğramadıysak, yeni bir "Ada" keşfettik demektir.
                if (!ziyaretEdilenler.Contains(node.Ad))
                {
                    // Bu düğümden başlayarak ulaşılabilen HERKESİ bul (Mevcut BFS metodumuzu kullanıyoruz)
                    var bilesen = BFS(graph, node);

                    // Bu adadaki herkesi "ziyaret edildi" olarak işaretle ki tekrar saymayalım
                    foreach (var n in bilesen)
                    {
                        ziyaretEdilenler.Add(n);
                    }

                    // Adayı listeye ekle
                    tumBilesenler.Add(bilesen);
                }
            }

            return tumBilesenler;
        }


    }
}