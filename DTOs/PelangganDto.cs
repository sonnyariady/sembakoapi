namespace SembakoAPI.DTOs
{
    public class PelangganDto
    {
        public int Id { get; set; }
        public string Nama { get; set; }
        public string Alamat { get; set; }
        // Mungkin tidak perlu menyertakan Transaksis di DTO Pelanggan untuk menghindari siklus
        // public List<TransaksiDto> Transaksis { get; set; }
    }
}
