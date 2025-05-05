// Services/IPelangganService.cs
using SembakoAPI.DTOs;
using SembakoAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SembakoAPI.Services;

public interface IPelangganService
{
    Task<IEnumerable<PelangganDto>> GetAllAsync();
    Task<PelangganDto?> GetByIdAsync(int id);
    Task<Pelanggan> AddAsync(Pelanggan pelanggan);
    Task<Pelanggan?> UpdateAsync(int id, Pelanggan pelanggan);
    Task<bool> DeleteAsync(int id);
}