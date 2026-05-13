# database

# Simulasi SQL Injection pada FormPesanan

## Alur Program

Pada form ini terdapat:

* Kolom input `txtStatusSearch`
* Tombol `Search`
* Tombol `Injection`
* Tombol `Reset`

Program menggunakan query SQL yang dibuat dengan **string concatenation**, sehingga rawan SQL Injection.

---

# 1. Kondisi Normal

User mengetik pada kolom:

```txt id="g9m7tf"
19
```

Lalu menekan tombol **Injection**.

Program akan membuat query:

```sql id="nk8rhb"
UPDATE Pesanan SET status='HACKED' WHERE id_pesanan='19'
```

## Apa yang terjadi?

* Database mencari `id_pesanan = 19`
* Hanya 1 data pesanan yang berubah
* Status berubah menjadi:

```txt id="n0m2my"
HACKED
```

---

# 2. Simulasi SQL Injection

User mengetik pada kolom search:

```txt id="8l2y4v"
19' OR '1'='1
```

Lalu menekan tombol **Injection**.

Program membuat query:

```sql id="55x8od"
UPDATE Pesanan
SET status='HACKED'
WHERE id_pesanan='19' OR '1'='1'
```

---

# Kenapa Bisa Berbahaya?

Bagian:

```sql id="s78r6w"
'1'='1'
```

selalu bernilai TRUE.

Akibatnya:

* Semua baris pada tabel `Pesanan` dianggap cocok
* Semua status pesanan berubah menjadi:

```txt id="goe0ij"
HACKED
```

---

# Apa yang Terjadi di Aplikasi?

Setelah tombol Injection ditekan:

1. Query SQL dikirim ke database
2. Database menjalankan query tersebut
3. Semua data pesanan berubah
4. DataGridView otomatis menampilkan perubahan
5. Muncul pesan:

```txt id="pn8g6o"
X baris terupdate
```

Jumlah baris tergantung banyak data pada tabel.

---

# Fungsi Tombol Reset

Tombol Reset digunakan untuk mengembalikan data seperti semula.

Saat tombol Reset ditekan, query berikut dijalankan:

```sql id="5r1b2q"
UPDATE Pesanan
SET status='Pending'
WHERE status='HACKED'
```

## Apa yang terjadi?

* Semua status yang sebelumnya berubah menjadi `HACKED`
* Dikembalikan lagi menjadi:

```txt id="9k7d2p"
Pending
```

---

# Kesimpulan

Kerentanan terjadi karena query dibuat seperti ini:

```csharp id="uy6d9r"
"... WHERE id_pesanan='" + input + "'"
```

Input user langsung dimasukkan ke query SQL tanpa validasi atau parameterisasi.

Akibatnya user dapat:

* Mengubah banyak data
* Memanipulasi query SQL
* Merusak database

Solusi paling aman adalah menggunakan:

* Parameterized Query
* Validasi input
* Stored Procedure


![Koneksi](SS/1.Koneksi.png)

![Tampilan Produk](SS/2.Tampilan%20Produk.png)

![Insert](SS/3.Insert.png)

![Hasil Insert](SS/4.Hasil%20Insert.png)

![Update](SS/5.Update.png)

![Hasil Update](SS/6.Hasil%20Update.png)

![Delete](SS/7.Delete.png)

![Hasil Delete](SS/8.Hasil%20Delete.png)

![Search](SS/9.Search.png)
