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

        UserNode seciliDugum = null;
        UserNode hedefDugum = null;

        public Form1()
        {
            InitializeComponent();
            graphManager = new GraphManager();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (gridSonuclar != null)
            {
                gridSonuclar.Columns.Clear();
                gridSonuclar.Columns.Add("Sira", "Sıra");
                gridSonuclar.Columns.Add("Bilgi", "Sonuç");
                gridSonuclar.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }
    }
}