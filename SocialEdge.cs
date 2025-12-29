using System;

namespace YazGelLab
{
    public class SocialEdge
    {
        public UserNode BaslangicDugumu { get; set; }
        public UserNode BitisDugumu { get; set; }
        public double Agirlik { get; private set; } // Otomatik hesaplanacak

        public SocialEdge(UserNode baslangic, UserNode bitis)
        {
            BaslangicDugumu = baslangic;
            BitisDugumu = bitis;
            AgirlikHesapla();
        }

        // PDF Madde 4.3'teki Dinamik Ağırlık Hesaplama Formülü
        private void AgirlikHesapla()
        {
            double diffAktiflik = BaslangicDugumu.Aktiflik - BitisDugumu.Aktiflik;
            double diffEtkilesim = BaslangicDugumu.Etkilesim - BitisDugumu.Etkilesim;
            double diffBaglanti = BaslangicDugumu.BaglantiSayisi - BitisDugumu.BaglantiSayisi;

            // Formül: 1 + Sqrt((Fark1^2) + (Fark2^2) + (Fark3^2))
            double karelerToplami = (diffAktiflik * diffAktiflik) +
                                    (diffEtkilesim * diffEtkilesim) +
                                    (diffBaglanti * diffBaglanti);

            Agirlik = 1 + Math.Sqrt(karelerToplami);
        }
    }
}