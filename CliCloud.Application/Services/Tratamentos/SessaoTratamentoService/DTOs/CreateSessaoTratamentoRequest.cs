using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Utility;

namespace CliCloud.Application.Services.Tratamentos.SessaoTratamentoService.DTOs
{
  public class CreateSessaoTratamentoRequest : IDto
  {
    public required string TratamentoId { get; set; }
    public int? NumSessao { get; set; }
    public DateTime? Data { get; set; }
    public string? HoraInic { get; set; }
    public int? IHoraIni { get; set; }
    public string? Duracao { get; set; }
    public int? IDuraca { get; set; }

    public string? FisioterapeutaId { get; set; }
    public string? AuxiliarId { get; set; }
    public string? OutroTecnicoId { get; set; }

    public string? HoraFisio { get; set; }
    public string? HoraAux { get; set; }
    public string? HoraOutro { get; set; }
    public string? DuracaoFisio { get; set; }
    public string? DuracaoAux { get; set; }
    public string? DuracaoOutro { get; set; }

    public string? ReciboId { get; set; }
    public DateTime? DataRecibo { get; set; }
    public int? Pago { get; set; }
    public int? Faturado { get; set; }
    public string? NumDevolucao { get; set; }
    public string? NumDestacavel { get; set; }
    public string? NumTransacao { get; set; }

    public int? EstadoU { get; set; }
    public int? EstadoI { get; set; }
    public int? Faltou { get; set; }
    public int? CompensaFalta { get; set; }
    public string? ObsFalta { get; set; }
    public int? Desmarcado { get; set; }

    public string? Destino { get; set; }
    public int? ConfFact { get; set; }
    public string? Obs { get; set; }
    public string? ObservSessao { get; set; }
    public int? HistSess { get; set; }
    public int? TipoCambio { get; set; }
    public string? TipoDocumentoId { get; set; }
    public string? DocumentoId { get; set; }
    public DateTime? DataApagar { get; set; }
  }

  public class CreateSessaoTratamentoValidator : AbstractValidator<CreateSessaoTratamentoRequest>
  {
    public CreateSessaoTratamentoValidator()
    {
      _ = RuleFor(x => x.TratamentoId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("TratamentoId inválido.");
      _ = RuleFor(x => x.FisioterapeutaId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id)).WithMessage("FisioterapeutaId inválido.");
      _ = RuleFor(x => x.AuxiliarId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id)).WithMessage("AuxiliarId inválido.");
      _ = RuleFor(x => x.OutroTecnicoId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id)).WithMessage("OutroTecnicoId inválido.");
      _ = RuleFor(x => x.ReciboId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id)).WithMessage("ReciboId inválido.");
      _ = RuleFor(x => x.TipoDocumentoId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id)).WithMessage("TipoDocumentoId inválido.");
      _ = RuleFor(x => x.DocumentoId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id)).WithMessage("DocumentoId inválido.");
      _ = RuleFor(x => x.HoraInic).MaximumLength(20);
      _ = RuleFor(x => x.Duracao).MaximumLength(50);
      _ = RuleFor(x => x.HoraFisio).MaximumLength(20);
      _ = RuleFor(x => x.HoraAux).MaximumLength(20);
      _ = RuleFor(x => x.HoraOutro).MaximumLength(20);
      _ = RuleFor(x => x.DuracaoFisio).MaximumLength(50);
      _ = RuleFor(x => x.DuracaoAux).MaximumLength(50);
      _ = RuleFor(x => x.DuracaoOutro).MaximumLength(50);
      _ = RuleFor(x => x.NumDevolucao).MaximumLength(50);
      _ = RuleFor(x => x.NumDestacavel).MaximumLength(50);
      _ = RuleFor(x => x.NumTransacao).MaximumLength(50);
      _ = RuleFor(x => x.ObsFalta).MaximumLength(500);
      _ = RuleFor(x => x.Destino).MaximumLength(200);
      _ = RuleFor(x => x.Obs).MaximumLength(2000);
      _ = RuleFor(x => x.ObservSessao).MaximumLength(2000);
    }
  }
}

