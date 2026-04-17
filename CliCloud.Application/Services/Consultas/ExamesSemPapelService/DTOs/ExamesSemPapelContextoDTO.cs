namespace CliCloud.Application.Services.Consultas.ExamesSemPapelService.DTOs;

public class ExamesSemPapelContextoDTO
{
  public int? PortaLeitorCartoes { get; set; }
  public bool TemAssinaturaCarregada { get; set; }
  public string? AreaPrestacaoAssinarESPDefeito { get; set; }
  public bool PermitirElaborarRelatorioESP { get; set; }
  public List<AreaPrestacaoOpcaoDTO> AreasPrestacao { get; set; } = [];
}
