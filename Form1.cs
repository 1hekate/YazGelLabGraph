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
    }
}
    