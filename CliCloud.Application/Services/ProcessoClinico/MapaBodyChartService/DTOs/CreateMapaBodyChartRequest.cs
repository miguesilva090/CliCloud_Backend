using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.MapaBodyChartService.DTOs
{
    public class CreateMapaBodyChartRequest : IDto
    {
        public string Nome { get; set; } = string.Empty;
        public string CaminhoImagem { get; set; } = string.Empty;
    }

    public class CreateMapaBodyChartValidator : AbstractValidator<CreateMapaBodyChartRequest>
    {
        public CreateMapaBodyChartValidator()
        {
            _ = RuleFor(x => x.Nome).NotEmpty();
            _ = RuleFor(x => x.CaminhoImagem).NotEmpty();
        }
    }
}
