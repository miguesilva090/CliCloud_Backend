using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.TaxasIva.MotivoIsencaoService.DTOs
{
    public class UpdateMotivoIsencaoRequest : IDto
    {
        public required string Codigo { get; set; }
        public required string Descricao { get; set; }
    }

    public class UpdateMotivoIsencaoValidator : AbstractValidator<UpdateMotivoIsencaoRequest>
    {
        public UpdateMotivoIsencaoValidator()
        {
            _ = RuleFor(x => x.Codigo).NotEmpty().MaximumLength(20);
            _ = RuleFor(x => x.Descricao).NotEmpty().MaximumLength(200);
        }
    }
}
