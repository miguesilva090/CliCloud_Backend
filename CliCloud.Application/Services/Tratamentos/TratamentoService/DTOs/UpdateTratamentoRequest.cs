using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Utility;

namespace CliCloud.Application.Services.Tratamentos.TratamentoService.DTOs
{
  public class UpdateTratamentoRequest : IDto
  {
    public string? UtenteId { get; set; }
    public string? MedicoId { get; set; }
    public string? FisioterapeutaId { get; set; }
    public string? AuxiliarId { get; set; }
    public string? OutroTecnicoId { get; set; }
    public string? OrganismoId { get; set; }
    public string? LocalTratamentoId { get; set; }
    public string? TratamentoPredId { get; set; }
    public string? LocalOrigemId { get; set; }

    public string? Designacao { get; set; }
    public int? NumSessao { get; set; }
    public DateTime? DataInic { get; set; }
    public int? ConfDfim { get; set; }
    public DateTime? DataFim { get; set; }
    public DateTime? Data { get; set; }

    public int? NFaltMax { get; set; }
    public int? NFaltComax { get; set; }
    public int? NFalta { get; set; }
    public int? NFaltaCons { get; set; }
    public int? NAltSess { get; set; }

    public double? Preco { get; set; }
    public double? DescInst { get; set; }
    public double? DescCli { get; set; }
    public double? ValorDesc { get; set; }
    public string? ReciboId { get; set; }
    public DateTime? DataRecibo { get; set; }
    public int? Pago { get; set; }
    public int? Faturado { get; set; }
    public string? NumDevolucao { get; set; }
    public string? NumDestacavel { get; set; }

    public int? EstadoU { get; set; }
    public int? EstadoI { get; set; }
    public int? Suspenso { get; set; }
    public DateTime? DataSuspensao { get; set; }
    public int? Provisorio { get; set; }

    public string? Obs { get; set; }
    public string? TecObs { get; set; }
    public int? Isencao { get; set; }
    public string? Credencial { get; set; }
    public int? CredencialExterna { get; set; }
    public int? DestacavelCredencial { get; set; }
    public int? TaxaMod { get; set; }
    public int? Inisess { get; set; }
    public string? HoraFisio { get; set; }
    public string? HoraAux { get; set; }
    public string? HoraOutro { get; set; }
    public string? DuracaoTotal { get; set; }
    public int? SelOutro { get; set; }
    public string? NumCartao { get; set; }
    public bool? Orespons { get; set; }
    public int? ConfirmaLoc { get; set; }
    public string? SinistroId { get; set; }
    public string? SeguradoraId { get; set; }
    public string? DocumentoId { get; set; }
    public int? SemanaCompleta { get; set; }
    public int? VemListEsp { get; set; }
    public int? CartaoDevolv { get; set; }
    public int TerapiaFala { get; set; }
    public string? NumBenif { get; set; }
    public string? Apolice { get; set; }
    public string? NomePatologia { get; set; }
    public bool? Frespons { get; set; }
    public bool? Arespons { get; set; }
    public int Lotes { get; set; }
    public bool SendEmail { get; set; }
  }

  public class UpdateTratamentoValidator : AbstractValidator<UpdateTratamentoRequest>
  {
    public UpdateTratamentoValidator()
    {
      _ = RuleFor(x => x.UtenteId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id)).WithMessage("UtenteId inválido.");
      _ = RuleFor(x => x.MedicoId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id)).WithMessage("MedicoId inválido.");
      _ = RuleFor(x => x.FisioterapeutaId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id)).WithMessage("FisioterapeutaId inválido.");
      _ = RuleFor(x => x.AuxiliarId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id)).WithMessage("AuxiliarId inválido.");
      _ = RuleFor(x => x.OutroTecnicoId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id)).WithMessage("OutroTecnicoId inválido.");
      _ = RuleFor(x => x.OrganismoId)
        .NotEmpty()
        .WithMessage("Certifique-se que o organismo está preenchido")
        .Must(id => GSHelpers.BeValidGuid(id))
        .WithMessage("OrganismoId inválido.");
      _ = RuleFor(x => x.LocalTratamentoId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id)).WithMessage("LocalTratamentoId inválido.");
      _ = RuleFor(x => x.TratamentoPredId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id)).WithMessage("TratamentoPredId inválido.");
      _ = RuleFor(x => x.LocalOrigemId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id)).WithMessage("LocalOrigemId inválido.");
      _ = RuleFor(x => x.ReciboId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id)).WithMessage("ReciboId inválido.");
      _ = RuleFor(x => x.SinistroId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id)).WithMessage("SinistroId inválido.");
      _ = RuleFor(x => x.SeguradoraId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id)).WithMessage("SeguradoraId inválido.");
      _ = RuleFor(x => x.DocumentoId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id)).WithMessage("DocumentoId inválido.");
      _ = RuleFor(x => x.Designacao).MaximumLength(250);
      _ = RuleFor(x => x.Obs).MaximumLength(2000);
      _ = RuleFor(x => x.TecObs).MaximumLength(2000);
      _ = RuleFor(x => x.Credencial).MaximumLength(100);
      _ = RuleFor(x => x.NumDevolucao).MaximumLength(50);
      _ = RuleFor(x => x.NumDestacavel).MaximumLength(50);
      _ = RuleFor(x => x.HoraFisio).MaximumLength(20);
      _ = RuleFor(x => x.HoraAux).MaximumLength(20);
      _ = RuleFor(x => x.HoraOutro).MaximumLength(20);
      _ = RuleFor(x => x.DuracaoTotal).MaximumLength(50);
      _ = RuleFor(x => x.NumCartao).MaximumLength(50);
      _ = RuleFor(x => x.NumBenif).MaximumLength(50);
      _ = RuleFor(x => x.Apolice).MaximumLength(50);
      _ = RuleFor(x => x.NomePatologia).MaximumLength(250);
    }
  }
}

