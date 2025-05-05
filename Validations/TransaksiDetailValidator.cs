using FluentValidation;
using SembakoAPI.Models;

namespace SembakoAPI.Validations;

public class TransaksiDetailValidator : AbstractValidator<TransaksiDetail>
{
    public TransaksiDetailValidator()
    {
        RuleFor(td => td.BarangId)
            .GreaterThan(0).WithMessage("ID Barang harus lebih besar dari 0.");

        RuleFor(td => td.Kuantitas)
            .GreaterThan(0).WithMessage("Kuantitas harus lebih besar dari 0.");
    }
}