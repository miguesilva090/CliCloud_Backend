using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.NotasBodyChartService.DTOs
{
    public class CreateNotasBodyChartRequest : IDto
    {
        public Guid TratamentoId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal XPercent { get; set; }
        public decimal YPercent { get; set; }
        public Guid MapaBodyChartId { get; set; }
        public Guid MarcadorBodyChartId { get; set; }
    }

    public class CreateNotasBodyChartValidator : AbstractValidator<CreateNotasBodyChartRequest>
    {
        public CreateNotasBodyChartValidator()
        {
            _ = RuleFor(x => x.TratamentoId).NotEmpty();
            _ = RuleFor(x => x.Nome).NotEmpty();
            _ = RuleFor(x => x.Descricao).NotEmpty();
            _ = RuleFor(x => x.XPercent).GreaterThan(0);
            _ = RuleFor(x => x.YPercent).GreaterThan(0);
            _ = RuleFor(x => x.MapaBodyChartId).NotEmpty();
            _ = RuleFor(x => x.MarcadorBodyChartId).NotEmpty();
        }
    }
}
