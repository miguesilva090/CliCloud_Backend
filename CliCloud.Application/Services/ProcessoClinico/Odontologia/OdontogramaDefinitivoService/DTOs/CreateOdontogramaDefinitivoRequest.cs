using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.Odontologia.OdontogramaDefinitivoService.DTOs
{
    public class CreateOdontogramaDefinitivoRequest : IDto
    {
        public Guid UtenteId { get; set; }
        public Guid ConsultaId { get; set; }
        public int NumeroDente { get; set; }
        public int? NumeroDenteAte { get; set; }
        public string? CodigoSuperficie { get; set; }
        public string? CodigoEstadoPadrao { get; set; }
        public string? CodigoTratamentoPadrao { get; set; }
        public string? CodigoEstadoPersonalizado { get; set; }
        public string? CodigoTratamentoPersonalizado { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string? Observacoes { get; set; }
        public bool Faturar { get; set; }
        public int Quantidade { get; set; }
        public decimal? ValorServico { get; set; }
        public decimal? ValorUtente { get; set; }
        public decimal? ValorEntidade { get; set; }
        public Guid? LinhaFaturacaoId { get; set; }
    }

    public class CreateOdontogramaDefinitivoValidator : AbstractValidator<CreateOdontogramaDefinitivoRequest>
    {
        public CreateOdontogramaDefinitivoValidator()
        {
            _ = RuleFor(x => x.UtenteId).NotEmpty();
            _ = RuleFor(x => x.ConsultaId).NotEmpty();
            _ = RuleFor(x => x.NumeroDente).GreaterThan(0);
            _ = RuleFor(x => x.Descricao).NotEmpty();
            _ = RuleFor(x => x.Quantidade).GreaterThan(0);
        }
    }
}
