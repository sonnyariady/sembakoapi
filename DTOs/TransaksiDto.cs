namespace SembakoAPI.DTOs
{
    public class TransaksiDto
    {
        public int Id { get; set; }
        public DateTime Tanggal { get; set; }
        public int PelangganId { get; set; }
        public PelangganDto Pelanggan { get; set; }
        public List<TransaksiDetailDto> TransaksiDetails { get; set; }
    }
}
