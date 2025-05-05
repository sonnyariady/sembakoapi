// Services/PelangganService.cs
using Microsoft.EntityFrameworkCore;
using SembakoAPI.Data;
using SembakoAPI.DTOs;
using SembakoAPI.Models;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SembakoAPI.Services;

public class PelangganService : IPelangganService
{
    private readonly SembakoDbContext _context;
    private readonly IValidator<Pelanggan> _validator;

    public PelangganService(SembakoDbContext context, IValidator<Pelanggan> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<IEnumerable<PelangganDto>> GetAllAsync()
    {
        var pelanggans = await _context.Pelanggans.ToListAsync();
        return pelanggans.Select(p => new PelangganDto
        {
            Id = p.Id,
            Nama = p.Nama,
            Alamat = p.Alamat
        });
    }

    public async Task<PelangganDto?> GetByIdAsync(int id)
    {
        var pelanggan = await _context.Pelanggans.FindAsync(id);
        if (pelanggan == null)
        {
            return null;
        }
        return new PelangganDto
        {
            Id = pelanggan.Id,
            Nama = pelanggan.Nama,
            Alamat = pelanggan.Alamat
        };
    }

    public async Task<Pelanggan> AddAsync(Pelanggan pelanggan)
    {
        var validationResult = await _validator.ValidateAsync(pelanggan);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        _context.Pelanggans.Add(pelanggan);
        await _context.SaveChangesAsync();
        return pelanggan;
    }

    public async Task<Pelanggan?> UpdateAsync(int id, Pelanggan pelanggan)
    {
        var existingPelanggan = await _context.Pelanggans.FindAsync(id);
        if (existingPelanggan == null)
        {
            return null;
        }

        pelanggan.Id = id; // Ensure ID consistency
        var validationResult = await _validator.ValidateAsync(pelanggan);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        _context.Entry(existingPelanggan).CurrentValues.SetValues(pelanggan);
        await _context.SaveChangesAsync();
        return existingPelanggan;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var pelanggan = await _context.Pelanggans.FindAsync(id);
        if (pelanggan == null)
        {
            return false;
        }

        _context.Pelanggans.Remove(pelanggan);
        var deletedCount = await _context.SaveChangesAsync();
        return deletedCount > 0;
    }
}