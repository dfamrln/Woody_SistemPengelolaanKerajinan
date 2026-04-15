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
