namespace Woody_SistemPengelolaanKerajinan
{
    partial class FormPesanan
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnJumlah = new System.Windows.Forms.Button();
            this.btnTampil = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnLaporan = new System.Windows.Forms.Button();
            this.txtStatusSearch = new System.Windows.Forms.TextBox();
            this.btnInject = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(239, 222);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 28;
            this.dataGridView1.Size = new System.Drawing.Size(848, 328);
            this.dataGridView1.TabIndex = 39;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // btnJumlah
            // 
            this.btnJumlah.Location = new System.Drawing.Point(239, 179);
            this.btnJumlah.Name = "btnJumlah";
            this.btnJumlah.Size = new System.Drawing.Size(142, 40);
            this.btnJumlah.TabIndex = 38;
            this.btnJumlah.Text = "Jumlah Pesanan";
            this.btnJumlah.UseVisualStyleBackColor = true;
            this.btnJumlah.Click += new System.EventHandler(this.btnJumlah_Click);
            // 
            // btnTampil
            // 
            this.btnTampil.Location = new System.Drawing.Point(985, 179);
            this.btnTampil.Name = "btnTampil";
            this.btnTampil.Size = new System.Drawing.Size(102, 40);
            this.btnTampil.TabIndex = 36;
            this.btnTampil.Text = "Tampilkan";
            this.btnTampil.UseVisualStyleBackColor = true;
            this.btnTampil.Click += new System.EventHandler(this.btnTampil_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(383, 191);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(18, 20);
            this.lblTotal.TabIndex = 42;
            this.lblTotal.Text = "0";
            this.lblTotal.Click += new System.EventHandler(this.lblTotal_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(611, 88);
            this.label1.MaximumSize = new System.Drawing.Size(100, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 20);
            this.label1.TabIndex = 100;
            this.label1.Text = "PESANAN";
            this.label1.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // btnLaporan
            // 
            this.btnLaporan.Location = new System.Drawing.Point(877, 179);
            this.btnLaporan.Name = "btnLaporan";
            this.btnLaporan.Size = new System.Drawing.Size(102, 40);
            this.btnLaporan.TabIndex = 101;
            this.btnLaporan.Text = "Laporan";
            this.btnLaporan.UseVisualStyleBackColor = true;
            this.btnLaporan.Click += new System.EventHandler(this.btnLaporan_Click);
            // 
            // txtStatusSearch
            // 
            this.txtStatusSearch.Location = new System.Drawing.Point(566, 179);
            this.txtStatusSearch.Name = "txtStatusSearch";
            this.txtStatusSearch.Size = new System.Drawing.Size(228, 26);
            this.txtStatusSearch.TabIndex = 102;
            // 
            // btnInject
            // 
            this.btnInject.Location = new System.Drawing.Point(985, 99);
            this.btnInject.Name = "btnInject";
            this.btnInject.Size = new System.Drawing.Size(81, 33);
            this.btnInject.TabIndex = 103;
            this.btnInject.Text = "Inject";
            this.btnInject.UseVisualStyleBackColor = true;
            this.btnInject.Click += new System.EventHandler(this.btnInject_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(1097, 101);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(85, 28);
            this.btnReset.TabIndex = 104;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(475, 179);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(85, 28);
            this.btnSearch.TabIndex = 105;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // FormPesanan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1326, 717);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnInject);
            this.Controls.Add(this.txtStatusSearch);
            this.Controls.Add(this.btnLaporan);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnJumlah);
            this.Controls.Add(this.btnTampil);
            this.MinimizeBox = false;
            this.Name = "FormPesanan";
            this.Text = " ";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.Click += new System.EventHandler(this.btnReset_Click);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnJumlah;
        private System.Windows.Forms.Button btnTampil;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnLaporan;
        private System.Windows.Forms.TextBox txtStatusSearch;
        private System.Windows.Forms.Button btnInject;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnSearch;
    }
}
