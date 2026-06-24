using System;

namespace Woody_SistemPengelolaanKerajinan
{
    partial class FormImportExcel
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridView dataGridViewPreview;
        private System.Windows.Forms.Label lblTotalRows;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.dataGridViewPreview = new System.Windows.Forms.DataGridView();
            this.lblTotalRows = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPreview)).BeginInit();
            this.SuspendLayout();

            // ============================================================
            // txtFilePath
            // ============================================================
            this.txtFilePath.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtFilePath.Location = new System.Drawing.Point(20, 20);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(500, 26);
            this.txtFilePath.TabIndex = 0;

            // ============================================================
            // btnBrowse
            // ============================================================
            this.btnBrowse.BackColor = System.Drawing.Color.LightBlue;
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowse.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnBrowse.Location = new System.Drawing.Point(530, 18);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(150, 30);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "📂 Pilih File Excel";
            this.btnBrowse.UseVisualStyleBackColor = false;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);

            // ============================================================
            // dataGridViewPreview
            // ============================================================
            this.dataGridViewPreview.ColumnHeadersHeightSizeMode =
                System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPreview.Location = new System.Drawing.Point(20, 70);
            this.dataGridViewPreview.Name = "dataGridViewPreview";
            this.dataGridViewPreview.RowHeadersWidth = 62;
            this.dataGridViewPreview.RowTemplate.Height = 28;
            this.dataGridViewPreview.Size = new System.Drawing.Size(760, 300);
            this.dataGridViewPreview.TabIndex = 2;

            // ============================================================
            // lblTotalRows
            // ============================================================
            this.lblTotalRows.AutoSize = true;
            this.lblTotalRows.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalRows.Location = new System.Drawing.Point(20, 390);
            this.lblTotalRows.Name = "lblTotalRows";
            this.lblTotalRows.Size = new System.Drawing.Size(102, 20);
            this.lblTotalRows.TabIndex = 3;
            this.lblTotalRows.Text = "Total Baris: 0";

            // ============================================================
            // btnImport
            // ============================================================
            this.btnImport.BackColor = System.Drawing.Color.ForestGreen;
            this.btnImport.Enabled = false;
            this.btnImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImport.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnImport.ForeColor = System.Drawing.Color.White;
            this.btnImport.Location = new System.Drawing.Point(520, 385);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(120, 35);
            this.btnImport.TabIndex = 4;
            this.btnImport.Text = "📥 Import";
            this.btnImport.UseVisualStyleBackColor = false;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);

            // ============================================================
            // btnCancel
            // ============================================================
            this.btnCancel.BackColor = System.Drawing.Color.IndianRed;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(660, 385);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 35);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "❌ Batal";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // ============================================================
            // openFileDialog1
            // ============================================================
            this.openFileDialog1.Filter = "Excel Files|*.xlsx;*.xls";
            this.openFileDialog1.Title = "Pilih File Excel";
            this.openFileDialog1.InitialDirectory =
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // ============================================================
            // FormImportExcel
            // ============================================================
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 440);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.dataGridViewPreview);
            this.Controls.Add(this.lblTotalRows);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnCancel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "FormImportExcel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "📊 Import Data Excel - Woody Ukiran Kayu";
            this.Load += new System.EventHandler(this.FormImportExcel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}