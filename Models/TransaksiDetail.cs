using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SembakoAPI.Models;

public class TransaksiDetail
{
    public int Id { get; set; }
    public int TransaksiId { get; set; }
    public Transaksi Transaksi { get; set; }
    public int BarangId { get; set; }
    public Barang Barang { get; set; }
    public int Kuantitas { get; set; }
    public decimal HargaSatuan { get; set; }
    public decimal Subtotal { get; set; }
}