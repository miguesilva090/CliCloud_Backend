using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Moedas.MoedaService.DTOs
{
    public class CreateMoedaRequest : IDto
    {
        public required string Descricao { get; set; }
        public string? Plural { get; set; }
        public decimal Cambio { get; set; } = 1m;
        public string? Abreviatura { get; set; }
        public string? Centesimos { get; set; }
        public string? CentesimoPlural { get; set; }
        public string? Simbolo { get; set; }
    }

    public class CreateMoedaValidator : AbstractValidator<CreateMoedaRequest>
    {
        public CreateMoedaValidator()
        {
            _ = RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("Descrição é obrigatória e deve ter no máximo 50 caracteres.");
            _ = RuleFor(x => x.Plural)
                .MaximumLength(50);
            _ = RuleFor(x => x.Cambio)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Câmbio deve ser maior ou igual a 0.");
            _ = RuleFor(x => x.Abreviatura)
                .MaximumLength(10);
            _ = RuleFor(x => x.Centesimos)
                .MaximumLength(10);
            _ = RuleFor(x => x.CentesimoPlural)
                .MaximumLength(50);
            _ = RuleFor(x => x.Simbolo)
                .MaximumLength(3);
        }
    }
}
