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
public class PelangganController : ControllerBase
{
    private readonly IPelangganService _pelangganService;
    private readonly IValidator<Pelanggan> _validator;
    private readonly ILogger<PelangganController> _logger;

    public PelangganController(IPelangganService pelangganService, IValidator<Pelanggan> validator, ILogger<PelangganController> logger)
    {
        _pelangganService = pelangganService;
        _validator = validator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            _logger.LogInformation("Mendapatkan semua data pelanggan.");
            var pelanggansDto = await _pelangganService.GetAllAsync();
            return Ok(pelanggansDto.ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Terjadi kesalahan saat mendapatkan semua data pelanggan.");
            return StatusCode(500, "Terjadi kesalahan server.");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            _logger.LogInformation($"Mendapatkan data pelanggan dengan ID: {id}.");
            var pelangganDto = await _pelangganService.GetByIdAsync(id);
            if (pelangganDto == null)
            {
                _logger.LogWarning($"Data pelanggan dengan ID: {id} tidak ditemukan.");
                return NotFound();
            }
            return Ok(pelangganDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Terjadi kesalahan saat mendapatkan data pelanggan dengan ID: {id}.");
            return StatusCode(500, "Terjadi kesalahan server.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Pelanggan pelanggan)
    {
        try
        {
            _logger.LogInformation($"Mencoba membuat data pelanggan baru: {System.Text.Json.JsonSerializer.Serialize(pelanggan)}.");
            var createdPelanggan = await _pelangganService.AddAsync(pelanggan);
            _logger.LogInformation($"Data pelanggan baru berhasil dibuat dengan ID: {createdPelanggan.Id}.");
            return CreatedAtAction(nameof(GetById), new { id = createdPelanggan.Id }, new PelangganDto { Id = createdPelanggan.Id, Nama = createdPelanggan.Nama, Alamat = createdPelanggan.Alamat });
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning($"Validasi data pelanggan gagal: {string.Join(", ", ex.Errors.Select(e => e.ErrorMessage))}.");
            return BadRequest(ex.Errors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Terjadi kesalahan saat membuat data pelanggan baru: {System.Text.Json.JsonSerializer.Serialize(pelanggan)}.");
            return StatusCode(500, "Terjadi kesalahan server.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Pelanggan pelanggan)
    {
        try
        {
            _logger.LogInformation($"Mencoba memperbarui data pelanggan dengan ID: {id} dengan data: {System.Text.Json.JsonSerializer.Serialize(pelanggan)}.");
            var updatedPelanggan = await _pelangganService.UpdateAsync(id, pelanggan);
            if (updatedPelanggan == null)
            {
                _logger.LogWarning($"Data pelanggan dengan ID: {id} tidak ditemukan untuk diperbarui.");
                return NotFound();
            }
            _logger.LogInformation($"Data pelanggan dengan ID: {id} berhasil diperbarui.");
            return Ok(new PelangganDto { Id = updatedPelanggan.Id, Nama = updatedPelanggan.Nama, Alamat = updatedPelanggan.Alamat });
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning($"Validasi data pelanggan gagal saat pembaruan ID: {id}: {string.Join(", ", ex.Errors.Select(e => e.ErrorMessage))}.");
            return BadRequest(ex.Errors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Terjadi kesalahan saat memperbarui data pelanggan dengan ID: {id} dengan data: {System.Text.Json.JsonSerializer.Serialize(pelanggan)}.");
            return StatusCode(500, "Terjadi kesalahan server.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            _logger.LogInformation($"Mencoba menghapus data pelanggan dengan ID: {id}.");
            var result = await _pelangganService.DeleteAsync(id);
            if (!result)
            {
                _logger.LogWarning($"Data pelanggan dengan ID: {id} tidak ditemukan untuk dihapus.");
                return NotFound();
            }
            _logger.LogInformation($"Data pelanggan dengan ID: {id} berhasil dihapus.");
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Terjadi kesalahan saat menghapus data pelanggan dengan ID: {id}.");
            return StatusCode(500, "Terjadi kesalahan server.");
        }
    }
}