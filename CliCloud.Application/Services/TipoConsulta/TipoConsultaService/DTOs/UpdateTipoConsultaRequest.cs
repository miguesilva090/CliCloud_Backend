using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.TiposConsulta.TipoConsultaService.DTOs
{
    public class UpdateTipoConsultaRequest : IDto
    {
        public string Designacao { get; set; } = string.Empty;
        public int? CodigoLegado { get; set; }
    }

    public class UpdateTipoConsultaValidator : AbstractValidator<UpdateTipoConsultaRequest>
    {
        public UpdateTipoConsultaValidator()
        {
            _ = RuleFor(x => x.Designacao).NotEmpty().MaximumLength(80);
            _ = RuleFor(x => x.CodigoLegado).GreaterThan(0).When(x => x.CodigoLegado.HasValue);
        }
    }
}
