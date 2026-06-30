using FluentValidation;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs
{
    public class UpdateLoteDirectRequest : CreateLoteDirectRequest
    {
    }

    public class UpdateLoteDirectValidator : AbstractValidator<UpdateLoteDirectRequest>
    {
        public UpdateLoteDirectValidator()
        {
            _ = RuleFor(x => x.UtenteId)
                .NotEmpty()
                .WithMessage("Utente em falta.");

            _ = RuleFor(x => x.Credencial)
                .NotEmpty()
                .WithMessage("Nº credencial em falta.");

            _ = RuleFor(x => x.Mes)
                .InclusiveBetween(1, 12)
                .WithMessage("Mês inválido.");

            _ = RuleFor(x => x.Ano)
                .GreaterThanOrEqualTo(1900)
                .WithMessage("Ano inválido.");
        }
    }
}
