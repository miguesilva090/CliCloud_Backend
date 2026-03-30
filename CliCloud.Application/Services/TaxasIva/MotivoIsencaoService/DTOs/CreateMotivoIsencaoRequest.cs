using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.TaxasIva.MotivoIsencaoService.DTOs
{
    public class CreateMotivoIsencaoRequest : IDto
    {
        public required string Codigo { get; set; }
        public required string Descricao { get; set; }
    }

    public class CreateMotivoIsencaoValidator : AbstractValidator<CreateMotivoIsencaoRequest>
    {
        public CreateMotivoIsencaoValidator()
        {
            _ = RuleFor(x => x.Codigo).NotEmpty().MaximumLength(20);
            _ = RuleFor(x => x.Descricao).NotEmpty().MaximumLength(200);
        }
    }
}
