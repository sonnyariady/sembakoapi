using Microsoft.AspNetCore.Mvc;
using SembakoAPI.DTOs;
using SembakoAPI.Models;
using SembakoAPI.Services;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SembakoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BarangController : ControllerBase
{
    private readonly IBarangService _barangService;
    private readonly IValidator<Barang> _validator;
    private readonly ILogger<BarangController> _logger;

    public BarangController(IBarangService barangService, IValidator<Barang> validator, ILogger<BarangController> logger)
    {
        _barangService = barangService;
        _validator = validator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            _logger.LogInformation("Mendapatkan semua data barang.");
            var barangsDto = await _barangService.GetAllAsync();
            return Ok(barangsDto.ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Terjadi kesalahan saat mendapatkan semua data barang.");
            return StatusCode(500, "Terjadi kesalahan server.");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            _logger.LogInformation($"Mendapatkan data barang dengan ID: {id}.");
            var barangDto = await _barangService.GetByIdAsync(id);
            if (barangDto == null)
            {
                _logger.LogWarning($"Data barang dengan ID: {id} tidak ditemukan.");
                return NotFound();
            }
            return Ok(barangDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Terjadi kesalahan saat mendapatkan data barang dengan ID: {id}.");
            return StatusCode(500, "Terjadi kesalahan server.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Barang barang)
    {
        try
        {
            _logger.LogInformation($"Mencoba membuat data barang baru: {System.Text.Json.JsonSerializer.Serialize(barang)}.");
            var createdBarang = await _barangService.AddAsync(barang);
            _logger.LogInformation($"Data barang baru berhasil dibuat dengan ID: {createdBarang.Id}.");
            return CreatedAtAction(nameof(GetById), new { id = createdBarang.Id }, new BarangDto { Id = createdBarang.Id, Nama = createdBarang.Nama, Harga = createdBarang.Harga, Stok = createdBarang.Stok });
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning($"Validasi data barang gagal: {string.Join(", ", ex.Errors.Select(e => e.ErrorMessage))}.");
            return BadRequest(ex.Errors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Terjadi kesalahan saat membuat data barang baru: {System.Text.Json.JsonSerializer.Serialize(barang)}.");
            return StatusCode(500, "Terjadi kesalahan server.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Barang barang)
    {
        try
        {
            _logger.LogInformation($"Mencoba memperbarui data barang dengan ID: {id} dengan data: {System.Text.Json.JsonSerializer.Serialize(barang)}.");
            var updatedBarang = await _barangService.UpdateAsync(id, barang);
            if (updatedBarang == null)
            {
                _logger.LogWarning($"Data barang dengan ID: {id} tidak ditemukan untuk diperbarui.");
                return NotFound();
            }
            _logger.LogInformation($"Data barang dengan ID: {id} berhasil diperbarui.");
            return Ok(new BarangDto { Id = updatedBarang.Id, Nama = updatedBarang.Nama, Harga = updatedBarang.Harga, Stok = updatedBarang.Stok });
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning($"Validasi data barang gagal saat pembaruan ID: {id}: {string.Join(", ", ex.Errors.Select(e => e.ErrorMessage))}.");
            return BadRequest(ex.Errors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Terjadi kesalahan saat memperbarui data barang dengan ID: {id} dengan data: {System.Text.Json.JsonSerializer.Serialize(barang)}.");
            return StatusCode(500, "Terjadi kesalahan server.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            _logger.LogInformation($"Mencoba menghapus data barang dengan ID: {id}.");
            var result = await _barangService.DeleteAsync(id);
            if (!result)
            {
                _logger.LogWarning($"Data barang dengan ID: {id} tidak ditemukan untuk dihapus.");
                return NotFound();
            }
            _logger.LogInformation($"Data barang dengan ID: {id} berhasil dihapus.");
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Terjadi kesalahan saat menghapus data barang dengan ID: {id}.");
            return StatusCode(500, "Terjadi kesalahan server.");
        }
    }
}