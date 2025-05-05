// DTOs/BarangDto.cs
namespace SembakoAPI.DTOs;

public class BarangDto
{
    public int Id { get; set; }
    public string Nama { get; set; }
    public decimal Harga { get; set; }
    public int Stok { get; set; }
}