// Services/BarangService.cs
using Microsoft.EntityFrameworkCore;
using SembakoAPI.Data;
using SembakoAPI.DTOs;
using SembakoAPI.Models;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SembakoAPI.Services;

public class BarangService : IBarangService
{
    private readonly SembakoDbContext _context;
    private readonly IValidator<Barang> _validator;

    public BarangService(SembakoDbContext context, IValidator<Barang> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<IEnumerable<BarangDto>> GetAllAsync()
    {
        var barangs = await _context.Barangs.ToListAsync();
        return barangs.Select(b => new BarangDto
        {
            Id = b.Id,
            Nama = b.Nama,
            Harga = b.Harga,
            Stok = b.Stok
        });
    }

    public async Task<BarangDto?> GetByIdAsync(int id)
    {
        var barang = await _context.Barangs.FindAsync(id);
        if (barang == null)
        {
            return null;
        }
        return new BarangDto
        {
            Id = barang.Id,
            Nama = barang.Nama,
            Harga = barang.Harga,
            Stok = barang.Stok
        };
    }

    public async Task<Barang> AddAsync(Barang barang)
    {
        var validationResult = await _validator.ValidateAsync(barang);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        _context.Barangs.Add(barang);
        await _context.SaveChangesAsync();
        return barang;
    }

    public async Task<Barang?> UpdateAsync(int id, Barang barang)
    {
        var existingBarang = await _context.Barangs.FindAsync(id);
        if (existingBarang == null)
        {
            return null;
        }

        barang.Id = id; // Ensure ID consistency
        var validationResult = await _validator.ValidateAsync(barang);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        _context.Entry(existingBarang).CurrentValues.SetValues(barang);
        await _context.SaveChangesAsync();
        return existingBarang;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var barang = await _context.Barangs.FindAsync(id);
        if (barang == null)
        {
            return false;
        }

        _context.Barangs.Remove(barang);
        var deletedCount = await _context.SaveChangesAsync();
        return deletedCount > 0;
    }
}