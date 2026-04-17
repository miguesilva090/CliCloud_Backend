namespace CliCloud.Application.Services.Consultas.ExamesSemPapelService.DTOs;

public class ExameSemPapelLoteRequest
{
  public List<string> Requisicoes { get; set; } = [];
  public string? AreaPrestacao { get; set; }
}
