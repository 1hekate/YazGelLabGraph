namespace YazGelLab
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlGraph = new Panel();
            btnRandomGraph = new Button();
            btnClear = new Button();
            btnLoadCSV = new Button();
            openFileDialog1 = new OpenFileDialog();
            btnBFS = new Button();
            btnShortestPath = new Button();
            btnCentrality = new Button();
            btnColoring = new Button();
            txtAd = new TextBox();
            txtAktiflik = new TextBox();
            txtEtkilesim = new TextBox();
            btnUpdateNode = new Button();
            btnDeleteNode = new Button();
            btnAddEdge = new Button();
            btnRemoveEdge = new Button();
            gridSonuclar = new DataGridView();
            btnAddNodeManuel = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            btnDFS = new Button();
            btnSaveCSV = new Button();
            saveFileDialog1 = new SaveFileDialog();
            btnAStar = new Button();
            btnComponents = new Button();
            btnRandomLarge = new Button();
            ((System.ComponentModel.ISupportInitialize)gridSonuclar).BeginInit();
            SuspendLayout();
            // 
            // pnlGraph
            // 
            pnlGraph.Location = new Point(567, 12);
            pnlGraph.Name = "pnlGraph";
            pnlGraph.Size = new Size(812, 459);
            pnlGraph.TabIndex = 0;
            pnlGraph.Paint += pnlGraph_Paint;
            pnlGraph.MouseClick += pnlGraph_MouseClick;
            // 
            // btnRandomGraph
            // 
            btnRandomGraph.Location = new Point(12, 12);
            btnRandomGraph.Name = "btnRandomGraph";
            btnRandomGraph.Size = new Size(94, 29);
            btnRandomGraph.TabIndex = 1;
            btnRandomGraph.Text = "RasgteleGraphOlustur";
            btnRandomGraph.UseVisualStyleBackColor = true;
            btnRandomGraph.Click += btnRandomGraph_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(112, 12);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(94, 29);
            btnClear.TabIndex = 2;
            btnClear.Text = "Temizle";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // btnLoadCSV
            // 
            btnLoadCSV.Location = new Point(113, 299);
            btnLoadCSV.Name = "btnLoadCSV";
            btnLoadCSV.Size = new Size(94, 29);
            btnLoadCSV.TabIndex = 3;
            btnLoadCSV.Text = "CSV Yükle";
            btnLoadCSV.UseVisualStyleBackColor = true;
            btnLoadCSV.Click += btnLoadCSV_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnBFS
            // 
            btnBFS.Location = new Point(112, 47);
            btnBFS.Name = "btnBFS";
            btnBFS.Size = new Size(94, 29);
            btnBFS.TabIndex = 4;
            btnBFS.Text = "BFS Çalıştır";
            btnBFS.UseVisualStyleBackColor = true;
            btnBFS.Click += btnBFS_Click;
            // 
            // btnShortestPath
            // 
            btnShortestPath.Location = new Point(12, 82);
            btnShortestPath.Name = "btnShortestPath";
            btnShortestPath.Size = new Size(94, 29);
            btnShortestPath.TabIndex = 5;
            btnShortestPath.Text = "Dijkstra";
            btnShortestPath.UseVisualStyleBackColor = true;
            btnShortestPath.Click += btnShortestPath_Click;
            // 
            // btnCentrality
            // 
            btnCentrality.Location = new Point(113, 117);
            btnCentrality.Name = "btnCentrality";
            btnCentrality.Size = new Size(94, 29);
            btnCentrality.TabIndex = 6;
            btnCentrality.Text = "En Etkili 5 Kişi";
            btnCentrality.UseVisualStyleBackColor = true;
            btnCentrality.Click += btnCentrality_Click;
            // 
            // btnColoring
            // 
            btnColoring.Location = new Point(12, 117);
            btnColoring.Name = "btnColoring";
            btnColoring.Size = new Size(94, 29);
            btnColoring.TabIndex = 7;
            btnColoring.Text = "GrafRenk";
            btnColoring.UseVisualStyleBackColor = true;
            btnColoring.Click += btnColoring_Click;
            // 
            // txtAd
            // 
            txtAd.Location = new Point(12, 328);
            txtAd.Name = "txtAd";
            txtAd.Size = new Size(125, 27);
            txtAd.TabIndex = 8;
            // 
            // txtAktiflik
            // 
            txtAktiflik.Location = new Point(12, 361);
            txtAktiflik.Name = "txtAktiflik";
            txtAktiflik.Size = new Size(125, 27);
            txtAktiflik.TabIndex = 9;
            // 
            // txtEtkilesim
            // 
            txtEtkilesim.Location = new Point(12, 394);
            txtEtkilesim.Name = "txtEtkilesim";
            txtEtkilesim.Size = new Size(125, 27);
            txtEtkilesim.TabIndex = 10;
            // 
            // btnUpdateNode
            // 
            btnUpdateNode.Location = new Point(112, 169);
            btnUpdateNode.Name = "btnUpdateNode";
            btnUpdateNode.Size = new Size(94, 29);
            btnUpdateNode.TabIndex = 11;
            btnUpdateNode.Text = "Güncelle";
            btnUpdateNode.UseVisualStyleBackColor = true;
            btnUpdateNode.Click += btnUpdateNode_Click;
            // 
            // btnDeleteNode
            // 
            btnDeleteNode.Location = new Point(13, 239);
            btnDeleteNode.Name = "btnDeleteNode";
            btnDeleteNode.Size = new Size(94, 29);
            btnDeleteNode.TabIndex = 12;
            btnDeleteNode.Text = "Düğüm Sil";
            btnDeleteNode.UseVisualStyleBackColor = true;
            btnDeleteNode.Click += btnDeleteNode_Click;
            // 
            // btnAddEdge
            // 
            btnAddEdge.Location = new Point(13, 169);
            btnAddEdge.Name = "btnAddEdge";
            btnAddEdge.Size = new Size(94, 29);
            btnAddEdge.TabIndex = 13;
            btnAddEdge.Text = "Bğlnt Ekle";
            btnAddEdge.UseVisualStyleBackColor = true;
            btnAddEdge.Click += btnAddEdge_Click;
            // 
            // btnRemoveEdge
            // 
            btnRemoveEdge.Location = new Point(12, 204);
            btnRemoveEdge.Name = "btnRemoveEdge";
            btnRemoveEdge.Size = new Size(94, 29);
            btnRemoveEdge.TabIndex = 14;
            btnRemoveEdge.Text = "Bğlnt Sil";
            btnRemoveEdge.UseVisualStyleBackColor = true;
            btnRemoveEdge.Click += btnRemoveEdge_Click;
            // 
            // gridSonuclar
            // 
            gridSonuclar.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridSonuclar.Location = new Point(231, 12);
            gridSonuclar.Name = "gridSonuclar";
            gridSonuclar.RowHeadersWidth = 51;
            gridSonuclar.RowTemplate.Height = 29;
            gridSonuclar.Size = new Size(303, 459);
            gridSonuclar.TabIndex = 15;
            // 
            // btnAddNodeManuel
            // 
            btnAddNodeManuel.Location = new Point(13, 274);
            btnAddNodeManuel.Name = "btnAddNodeManuel";
            btnAddNodeManuel.Size = new Size(94, 29);
            btnAddNodeManuel.TabIndex = 16;
            btnAddNodeManuel.Text = "DüğümEkle";
            btnAddNodeManuel.UseVisualStyleBackColor = true;
            btnAddNodeManuel.Click += btnAddNodeManuel_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(101, 331);
            label1.Name = "label1";
            label1.Size = new Size(36, 20);
            label1.TabIndex = 17;
            label1.Text = "İsim";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(87, 364);
            label2.Name = "label2";
            label2.Size = new Size(99, 20);
            label2.TabIndex = 18;
            label2.Text = "Aktiflik (0-10)";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(87, 397);
            label3.Name = "label3";
            label3.Size = new Size(120, 20);
            label3.TabIndex = 19;
            label3.Text = "Etkileşim (0-100)";
            // 
            // btnDFS
            // 
            btnDFS.Location = new Point(113, 82);
            btnDFS.Name = "btnDFS";
            btnDFS.Size = new Size(94, 29);
            btnDFS.TabIndex = 20;
            btnDFS.Text = "DFS Çalıştır";
            btnDFS.UseVisualStyleBackColor = true;
            btnDFS.Click += btnDFS_Click;
            // 
            // btnSaveCSV
            // 
            btnSaveCSV.Location = new Point(113, 274);
            btnSaveCSV.Name = "btnSaveCSV";
            btnSaveCSV.Size = new Size(94, 29);
            btnSaveCSV.TabIndex = 21;
            btnSaveCSV.Text = "GrafKaydet";
            btnSaveCSV.UseVisualStyleBackColor = true;
            btnSaveCSV.Click += btnSaveCSV_Click;
            // 
            // btnAStar
            // 
            btnAStar.Location = new Point(112, 204);
            btnAStar.Name = "btnAStar";
            btnAStar.Size = new Size(94, 29);
            btnAStar.TabIndex = 22;
            btnAStar.Text = "A*Çalıştır";
            btnAStar.UseVisualStyleBackColor = true;
            btnAStar.Click += btnAStar_Click;
            // 
            // btnComponents
            // 
            btnComponents.Location = new Point(113, 239);
            btnComponents.Name = "btnComponents";
            btnComponents.Size = new Size(94, 29);
            btnComponents.TabIndex = 23;
            btnComponents.Text = "Topluluk Analizi";
            btnComponents.UseVisualStyleBackColor = true;
            btnComponents.Click += btnComponents_Click;
            // 
            // btnRandomLarge
            // 
            btnRandomLarge.Location = new Point(13, 47);
            btnRandomLarge.Name = "btnRandomLarge";
            btnRandomLarge.Size = new Size(94, 29);
            btnRandomLarge.TabIndex = 24;
            btnRandomLarge.Text = "randombig";
            btnRandomLarge.UseVisualStyleBackColor = true;
            btnRandomLarge.Click += btnRandomLarge_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1417, 496);
            Controls.Add(btnRandomLarge);
            Controls.Add(btnComponents);
            Controls.Add(btnAStar);
            Controls.Add(btnSaveCSV);
            Controls.Add(btnDFS);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnAddNodeManuel);
            Controls.Add(gridSonuclar);
            Controls.Add(btnRemoveEdge);
            Controls.Add(btnAddEdge);
            Controls.Add(btnDeleteNode);
            Controls.Add(btnUpdateNode);
            Controls.Add(txtEtkilesim);
            Controls.Add(txtAktiflik);
            Controls.Add(txtAd);
            Controls.Add(btnColoring);
            Controls.Add(btnCentrality);
            Controls.Add(btnShortestPath);
            Controls.Add(btnBFS);
            Controls.Add(btnLoadCSV);
            Controls.Add(btnClear);
            Controls.Add(btnRandomGraph);
            Controls.Add(pnlGraph);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)gridSonuclar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlGraph;
        private Button btnRandomGraph;
        private Button btnClear;
        private Button btnLoadCSV;
        private OpenFileDialog openFileDialog1;
        private Button btnBFS;
        private Button btnShortestPath;
        private Button btnCentrality;
        private Button btnColoring;
        private TextBox txtAd;
        private TextBox txtAktiflik;
        private TextBox txtEtkilesim;
        private Button btnUpdateNode;
        private Button btnDeleteNode;
        private Button btnAddEdge;
        private Button btnRemoveEdge;
        private DataGridView gridSonuclar;
        private Button btnAddNodeManuel;
        private Label label1;
        private Label label2;
        private Label label3;
        private Button btnDFS;
        private Button btnSaveCSV;
        private SaveFileDialog saveFileDialog1;
        private Button btnAStar;
        private Button btnComponents;
        private Button btnRandomLarge;
    }
}