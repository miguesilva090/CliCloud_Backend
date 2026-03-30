using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.PrioridadeService.DTOs
{
    public class UpdatePrioridadeRequest : IDto
    {
        public string Descricao { get; set; } = string.Empty;
    }

    public class UpdatePrioridadeValidator : AbstractValidator<UpdatePrioridadeRequest>
    {
        public UpdatePrioridadeValidator()
        {
            _ = RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(80)
                .WithMessage("Descrição é obrigatória e deve ter no máximo 80 caracteres.");
        }
    }
}
