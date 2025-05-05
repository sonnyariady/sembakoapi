-- Membuat Tabel Pelanggan
CREATE TABLE Pelanggans (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nama NVARCHAR(100) NOT NULL,
    Alamat NVARCHAR(200) NOT NULL
);

-- Membuat Tabel Barang
CREATE TABLE Barangs (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nama NVARCHAR(100) NOT NULL,
    Harga DECIMAL(18, 2) NOT NULL,
    Stok INT NOT NULL
);

-- Membuat Tabel Transaksi
CREATE TABLE Transaksis (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Tanggal DATETIME NOT NULL DEFAULT GETDATE(),
    PelangganId INT NOT NULL,
    FOREIGN KEY (PelangganId) REFERENCES Pelanggans(Id)
);

-- Membuat Tabel TransaksiDetail
CREATE TABLE TransaksiDetails (
    Id INT PRIMARY KEY IDENTITY(1,1),
    TransaksiId INT NOT NULL,
    BarangId INT NOT NULL,
    Kuantitas INT NOT NULL,
    HargaSatuan DECIMAL(18, 2) NOT NULL,
    Subtotal DECIMAL(18, 2) NOT NULL,
    FOREIGN KEY (TransaksiId) REFERENCES Transaksis(Id) ON DELETE CASCADE,
    FOREIGN KEY (BarangId) REFERENCES Barangs(Id)
);

-- Membuat Index untuk kolom ForeignKey untuk performa query
CREATE INDEX IX_PelangganId ON Transaksis (PelangganId);
CREATE INDEX IX_TransaksiId ON TransaksiDetails (TransaksiId);
CREATE INDEX IX_BarangId ON TransaksiDetails (BarangId);