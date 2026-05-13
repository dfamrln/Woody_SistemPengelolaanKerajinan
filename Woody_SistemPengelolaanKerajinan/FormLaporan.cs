using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Woody_SistemPengelolaanKerajinan
{
    public partial class FromLaporan : System.Windows.Forms.Form
    {
        // ============================================================
        // KONEKSI & BINDING
        // ============================================================
        private readonly string connStr =
            "Server=P\\SQLEXPRESS;Initial Catalog=UkiranKayuDB;Integrated Security=True;";

        private SqlConnection conn;
        private BindingSource bs = new BindingSource();

        public FromLaporan()
        {
            InitializeComponent();
        }

        private void FormLaporan_Load(object sender, EventArgs e)
        {
            BukaKoneksi();
            tampilLaporan();
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
        // TAMPIL LAPORAN
        // Ringkasan: menggunakan VIEW vw_RingkasanLaporan (1 query)
        // Detail   : menggunakan VIEW vw_DetailPesanan
        // ============================================================
        void tampilLaporan()
        {
            try
            {
                BukaKoneksi();

                // --- Ringkasan: ambil dari VIEW vw_RingkasanLaporan (1 query, efisien) ---
                using (SqlCommand cmdRingkasan = new SqlCommand("SELECT * FROM vw_RingkasanLaporan", conn))
                using (SqlDataReader reader = cmdRingkasan.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        long totalPendapatan = reader["total_pendapatan"] == DBNull.Value
                            ? 0 : Convert.ToInt64(reader["total_pendapatan"]);

                        lblTotalPendapatan.Text = "Total Pendapatan: Rp " +
                            totalPendapatan.ToString("N0");
                        lblPending.Text = "Pending: " + reader["jumlah_pending"].ToString();
                        lblSelesai.Text = "Selesai: " + reader["jumlah_selesai"].ToString();
                        lblDiproses.Text = "Diproses: " + reader["jumlah_diproses"].ToString();
                    }
                }

                // --- Detail pesanan: menggunakan VIEW vw_DetailPesanan + BindingSource ---
                using (SqlCommand cmdDetail = new SqlCommand("SELECT * FROM vw_DetailPesanan", conn))
                using (SqlDataAdapter da = new SqlDataAdapter(cmdDetail))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    bs.DataSource = dt;
                    dataGridView1.DataSource = bs;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat laporan: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FromLaporan_FormClosing(object sender, FormClosingEventArgs e) => TutupKoneksi();
    }
}
