CREATE DATABASE UkiranKayuDB;
GO

USE UkiranKayuDB;
GO

CREATE TABLE Produk (
    id_produk INT IDENTITY(1,1) PRIMARY KEY,
    nama_produk VARCHAR(100) NOT NULL,
    jenis_kayu VARCHAR(50) NOT NULL,
    harga INT NOT NULL,
    stok INT NOT NULL
);

CREATE TABLE Pesanan (
    id_pesanan INT IDENTITY(1,1) PRIMARY KEY,
    tanggal_pesanan DATE NOT NULL,
    id_produk INT NOT NULL,
    jumlah INT NOT NULL,
    total_harga INT NOT NULL,
    
    FOREIGN KEY (id_produk) REFERENCES Produk(id_produk)
);

INSERT INTO Produk (nama_produk, jenis_kayu, harga, stok)
VALUES 
('Meja Ukir Jepara','Jati',1500000,10),
('Kursi Ukir','Mahoni',800000,20),
('Lemari Ukir','Jati',2500000,5);

-- Insert data contoh ke tabel Pesanan
INSERT INTO Pesanan (tanggal_pesanan, status, total_harga)
VALUES 
('2026-01-15', 'Pending', 1500000),
('2026-01-16', 'Selesai', 800000),
('2026-01-17', 'Diproses', 2500000);

-- Insert data contoh ke tabel DetailPesanan
INSERT INTO DetailPesanan (id_pesanan, id_produk, jumlah, subtotal)
VALUES
(19, 14, 1, 1500000),
(20, 15, 1, 800000),
(21, 16, 1, 2500000);



-- ============================================================
-- 1. VIEW - Menggantikan SELECT langsung di semua Form
-- ============================================================

-- VIEW untuk Form1 (Produk)
CREATE OR ALTER VIEW vw_Produk AS
SELECT 
    id_produk,
    nama_produk,
    jenis_kayu,
    harga,
    stok,
    CASE 
        WHEN stok = 0 THEN 'Habis'
        WHEN stok <= 5 THEN 'Menipis'
        ELSE 'Tersedia'
    END AS status_stok,
    harga * stok AS nilai_inventaris
FROM Produk;
GO

-- VIEW untuk FormPesanan dan FormLaporan (JOIN 3 tabel)
CREATE OR ALTER VIEW vw_DetailPesanan AS
SELECT 
    p.id_pesanan,
    p.tanggal_pesanan,
    pr.nama_produk,
    pr.jenis_kayu,
    dp.jumlah,
    dp.subtotal,
    p.status,
    p.total_harga,
    DATEDIFF(DAY, p.tanggal_pesanan, GETDATE()) AS hari_sejak_pesanan
FROM Pesanan p
JOIN DetailPesanan dp ON p.id_pesanan = dp.id_pesanan
JOIN Produk pr ON dp.id_produk = pr.id_produk;
GO

-- VIEW untuk Laporan Ringkasan
CREATE OR ALTER VIEW vw_RingkasanLaporan AS
SELECT
    COUNT(*) AS total_pesanan,
    SUM(total_harga) AS total_pendapatan,
    SUM(CASE WHEN status = 'Pending'  THEN 1 ELSE 0 END) AS jumlah_pending,
    SUM(CASE WHEN status = 'Selesai'  THEN 1 ELSE 0 END) AS jumlah_selesai,
    SUM(CASE WHEN status = 'Diproses' THEN 1 ELSE 0 END) AS jumlah_diproses
FROM Pesanan;
GO

-- ============================================================
-- 2. STORED PROCEDURE - INSERT
-- Tambahan: cek stok duplikat nama produk sebelum insert
-- ============================================================

CREATE OR ALTER PROCEDURE sp_InsertProduk
    @nama     VARCHAR(100),
    @jenis    VARCHAR(50),
    @harga    INT,
    @stok     INT,
    @hasil    NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Validasi: harga dan stok tidak boleh negatif
    IF @harga < 0 OR @stok < 0
    BEGIN
        SET @hasil = 'GAGAL: Harga dan stok tidak boleh negatif.';
        RETURN;
    END

    -- Validasi: nama produk tidak boleh duplikat
    IF EXISTS (SELECT 1 FROM Produk WHERE nama_produk = @nama AND jenis_kayu = @jenis)
    BEGIN
        SET @hasil = 'GAGAL: Produk dengan nama dan jenis kayu yang sama sudah ada.';
        RETURN;
    END

    INSERT INTO Produk (nama_produk, jenis_kayu, harga, stok)
    VALUES (@nama, @jenis, @harga, @stok);

    SET @hasil = 'SUKSES: Data produk berhasil ditambahkan. ID baru: ' + CAST(SCOPE_IDENTITY() AS VARCHAR);
END;
GO

-- ============================================================
-- 3. STORED PROCEDURE - UPDATE
-- Tambahan: catat log perubahan harga jika ada perubahan
-- ============================================================

-- Tabel log (dibuat dulu jika belum ada)
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='LogPerubahanHarga' AND xtype='U')
CREATE TABLE LogPerubahanHarga (
    id_log        INT IDENTITY(1,1) PRIMARY KEY,
    id_produk     INT,
    harga_lama    INT,
    harga_baru    INT,
    waktu_ubah    DATETIME DEFAULT GETDATE()
);
GO

CREATE OR ALTER PROCEDURE sp_UpdateProduk
    @id       INT,
    @nama     VARCHAR(100),
    @jenis    VARCHAR(50),
    @harga    INT,
    @stok     INT,
    @hasil    NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Cek apakah produk dengan ID tsb ada
    IF NOT EXISTS (SELECT 1 FROM Produk WHERE id_produk = @id)
    BEGIN
        SET @hasil = 'GAGAL: Produk dengan ID tersebut tidak ditemukan.';
        RETURN;
    END

    -- Validasi: harga dan stok tidak boleh negatif
    IF @harga < 0 OR @stok < 0
    BEGIN
        SET @hasil = 'GAGAL: Harga dan stok tidak boleh negatif.';
        RETURN;
    END

    -- Catat log jika harga berubah
    DECLARE @harga_lama INT;
    SELECT @harga_lama = harga FROM Produk WHERE id_produk = @id;

    IF @harga_lama <> @harga
    BEGIN
        INSERT INTO LogPerubahanHarga (id_produk, harga_lama, harga_baru)
        VALUES (@id, @harga_lama, @harga);
    END

    UPDATE Produk
    SET nama_produk = @nama,
        jenis_kayu  = @jenis,
        harga       = @harga,
        stok        = @stok
    WHERE id_produk = @id;

    SET @hasil = 'SUKSES: Data produk berhasil diperbarui.';
END;
GO

-- ============================================================
-- 4. STORED PROCEDURE - DELETE
-- Tambahan: cek apakah produk masih dipakai di DetailPesanan
-- ============================================================

CREATE OR ALTER PROCEDURE sp_DeleteProduk
    @id    INT,
    @hasil NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Cek apakah produk masih ada di pesanan aktif
    IF EXISTS (
        SELECT 1
        FROM DetailPesanan dp
        JOIN Pesanan p ON dp.id_pesanan = p.id_pesanan
        WHERE dp.id_produk = @id AND p.status <> 'Selesai'
    )
    BEGIN
        SET @hasil = 'GAGAL: Produk masih digunakan dalam pesanan yang belum selesai.';
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM Produk WHERE id_produk = @id)
    BEGIN
        SET @hasil = 'GAGAL: Produk tidak ditemukan.';
        RETURN;
    END

    DELETE FROM Produk WHERE id_produk = @id;

    SET @hasil = 'SUKSES: Data produk berhasil dihapus.';
END;
GO

-- ============================================================
-- 5. STORED PROCEDURE - SEARCH (AMAN - menggunakan parameter)
-- Tambahan: filter by jenis_kayu dan range harga
-- ============================================================

CREATE OR ALTER PROCEDURE sp_SearchProduk
    @nama       VARCHAR(100) = NULL,
    @jenis      VARCHAR(50)  = NULL,
    @harga_min  INT          = NULL,
    @harga_max  INT          = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        id_produk,
        nama_produk,
        jenis_kayu,
        harga,
        stok,
        CASE 
            WHEN stok = 0 THEN 'Habis'
            WHEN stok <= 5 THEN 'Menipis'
            ELSE 'Tersedia'
        END AS status_stok
    FROM Produk
    WHERE
        (@nama      IS NULL OR nama_produk LIKE '%' + @nama + '%')
        AND (@jenis IS NULL OR jenis_kayu  LIKE '%' + @jenis + '%')
        AND (@harga_min IS NULL OR harga   >= @harga_min)
        AND (@harga_max IS NULL OR harga   <= @harga_max);
END;
GO

-- ============================================================
-- 6. SQL INJECTION DEMO

CREATE OR ALTER PROCEDURE sp_SearchPesananAman
    @status VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM vw_DetailPesanan
    WHERE status = @status;
END;


-- ============================================================
-- VIEW untuk Laporan Pembukuan
-- ============================================================

-- View detail penjualan
CREATE OR ALTER VIEW vw_LaporanPenjualan AS
SELECT 
    p.id_pesanan,
    p.tanggal_pesanan,
    p.status,
    pr.nama_produk,
    pr.jenis_kayu,
    pr.harga AS harga_satuan,
    dp.jumlah,
    dp.subtotal,
    p.total_harga,
    FORMAT(p.tanggal_pesanan, 'dd/MM/yyyy') AS tgl_format,
    FORMAT(p.tanggal_pesanan, 'MMMM yyyy', 'id-ID') AS bulan_tahun
FROM Pesanan p
JOIN DetailPesanan dp ON p.id_pesanan = dp.id_pesanan
JOIN Produk pr ON dp.id_produk = pr.id_produk;
GO

-- View ringkasan pembukuan
CREATE OR ALTER VIEW vw_RingkasanPembukuan AS
SELECT
    -- Pendapatan
    ISNULL((SELECT SUM(total_harga) FROM Pesanan), 0) AS total_pendapatan,
    ISNULL((SELECT SUM(total_harga) FROM Pesanan WHERE status = 'Selesai'), 0) AS pendapatan_selesai,
    ISNULL((SELECT SUM(total_harga) FROM Pesanan WHERE status = 'Pending'), 0) AS pendapatan_pending,
    ISNULL((SELECT SUM(total_harga) FROM Pesanan WHERE status = 'Diproses'), 0) AS pendapatan_diproses,
    
    -- Aset / Inventaris
    ISNULL((SELECT SUM(harga * stok) FROM Produk), 0) AS total_aset,
    ISNULL((SELECT COUNT(*) FROM Produk), 0) AS total_produk,
    ISNULL((SELECT SUM(stok) FROM Produk), 0) AS total_stok,
    
    -- Statistik
    ISNULL((SELECT COUNT(*) FROM Pesanan), 0) AS total_pesanan,
    ISNULL((SELECT SUM(jumlah) FROM DetailPesanan), 0) AS total_produk_terjual,
    
    -- Produk terlaris
    (SELECT TOP 1 pr.nama_produk 
     FROM DetailPesanan dp 
     JOIN Produk pr ON dp.id_produk = pr.id_produk 
     GROUP BY pr.nama_produk 
     ORDER BY SUM(dp.jumlah) DESC) AS produk_terlaris,
    
    -- Stok menipis
    (SELECT COUNT(*) FROM Produk WHERE stok <= 5 AND stok > 0) AS stok_menipis,
    (SELECT COUNT(*) FROM Produk WHERE stok = 0) AS stok_habis,
    
    GETDATE() AS tanggal_cetak
GO
<<<<<<< HEAD

-- ============================================================
-- SP untuk Laporan Pembukuan dengan Filter Periode
-- ============================================================
CREATE OR ALTER PROCEDURE sp_LaporanPembukuan
    @bulan INT = NULL,
    @tahun INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Jika bulan dan tahun NULL, ambil semua data
    SELECT 
        p.id_pesanan,
        p.tanggal_pesanan,
        p.status,
        pr.nama_produk,
        pr.jenis_kayu,
        pr.harga AS harga_satuan,
        dp.jumlah,
        dp.subtotal,
        p.total_harga,
        FORMAT(p.tanggal_pesanan, 'dd/MM/yyyy') AS tgl_format,
        MONTH(p.tanggal_pesanan) AS bulan,
        YEAR(p.tanggal_pesanan) AS tahun
    FROM Pesanan p
    JOIN DetailPesanan dp ON p.id_pesanan = dp.id_pesanan
    JOIN Produk pr ON dp.id_produk = pr.id_produk
    WHERE 
        (@bulan IS NULL OR MONTH(p.tanggal_pesanan) = @bulan)
        AND (@tahun IS NULL OR YEAR(p.tanggal_pesanan) = @tahun)
    ORDER BY p.tanggal_pesanan DESC;
END
GO


UPDATE DetailPesanan 
SET subtotal = (SELECT harga FROM Produk WHERE id_produk = DetailPesanan.id_produk) * jumlah
WHERE subtotal IS NULL OR subtotal = 0;

UPDATE Pesanan 
SET total_harga = (SELECT SUM(subtotal) FROM DetailPesanan WHERE id_pesanan = Pesanan.id_pesanan)
WHERE total_harga IS NULL OR total_harga = 0;


SELECT 
    p.id_pesanan,
    p.tanggal_pesanan,
    FORMAT(p.tanggal_pesanan, 'dd/MM/yyyy') AS tgl_format,
    p.status,
    pr.nama_produk,
    pr.jenis_kayu,
    pr.harga AS harga_satuan,
    dp.jumlah,
    pr.harga * dp.jumlah AS subtotal,           
    (SELECT SUM(pr2.harga * dp2.jumlah) 
     FROM DetailPesanan dp2 
     JOIN Produk pr2 ON dp2.id_produk = pr2.id_produk 
     WHERE dp2.id_pesanan = p.id_pesanan) AS total_harga   
FROM Pesanan p
JOIN DetailPesanan dp ON p.id_pesanan = dp.id_pesanan
JOIN Produk pr ON dp.id_produk = pr.id_produk


-- Update subtotal di DetailPesanan
UPDATE dp
SET dp.subtotal = pr.harga * dp.jumlah
FROM DetailPesanan dp
JOIN Produk pr ON dp.id_produk = pr.id_produk;

-- Update total_harga di Pesanan
UPDATE p
SET p.total_harga = (
    SELECT SUM(dp.subtotal) 
    FROM DetailPesanan dp 
    WHERE dp.id_pesanan = p.id_pesanan
)
FROM Pesanan p;


USE UkiranKayuDB;
GO

-- ============================================================
-- TABEL LOG (BUAT DULU JIKA BELUM ADA)
-- ============================================================
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='LogProduk' AND xtype='U')
CREATE TABLE LogProduk (
    id_log        INT IDENTITY(1,1) PRIMARY KEY,
    id_produk     INT,
    aksi          VARCHAR(20),
    nama_lama     VARCHAR(100),
    nama_baru     VARCHAR(100),
    harga_lama    INT,
    harga_baru    INT,
    stok_lama     INT,
    stok_baru     INT,
    username      VARCHAR(50),
    waktu_aksi    DATETIME DEFAULT GETDATE()
);
GO

-- ============================================================
-- TRIGGER INSERT
-- ============================================================
CREATE OR ALTER TRIGGER trg_Produk_Insert
ON Produk
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO LogProduk (id_produk, aksi, nama_baru, harga_baru, stok_baru, username)
    SELECT 
        id_produk,
        'INSERT',
        nama_produk,
        harga,
        stok,
        SYSTEM_USER
    FROM inserted;
END;
GO

-- ============================================================
-- TRIGGER UPDATE
-- ============================================================
CREATE OR ALTER TRIGGER trg_Produk_Update
ON Produk
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO LogProduk (id_produk, aksi, nama_lama, nama_baru, harga_lama, harga_baru, stok_lama, stok_baru, username)
    SELECT 
        i.id_produk,
        'UPDATE',
        d.nama_produk,
        i.nama_produk,
        d.harga,
        i.harga,
        d.stok,
        i.stok,
        SYSTEM_USER
    FROM inserted i
    INNER JOIN deleted d ON i.id_produk = d.id_produk;
END;
GO

-- ============================================================
-- TRIGGER DELETE
-- ============================================================
CREATE OR ALTER TRIGGER trg_Produk_Delete
ON Produk
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO LogProduk (id_produk, aksi, nama_lama, harga_lama, stok_lama, username)
    SELECT 
        id_produk,
        'DELETE',
        nama_produk,
        harga,
        stok,
        SYSTEM_USER
    FROM deleted;
END;
GO

-- ============================================================
-- CEK TRIGGER
-- ============================================================
SELECT 
    name AS TriggerName,
    OBJECT_NAME(parent_id) AS TableName
FROM sys.triggers
WHERE parent_class = 1;

-- Cek hasil
SELECT * FROM DetailPesanan;
SELECT * FROM Pesanan;

SELECT id_produk, nama_produk, harga FROM Produk;

-- Cek trigger Insert
-- Cek trigger Insert
EXEC sp_helptext 'trg_Produk_Insert';

-- Cek trigger Update
EXEC sp_helptext 'trg_Produk_Update';

-- Cek trigger Delete
EXEC sp_helptext 'trg_Produk_Delete';

SELECT * FROM LogProduk
ORDER BY waktu_aksi DESC;

SELECT * FROM LogProduk WHERE aksi = 'INSERT';