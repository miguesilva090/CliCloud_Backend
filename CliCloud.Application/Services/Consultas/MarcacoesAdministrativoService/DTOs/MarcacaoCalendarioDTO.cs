namespace CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.DTOs;

public class MarcacaoCalendarioDTO
{
  public MarcacaoCalendarioConfigDTO Config { get; set; } = new();
  public List<MarcacaoCalendarioEventoDTO> Eventos { get; set; } = [];
}
