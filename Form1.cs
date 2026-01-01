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

        
    }
}