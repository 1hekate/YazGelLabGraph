using System;
using System.Drawing; // Point ve Color sınıfları için bu kütüphane şart

namespace YazGelLab
{
    public class UserNode
    {
        public string Ad { get; set; }

        // PDF'teki özellikler tablosuna göre
        public double Aktiflik { get; set; }    // Ozellik_I
        public double Etkilesim { get; set; }   // Ozellik_II
        public double BaglantiSayisi { get; set; } // Ozellik_III

        // Görselleştirme için koordinatlar (Canvas üzerindeki yeri)
        public Point Konum { get; set; }

        // --- YENİ EKLENEN ÖZELLİK (Welsh-Powell İçin) ---
        // Her düğüm kendi rengini hafızasında tutmalı.
        // Varsayılan olarak "Mavi" (CornflowerBlue) yaptık.
        public Color Renk { get; set; } = Color.CornflowerBlue;

        public UserNode(string ad, double aktiflik, double etkilesim, double baglantiSayisi)
        {
            Ad = ad;
            Aktiflik = aktiflik;
            Etkilesim = etkilesim;
            BaglantiSayisi = baglantiSayisi;

            // Konum ve Renk sonradan atanacağı için burada parametre olarak almaya gerek yok.
        }

        public override string ToString()
        {
            return $"{Ad} (Aktiflik: {Aktiflik})";
        }
    }
}