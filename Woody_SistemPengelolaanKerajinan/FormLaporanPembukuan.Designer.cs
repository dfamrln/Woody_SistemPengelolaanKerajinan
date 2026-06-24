namespace Woody_SistemPengelolaanKerajinan
{
    partial class FormLaporanPembukuan
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ComboBox cmbBulan;
        private System.Windows.Forms.ComboBox cmbTahun;
        private System.Windows.Forms.Button btnCetak;
        private System.Windows.Forms.Button btnExportPDF;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panelFilter;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.cmbBulan = new System.Windows.Forms.ComboBox();
            this.cmbTahun = new System.Windows.Forms.ComboBox();
            this.btnCetak = new System.Windows.Forms.Button();
            this.btnExportPDF = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panelFilter = new System.Windows.Forms.Panel();
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.panelFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbBulan
            // 
            this.cmbBulan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBulan.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbBulan.Location = new System.Drawing.Point(78, 17);
            this.cmbBulan.Name = "cmbBulan";
            this.cmbBulan.Size = new System.Drawing.Size(120, 36);
            this.cmbBulan.TabIndex = 1;
            // 
            // cmbTahun
            // 
            this.cmbTahun.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTahun.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbTahun.Location = new System.Drawing.Point(278, 17);
            this.cmbTahun.Name = "cmbTahun";
            this.cmbTahun.Size = new System.Drawing.Size(100, 36);
            this.cmbTahun.TabIndex = 3;
            // 
            // btnCetak
            // 
            this.btnCetak.BackColor = System.Drawing.Color.SteelBlue;
            this.btnCetak.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCetak.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCetak.ForeColor = System.Drawing.Color.White;
            this.btnCetak.Location = new System.Drawing.Point(400, 15);
            this.btnCetak.Name = "btnCetak";
            this.btnCetak.Size = new System.Drawing.Size(100, 38);
            this.btnCetak.TabIndex = 4;
            this.btnCetak.Text = "🖨️ Cetak";
            this.btnCetak.UseVisualStyleBackColor = false;
            this.btnCetak.Click += new System.EventHandler(this.btnCetak_Click);
            // 
            // btnExportPDF
            // 
            this.btnExportPDF.BackColor = System.Drawing.Color.ForestGreen;
            this.btnExportPDF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportPDF.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnExportPDF.ForeColor = System.Drawing.Color.White;
            this.btnExportPDF.Location = new System.Drawing.Point(506, 15);
            this.btnExportPDF.Name = "btnExportPDF";
            this.btnExportPDF.Size = new System.Drawing.Size(124, 39);
            this.btnExportPDF.TabIndex = 5;
            this.btnExportPDF.Text = "📎 Export PDF";
            this.btnExportPDF.UseVisualStyleBackColor = false;
            this.btnExportPDF.Click += new System.EventHandler(this.btnExportPDF_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "Bulan:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(220, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 28);
            this.label2.TabIndex = 2;
            this.label2.Text = "Tahun:";
            // 
            // panelFilter
            // 
            this.panelFilter.BackColor = System.Drawing.Color.LightGray;
            this.panelFilter.Controls.Add(this.label1);
            this.panelFilter.Controls.Add(this.label2);
            this.panelFilter.Controls.Add(this.cmbBulan);
            this.panelFilter.Controls.Add(this.cmbTahun);
            this.panelFilter.Controls.Add(this.btnCetak);
            this.panelFilter.Controls.Add(this.btnExportPDF);
            this.panelFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFilter.Location = new System.Drawing.Point(0, 0);
            this.panelFilter.Name = "panelFilter";
            this.panelFilter.Size = new System.Drawing.Size(1000, 60);
            this.panelFilter.TabIndex = 0;
            // 
            // crystalReportViewer1
            // 
            this.crystalReportViewer1.ActiveViewIndex = -1;
            this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer1.Location = new System.Drawing.Point(0, 60);
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.crystalReportViewer1.Size = new System.Drawing.Size(1000, 540);
            this.crystalReportViewer1.TabIndex = 1;
            // 
            // FormLaporanPembukuan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.crystalReportViewer1);
            this.Controls.Add(this.panelFilter);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "FormLaporanPembukuan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "📊 Laporan Pembukuan - Woody Ukiran Kayu";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormLaporanPembukuan_Load);
            this.panelFilter.ResumeLayout(false);
            this.panelFilter.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}