using Microsoft.AspNetCore.Mvc;
using SembakoAPI.DTOs;
using SembakoAPI.Models;
using SembakoAPI.Services;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace SembakoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransaksiController : ControllerBase
{
    private readonly ITransaksiService _transaksiService;
    private readonly ILogger<TransaksiController> _logger;

    public TransaksiController(ITransaksiService transaksiService, ILogger<TransaksiController> logger)
    {
        _transaksiService = transaksiService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTransaksiSimple([FromBody] DtoInputTransaksi inputTransaksi)
    {
        _logger.LogInformation($"Mencoba membuat transaksi baru (simple input) untuk Pelanggan ID: {inputTransaksi.PelangganId} dengan detail: {System.Text.Json.JsonSerializer.Serialize(inputTransaksi.TransaksiDetails)}.");

        try
        {
            var createdTransaksi = await _transaksiService.CreateTransaksiAsync(inputTransaksi);
            _logger.LogInformation($"Transaksi berhasil dibuat dengan ID: {createdTransaksi.Id} (simple input).");
            return CreatedAtAction(nameof(GetTransaksiById), new { id = createdTransaksi.Id }, createdTransaksi);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning($"Validasi transaksi gagal (simple input): {string.Join(", ", ex.Errors.Select(e => e.ErrorMessage))}.");
            return BadRequest(ex.Errors);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, $"Gagal membuat transaksi (simple input) karena operasi tidak valid: {ex.Message}.");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Terjadi kesalahan saat memproses transaksi (simple input).");
            return StatusCode(500, "Terjadi kesalahan saat memproses transaksi.");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTransaksiById(int id)
    {
        try
        {
            _logger.LogInformation($"Mendapatkan data transaksi dengan ID: {id}.");
            var transaksiDto = await _transaksiService.GetTransaksiByIdAsync(id);

            if (transaksiDto == null)
            {
                _logger.LogWarning($"Data transaksi dengan ID: {id} tidak ditemukan.");
                return NotFound();
            }

            return Ok(transaksiDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Terjadi kesalahan saat mendapatkan data transaksi dengan ID: {id}.");
            return StatusCode(500, "Terjadi kesalahan server.");
        }
    }

    // Anda dapat menambahkan endpoint lain untuk transaksi jika diperlukan
}