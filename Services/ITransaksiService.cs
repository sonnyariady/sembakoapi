// Services/ITransaksiService.cs
using SembakoAPI.DTOs;
using SembakoAPI.Models;
using System.Threading.Tasks;

namespace SembakoAPI.Services;

public interface ITransaksiService
{
    Task<TransaksiDto?> GetTransaksiByIdAsync(int id);
    Task<Transaksi> CreateTransaksiAsync(Transaksi transaksi);
    Task<Transaksi> CreateTransaksiAsync(DtoInputTransaksi inputTransaksi);
}