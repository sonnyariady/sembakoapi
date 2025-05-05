using FluentValidation;
using SembakoAPI.Models;

namespace SembakoAPI.Validations;

public class TransaksiValidator : AbstractValidator<Transaksi>
{
    public TransaksiValidator()
    {
        RuleFor(t => t.PelangganId)
            .GreaterThan(0).WithMessage("ID Pelanggan harus lebih besar dari 0.");

        RuleFor(t => t.TransaksiDetails)
            .NotNull().WithMessage("Detail transaksi tidak boleh kosong.")
            .NotEmpty().WithMessage("Detail transaksi tidak boleh kosong.")
            .ForEach(details =>
            {
                details.SetValidator(new TransaksiDetailValidator());
            });
    }
}