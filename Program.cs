using Microsoft.EntityFrameworkCore;
using SembakoAPI.Data;
using SembakoAPI.Services;
using SembakoAPI.Validations;
using FluentValidation;
using SembakoAPI.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Konfigurasi Database
builder.Services.AddDbContext<SembakoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .LogTo(Console.WriteLine, LogLevel.Debug) // Ubah ke LogLevel.Debug atau LogLevel.Trace
           .EnableSensitiveDataLogging());

// Tambahkan layanan dan validasi
builder.Services.AddScoped<IBarangService, BarangService>();
builder.Services.AddScoped<IPelangganService, PelangganService>();
builder.Services.AddScoped<ITransaksiService, TransaksiService>();
builder.Services.AddScoped<IValidator<Barang>, BarangValidator>();
builder.Services.AddScoped<IValidator<Pelanggan>, PelangganValidator>();
builder.Services.AddScoped<IValidator<Transaksi>, TransaksiValidator>();
builder.Services.AddScoped<IValidator<TransaksiDetail>, TransaksiDetailValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();