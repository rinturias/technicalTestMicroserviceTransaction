using FluentValidation;
using Yape.Transactions.Application.DTO;

namespace Yape.Transactions.Application.Validators
{
    public class TransactionCreateValidator : AbstractValidator<TransactionCreateDto>
    {
        public TransactionCreateValidator()
        {
            RuleFor(x => x.SourceAccountId)
                .NotEmpty().WithMessage("El campo 'SourceAccountId' es obligatorio.");

            RuleFor(x => x.TargetAccountId)
                .NotEmpty().WithMessage("El campo 'TargetAccountId' es obligatorio.");



            RuleFor(u => u.TransferTypeId)
                 .Must(code => (code == 1 || code == 2))
            .GreaterThan(0).WithMessage("El código de TransferTypeId ingresado no está habilitado.");
            

            RuleFor(x => x.Value)
                .GreaterThan(0).WithMessage("El 'Value' debe ser mayor que 0.");
        }
    }
}