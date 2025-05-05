using System.Text.Json.Serialization;

namespace SembakoAPI.Models;

public class Barang
{
    public int Id { get; set; }
    public string Nama { get; set; }
    public decimal Harga { get; set; }
    public int Stok { get; set; }
    // public ICollection<TransaksiDetail> TransaksiDetails { get; set; } // Opsional
}