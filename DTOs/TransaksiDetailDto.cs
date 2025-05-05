namespace SembakoAPI.DTOs
{
    public class TransaksiDetailDto
    {
        public int Id { get; set; }
        public int BarangId { get; set; }
        public string NamaBarang { get; set; } // Bisa diambil dari entitas Barang
        public int Kuantitas { get; set; }
        public decimal HargaSatuan { get; set; }
        public decimal Subtotal { get; set; }
    }
}
