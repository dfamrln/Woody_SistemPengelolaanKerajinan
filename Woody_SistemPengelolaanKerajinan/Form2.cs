using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Woody_SistemPengelolaanKerajinan
{
    public partial class FormPesanan : System.Windows.Forms.Form
    {
        private readonly string connStr = "Server=P\\SQLLL;Initial Catalog=UkiranKayuDB;User ID=sa;Password=Daffa245206;";

        private SqlConnection conn;
        private BindingSource bs = new BindingSource();

        public FormPesanan()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            BukaKoneksi();
            LoadComboProduk();  
            tampilPesanan();
        }

        private void BukaKoneksi()
        {
            if (conn == null) conn = new SqlConnection(connStr);
            if (conn.State != ConnectionState.Open) conn.Open();
        }

        private void TutupKoneksi()
        {
            if (conn != null && conn.State == ConnectionState.Open) conn.Close();
        }

        // ============================================================
        // LOAD COMBOBOX PRODUK
        // ============================================================
        private void LoadComboProduk()
        {
            try
            {
                BukaKoneksi();
                string query = "SELECT id_produk, nama_produk, harga FROM Produk";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbProduk.DisplayMember = "nama_produk";
                cmbProduk.ValueMember = "id_produk";
                cmbProduk.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error load produk: " + ex.Message);
            }
        }

        // ============================================================
        // TAMPIL DATA - VIEW vw_DetailPesanan + BindingSource
        // ============================================================
        void tampilPesanan()
        {
            try
            {
                BukaKoneksi();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM vw_DetailPesanan", conn))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    bs.DataSource = dt;
                    dataGridView1.DataSource = bs;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTampil_Click(object sender, EventArgs e)
        {
            tampilPesanan();
        }

        // ============================================================
        // TAMBAH PESANAN
        // ============================================================
        private void btnTambahPesanan_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbProduk.SelectedValue == null || string.IsNullOrEmpty(txtJumlah.Text))
                {
                    MessageBox.Show("Pilih produk dan masukkan jumlah!", "Peringatan",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int idProduk = Convert.ToInt32(cmbProduk.SelectedValue);
                int jumlah = Convert.ToInt32(txtJumlah.Text);

                if (jumlah <= 0)
                {
                    MessageBox.Show("Jumlah harus lebih dari 0!", "Peringatan",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Ambil harga
                string queryHarga = "SELECT harga FROM Produk WHERE id_produk = @id";
                SqlCommand cmdHarga = new SqlCommand(queryHarga, conn);
                cmdHarga.Parameters.AddWithValue("@id", idProduk);
                int harga = Convert.ToInt32(cmdHarga.ExecuteScalar());
                int subtotal = harga * jumlah;

                // Insert ke Pesanan
                string queryInsertPesanan = @"
                    INSERT INTO Pesanan (tanggal_pesanan, status, total_harga)
                    VALUES (GETDATE(), 'Pending', 0);
                    SELECT SCOPE_IDENTITY();";

                SqlCommand cmdPesanan = new SqlCommand(queryInsertPesanan, conn);
                int idPesanan = Convert.ToInt32(cmdPesanan.ExecuteScalar());

                // Insert ke DetailPesanan
                string queryDetail = @"
                    INSERT INTO DetailPesanan (id_pesanan, id_produk, jumlah, subtotal)
                    VALUES (@idPesanan, @idProduk, @jumlah, @subtotal)";

                SqlCommand cmdDetail = new SqlCommand(queryDetail, conn);
                cmdDetail.Parameters.AddWithValue("@idPesanan", idPesanan);
                cmdDetail.Parameters.AddWithValue("@idProduk", idProduk);
                cmdDetail.Parameters.AddWithValue("@jumlah", jumlah);
                cmdDetail.Parameters.AddWithValue("@subtotal", subtotal);
                cmdDetail.ExecuteNonQuery();

                // Update total_harga
                string queryUpdateTotal = @"
                    UPDATE Pesanan 
                    SET total_harga = (SELECT SUM(subtotal) FROM DetailPesanan WHERE id_pesanan = @idPesanan)
                    WHERE id_pesanan = @idPesanan";

                SqlCommand cmdUpdate = new SqlCommand(queryUpdateTotal, conn);
                cmdUpdate.Parameters.AddWithValue("@idPesanan", idPesanan);
                cmdUpdate.ExecuteNonQuery();

                MessageBox.Show($"Pesanan berhasil ditambahkan!\nID Pesanan: {idPesanan}\nTotal: Rp {subtotal:N0}",
                    "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                tampilPesanan();
                txtJumlah.Text = "1";
                cmbProduk.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // HAPUS PESANAN
        // ============================================================
        private void btnHapusPesanan_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih pesanan yang ingin dihapus!", "Peringatan",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Yakin ingin menghapus pesanan ini?", "Konfirmasi",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    int idPesanan = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["id_pesanan"].Value);

                    // Hapus detail pesanan
                    string queryDeleteDetail = "DELETE FROM DetailPesanan WHERE id_pesanan = @id";
                    SqlCommand cmdDetail = new SqlCommand(queryDeleteDetail, conn);
                    cmdDetail.Parameters.AddWithValue("@id", idPesanan);
                    cmdDetail.ExecuteNonQuery();

                    // Hapus pesanan
                    string queryDeletePesanan = "DELETE FROM Pesanan WHERE id_pesanan = @id";
                    SqlCommand cmdPesanan = new SqlCommand(queryDeletePesanan, conn);
                    cmdPesanan.Parameters.AddWithValue("@id", idPesanan);
                    cmdPesanan.ExecuteNonQuery();

                    MessageBox.Show("Pesanan berhasil dihapus!", "Sukses",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    tampilPesanan();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ============================================================
        // SEARCH - VULNERABLE (cari berdasarkan nama_produk)
        // ============================================================
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                BukaKoneksi();
                string input = txtStatusSearch.Text;

                // !! VULNERABLE: string concatenation tanpa parameterisasi !!
                string query = "SELECT * FROM vw_DetailPesanan WHERE nama_produk LIKE '%" + input + "%'";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    bs.DataSource = dt;
                    dataGridView1.DataSource = bs;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // TOMBOL INJECT
        // ============================================================
        private void btnInject_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStatusSearch.Text))
            {
                MessageBox.Show("Masukkan ID Pesanan terlebih dahulu.\nContoh: 19",
                    "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                BukaKoneksi();

                string query = "UPDATE Pesanan SET status='HACKED' WHERE id_pesanan='" + txtStatusSearch.Text + "'";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    int result = cmd.ExecuteNonQuery();
                    MessageBox.Show(result + " baris terupdate", "SQL Injection",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                tampilPesanan();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // TOMBOL RESET
        // ============================================================
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                BukaKoneksi();

                string query = "UPDATE Pesanan SET status='Pending' WHERE status='HACKED'";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    int result = cmd.ExecuteNonQuery();
                    MessageBox.Show(result + " baris berhasil direset.", "Reset",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                txtStatusSearch.Text = "";
                tampilPesanan();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // JUMLAH PESANAN
        // ============================================================
        private void btnJumlah_Click(object sender, EventArgs e)
        {
            try
            {
                BukaKoneksi();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Pesanan", conn);
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
        // BUKA LAPORAN
        // ============================================================
        private void btnLaporan_Click(object sender, EventArgs e)
        {
            FromLaporan fl = new FromLaporan();
            fl.Show();
        }

        // ============================================================
        // EVENT KOSONG (wajib ada)
        // ============================================================

        private void lblTotal_Click(object sender, EventArgs e) { }
        private void label1_Click_1(object sender, EventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void FormPesanan_FormClosing(object sender, FormClosingEventArgs e) => TutupKoneksi();
    }
}