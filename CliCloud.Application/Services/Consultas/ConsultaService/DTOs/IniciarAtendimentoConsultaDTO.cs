using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.ConsultaService.DTOs;

public class IniciarAtendimentoConsultaDTO : IDto
{
  public Guid ConsultaId { get; set; }
  public Guid? ConsultaMarcacaoId { get; set; }
  public Guid? AdmissaoId { get; set; }
  public Guid UtenteId { get; set; }
  public Guid? MedicoId { get; set; }
  public string? UtenteNome { get; set; }
  public string Origem { get; set; } = string.Empty;
  public bool ConsultaCriada { get; set; }
}
