// Services/TransaksiService.cs
using Microsoft.EntityFrameworkCore;
using SembakoAPI.Data;
using SembakoAPI.DTOs;
using SembakoAPI.Models;
using FluentValidation;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SembakoAPI.Services;

public class TransaksiService : ITransaksiService
{
    private readonly SembakoDbContext _context;
    private readonly IValidator<Transaksi> _transaksiValidator;
    private readonly IValidator<TransaksiDetail> _transaksiDetailValidator;

    public TransaksiService(SembakoDbContext context, IValidator<Transaksi> transaksiValidator, IValidator<TransaksiDetail> transaksiDetailValidator)
    {
        _context = context;
        _transaksiValidator = transaksiValidator;
        _transaksiDetailValidator = transaksiDetailValidator;
    }

    public async Task<TransaksiDto?> GetTransaksiByIdAsync(int id)
    {
        var transaksi = await _context.Transaksis
            .Include(t => t.Pelanggan)
            .Include(t => t.TransaksiDetails)
                .ThenInclude(td => td.Barang)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (transaksi == null)
        {
            return null;
        }

        var transaksiDto = new TransaksiDto
        {
            Id = transaksi.Id,
            Tanggal = transaksi.Tanggal,
            PelangganId = transaksi.PelangganId,
            Pelanggan = new PelangganDto
            {
                Id = transaksi.Pelanggan.Id,
                Nama = transaksi.Pelanggan.Nama,
                Alamat = transaksi.Pelanggan.Alamat
            },
            TransaksiDetails = transaksi.TransaksiDetails.Select(td => new TransaksiDetailDto
            {
                Id = td.Id,
                BarangId = td.BarangId,
                NamaBarang = td.Barang.Nama,
                Kuantitas = td.Kuantitas,
                HargaSatuan = td.HargaSatuan,
                Subtotal = td.Subtotal
            }).ToList()
        };

        return transaksiDto;
    }

    public async Task<Transaksi> CreateTransaksiAsync(Transaksi transaksi)
    {
        var transaksiValidationResult = await _transaksiValidator.ValidateAsync(transaksi);
        if (!transaksiValidationResult.IsValid)
        {
            throw new ValidationException(transaksiValidationResult.Errors);
        }

        if (transaksi.TransaksiDetails == null || !transaksi.TransaksiDetails.Any())
        {
            throw new ValidationException("Detail transaksi tidak boleh kosong.");
        }

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            transaksi.Tanggal = DateTime.Now;
            _context.Transaksis.Add(transaksi);
            await _context.SaveChangesAsync();

            foreach (var detail in transaksi.TransaksiDetails)
            {
                var barang = await _context.Barangs.FindAsync(detail.BarangId);
                if (barang == null)
                {
                    throw new ValidationException($"Barang dengan ID {detail.BarangId} tidak ditemukan.");
                }
                if (barang.Stok < detail.Kuantitas)
                {
                    throw new InvalidOperationException($"Stok barang {barang.Nama} tidak mencukupi.");
                }
                barang.Stok -= detail.Kuantitas;
                detail.HargaSatuan = barang.Harga;
                detail.Subtotal = detail.HargaSatuan * detail.Kuantitas;
                detail.TransaksiId = transaksi.Id;
                _context.TransaksiDetails.Add(detail);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return transaksi;
        }
        catch (InvalidOperationException)
        {
            await transaction.RollbackAsync();
            throw; // Re-throw the specific exception
        }
        catch (ValidationException)
        {
            await transaction.RollbackAsync();
            throw;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw; // Re-throw the generic exception
        }
    }

    public async Task<Transaksi> CreateTransaksiAsync(DtoInputTransaksi inputTransaksi)
    {
        var transaksi = new Transaksi
        {
            PelangganId = inputTransaksi.PelangganId,
            TransaksiDetails = inputTransaksi.TransaksiDetails.Select(detailInput => new TransaksiDetail
            {
                BarangId = detailInput.BarangId,
                Kuantitas = detailInput.Kuantitas
            }).ToList()
        };

        // Validasi transaksi (termasuk detail)
        var transaksiValidationResult = await _transaksiValidator.ValidateAsync(transaksi);
        if (!transaksiValidationResult.IsValid)
        {
            throw new ValidationException(transaksiValidationResult.Errors);
        }

        if (!transaksi.TransaksiDetails.Any())
        {
            throw new ValidationException("Detail transaksi tidak boleh kosong.");
        }

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            transaksi.Tanggal = DateTime.Now;
            _context.Transaksis.Add(transaksi);
            await _context.SaveChangesAsync();

            foreach (var detail in transaksi.TransaksiDetails)
            {
                var barang = await _context.Barangs.FindAsync(detail.BarangId);
                if (barang == null)
                {
                    throw new ValidationException($"Barang dengan ID {detail.BarangId} tidak ditemukan.");
                }
                if (barang.Stok < detail.Kuantitas)
                {
                    throw new InvalidOperationException($"Stok barang {barang.Nama} tidak mencukupi.");
                }
                barang.Stok -= detail.Kuantitas;
                detail.Id = 0;
                detail.HargaSatuan = barang.Harga;
                detail.Subtotal = detail.HargaSatuan * detail.Kuantitas;
                detail.TransaksiId = transaksi.Id;
                _context.TransaksiDetails.Add(detail);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return transaksi;
        }
        catch (InvalidOperationException)
        {
            await transaction.RollbackAsync();
            throw; // Re-throw the specific exception
        }
        catch (ValidationException)
        {
            await transaction.RollbackAsync();
            throw;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw; // Re-throw the generic exception
        }
    }
}