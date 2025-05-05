using FluentValidation;
using SembakoAPI.Models;

namespace SembakoAPI.Validations;

public class BarangValidator : AbstractValidator<Barang>
{
    public BarangValidator()
    {
        RuleFor(b => b.Nama)
            .NotEmpty().WithMessage("Nama barang tidak boleh kosong.")
            .MaximumLength(100).WithMessage("Nama barang maksimal 100 karakter.");

        RuleFor(b => b.Harga)
            .GreaterThan(0).WithMessage("Harga harus lebih besar dari 0.");

        RuleFor(b => b.Stok)
            .GreaterThanOrEqualTo(0).WithMessage("Stok tidak boleh negatif.");
    }
}