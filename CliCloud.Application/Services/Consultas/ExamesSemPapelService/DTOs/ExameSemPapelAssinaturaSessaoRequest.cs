namespace CliCloud.Application.Services.Consultas.ExamesSemPapelService.DTOs;

public class ExameSemPapelAssinaturaSessaoRequest
{
  public string CMedico { get; set; } = string.Empty;
  public string TipoCartao { get; set; } = string.Empty;
  public string DigestValue { get; set; } = string.Empty;
  public string SignatureValue { get; set; } = string.Empty;
  public string Assinatura { get; set; } = string.Empty;
  public string AssinaturaSubCA { get; set; } = string.Empty;
}
