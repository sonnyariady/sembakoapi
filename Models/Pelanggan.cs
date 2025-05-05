using System.Text.Json.Serialization;

namespace SembakoAPI.Models;

public class Pelanggan
{
    public int Id { get; set; }
    public string Nama { get; set; }
    public string Alamat { get; set; }
    public ICollection<Transaksi> Transaksis { get; set; } = new List<Transaksi>();
}