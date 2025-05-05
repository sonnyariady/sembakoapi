using System.ComponentModel.DataAnnotations;

namespace SembakoAPI.Models;

public class Transaksi
{
    public int Id { get; set; }
    public DateTime Tanggal { get; set; } = DateTime.Now;
    public int PelangganId { get; set; }
    public Pelanggan Pelanggan { get; set; }
    public ICollection<TransaksiDetail> TransaksiDetails { get; set; } = new List<TransaksiDetail>();
}