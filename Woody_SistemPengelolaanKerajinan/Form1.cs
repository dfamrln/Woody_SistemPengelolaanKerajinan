using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Woody_SistemPengelolaanKerajinan
{
    public partial class Form1 : Form
    {

        SqlConnection conn = new SqlConnection(
        "Data Source=P\\SQLEXPRESS;Initial Catalog=UkiranKayuDB;Integrated Security=True");

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        // ======================
        // KONEKSI DATABASE
        // ======================

        private void btnKoneksi_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                MessageBox.Show("Koneksi Berhasil");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Koneksi Gagal: " + ex.Message);
            }
        }

        // ======================
        // INSERT DATA
        // ======================

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (txtNama.Text == "" || txtJenis.Text == "" || txtHarga.Text == "" || txtStok.Text == "")
            {
                MessageBox.Show("Semua field harus diisi!");
                return;
            }

            string query = "INSERT INTO Produk (nama_produk, jenis_kayu, harga, stok) VALUES (@nama,@jenis,@harga,@stok)";
            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@nama", txtNama.Text);
            cmd.Parameters.AddWithValue("@jenis", txtJenis.Text);
            cmd.Parameters.AddWithValue("@harga", txtHarga.Text);
            cmd.Parameters.AddWithValue("@stok", txtStok.Text);

            cmd.ExecuteNonQuery();

            MessageBox.Show("Data berhasil ditambahkan");

            tampilData();
        }

        // ======================
        // UPDATE DATA
        // ======================

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Yakin ingin update data?", "Konfirmasi", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                string query = "UPDATE Produk SET nama_produk=@nama, jenis_kayu=@jenis, harga=@harga, stok=@stok WHERE id_produk=@id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", txtID.Text);
                cmd.Parameters.AddWithValue("@nama", txtNama.Text);
                cmd.Parameters.AddWithValue("@jenis", txtJenis.Text);
                cmd.Parameters.AddWithValue("@harga", txtHarga.Text);
                cmd.Parameters.AddWithValue("@stok", txtStok.Text);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Data berhasil diupdate");

                tampilData();
            }
        }

        // ======================
        // DELETE DATA
        // ======================

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Yakin ingin hapus data?", "Konfirmasi", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                string query = "DELETE FROM Produk WHERE id_produk=@id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", txtID.Text);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Data berhasil dihapus");

                tampilData();
            }
        }

        // ======================
        // TAMPIL DATA
        // ======================

        private void btnTampil_Click(object sender, EventArgs e)
        {
            tampilData();
        }

        void tampilData()
        {
            string query = "SELECT * FROM Produk";

            SqlCommand cmd = new SqlCommand(query, conn);

            SqlDataReader reader = cmd.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(reader);

            dataGridView1.DataSource = dt;

            reader.Close();
        }

        // ======================
        // SEARCH DATA
        // ======================

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM Produk WHERE nama_produk LIKE '%'+@nama+'%'";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@nama", txtSearch.Text);

            SqlDataReader reader = cmd.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(reader);

            dataGridView1.DataSource = dt;

            reader.Close();
        }

        // ======================
        // EXECUTE SCALAR
        // ======================

        private void btnJumlah_Click(object sender, EventArgs e)
        {
            string query = "SELECT COUNT(*) FROM Produk";

            SqlCommand cmd = new SqlCommand(query, conn);

            int total = (int)cmd.ExecuteScalar();

            lblTotal.Text = total.ToString();
        }

        // ======================
        // KLIK DATAGRID
        // ======================

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            txtNama.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            txtJenis.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            txtHarga.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            txtStok.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void lblTotal_Click(object sender, EventArgs e)
        {

        }
    }
}
