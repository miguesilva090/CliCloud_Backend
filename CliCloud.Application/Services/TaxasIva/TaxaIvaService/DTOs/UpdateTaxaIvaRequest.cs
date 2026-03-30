using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.TaxasIva.TaxaIvaService.DTOs
{
    public class UpdateTaxaIvaRequest : IDto
    {
        public required string Descricao { get; set; }
        public decimal Taxa { get; set; }
    }

    public class UpdateTaxaIvaValidator : AbstractValidator<UpdateTaxaIvaRequest>
    {
        public UpdateTaxaIvaValidator()
        {
            _ = RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(80)
                .WithMessage("Descricao é obrigatória e deve ter no máximo 80 caracteres.");
            _ = RuleFor(x => x.Taxa)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(100)
                .WithMessage("Taxa deve estar entre 0 e 100.");
        }
    }
}
