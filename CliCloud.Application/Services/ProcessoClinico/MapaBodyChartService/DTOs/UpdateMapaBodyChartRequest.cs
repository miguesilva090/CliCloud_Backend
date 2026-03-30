using FluentValidation;
using CliCloud.Application.Common.Marker;


namespace CliCloud.Application.Services.ProcessoClinico.MapaBodyChartService.DTOs
{
    public class UpdateMapaBodyChartRequest : IDto
    {
        public string Nome { get; set; } = string.Empty;
        public string CaminhoImagem { get; set; } = string.Empty;
    }

    public class UpdateMapaBodyChartValidator : AbstractValidator<UpdateMapaBodyChartRequest>
    {
        public UpdateMapaBodyChartValidator()
        {
            _ = RuleFor(x => x.Nome).NotEmpty();
            _ = RuleFor(x => x.CaminhoImagem).NotEmpty();
        }
    }
}

