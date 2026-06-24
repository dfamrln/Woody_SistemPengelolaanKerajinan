using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Woody_SistemPengelolaanKerajinan
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        // ============================================================
        // KONEKSI & BINDING
        // ============================================================
        private readonly string connStr = "Server=P\\SQLLL;Initial Catalog=UkiranKayuDB;User ID=sa;Password=Daffa245206;";

        private SqlConnection conn;
        private BindingSource bs = new BindingSource();
        private DataTable dt = new DataTable();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BukaKoneksi();
            tampilData();
        }

        // ============================================================
        // HELPER: BUKA / TUTUP KONEKSI
        // ============================================================
        private void BukaKoneksi()
        {
            if (conn == null)
                conn = new SqlConnection(connStr);

            if (conn.State != ConnectionState.Open)
                conn.Open();
        }

        private void TutupKoneksi()
        {
            if (conn != null && conn.State == ConnectionState.Open)
                conn.Close();
        }

        // ============================================================
        // KONEKSI (tombol manual, opsional)
        // ============================================================
        private void btnKoneksi_Click(object sender, EventArgs e)
        {
            try
            {
                BukaKoneksi();
                MessageBox.Show("Koneksi Berhasil!", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Koneksi Gagal: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // INSERT - memanggil STORED PROCEDURE sp_InsertProduk
        // ============================================================
        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (txtNama.Text == "" || txtJenis.Text == "" ||
                txtHarga.Text == "" || txtStok.Text == "")
            {
                MessageBox.Show("Semua field harus diisi!", "Peringatan",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                BukaKoneksi();

                using (SqlCommand cmd = new SqlCommand("sp_InsertProduk", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@nama", txtNama.Text.Trim());
                    cmd.Parameters.AddWithValue("@jenis", txtJenis.Text.Trim());
                    cmd.Parameters.AddWithValue("@harga", int.Parse(txtHarga.Text));
                    cmd.Parameters.AddWithValue("@stok", int.Parse(txtStok.Text));

                    // OUTPUT parameter untuk pesan dari SP
                    SqlParameter paramHasil = new SqlParameter("@hasil", SqlDbType.NVarChar, 200);
                    paramHasil.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(paramHasil);

                    cmd.ExecuteNonQuery();

                    string hasil = paramHasil.Value.ToString();
                    if (hasil.StartsWith("SUKSES"))
                    {
                        MessageBox.Show(hasil, "Berhasil",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        BersihkanField();
                        tampilData();
                    }
                    else
                    {
                        MessageBox.Show(hasil, "Gagal",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // UPDATE - memanggil STORED PROCEDURE sp_UpdateProduk
        // ============================================================
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "")
            {
                MessageBox.Show("Pilih data yang ingin diupdate!", "Peringatan",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Yakin ingin update data ini?", "Konfirmasi",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    BukaKoneksi();

                    using (SqlCommand cmd = new SqlCommand("sp_UpdateProduk", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@id", int.Parse(txtID.Text));
                        cmd.Parameters.AddWithValue("@nama", txtNama.Text.Trim());
                        cmd.Parameters.AddWithValue("@jenis", txtJenis.Text.Trim());
                        cmd.Parameters.AddWithValue("@harga", int.Parse(txtHarga.Text));
                        cmd.Parameters.AddWithValue("@stok", int.Parse(txtStok.Text));

                        SqlParameter paramHasil = new SqlParameter("@hasil", SqlDbType.NVarChar, 200);
                        paramHasil.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(paramHasil);

                        cmd.ExecuteNonQuery();

                        string hasil = paramHasil.Value.ToString();
                        if (hasil.StartsWith("SUKSES"))
                        {
                            MessageBox.Show(hasil, "Berhasil",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            BersihkanField();
                            tampilData();
                        }
                        else
                        {
                            MessageBox.Show(hasil, "Gagal",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ============================================================
        // DELETE - memanggil STORED PROCEDURE sp_DeleteProduk
        // ============================================================
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "")
            {
                MessageBox.Show("Pilih data yang ingin dihapus!", "Peringatan",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Yakin ingin hapus data ini?", "Konfirmasi",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    BukaKoneksi();

                    using (SqlCommand cmd = new SqlCommand("sp_DeleteProduk", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@id", int.Parse(txtID.Text));

                        SqlParameter paramHasil = new SqlParameter("@hasil", SqlDbType.NVarChar, 200);
                        paramHasil.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(paramHasil);

                        cmd.ExecuteNonQuery();

                        string hasil = paramHasil.Value.ToString();
                        if (hasil.StartsWith("SUKSES"))
                        {
                            MessageBox.Show(hasil, "Berhasil",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            BersihkanField();
                            tampilData();
                        }
                        else
                        {
                            MessageBox.Show(hasil, "Gagal",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ============================================================
        // TAMPIL DATA - menggunakan VIEW vw_Produk + BindingSource
        // ============================================================
        private void btnTampil_Click(object sender, EventArgs e)
        {
            tampilData();
        }

        public void tampilData()
        {
            try
            {
                BukaKoneksi();

                // Menggunakan VIEW sebagai pengganti SELECT * FROM Produk
                string query = "SELECT * FROM vw_Produk";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    dt.Clear();
                    da.Fill(dt);
                }

                // BindingSource sebagai jembatan data ke DataGridView
                bs.DataSource = dt;
                dataGridView1.DataSource = bs;
                bindingNavigator1.BindingSource = bs;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // SEARCH - memanggil STORED PROCEDURE sp_SearchProduk
        // ============================================================
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                BukaKoneksi();

                using (SqlCommand cmd = new SqlCommand("sp_SearchProduk", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Kirim null jika kosong agar SP mengembalikan semua data
                    cmd.Parameters.AddWithValue("@nama",
                        string.IsNullOrWhiteSpace(txtSearch.Text) ? (object)DBNull.Value : txtSearch.Text.Trim());
                    cmd.Parameters.AddWithValue("@jenis", DBNull.Value);
                    cmd.Parameters.AddWithValue("@harga_min", DBNull.Value);
                    cmd.Parameters.AddWithValue("@harga_max", DBNull.Value);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dtSearch = new DataTable();
                        da.Fill(dtSearch);

                        // Tetap gunakan BindingSource agar navigator sinkron
                        bs.DataSource = dtSearch;
                        dataGridView1.DataSource = bs;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error search: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // EXECUTE SCALAR - COUNT produk
        // ============================================================
        private void btnJumlah_Click(object sender, EventArgs e)
        {
            try
            {
                BukaKoneksi();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Produk", conn);
                int total = (int)cmd.ExecuteScalar();
                lblTotal.Text = total.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // KLIK DATAGRIDVIEW - isi field dari baris yang dipilih
        // ============================================================
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtID.Text = row.Cells["id_produk"].Value?.ToString() ?? "";
                txtNama.Text = row.Cells["nama_produk"].Value?.ToString() ?? "";
                txtJenis.Text = row.Cells["jenis_kayu"].Value?.ToString() ?? "";
                txtHarga.Text = row.Cells["harga"].Value?.ToString() ?? "";
                txtStok.Text = row.Cells["stok"].Value?.ToString() ?? "";
            }
            catch { /* abaikan jika kolom tidak ditemukan */ }
        }

        // ============================================================
        // BINDING NAVIGATOR - sinkronisasi field dengan navigasi
        // ============================================================
        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {
            if (bs.Current == null) return;

            try
            {
                DataRowView row = (DataRowView)bs.Current;
                txtID.Text = row["id_produk"].ToString();
                txtNama.Text = row["nama_produk"].ToString();
                txtJenis.Text = row["jenis_kayu"].ToString();
                txtHarga.Text = row["harga"].ToString();
                txtStok.Text = row["stok"].ToString();
            }
            catch { /* abaikan jika kolom tidak tersedia */ }
        }

        // ============================================================
        // NAVIGASI KE FORM PESANAN
        // ============================================================
        private void btnPesanan_Click(object sender, EventArgs e)
        {
            FormPesanan fp = new FormPesanan();
            fp.Show();
        }

        // ============================================================
        // HELPER: BERSIHKAN FIELD INPUT
        // ============================================================
        private void BersihkanField()
        {
            txtID.Text = "";
            txtNama.Text = "";
            txtJenis.Text = "";
            txtHarga.Text = "";
            txtStok.Text = "";
        }

        // ============================================================
        // VALIDASI INPUT - hanya huruf & spasi
        // ============================================================
        private void txtNama_TextChanged(object sender, EventArgs e)
        {
            ValidasiHuruf(txtNama);
        }

        private void txtJenis_TextChanged(object sender, EventArgs e)
        {
            ValidasiHuruf(txtJenis);
        }

        private void txtHarga_TextChanged(object sender, EventArgs e)
        {
            ValidasiAngka(txtHarga);
        }

        private void txtStok_TextChanged(object sender, EventArgs e)
        {
            ValidasiAngka(txtStok);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string value = txtSearch.Text;
            foreach (char c in value)
            {
                if (!char.IsLetterOrDigit(c) && c != ' ')
                {
                    txtSearch.Text = value.Replace(c.ToString(), "");
                    txtSearch.SelectionStart = txtSearch.Text.Length;
                    break;
                }
            }
        }

        private void ValidasiHuruf(TextBox tb)
        {
            string value = tb.Text;
            foreach (char c in value)
            {
                if (!char.IsLetter(c) && c != ' ')
                {
                    tb.Text = value.Replace(c.ToString(), "");
                    tb.SelectionStart = tb.Text.Length;
                    break;
                }
            }
        }

        private void ValidasiAngka(TextBox tb)
        {
            string value = tb.Text;
            foreach (char c in value)
            {
                if (!char.IsDigit(c))
                {
                    tb.Text = value.Replace(c.ToString(), "");
                    tb.SelectionStart = tb.Text.Length;
                    break;
                }
            }
        }

        // ============================================================
        // EVENT KOSONG (diperlukan designer)
        // ============================================================
        private void label5_Click(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
        private void lblTotal_Click(object sender, EventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e) => TutupKoneksi();

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            FormImportExcel fie = new FormImportExcel();
            fie.ShowDialog();
        }
    }
}
