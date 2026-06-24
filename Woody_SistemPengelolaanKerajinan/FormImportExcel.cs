using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using ExcelDataReader;

namespace Woody_SistemPengelolaanKerajinan
{
    public partial class FormImportExcel : Form
    {
        private readonly string connStr = "Server=P\\SQLLL;Initial Catalog=UkiranKayuDB;User ID=sa;Password=Daffa245206;";
        private SqlConnection conn;
        private DataTable dtExcel = new DataTable();

        public FormImportExcel()
        {
            InitializeComponent();
        }

        private void FormImportExcel_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(connStr);
            btnImport.Enabled = false;
        }

        // ============================================================
        // PILIH FILE EXCEL
        // ============================================================
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Excel Files|*.xlsx;*.xls";
                ofd.Title = "Pilih File Excel";
                ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath.Text = ofd.FileName;
                    LoadExcelData(ofd.FileName);
                }
            }
        }

        // ============================================================
        // LOAD DATA DARI EXCEL
        // ============================================================
        private void LoadExcelData(string filePath)
        {
            try
            {
                // Register encoding provider untuk Excel
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var config = new ExcelDataSetConfiguration()
                        {
                            ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true // Baris pertama sebagai header
                            }
                        };

                        var result = reader.AsDataSet(config);
                        dtExcel = result.Tables[0]; // Ambil sheet pertama

                        dataGridViewPreview.DataSource = dtExcel;
                        lblTotalRows.Text = $"Total data: {dtExcel.Rows.Count} baris";

                        // Validasi kolom yang diperlukan
                        ValidateColumns();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error membaca Excel: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnImport.Enabled = false;
            }
        }

        // ============================================================
        // VALIDASI KOLOM EXCEL
        // ============================================================
        private void ValidateColumns()
        {
            string[] requiredColumns = { "nama_produk", "jenis_kayu", "harga", "stok" };

            foreach (string col in requiredColumns)
            {
                if (!dtExcel.Columns.Contains(col))
                {
                    MessageBox.Show($"Kolom '{col}' tidak ditemukan di Excel!\n" +
                        $"Pastikan header Excel: {string.Join(", ", requiredColumns)}",
                        "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnImport.Enabled = false;
                    return;
                }
            }
            btnImport.Enabled = dtExcel.Rows.Count > 0;
        }

        // ============================================================
        // IMPORT KE DATABASE
        // ============================================================
        private void btnImport_Click(object sender, EventArgs e)
        {
            if (dtExcel.Rows.Count == 0)
            {
                MessageBox.Show("Tidak ada data yang diimport!", "Peringatan",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Yakin ingin mengimport {dtExcel.Rows.Count} data produk?",
                "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No) return;

            int successCount = 0;
            int errorCount = 0;

            try
            {
                conn.Open();

                foreach (DataRow row in dtExcel.Rows)
                {
                    try
                    {
                        string nama = row["nama_produk"].ToString().Trim();
                        string jenis = row["jenis_kayu"].ToString().Trim();
                        int harga = Convert.ToInt32(row["harga"]);
                        int stok = Convert.ToInt32(row["stok"]);

                        // Validasi data
                        if (string.IsNullOrEmpty(nama) || string.IsNullOrEmpty(jenis) || harga < 0 || stok < 0)
                        {
                            errorCount++;
                            continue;
                        }

                        // Cek duplikat
                        SqlCommand cmdCek = new SqlCommand(
                            "SELECT COUNT(*) FROM Produk WHERE nama_produk = @nama AND jenis_kayu = @jenis", conn);
                        cmdCek.Parameters.AddWithValue("@nama", nama);
                        cmdCek.Parameters.AddWithValue("@jenis", jenis);

                        int exists = (int)cmdCek.ExecuteScalar();
                        if (exists > 0)
                        {
                            errorCount++;
                            continue;
                        }

                        // Insert menggunakan Stored Procedure
                        SqlCommand cmd = new SqlCommand("sp_InsertProduk", conn);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@nama", nama);
                        cmd.Parameters.AddWithValue("@jenis", jenis);
                        cmd.Parameters.AddWithValue("@harga", harga);
                        cmd.Parameters.AddWithValue("@stok", stok);

                        SqlParameter paramHasil = new SqlParameter("@hasil", SqlDbType.NVarChar, 200);
                        paramHasil.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(paramHasil);

                        cmd.ExecuteNonQuery();

                        string hasil = paramHasil.Value.ToString();
                        if (hasil.StartsWith("SUKSES"))
                            successCount++;
                        else
                            errorCount++;
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                    }
                }

                MessageBox.Show(
                    $"Import Selesai!\n" +
                    $"✅ Berhasil: {successCount} data\n" +
                    $"❌ Gagal: {errorCount} data",
                    "Hasil Import", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Refresh data di Form1
                RefreshParentForm();

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error import: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        // ============================================================
        // REFRESH FORM UTAMA
        // ============================================================
        private void RefreshParentForm()
        {
            // Cari Form1 yang terbuka dan refresh
            foreach (Form form in Application.OpenForms)
            {
                if (form is Form1 f1)
                {
                    f1.tampilData();
                    break;
                }
            }
        }

        // ============================================================
        // BATAL / TUTUP
        // ============================================================
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}