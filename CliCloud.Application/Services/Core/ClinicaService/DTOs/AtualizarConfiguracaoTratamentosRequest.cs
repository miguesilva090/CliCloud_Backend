using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Core.ClinicaService.DTOs
{
  public class AtualizarConfiguracaoTratamentosRequest : IDto
  {
    public string? TipoSrvTratamentos { get; set; }
    public string? AreaPrestacaoDefeitoAreaZ { get; set; }
    public bool? ControlarAparelhos { get; set; }

    public int? Segundos { get; set; }
    public int? FaltasMax { get; set; }
    public int? FaltasConsecutivasMax { get; set; }
    public decimal? Taxamoderadora { get; set; }
    public bool? CredencialExternaAdse { get; set; }

    public int? TipoPagamento { get; set; }
    public bool? AvisoInqueritoSessoesDiarias { get; set; }
  }

  public class AtualizarConfiguracaoTratamentosValidator
    : AbstractValidator<AtualizarConfiguracaoTratamentosRequest>
  {
    public AtualizarConfiguracaoTratamentosValidator()
    {
      _ = RuleFor(x => x.TipoSrvTratamentos).MaximumLength(50);
      _ = RuleFor(x => x.AreaPrestacaoDefeitoAreaZ).MaximumLength(50);

      _ = RuleFor(x => x.Segundos).GreaterThanOrEqualTo(0).When(x => x.Segundos.HasValue);
      _ = RuleFor(x => x.FaltasMax).GreaterThanOrEqualTo(0).When(x => x.FaltasMax.HasValue);
      _ = RuleFor(x => x.FaltasConsecutivasMax)
        .GreaterThanOrEqualTo(0)
        .When(x => x.FaltasConsecutivasMax.HasValue);
      _ = RuleFor(x => x.Taxamoderadora).GreaterThanOrEqualTo(0).When(x => x.Taxamoderadora.HasValue);

      _ = RuleFor(x => x.TipoPagamento).InclusiveBetween(0, 1).When(x => x.TipoPagamento.HasValue);
    }
  }
}

