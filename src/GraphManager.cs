using System;
using System.Collections.Generic;
using System.Linq;

namespace YazGelLab
{
    public class GraphManager
    {
        public List<UserNode> Nodes { get; set; }
        public List<SocialEdge> Edges { get; set; }

        public GraphManager()
        {
            Nodes = new List<UserNode>();
            Edges = new List<SocialEdge>();
        }

        public void AddNode(UserNode node)
        {
            // Aynı isimde düğüm var mı kontrolü (PDF Madde 4.5)
            if (Nodes.Any(n => n.Ad == node.Ad))
            {
                // Kullanıcı zaten varsa eklemeyi reddedebiliriz veya hata fırlatabiliriz.
                // Şimdilik sessizce geçiyoruz veya uyarı verebiliriz.
                return;
            }
            Nodes.Add(node);
        }

        public void AddEdge(string ad1, string ad2)
        {
            UserNode u1 = Nodes.FirstOrDefault(n => n.Ad == ad1);
            UserNode u2 = Nodes.FirstOrDefault(n => n.Ad == ad2);

            if (u1 != null && u2 != null && u1 != u2)
            {
                // Bağlantıyı ekle
                Edges.Add(new SocialEdge(u1, u2));
            }
        }
    }
}