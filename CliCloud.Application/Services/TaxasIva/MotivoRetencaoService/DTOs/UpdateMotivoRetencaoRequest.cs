using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.TaxasIva.MotivoRetencaoService.DTOs
{
    public class UpdateMotivoRetencaoRequest : IDto
    {
        public int Codigo { get; set; }
        public required string Descricao { get; set; }
        public required string TipoImposto { get; set; }
    }

    public class UpdateMotivoRetencaoValidator : AbstractValidator<UpdateMotivoRetencaoRequest>
    {
        public UpdateMotivoRetencaoValidator()
        {
            _ = RuleFor(x => x.Codigo).GreaterThan(0);
            _ = RuleFor(x => x.Descricao).NotEmpty().MaximumLength(150);
            _ = RuleFor(x => x.TipoImposto).NotEmpty().MaximumLength(3);
        }
    }
}
