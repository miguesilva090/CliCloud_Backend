using FluentValidation;
using CliCloud.Application.Common.Marker;


namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseDentariaService.DTOs
{
    public class UpdateAnamneseOrtodonticaAnaliseDentariaRequest : IDto
    {
        public Guid UtenteId { get; set; }
        public int? TipoDentadura { get; set; }
        public string? Anomalia { get; set; }
        public int? RelacaoMolar { get; set; }
        public int? SubDivisaoMolar { get; set; }
        public int? RelacaoCanino { get; set; }
        public int? SubDivisaoCanino { get; set; }
        public int? RelacaoMolar2 { get; set; }
        public string? LinhaMedianaSuperior { get; set; }
        public string? LinhaMedianaInferior { get; set; }
        public string? LinhaMedianaDente { get; set; }
        public string? MordidaCruzada { get; set; }
        public string? MordidaAberta { get; set; }
        public string? TrespasseVertical { get; set; }
        public string? TrespasseHorizontal { get; set; }
        public string? CurvaSpee { get; set; }
        public int? CaracArcoDentarioMaxila { get; set; }
        public int? CaracArcoDentarioMandibula { get; set; }
        public int? CaracPalato { get; set; }
        public string? Obs { get; set; }
    }

    public class UpdateAnamneseOrtodonticaAnaliseDentariaValidator : AbstractValidator<UpdateAnamneseOrtodonticaAnaliseDentariaRequest>
    {
        public UpdateAnamneseOrtodonticaAnaliseDentariaValidator()
        {
            _ = RuleFor(x => x.UtenteId).NotEmpty().NotNull();
        }
    }
}

