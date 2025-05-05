// Services/IBarangService.cs
using SembakoAPI.DTOs;
using SembakoAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SembakoAPI.Services;

public interface IBarangService
{
    Task<IEnumerable<BarangDto>> GetAllAsync();
    Task<BarangDto?> GetByIdAsync(int id);
    Task<Barang> AddAsync(Barang barang);
    Task<Barang?> UpdateAsync(int id, Barang barang);
    Task<bool> DeleteAsync(int id);
}