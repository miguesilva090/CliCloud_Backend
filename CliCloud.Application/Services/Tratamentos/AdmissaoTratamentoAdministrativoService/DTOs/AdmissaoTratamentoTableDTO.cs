using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.AdmissaoTratamentoAdministrativoService.DTOs;

public class AdmissaoTratamentoTableDTO : IDto
{
  public Guid Id { get; set; }
  public Guid TratamentoId { get; set; }
  public int? NumSessao { get; set; }
  public DateTime? Data { get; set; }
  public string? HoraInic { get; set; }
  public string? HoraFisio { get; set; }

  public Guid? UtenteId { get; set; }
  public string? UtenteNome { get; set; }
  public string? NumeroUtente { get; set; }

  public Guid? FisioterapeutaId { get; set; }
  public string? FisioterapeutaNome { get; set; }
  public Guid? AuxiliarId { get; set; }
  public string? AuxiliarNome { get; set; }
  public Guid? OutroTecnicoId { get; set; }
  public string? OutroTecnicoNome { get; set; }

  public Guid? LocalTratamentoId { get; set; }
  public string? LocalTratamentoNome { get; set; }

  public int? Confirmado { get; set; }
  public int? Efetuado { get; set; }
  public int? Faltou { get; set; }
  public int? Desmarcado { get; set; }

  public int? NumSessaoTratamento { get; set; }
  public int? NFalta { get; set; }
  public string? Designacao { get; set; }
}
