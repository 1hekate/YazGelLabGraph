# 📊 Sosyal Ağ Analizi Uygulaması
*Yazılım Geliştirme Laboratuvarı – I | Proje 2*

---

## 📌 Proje Bilgileri
- *Üniversite:* Kocaeli Üniversitesi  
- *Fakülte:* Teknoloji Fakültesi  
- *Bölüm:* Bilişim Sistemleri Mühendisliği  
- *Ders:* Yazılım Geliştirme Laboratuvarı – I  
- *Proje Türü:* Sosyal Ağ Analizi (Graf Tabanlı)  

### 👥 Ekip Üyeleri
- *Eren Dağlı – 231307033*
- *Semih Gökmen – 231307070*

---

## 1️⃣ Giriş

Bu projede kullanıcılar arasındaki ilişkiler graf veri yapısı kullanılarak modellenmiştir.  
Her kullanıcı bir *düğüm (node), aralarındaki ilişkiler ise **kenar (edge)* olarak temsil edilmiştir.

Amaç:
- Sosyal ağ yapısını analiz etmek
- Graf algoritmalarını uygulamak
- Algoritmaların doğruluğunu ve performansını incelemek
- Nesne yönelimli programlama prensiplerini kullanmak

---

## 2️⃣ Kullanılan Teknolojiler

| Alan | Teknoloji |
|----|----|
| Programlama Dili | C# (.NET 6.0) |
| Arayüz | Windows Forms |
| Veri Kaynağı | CSV |
| Algoritmalar | BFS, DFS, Dijkstra, A*, Welsh–Powell |
| Dokümantasyon | Markdown, Mermaid |

---

## 3️⃣ Uygulanan Algoritmalar

### 🔹 BFS – Genişlik Öncelikli Arama

- Bağlı tüm düğümleri katman katman dolaşır  
- Zaman karmaşıklığı: *O(V + E)*
- 
<img width="1600" height="852" alt="image" src="https://github.com/user-attachments/assets/cec2a5fc-ec1e-4549-a221-1dbfced24fb3" />
<img width="1600" height="849" alt="image" src="https://github.com/user-attachments/assets/9c49d462-b1d9-4a57-b92f-8708426733c2" />


mermaid
flowchart TD
    A[Başlangıç Düğümü] --> B[Kuyruğa Ekle]
    B --> C[Komşuları Ziyaret Et]
    C --> D{Yeni Düğüm Var mı?}
    D -->|Evet| B
    D -->|Hayır| E[Bitiş]


---

### 🔹 DFS – Derinlik Öncelikli Arama

- Stack mantığı ile derinlemesine arama yapar  
- Zaman karmaşıklığı: *O(V + E)*  

<img width="1600" height="852" alt="image" src="https://github.com/user-attachments/assets/7488c515-c62b-4877-9b29-be5a957ee2fb" />
<img width="1600" height="851" alt="image" src="https://github.com/user-attachments/assets/ca077cd3-8219-4e45-bd62-4eb98fef1ae6" />




---

### 🔹 Dijkstra – En Kısa Yol Algoritması

- Ağırlıklı graflarda en kısa yolu hesaplar  
- Zaman karmaşıklığı: *O(E log V)*  

<img width="1600" height="850" alt="image" src="https://github.com/user-attachments/assets/faa0a7ad-6f6b-46be-ab71-3d3825df84e8" />
<img width="1600" height="850" alt="image" src="https://github.com/user-attachments/assets/da618413-80d1-4677-ab94-fa91452ddd6f" />



---

### 🔹 A* (A-Star) Algoritması

- Dijkstra algoritmasına heuristic yaklaşım ekler  
- Hedef odaklı daha hızlı yol bulur  

<img width="1600" height="850" alt="image" src="https://github.com/user-attachments/assets/1dd3719c-bfcb-4a78-a8f3-4763c4a7ee12" />
<img width="1600" height="848" alt="image" src="https://github.com/user-attachments/assets/5d4417cc-6b24-4674-91e5-e5baf2f65858" />



---

### 🔹 En Etkili 5 Kullanıcı (Degree Centrality)

- Düğüm derecelerine göre en fazla bağlantıya sahip kullanıcıları belirler  

<img width="1600" height="852" alt="image" src="https://github.com/user-attachments/assets/3c9f1b12-9843-4ff1-8304-e7083ae81487" />
<img width="1600" height="847" alt="image" src="https://github.com/user-attachments/assets/99218d59-81d9-47a7-ab8a-9bef1a899c6e" />



---

### 🔹 Welsh–Powell Graf Renklendirme

- Komşu düğümlerin aynı renge sahip olmaması sağlanır  
- Ayrık topluluklar kendi içinde değerlendirilir  

<img width="1600" height="852" alt="image" src="https://github.com/user-attachments/assets/efc0fa8c-6aa0-4446-a0cc-bb3858d46065" />
<img width="1600" height="852" alt="image" src="https://github.com/user-attachments/assets/4dcb672d-b390-4b86-9407-c6cd41a57198" />


mermaid
graph LR
A --- B
B --- C
C --- D
A --- D


---

## 4️⃣ Nesne Yönelimli Tasarım (OOP)

mermaid
classDiagram
    class UserNode {
        string Ad
        double Aktiflik
        double Etkilesim
        double BaglantiSayisi
        Point Konum
        Color Renk
    }

    class SocialEdge {
        UserNode Baslangic
        UserNode Bitis
        double Agirlik
    }

    class GraphManager {
        List<UserNode> Nodes
        List<SocialEdge> Edges
    }

    GraphManager --> UserNode
    GraphManager --> SocialEdge


---

## 5️⃣ Performans Testleri

Performans ölçümleri *küçük ölçekli* ve *büyük ölçekli* graf yapıları üzerinde
gerçekleştirilmiştir. Ölçümler ilk test sonuçlarına dayanmaktadır.

| Algoritma | Küçük Graf (ms) | Büyük Graf (ms) |
|---------|----------------|----------------|
| BFS | 0.017 | 1.17 |
| DFS | 0.028 | 1.90 |
| Dijkstra | 0.03 | 2.70 |
| A* | 0.17 | 5.77 |
| Welsh–Powell Renklendirme | 0.14 | 8.58 |
| En Etkili 5 Kullanıcı | 0.08 | 4.20 |

---

## 6️⃣ Sonuç ve Değerlendirme

- Tüm algoritmalar başarıyla uygulanmıştır  
- Küçük graf yapılarında algoritmalar oldukça hızlı çalışmaktadır  
- Büyük graf yapılarında çalışma sürelerinin belirgin şekilde arttığı gözlemlenmiştir  
- Sonuçlar algoritmaların teorik zaman karmaşıklıklarıyla uyumludur  

---

## 📌 Notlar
- Proje GitHub üzerinden teslim edilecek şekilde düzenlenmiştir  
- README dosyası Markdown ve Mermaid uyumludur  

---

*© 2026 – Kocaeli Üniversitesi*
