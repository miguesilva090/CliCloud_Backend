namespace CliCloud.Application.Services.Consultas.ExamesSemPapelService.DTOs;

public class ExameSemPapelLoteResultadoDTO
{
  public int Total { get; set; }
  public int Sucesso { get; set; }
  public int Falha { get; set; }
  public List<string> Mensagens { get; set; } = [];
}
