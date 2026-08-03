using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Utility;

namespace CliCloud.Application.Services.Tratamentos.TratamentoService.DTOs
{
  public class CreateMarcacaoManualServicoItem : IDto
  {
    public required string ServicoId { get; set; }
    public string? Duracao { get; set; }
    public int? Ordem { get; set; }
    public int? UsaFisioter { get; set; }
    public int? UsaAuxiliar { get; set; }
    public int? UsaOutro { get; set; }
    public decimal? Preco { get; set; }
    public decimal? DescInst { get; set; }
    public decimal? ValorUt { get; set; }
    public string? Obs { get; set; }
  }

  public class CreateMarcacaoManualSessaoItem : IDto
  {
    public int? NumSessao { get; set; }
    public DateTime? Data { get; set; }
    public string? HoraInic { get; set; }
    public string? Duracao { get; set; }
    public string? FisioterapeutaId { get; set; }
    public string? AuxiliarId { get; set; }
    public string? OutroTecnicoId { get; set; }
  }

  public class CreateMarcacaoManualTratamentoRequest : IDto
  {
    public string? ListaEsperaTratamentoId { get; set; }

    public required string UtenteId { get; set; }
    public required string OrganismoId { get; set; }
    public string? MedicoId { get; set; }
    public string? FisioterapeutaId { get; set; }
    public string? AuxiliarId { get; set; }
    public string? OutroTecnicoId { get; set; }
    public string? LocalTratamentoId { get; set; }
    public string? LocalOrigemId { get; set; }

    public string? Designacao { get; set; }
    public string? NomePatologia { get; set; }
    public int? NumSessao { get; set; }
    public DateTime? DataInic { get; set; }
    public DateTime? DataFim { get; set; }
    public string? DuracaoTotal { get; set; }
    public string? Credencial { get; set; }
    public string? NumBenif { get; set; }
    public string? Apolice { get; set; }
    public int? NFaltMax { get; set; }
    public int? NFaltComax { get; set; }
    public int? TaxaMod { get; set; }
    public int? Provisorio { get; set; }
    public string? Obs { get; set; }
    public string? TecObs { get; set; }
    public string? SinistroId { get; set; }
    public string? SeguradoraId { get; set; }
    public int? Isencao { get; set; }
    public int? ConfDfim { get; set; }
    public int? CredencialExterna { get; set; }
    public int TerapiaFala { get; set; }
    public string? NumCartao { get; set; }
    public string? HoraFisio { get; set; }
    public string? HoraAux { get; set; }
    public string? HoraOutro { get; set; }

    public List<CreateMarcacaoManualServicoItem> Servicos { get; set; } = [];
    public List<CreateMarcacaoManualSessaoItem> Sessoes { get; set; } = [];
  }

  public class CreateMarcacaoManualTratamentoValidator
    : AbstractValidator<CreateMarcacaoManualTratamentoRequest>
  {
    public CreateMarcacaoManualTratamentoValidator()
    {
      _ = RuleFor(x => x.UtenteId).NotEmpty().Must(GSHelpers.BeValidGuid)
        .WithMessage("Seleccione o utente.");
      _ = RuleFor(x => x.OrganismoId).NotEmpty().Must(GSHelpers.BeValidGuid)
        .WithMessage("Certifique-se que o organismo está preenchido");
      _ = RuleFor(x => x.ListaEsperaTratamentoId)
        .Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id))
        .WithMessage("ListaEsperaTratamentoId inválido.");
      _ = RuleFor(x => x.MedicoId)
        .Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id));
      _ = RuleFor(x => x.FisioterapeutaId)
        .Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id));
      _ = RuleFor(x => x.AuxiliarId)
        .Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id));
      _ = RuleFor(x => x.OutroTecnicoId)
        .Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id));
      _ = RuleFor(x => x.LocalTratamentoId)
        .Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id));
      _ = RuleFor(x => x.Servicos).NotEmpty()
        .WithMessage("Não existem serviços selecionados");
      _ = RuleForEach(x => x.Servicos).ChildRules(s =>
      {
        _ = s.RuleFor(i => i.ServicoId).NotEmpty().Must(GSHelpers.BeValidGuid)
          .WithMessage("ServicoId inválido.");
      });
      _ = RuleFor(x => x.Sessoes).NotEmpty()
        .WithMessage("Não foram inseridas sessões selecionados");
      _ = RuleForEach(x => x.Sessoes).ChildRules(s =>
      {
        _ = s.RuleFor(i => i.Data).NotNull()
          .WithMessage("A data da sessão é obrigatória.");
      });
    }
  }
}