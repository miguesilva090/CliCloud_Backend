namespace CliCloud.Application.Services.Consultas.ExamesSemPapelService.DTOs;

public class ExameSemPapelTabelaDTO
{
  public string Id { get; set; } = string.Empty;
  public string RequisicaoNum { get; set; } = string.Empty;
  public string Utente { get; set; } = string.Empty;
  public string Area { get; set; } = string.Empty;
  public string Estado { get; set; } = string.Empty;
  public bool Lotes { get; set; }
  public string Medico { get; set; } = string.Empty;
  public bool IsencaoTaxa { get; set; }
  public bool ComTaxa { get; set; }
  public bool Pnp { get; set; }
  public bool Assinado { get; set; }
  public string DataRequisicao { get; set; } = string.Empty;
}
