// DTOs/DtoInputTransaksi.cs
using System.Collections.Generic;

namespace SembakoAPI.DTOs;

public class DtoInputTransaksi
{
    public int PelangganId { get; set; }
    public List<DtoInputTransaksiDetail> TransaksiDetails { get; set; }
}

public class DtoInputTransaksiDetail
{
    public int BarangId { get; set; }
    public int Kuantitas { get; set; }
}