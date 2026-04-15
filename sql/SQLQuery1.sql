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
    status VARCHAR(50),
    total_harga INT
);


INSERT INTO Produk (nama_produk, jenis_kayu, harga, stok)
VALUES 
('Meja Ukir Jepara','Jati',1500000,10),
('Kursi Ukir','Mahoni',800000,20),
('Lemari Ukir','Jati',2500000,5);

