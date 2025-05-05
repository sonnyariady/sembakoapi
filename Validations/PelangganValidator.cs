using FluentValidation;
using SembakoAPI.Models;

namespace SembakoAPI.Validations;

public class PelangganValidator : AbstractValidator<Pelanggan>
{
    public PelangganValidator()
    {
        RuleFor(p => p.Nama)
            .NotEmpty().WithMessage("Nama pelanggan tidak boleh kosong.")
            .MaximumLength(100).WithMessage("Nama pelanggan maksimal 100 karakter.");

        RuleFor(p => p.Alamat)
            .NotEmpty().WithMessage("Alamat pelanggan tidak boleh kosong.")
            .MaximumLength(200).WithMessage("Alamat pelanggan maksimal 200 karakter.");
    }
}