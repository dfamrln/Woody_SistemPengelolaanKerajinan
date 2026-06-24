namespace Woody_SistemPengelolaanKerajinan
{
    partial class FromLaporan
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblTotalPendapatan = new System.Windows.Forms.Label();
            this.lblPending = new System.Windows.Forms.Label();
            this.lblSelesai = new System.Windows.Forms.Label();
            this.lblDiproses = new System.Windows.Forms.Label();
            this.btnLaporanPembukuan = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(43, 240);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 28;
            this.dataGridView1.Size = new System.Drawing.Size(1046, 356);
            this.dataGridView1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Bookman Old Style", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(428, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(299, 65);
            this.label1.TabIndex = 1;
            this.label1.Text = "LAPORAN";
            // 
            // lblTotalPendapatan
            // 
            this.lblTotalPendapatan.AutoSize = true;
            this.lblTotalPendapatan.Location = new System.Drawing.Point(51, 204);
            this.lblTotalPendapatan.Name = "lblTotalPendapatan";
            this.lblTotalPendapatan.Size = new System.Drawing.Size(135, 20);
            this.lblTotalPendapatan.TabIndex = 2;
            this.lblTotalPendapatan.Text = "Total Pendapatan";
            this.lblTotalPendapatan.Click += new System.EventHandler(this.FormLaporan_Load);
            // 
            // lblPending
            // 
            this.lblPending.AutoSize = true;
            this.lblPending.Location = new System.Drawing.Point(457, 204);
            this.lblPending.Name = "lblPending";
            this.lblPending.Size = new System.Drawing.Size(67, 20);
            this.lblPending.TabIndex = 3;
            this.lblPending.Text = "Pending";
            // 
            // lblSelesai
            // 
            this.lblSelesai.AutoSize = true;
            this.lblSelesai.Location = new System.Drawing.Point(694, 204);
            this.lblSelesai.Name = "lblSelesai";
            this.lblSelesai.Size = new System.Drawing.Size(61, 20);
            this.lblSelesai.TabIndex = 4;
            this.lblSelesai.Text = "Selesai";
            // 
            // lblDiproses
            // 
            this.lblDiproses.AutoSize = true;
            this.lblDiproses.Location = new System.Drawing.Point(903, 204);
            this.lblDiproses.Name = "lblDiproses";
            this.lblDiproses.Size = new System.Drawing.Size(72, 20);
            this.lblDiproses.TabIndex = 5;
            this.lblDiproses.Text = "Diproses";
            // 
            // btnLaporanPembukuan
            // 
            this.btnLaporanPembukuan.Location = new System.Drawing.Point(887, 108);
            this.btnLaporanPembukuan.Name = "btnLaporanPembukuan";
            this.btnLaporanPembukuan.Size = new System.Drawing.Size(168, 43);
            this.btnLaporanPembukuan.TabIndex = 6;
            this.btnLaporanPembukuan.Text = "LaporanPembukuan";
            this.btnLaporanPembukuan.UseVisualStyleBackColor = true;
            this.btnLaporanPembukuan.Click += new System.EventHandler(this.btnLaporanPembukuan_Click);
            // 
            // FromLaporan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1119, 629);
            this.Controls.Add(this.btnLaporanPembukuan);
            this.Controls.Add(this.lblDiproses);
            this.Controls.Add(this.lblSelesai);
            this.Controls.Add(this.lblPending);
            this.Controls.Add(this.lblTotalPendapatan);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "FromLaporan";
            this.Text = "Form Laporan";
            this.Load += new System.EventHandler(this.FormLaporan_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTotalPendapatan;
        private System.Windows.Forms.Label lblPending;
        private System.Windows.Forms.Label lblSelesai;
        private System.Windows.Forms.Label lblDiproses;
        private System.Windows.Forms.Button btnLaporanPembukuan;
    }
}
