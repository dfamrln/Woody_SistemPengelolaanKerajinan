using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Woody_SistemPengelolaanKerajinan
{
    public partial class FormPesanan : System.Windows.Forms.Form
    {
        private readonly string connStr =
            "Server=P\\SQLEXPRESS;Initial Catalog=UkiranKayuDB;Integrated Security=True;";

        private SqlConnection conn;
        private BindingSource bs = new BindingSource();

        public FormPesanan()
        {
            InitializeComponent();
        }

        private void FormPesanan_Load(object sender, EventArgs e)
        {
            BukaKoneksi();
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
        // TAMPIL DATA NORMAL - VIEW vw_DetailPesanan + BindingSource
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
        // UPDATE langsung ke database - tanggal_pesanan diubah jadi 'HACKED'
        // menggunakan string concatenation (vulnerable)
        // ============================================================
        private void btnInject_Click(object sender, EventArgs e)
        {
            // Cek field kosong
            if (string.IsNullOrWhiteSpace(txtStatusSearch.Text))
            {
                MessageBox.Show("Masukkan ID Pesanan terlebih dahulu.\nContoh: 19",
                    "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                BukaKoneksi();

                // !! VULNERABLE: string concatenation tanpa parameterisasi !!
                // Sama persis seperti contoh Alvin - UPDATE langsung ke DB
                string query =
                    "UPDATE Pesanan SET status='HACKED' WHERE id_pesanan='" +
                    txtStatusSearch.Text + "'";

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
        // TOMBOL RESET - kembalikan status pesanan ke semula
        // ============================================================
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                BukaKoneksi();

                // Kembalikan status yang ter-inject ke 'Pending'
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

        private void btnLaporan_Click(object sender, EventArgs e)
        {
            FromLaporan fl = new FromLaporan();
            fl.Show();
        }

        private void Form2_Load(object sender, EventArgs e) { }
        private void lblTotal_Click(object sender, EventArgs e) { }
        private void label1_Click_1(object sender, EventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void FormPesanan_FormClosing(object sender, FormClosingEventArgs e) => TutupKoneksi();
    }
}
