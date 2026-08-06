using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.HistoricoTratamentoAdministrativoService.DTOs;

public class HistoricoTratamentoTableDTO : IDto
{
  public Guid Id { get; set; }
  public string? Designacao { get; set; }
  public Guid? UtenteId { get; set; }
  public string? NumeroUtente { get; set; }
  public string? UtenteNome { get; set; }
  public DateTime? DataInic { get; set; }
  public DateTime? DataFim { get; set; }
  public int? NumSessao { get; set; }
  public int? Pago { get; set; }
  public int? Faturado { get; set; }
  public int? ConfDfim { get; set; }
  public string? Credencial { get; set; }
  public int? Isencao { get; set; }
  public Guid? MedicoId { get; set; }
  public string? MedicoNome { get; set; }
  public Guid? OrganismoId { get; set; }
  public string? OrganismoNome { get; set; }
  public Guid? FisioterapeutaId { get; set; }
  public string? FisioterapeutaNome { get; set; }
  public Guid? AuxiliarId { get; set; }
  public string? AuxiliarNome { get; set; }
  public Guid? OutroTecnicoId { get; set; }
  public string? OutroTecnicoNome { get; set; }
}
