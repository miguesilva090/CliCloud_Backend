using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.TaxasIva.MotivoIsencaoService.DTOs
{
    public class UpdateMotivoIsencaoRequest : IDto
    {
        public required string Codigo { get; set; }
        public required string CodigoSaft { get; set; }
        public required string Descricao { get; set; }
        public string? Norma { get; set; }
        public string? Mencao { get; set; }
    }

    public class UpdateMotivoIsencaoValidator : AbstractValidator<UpdateMotivoIsencaoRequest>
    {
        public UpdateMotivoIsencaoValidator()
        {
            _ = RuleFor(x => x.Codigo).NotEmpty().MaximumLength(20);
            _ = RuleFor(x => x.CodigoSaft).NotEmpty().MaximumLength(12);
            _ = RuleFor(x => x.Descricao).NotEmpty().MaximumLength(254);
            _ = RuleFor(x => x.Norma).MaximumLength(254).When(x => !string.IsNullOrWhiteSpace(x.Norma));
            _ = RuleFor(x => x.Mencao).MaximumLength(254).When(x => !string.IsNullOrWhiteSpace(x.Mencao));
        }
    }
}
