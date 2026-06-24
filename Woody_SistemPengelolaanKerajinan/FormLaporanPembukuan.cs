using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace Woody_SistemPengelolaanKerajinan
{
    public partial class FormLaporanPembukuan : Form
    {
        private readonly string connStr = "Server=P\\SQLLL;Initial Catalog=UkiranKayuDB;User ID=sa;Password=Daffa245206;";

        public FormLaporanPembukuan()
        {
            InitializeComponent();
        }

        // ============================================================
        // LOAD FORM - Isi ComboBox
        // ============================================================
        private void FormLaporanPembukuan_Load(object sender, EventArgs e)
        {
            LoadComboBulanTahun();
        }

        private void LoadComboBulanTahun()
        {
            // Isi combo bulan
            cmbBulan.Items.Add("-- Semua --");
            string[] bulan = { "Januari", "Februari", "Maret", "April", "Mei", "Juni",
                               "Juli", "Agustus", "September", "Oktober", "November", "Desember" };
            cmbBulan.Items.AddRange(bulan);
            cmbBulan.SelectedIndex = 0;

            // Isi combo tahun
            cmbTahun.Items.Add("-- Semua --");
            for (int i = 2024; i <= 2026; i++)
            {
                cmbTahun.Items.Add(i.ToString());
            }
            cmbTahun.SelectedIndex = 0;
        }

        // ============================================================
        // AMBIL DATA DARI DATABASE
        // ============================================================
        private List<DATA> GetDataLaporan(int? bulan, int? tahun)
        {
            List<DATA> listData = new List<DATA>();
            DATA ringkasan = new DATA(); // <-- Tambahkan ini untuk menyimpan ringkasan

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    // ============================================================
                    // 1. Ambil Data Detail Pesanan
                    // ============================================================
                    string queryDetail = @"
                SELECT 
                    p.id_pesanan,
                    p.tanggal_pesanan,
                    FORMAT(p.tanggal_pesanan, 'dd/MM/yyyy') AS tgl_format,
                    p.status,
                    pr.nama_produk,
                    pr.jenis_kayu,
                    pr.harga AS harga_satuan,
                    dp.jumlah,
                    dp.subtotal,
                    p.total_harga
                FROM Pesanan p
                JOIN DetailPesanan dp ON p.id_pesanan = dp.id_pesanan
                JOIN Produk pr ON dp.id_produk = pr.id_produk
                WHERE 
                    (@bulan IS NULL OR MONTH(p.tanggal_pesanan) = @bulan)
                    AND (@tahun IS NULL OR YEAR(p.tanggal_pesanan) = @tahun)
                ORDER BY p.tanggal_pesanan DESC";

                    using (SqlCommand cmd = new SqlCommand(queryDetail, conn))
                    {
                        cmd.Parameters.AddWithValue("@bulan", bulan.HasValue ? (object)bulan.Value : DBNull.Value);
                        cmd.Parameters.AddWithValue("@tahun", tahun.HasValue ? (object)tahun.Value : DBNull.Value);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DATA item = new DATA
                                {
                                    id_pesanan = Convert.ToInt32(reader["id_pesanan"]),
                                    tanggal_pesanan = Convert.ToDateTime(reader["tanggal_pesanan"]),
                                    tgl_format = reader["tgl_format"].ToString(),
                                    status = reader["status"].ToString(),
                                    nama_produk = reader["nama_produk"].ToString(),
                                    jenis_kayu = reader["jenis_kayu"].ToString(),
                                    harga_satuan = Convert.ToInt32(reader["harga_satuan"]),
                                    jumlah = Convert.ToInt32(reader["jumlah"]),
                                    subtotal = Convert.ToInt32(reader["subtotal"]),
                                    total_harga = Convert.ToInt32(reader["total_harga"])
                                };
                                listData.Add(item);
                            }
                        }
                    }

                    // ============================================================
                    // 2. Ambil Data Ringkasan (hanya 1 baris)
                    // ============================================================
                    string queryRingkasan = @"
                SELECT 
                    ISNULL((SELECT SUM(total_harga) FROM Pesanan), 0) AS total_pendapatan,
                    ISNULL((SELECT SUM(harga * stok) FROM Produk), 0) AS total_aset,
                    ISNULL((SELECT COUNT(*) FROM Pesanan), 0) AS total_pesanan,
                    ISNULL((SELECT SUM(jumlah) FROM DetailPesanan), 0) AS total_produk_terjual,
                    ISNULL((SELECT TOP 1 pr.nama_produk 
                            FROM DetailPesanan dp 
                            JOIN Produk pr ON dp.id_produk = pr.id_produk 
                            GROUP BY pr.nama_produk 
                            ORDER BY SUM(dp.jumlah) DESC), '-') AS produk_terlaris,
                    ISNULL((SELECT COUNT(*) FROM Produk WHERE stok <= 5 AND stok > 0), 0) AS stok_menipis,
                    ISNULL((SELECT COUNT(*) FROM Produk WHERE stok = 0), 0) AS stok_habis";

                    using (SqlCommand cmd = new SqlCommand(queryRingkasan, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ringkasan.total_pendapatan = Convert.ToInt64(reader["total_pendapatan"]);
                                ringkasan.total_aset = Convert.ToInt64(reader["total_aset"]);
                                ringkasan.total_pesanan = Convert.ToInt32(reader["total_pesanan"]);
                                ringkasan.total_produk_terjual = Convert.ToInt32(reader["total_produk_terjual"]);
                                ringkasan.produk_terlaris = reader["produk_terlaris"].ToString();
                                ringkasan.stok_menipis = Convert.ToInt32(reader["stok_menipis"]);
                                ringkasan.stok_habis = Convert.ToInt32(reader["stok_habis"]);
                            }
                        }
                    }

                    // ============================================================
                    // 3. Set info cetak & ringkasan ke SEMUA baris
                    // ============================================================
                    string periode = "";
                    if (bulan.HasValue && tahun.HasValue)
                        periode = $"{GetNamaBulan(bulan.Value)} {tahun.Value}";
                    else if (tahun.HasValue)
                        periode = $"Tahun {tahun.Value}";
                    else
                        periode = "Semua Periode";

                    foreach (var item in listData)
                    {
                        item.tanggal_cetak = DateTime.Now;
                        item.user_cetak = Environment.UserName;
                        item.periode = periode;
                        item.bulan = bulan ?? 0;
                        item.tahun = tahun ?? 0;
                        item.nama_bulan = bulan.HasValue ? GetNamaBulan(bulan.Value) : "Semua";

                        // === ISI DATA RINGKASAN KE SEMUA BARIS ===
                        item.total_pendapatan = ringkasan.total_pendapatan;
                        item.total_aset = ringkasan.total_aset;
                        item.total_pesanan = ringkasan.total_pesanan;
                        item.total_produk_terjual = ringkasan.total_produk_terjual;
                        item.produk_terlaris = ringkasan.produk_terlaris;
                        item.stok_menipis = ringkasan.stok_menipis;
                        item.stok_habis = ringkasan.stok_habis;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error ambil data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return listData;
        }

        private string GetNamaBulan(int bulan)
        {
            string[] namaBulan = { "Januari", "Februari", "Maret", "April", "Mei", "Juni",
                                   "Juli", "Agustus", "September", "Oktober", "November", "Desember" };
            return namaBulan[bulan - 1];
        }

        // ============================================================
        // TOMBOL CETAK
        // ============================================================
        private void btnCetak_Click(object sender, EventArgs e)
        {
            try
            {
                int? bulan = null;
                int? tahun = null;

                if (cmbBulan.SelectedIndex > 0)
                    bulan = cmbBulan.SelectedIndex;

                if (cmbTahun.SelectedIndex > 0)
                    tahun = int.Parse(cmbTahun.SelectedItem.ToString());

                List<DATA> dataLaporan = GetDataLaporan(bulan, tahun);

                if (dataLaporan.Count == 0)
                {
                    MessageBox.Show("Tidak ada data untuk periode ini!", "Info",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Cek apakah file report ada
                string reportPath = Application.StartupPath + "\\LaporanPembukuan.rpt";
                if (!System.IO.File.Exists(reportPath))
                {
                    MessageBox.Show($"File report tidak ditemukan!\nPath: {reportPath}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ReportDocument report = new ReportDocument();
                report.Load(reportPath);

                report.SetDataSource(dataLaporan);

                crystalReportViewer1.ReportSource = report;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cetak: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // TOMBOL EXPORT PDF
        // ============================================================
        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "PDF Files|*.pdf";
                sfd.Title = "Export Laporan Pembukuan";
                sfd.FileName = $"Laporan_Pembukuan_{DateTime.Now:yyyyMMdd}";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ReportDocument report = crystalReportViewer1.ReportSource as ReportDocument;
                    if (report != null)
                    {
                        report.ExportToDisk(ExportFormatType.PortableDocFormat, sfd.FileName);
                        MessageBox.Show($"Laporan berhasil diexport ke:\n{sfd.FileName}",
                            "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error export: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}