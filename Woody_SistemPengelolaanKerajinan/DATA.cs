using System;

namespace Woody_SistemPengelolaanKerajinan
{
    public class DATA
    {
        // ============================================================
        // DATA PESANAN
        // ============================================================
        public int id_pesanan { get; set; }
        public DateTime tanggal_pesanan { get; set; }
        public string tgl_format { get; set; }
        public string status { get; set; }

        // ============================================================
        // DATA PRODUK
        // ============================================================
        public string nama_produk { get; set; }
        public string jenis_kayu { get; set; }
        public int harga_satuan { get; set; }

        // ============================================================
        // DETAIL PESANAN
        // ============================================================
        public int jumlah { get; set; }
        public long subtotal { get; set; }
        public long total_harga { get; set; }

        // ============================================================
        // RINGKASAN PEMBUKUAN
        // ============================================================
        public long total_pendapatan { get; set; }
        public long total_aset { get; set; }
        public int total_pesanan { get; set; }
        public int total_produk_terjual { get; set; }
        public string produk_terlaris { get; set; }
        public int stok_menipis { get; set; }
        public int stok_habis { get; set; }

        // ============================================================
        // INFO CETAK
        // ============================================================
        public DateTime tanggal_cetak { get; set; }
        public string user_cetak { get; set; }
        public string periode { get; set; }

        // ============================================================
        // FILTER
        // ============================================================
        public int bulan { get; set; }
        public int tahun { get; set; }
        public string nama_bulan { get; set; }

        // ============================================================
        // PROPERTI FORMAT (untuk Crystal Report)
        // ============================================================
        public string total_pendapatan_format
        {
            get { return string.Format("Rp {0:N0}", total_pendapatan); }
        }

        public string total_aset_format
        {
            get { return string.Format("Rp {0:N0}", total_aset); }
        }

        public string subtotal_format
        {
            get { return string.Format("Rp {0:N0}", subtotal); }
        }

        public string harga_satuan_format
        {
            get { return string.Format("Rp {0:N0}", harga_satuan); }
        }
    }
}