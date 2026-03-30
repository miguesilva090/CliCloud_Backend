using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Servicos.TipoServicoService.DTOs
{
  public class TipoServicoLightDTO : IDto
  {
    public Guid Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
  }
}

