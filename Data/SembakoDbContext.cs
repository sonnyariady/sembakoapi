// Data/SembakoDbContext.cs
using Microsoft.EntityFrameworkCore;
using SembakoAPI.Models;

namespace SembakoAPI.Data;

public class SembakoDbContext : DbContext
{
    public SembakoDbContext(DbContextOptions<SembakoDbContext> options) : base(options) { }

    public DbSet<Barang> Barangs { get; set; }
    public DbSet<Pelanggan> Pelanggans { get; set; }
    public DbSet<Transaksi> Transaksis { get; set; }
    public DbSet<TransaksiDetail> TransaksiDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Konfigurasi relasi Transaksi dengan Pelanggan
        modelBuilder.Entity<Transaksi>()
            .HasOne(t => t.Pelanggan)
            .WithMany(p => p.Transaksis)
            .HasForeignKey(t => t.PelangganId)
            .OnDelete(DeleteBehavior.Restrict); // Atau opsi lain sesuai kebutuhan

        modelBuilder.Entity<TransaksiDetail>()
        .Property(td => td.Id)
        .ValueGeneratedOnAdd(); // Ini adalah default untuk identity, tapi bisa dieksplisitkan

        // Konfigurasi relasi TransaksiDetail dengan Transaksi
        modelBuilder.Entity<TransaksiDetail>()
            .HasOne(td => td.Transaksi)
            .WithMany(t => t.TransaksiDetails)
            .HasForeignKey(td => td.TransaksiId)
            .OnDelete(DeleteBehavior.Cascade);

        // Konfigurasi relasi TransaksiDetail dengan Barang
        modelBuilder.Entity<TransaksiDetail>()
            .HasOne(td => td.Barang)
            .WithMany() // Atau WithMany(b => b.TransaksiDetails) jika ada properti navigasi di Barang
            .HasForeignKey(td => td.BarangId)
            .OnDelete(DeleteBehavior.Restrict); // Atau opsi lain sesuai kebutuhan
    }
}