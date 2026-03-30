using FluentValidation;
using CliCloud.Application.Common.Marker;


namespace CliCloud.Application.Services.ProcessoClinico.NotasBodyChartService.DTOs
{
    public class UpdateNotasBodyChartRequest : IDto
    {
        public Guid TratamentoId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal XPercent { get; set; }
        public decimal YPercent { get; set; }
        public Guid MapaBodyChartId { get; set; }
        public Guid MarcadorBodyChartId { get; set; }
    }

    public class UpdateNotasBodyChartValidator : AbstractValidator<UpdateNotasBodyChartRequest>
    {
        public UpdateNotasBodyChartValidator()
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

