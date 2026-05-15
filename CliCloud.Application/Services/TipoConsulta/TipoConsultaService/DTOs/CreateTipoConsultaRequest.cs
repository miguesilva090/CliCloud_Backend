using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.TiposConsulta.TipoConsultaService.DTOs
{
    public class CreateTipoConsultaRequest : IDto
    {
        public string Designacao { get; set; } = string.Empty;
    }

    public class CreateTipoConsultaValidator : AbstractValidator<CreateTipoConsultaRequest>
    {
        public CreateTipoConsultaValidator()
        {
            _ = RuleFor(x => x.Designacao).NotEmpty().MaximumLength(80);
        }
    }
}
