using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Utility;

namespace CliCloud.Application.Services.Tratamentos.ServicoSessaoService.DTOs
{
  public class CreateServicoSessaoRequest : IDto
  {
    public required string SessaoTratamentoId { get; set; }
    public string? FisioterapeutaId { get; set; }
    public string? AuxiliarId { get; set; }
    public string? ServicoId { get; set; }

    public string? HoraInic { get; set; }
    public int? IHoraIni { get; set; }
    public string? HoraFim { get; set; }
    public int? IHoraFim { get; set; }

    public string? Duracao { get; set; }
    public int? IDuraca { get; set; }

    public int? Ordem { get; set; }
    public string? AparelhoId { get; set; }

    public decimal? Preco { get; set; }
    public decimal? DescInst { get; set; }
    public decimal? ValorDesc { get; set; }
    public decimal? ValorUt { get; set; }

    public string? Obs { get; set; }
  }

  public class CreateServicoSessaoValidator : AbstractValidator<CreateServicoSessaoRequest>
  {
    public CreateServicoSessaoValidator()
    {
      _ = RuleFor(x => x.SessaoTratamentoId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("SessaoTratamentoId inválido.");
      _ = RuleFor(x => x.FisioterapeutaId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id)).WithMessage("FisioterapeutaId inválido.");
      _ = RuleFor(x => x.AuxiliarId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id)).WithMessage("AuxiliarId inválido.");
      _ = RuleFor(x => x.ServicoId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id)).WithMessage("ServicoId inválido.");
      _ = RuleFor(x => x.AparelhoId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id)).WithMessage("AparelhoId inválido.");
      _ = RuleFor(x => x.HoraInic).MaximumLength(20);
      _ = RuleFor(x => x.HoraFim).MaximumLength(20);
      _ = RuleFor(x => x.Duracao).MaximumLength(50);
      _ = RuleFor(x => x.Obs).MaximumLength(2000);
    }
  }
}

